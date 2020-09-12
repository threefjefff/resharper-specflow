using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon.SyntaxHighlighting;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon
{
    [StaticSeverityHighlighting(Severity.ERROR, nameof(StepDefinitionMissingHighlighting), OverlapResolve = OverlapResolveKind.NONE)]
    public class StepDefinitionMissingHighlighting: IHighlighting
    {
        private readonly GherkinStep _step;

        public StepDefinitionMissingHighlighting(GherkinStep step)
        {
            _step = step;
        }

        public DocumentRange CalculateRange() => _step.GetDocumentRange();
        public bool IsValid() => _step.IsValid();
        public string ToolTip => "No step definition found";
        public string ErrorStripeToolTip => ToolTip;
    }
}