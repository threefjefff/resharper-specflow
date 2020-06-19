using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.ActionsRevised.Handlers;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Navigation.NavigationExtensions;
using JetBrains.ReSharper.Intentions.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.RiderTutorials.Utils;
using JetBrains.Util;
using JetBrains.Util.Logging;
using NuGet.Commands;
using ReSharperPlugin.SpecflowRiderPlugin.Daemon;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Navigation
{
    [ContextNavigationProvider]
    public class NavigateToStepDefinitionProvider: IGotoImplementationsProvider
    {
        public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
        {
            var node = dataContext.GetSelectedTreeNode<ITreeNode>();
            var step = node?.GetParentOfType<GherkinStep>();
            var key = FeatureStepDefinitionFinder.UserDataKey;
            if (step != null)
            {
                if (step.UserData.HasKey(key))
                {
                    var attribute = step.UserData.GetData(key);
                    Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Creating navigation context for: {attribute?.Name}");
                    yield return new ContextNavigation("Step Definition", "GetStepDefinition", NavigationActionGroup.Important, () =>
                    {
                        attribute.NavigateToTreeNode(true);
                    });
                }
            }
        }
    }
}