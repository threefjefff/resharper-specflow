using System;
using System.Text;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace ReSharperPlugin.SpecflowRiderPlugin.Psi
{
    public enum GherkinStepKeyword
    {
        Given,
        When,
        Then,
        And,
        Unknown
    }
    
    public class GherkinStep : GherkinElement
    {
        public GherkinStep() : base(GherkinNodeTypes.STEP)
        {
        }

        public GherkinStepKeyword GetStepKeyword()
        {
            //TODO: Run through the i18n script
            var stepKeyword = this.FindChild<GherkinToken>(o => o.NodeType == GherkinTokenTypes.STEP_KEYWORD);
            if (stepKeyword == null) return GherkinStepKeyword.Unknown;

            return Enum.TryParse<GherkinStepKeyword>(stepKeyword.GetText(), out var verb) ? verb : GherkinStepKeyword.Unknown;
        }
        
        /// <summary>
        /// Assembles all GherkinTokens in a step, replacing GherkinStepParameter
        /// with regex capturing group tokens
        /// </summary>
        /// <returns>Step name, or empty string</returns>
        public string GetStepNameWithRegex()
        {
            var sb = new StringBuilder();
            for (var te = (TreeElement)FirstChild; te != null; te = te.nextSibling)
            {
                switch (te)
                {
                    case GherkinStepParameter _:
                        sb.Append("()");
                        break;
                    case GherkinToken token:
                        if(token.NodeType != GherkinTokenTypes.STEP_KEYWORD)
                            sb.Append(token.GetText());
                        break;
                }
            }

            return sb.ToString();
        }
    }
}