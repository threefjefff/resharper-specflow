using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Navigation.NavigationExtensions;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.RiderTutorials.Utils;
using JetBrains.Util;
using JetBrains.Util.Logging;
using ReSharperPlugin.SpecflowRiderPlugin.Daemon;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Navigation
{
    [ContextNavigationProvider]
    public class NavigateToStepDefinitionAttributeProvider: IGotoImplementationsProvider
    {
        public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
        {
            var node = dataContext.GetSelectedTreeNode<ITreeNode>();
            var step = node?.GetParentOfType<GherkinStep>();
            var key = SpecflowStepDefinitionLinker.AttributeUserDataKey;
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
    
    [ContextNavigationProvider]
    public class NavigateToStepDefinitionFeatureFileProvider: IGotoImplementationsProvider
    {
        public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
        {
            var node = dataContext.GetSelectedTreeNode<ITreeNode>();
            var attribute = node?.GetParentOfType<IAttribute>();
            var key = SpecflowStepDefinitionLinker.StepUserDataKey;
            if (attribute != null)
            {
                if (attribute.UserData.HasKey(key))
                {
                    var step = attribute.UserData.GetData(key);
                    Logger.Root.Log(LoggingLevel.TRACE, $"JSMB - Creating navigation context for: {step?.GetStepNameWithRegex()}");
                    yield return new ContextNavigation("Step Definition", "GetStepDefinition", NavigationActionGroup.Important, () =>
                    {
                        step.NavigateToTreeNode(true);
                    });
                }
            }
        }
    }
}