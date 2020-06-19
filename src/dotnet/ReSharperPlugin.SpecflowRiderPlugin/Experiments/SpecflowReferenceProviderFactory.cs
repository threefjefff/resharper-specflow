using System;
using System.Collections;
using System.Reflection;
using System.Text;
using JetBrains.DataFlow;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.Rd.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.SpecflowRiderPlugin.Experiments
{
    [ReferenceProviderFactory]
    public class SpecflowReferenceProviderFactory : IReferenceProviderFactory
    {
        public IReferenceFactory CreateFactory(IPsiSourceFile sourceFile, IFile file, IWordIndex wordIndexForChecks)
        {
            if (sourceFile.PrimaryPsiLanguage.Is<CSharpLanguage>())
                return new CSharpToGherkinReferenceFactory();
            return null;
        }

        public ISignal<IReferenceProviderFactory> Changed { get; } = new Signal<IReferenceProviderFactory>(Lifetime.Eternal, nameof(SpecflowReferenceProviderFactory));
    }

    public class CSharpToGherkinReferenceFactory : IReferenceFactory
    {
        public static readonly ClrTypeName GivenAttribute =
            new ClrTypeName("TechTalk.SpecFlow.GivenAttribute");
        public static readonly ClrTypeName WhenAttribute =
            new ClrTypeName("TechTalk.SpecFlow.WhenAttribute");
        public static readonly ClrTypeName ThenAttribute =
            new ClrTypeName("TechTalk.SpecFlow.ThenAttribute");

        public ReferenceCollection GetReferences(ITreeNode element, ReferenceCollection oldReferences)
        {
            if (element.IsPhysical() 
                && element.IsValid() 
                && element is ILiteralExpression literal 
                && literal.ConstantValue.Value is string)
            {
                var argumentExpression = literal as ICSharpExpression;
                var attribute = AttributeNavigator.GetByConstructorArgumentExpression(argumentExpression);

                if (attribute?.Name.Reference.Resolve().DeclaredElement is IClass @class && IsStepDefinition(@class, out var clrTypeName))
                {
                    var stringRegexLiteral = attribute.ConstructorArgumentExpressions[0] as ILiteralExpression;
                    var stringToken = stringRegexLiteral.Literal as CSharpGenericToken;
                    var regexString = SimplifyRegexCapgroups(stringToken.GetText());
                    Protocol.Logger.Log(LoggingLevel.INFO, $"Found step definiton: {clrTypeName.ShortName} {regexString}");
                    return oldReferences;
                }
            }

            return oldReferences;
        }

        public bool HasReference(ITreeNode element, IReferenceNameContainer names)
        {
            return false;
        }

        private bool IsStepDefinition(IClass @class, out IClrTypeName clrTypeName)
        {
            clrTypeName = @class.GetClrName();
            return Equals(clrTypeName, GivenAttribute) 
                || Equals(clrTypeName, WhenAttribute)
                || Equals(clrTypeName, ThenAttribute);
        }
        
        private string SimplifyRegexCapgroups(string regex)
        {
            return regex.Replace("(.*)", "()");
            // var firstCapOpen = regex.IndexOf('(');
            // if (firstCapOpen == -1) return regex;
            //
            // var chars = regex.ToCharArray();
            // var open = 1;
            // var pointer = firstCapOpen + 1;
            // int outerOpen = firstCapOpen;
            //
            //
            // while (pointer < chars.Length - 1 && open > 0)
            // {
            //     switch (chars[pointer])
            //     {
            //         case '(':
            //             open += 1;
            //             break;
            //         case ')':
            //             open -= 1;
            //             break;
            //     }
            // }
            //
            // if (open == 0)
            // {
            //     chars = regex.Remove(outerOpen, pointer - outerOpen).ToCharArray();
            //     pointer += 1;
            // }
        }
    }

    public static class ObjectDumper
    {
        public static void DumpObj<T>(this T obj, int level = 0)
        {
            var sb = new StringBuilder();
            WriteObjectDump(obj, obj.GetType(), level, sb, 0);
            Protocol.Logger.Log(LoggingLevel.INFO, sb.ToString());
        }

        private static void WriteObjectDump(object obj, Type type, int level, StringBuilder sb, int indent)
        {
            sb.AppendLine();
            sb.Append("".PadLeft(indent));
            sb.AppendLine($"{type.Name}:");
                
            DumpProperties(obj, type, sb, indent);

            if (level > 0)
            {
                type = type.BaseType;
                if (type == null)
                    return;

                if (type != typeof(object))
                    WriteObjectDump(obj, type, level - 1, sb, indent + 4);
            }
        }

        private static void DumpProperties<T>(T obj, Type type, StringBuilder sb, int indent)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var propertyInfo in props)
            {
                var propertyValue = propertyInfo.GetValue(obj);

                if (propertyValue is IEnumerable enumerable)
                {
                    int i = 0;
                    foreach (var objInList in enumerable)
                    {
                        sb.Append("".PadLeft(indent));
                        sb.AppendLine($"{propertyInfo.Name}[{i}]:");
                        WriteObjectDump(objInList, objInList.GetType(), 0, sb, indent + 4);
                        i++;
                    }
                }
                else
                {
                    sb.Append("".PadLeft(indent));
                    sb.AppendLine($"{propertyInfo.Name}: {propertyValue}");
                }
            }
        }
    }
}