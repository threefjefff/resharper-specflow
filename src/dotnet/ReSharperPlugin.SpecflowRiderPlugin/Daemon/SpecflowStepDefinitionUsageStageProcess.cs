using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage), typeof(CollectUsagesStage) })]
    public class SpecflowStepDefinitionLinker : IDaemonStage
    {
        public IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process, 
            IContextBoundSettingsStore settings, 
            DaemonProcessKind processKind)
        {
            //CSharpDaemonStageProcessBase
            //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Running {nameof(SpecflowStepDefinitionLinker)}");
            // if (IsCSharpFile(process.SourceFile))
            // {
            //     Protocol.Logger.Log(LoggingLevel.TRACE, $"JSMB - Found feature file: {process.SourceFile.Name}");
            //     return process.SourceFile.GetPsiFiles<CSharpLanguage>().SelectNotNull((Func<IFile, IDaemonStageProcess>) 
            //         (file => new CSharpStepDefinitionFinder(process, settings, processKind, (ICSharpFile) file)));
            // }

            if(IsFeatureFile(process.SourceFile))
            {
                //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Found feature file: {process.SourceFile.Name}");
                return process.SourceFile.GetPsiFiles<GherkinLanguage>().SelectNotNull((Func<IFile, IDaemonStageProcess>) 
                    (file => new FeatureStepDefinitionFinder(process, settings, processKind, file)));
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
        public static Key<IAttribute> UserDataKey = new Key<IAttribute>(nameof(FeatureStepDefinitionFinder));
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
                foreach (var (specflowAttribute, gherkinStepKeyword) in SpecflowAttributeFinder.GetSpecflowAttributes(file))
                {
                    //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Searching through Module files, found: {file.Name}");
                    var literal = specflowAttribute.ConstructorArgumentExpressions[0] as ILiteralExpression;
                    if (!(literal is ICSharpLiteralExpression csharpLiteral)) continue;
                    var regexString = csharpLiteral.ParseStringLiteral();
                    regexString = SpecflowAttributeFinder.SimplifyRegexCapgroups(regexString);
                    var dictKey = $"{gherkinStepKeyword} {regexString}";
                    codeDict.Add(dictKey, specflowAttribute);
                    //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Adding: {dictKey} to dict");
                }
            }
            
            var lastSeenKeyword = GherkinStepKeyword.Unknown;
            //For each step defition
            foreach (var step in _file.ThisAndDescendants<GherkinStep>())
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
                    //Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Found matching attribute in: {matchedAttribute.GetSourceFile().Name}");
                    step.UserData.PutData(UserDataKey, matchedAttribute);
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

    public class CSharpStepDefinitionFinder : IDaemonStageProcess
    {
        public CSharpStepDefinitionFinder(IDaemonProcess process, 
            IContextBoundSettingsStore settings, 
            DaemonProcessKind processKind, 
            ICSharpFile file)
        {
            DaemonProcess = process;
        }

        public void Execute(Action<DaemonStageResult> committer)
        {
            //For each step defition
            //1. Normalize (replace capture groups with placeholder "()", add verb to start of match string)
            //2. Scan feature files for attributes of the correct type
            //3. Match the first attribute with an acceptable step definiton string
        }

        public IDaemonProcess DaemonProcess { get; }
    }
}