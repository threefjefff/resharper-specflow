using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.Rd.Impl;
using JetBrains.ReSharper.Daemon.SyntaxHighlighting;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Host.Features.SyntaxHighlighting;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting.Rider
{
    [Language(typeof(GherkinLanguage))]
    public class GherkinSyntaxHighlightingManager : RiderSyntaxHighlightingManager
    {
        public GherkinSyntaxHighlightingManager()
        {
            Protocol.Logger.Log(LoggingLevel.INFO, $"GherkinSyntaxHighlightingManager");
        }

        public override SyntaxHighlightingStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, IFile getPrimaryPsiFile)
        {
            Protocol.Logger.Log(LoggingLevel.INFO, $"GherkinSyntaxHighlightingManager:CreateProcess - {getPrimaryPsiFile.GetSourceFile().Name}");
            
            return new GherkinSyntaxHighlightingStageProcess(process, settings, getPrimaryPsiFile, new GherkinSyntaxHighlightingProcessor());
        }
    }
}