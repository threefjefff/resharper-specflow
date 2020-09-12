using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.Logging;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage), typeof(CollectUsagesStage) })]
    public class SpecflowStepDefinitionLinker : IDaemonStage
    {
        public static readonly Key<IAttribute> AttributeUserDataKey = new Key<IAttribute>(nameof(FeatureStepDefinitionFinder));
        public static readonly Key<List<GherkinStep>> StepUserDataKey = new Key<List<GherkinStep>>(nameof(FeatureStepDefinitionFinder));
        public IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process, 
            IContextBoundSettingsStore settings, 
            DaemonProcessKind processKind)
        {
            if(IsFeatureFile(process.SourceFile))
            {
                return process.SourceFile.GetPsiFiles<GherkinLanguage>().SelectNotNull((Func<IFile, IDaemonStageProcess>) 
                    (file => new FeatureStepDefinitionFinder(process, settings, processKind, file)));
            }

            if (IsCSharpFile(process.SourceFile))
            {
                return process.SourceFile.GetPsiFiles<CSharpLanguage>().SelectNotNull((Func<IFile, IDaemonStageProcess>) 
                    (file => new CSharpStepDefinitionFinder(process, (ICSharpFile) file)));
            }

            return EmptyList<IDaemonStageProcess>.Instance;
        }
        
        protected bool IsCSharpFile([CanBeNull] IPsiSourceFile sourceFile)
        {
            if (sourceFile == null || !sourceFile.IsValid())
                return false;
            IPsiSourceFileProperties properties = sourceFile.Properties;
            return !properties.IsNonUserFile && properties.ProvidesCodeModel && sourceFile.IsLanguageSupported<CSharpLanguage>();
        }
        
        protected bool IsFeatureFile([CanBeNull] IPsiSourceFile sourceFile)
        {
            //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - File {sourceFile.Name}, Is valid {sourceFile.IsValid()}");
            if (sourceFile == null || !sourceFile.IsValid())
                return false;
            IPsiSourceFileProperties properties = sourceFile.Properties;
            return !properties.IsNonUserFile && properties.ProvidesCodeModel && sourceFile.IsLanguageSupported<GherkinLanguage>();
        }
    }

    public class FeatureStepDefinitionFinder : IDaemonStageProcess
    {
        private IFile _file;
        public FeatureStepDefinitionFinder(IDaemonProcess process, 
            IContextBoundSettingsStore settings, 
            DaemonProcessKind processKind,
            IFile file)
        {
            _file = file;
            DaemonProcess = process;
        }

        public void Execute(Action<DaemonStageResult> committer)
        {
            var psiSource = _file?.GetSourceFile();
            //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Processing: {psiSource.Name}");
            
            var consumer = new DefaultHighlightingConsumer(psiSource);
            //Scan CSharp code files for attributes of the correct type (Given/When/Then)
            var csharpFiles = DaemonProcess.PsiModule.SourceFiles
                .Where(file => file.IsLanguageSupported<CSharpLanguage>());
            var codeDict = new Dictionary<string, IAttribute>();
            foreach (var file in csharpFiles)
            {
                foreach (var (specflowAttribute, gherkinStepKeyword) in SpecflowAttributeLinkHelper.GetSpecflowAttributes(file))
                {
                    //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Searching through Module files, found: {file.Name}");
                    var literal = specflowAttribute.ConstructorArgumentExpressions[0] as ILiteralExpression;
                    if (!(literal is ICSharpLiteralExpression csharpLiteral)) continue;
                    var regexString = csharpLiteral.ParseStringLiteral();
                    regexString = SpecflowAttributeLinkHelper.SimplifyRegexCapgroups(regexString);
                    var dictKey = $"{gherkinStepKeyword} {regexString}";
                    codeDict.Add(dictKey, specflowAttribute);
                    //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Adding: {dictKey} to dict");
                }
            }
            
            var lastSeenKeyword = GherkinStepKeyword.Unknown;
            foreach (var step in SpecflowAttributeLinkHelper.GetGherkinSteps(_file))
            {
                //Normalize (turn "And" into their prefered verb, replace variables with placeholder "()")
                var stepText = step.GetStepNameWithRegex();
                var stepKeyword = step.GetStepKeyword();
                
                if (stepKeyword == GherkinStepKeyword.And)
                {
                    stepKeyword = lastSeenKeyword;
                }
                else
                {
                    lastSeenKeyword = stepKeyword;
                }
                
                var dictKey = $"{stepKeyword}{stepText}";
                //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Found step definition: {dictKey}");
                //3. Match the first attribute with an acceptable regex string
                if (codeDict.TryGetValue(dictKey, out var matchedAttribute))
                {
                    SpecflowAttributeLinkHelper.StoreLink(matchedAttribute, step);
                }
                else
                {
                    consumer.AddHighlighting(new StepDefinitionMissingHighlighting(step));
                }
            }
            
            var highlights = new List<HighlightingInfo>();
            highlights.AddRange(consumer.Highlightings);
            committer(new DaemonStageResult(highlights));
        }

        public IDaemonProcess DaemonProcess { get; }
    }

    public class CSharpStepDefinitionFinder : CSharpDaemonStageProcessBase
    {
        public CSharpStepDefinitionFinder([NotNull] IDaemonProcess process, [NotNull] ICSharpFile file) 
            : base(process, file)
        {
        }
        
        public override void Execute(Action<DaemonStageResult> committer)
        {
            var consumer = new DefaultHighlightingConsumer(File.GetSourceFile());
            var codeDict = new Dictionary<string, IAttribute>();
            foreach (var (specflowAttribute, gherkinStepKeyword) in SpecflowAttributeLinkHelper.GetSpecflowAttributes(File))
            {
                //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Searching through Module files, found: {file.Name}");
                var literal = specflowAttribute.ConstructorArgumentExpressions[0] as ILiteralExpression;
                if (!(literal is ICSharpLiteralExpression csharpLiteral)) continue;
                var regexString = csharpLiteral.ParseStringLiteral();
                regexString = SpecflowAttributeLinkHelper.SimplifyRegexCapgroups(regexString);
                var dictKey = $"{gherkinStepKeyword} {regexString}";
                codeDict.Add(dictKey, specflowAttribute);
                //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Adding: {dictKey} to dict");
            }
            var featureFiles = DaemonProcess.PsiModule.SourceFiles
                .Where(file => file.IsLanguageSupported<GherkinLanguage>());
            var lastSeenKeyword = GherkinStepKeyword.Unknown;
            foreach (var file in featureFiles)
            {
                foreach (var step in SpecflowAttributeLinkHelper.GetGherkinSteps(file))
                {
                    //Normalize (turn "And" into their prefered verb, replace variables with placeholder "()")
                    var stepText = step.GetStepNameWithRegex();
                    var stepKeyword = step.GetStepKeyword();
                    if (stepKeyword == GherkinStepKeyword.And)
                    {
                        stepKeyword = lastSeenKeyword;
                    }
                    else
                    {
                        lastSeenKeyword = stepKeyword;
                    }

                    var dictKey = $"{stepKeyword}{stepText}";
                    //3. Match the first attribute with an acceptable regex string
                    if (codeDict.TryGetValue(dictKey, out var matchedAttribute))
                    {
                        SpecflowAttributeLinkHelper.StoreLink(matchedAttribute, step);
                    }
                    else
                    {
                        consumer.AddHighlighting(new StepDefinitionMissingHighlighting(step));
                    }
                }
            }
            
            var highlights = new List<HighlightingInfo>();
            highlights.AddRange(consumer.Highlightings);
            committer(new DaemonStageResult(highlights));
        }
    }
}