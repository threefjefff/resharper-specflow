using System.Collections.Generic;
using System.Linq;
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
            var method = node?.GetParentOfType<IMethodDeclaration>();
            
            var key = SpecflowStepDefinitionLinker.StepUserDataKey;
            return method?.AttributesEnumerable
                .Where(a => SpecflowAttributeLinkHelper.IsSpecflowAttribute(a, out _))
                .Where(a => a.UserData.HasKey(key))
                .Select(a =>
                {
                    var step = a.UserData.GetData(key);
                    return new ContextNavigation(step!.GetText(), "GetStepDefinition",
                        NavigationActionGroup.Important,
                        () => { step.NavigateToTreeNode(true); });
                }) ?? Enumerable.Empty<ContextNavigation>();
        }
    }
}