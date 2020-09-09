using System.Collections.Generic;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon
{
    public static class SpecflowAttributeLinkHelper
    {
        private const string GivenAttributeClrName = "TechTalk.SpecFlow.GivenAttribute";
        private const string WhenAttributeClrName = "TechTalk.SpecFlow.WhenAttribute";
        private const string ThenAttributeClrName = "TechTalk.SpecFlow.ThenAttribute";
        public static readonly ClrTypeName GivenAttribute =
            new ClrTypeName(GivenAttributeClrName);
        public static readonly ClrTypeName WhenAttribute =
            new ClrTypeName(WhenAttributeClrName);
        public static readonly ClrTypeName ThenAttribute =
            new ClrTypeName(ThenAttributeClrName);
        
        public static IEnumerable<(IAttribute, GherkinStepKeyword)> GetSpecflowAttributes(IPsiSourceFile sourceFile) =>
            sourceFile.EnumerateDominantPsiFiles().SelectMany(GetSpecflowAttributes);
        
        public static IEnumerable<(IAttribute, GherkinStepKeyword)> GetSpecflowAttributes(IFile file)
        {
            foreach (var attribute in file.ThisAndDescendants<IAttribute>())
            {
                if (IsSpecflowAttribute(attribute, out var keyword))
                {
                    yield return (attribute, keyword);
                }
            }
        }

        public static IEnumerable<GherkinStep> GetGherkinSteps(IPsiSourceFile sourceFile) =>
            sourceFile.EnumerateDominantPsiFiles().SelectMany(GetGherkinSteps);
        
        public static IEnumerable<GherkinStep> GetGherkinSteps(IFile file)
        {
            foreach (var step in file.ThisAndDescendants<GherkinStep>())
            {
                yield return step;
            }
        }
        
        private static bool IsStepDefinition(IClass @class, out IClrTypeName clrTypeName)
        {
            clrTypeName = @class.GetClrName();
            return Equals(clrTypeName, GivenAttribute) 
                   || Equals(clrTypeName, WhenAttribute)
                   || Equals(clrTypeName, ThenAttribute);
        }

        public static string SimplifyRegexCapgroups(string regex)
        {
            return regex.Replace("(.*)", "()");
        }
        
        public static bool IsSpecflowAttribute(IAttribute attribute, out GherkinStepKeyword keyword)
        {
            keyword = GherkinStepKeyword.Unknown;
            if (attribute?.Name.Reference.Resolve().DeclaredElement is IClass @class 
                && IsStepDefinition(@class, out var clrTypeName))
            {
                switch (clrTypeName.FullName)
                {
                    case GivenAttributeClrName:
                        keyword = GherkinStepKeyword.Given;
                        break;
                    case WhenAttributeClrName:
                        keyword = GherkinStepKeyword.When;
                        break;
                    case ThenAttributeClrName:
                        keyword = GherkinStepKeyword.Then;
                        break;
                }
                return true;
            }
            return false;
        }
    }
}