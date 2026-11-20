using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.CrossCutting
{
    /// <summary>
    /// QG-004/QG-602: запрещён «абстрактный агрегат» — metadata-driven подход пакета
    /// AbstractAggregate (DynamicAggregateRoot, AbstractAggregateFactory, DynamicAnemicModel и т.п.)
    /// и generic-оболочка SeedWorks <c>Aggregate&lt;TBC&gt;</c>.
    /// НЕ запрещает SeedWorks <c>IAggregate&lt;T&gt;</c> — это штатный интерфейс микросервисного агрегата.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoAbstractAggregateAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG602";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Абстрактный (metadata-driven) агрегат запрещён",
            "Тип '{0}' — абстрактный агрегат; используй классический CEQRS (обычный класс + IAggregate<T>)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        // Имена типов пакета DigiTFactory.Libraries.AbstractAggregate (metadata-driven)
        private static readonly HashSet<string> MetadataDrivenTypes = new HashSet<string>
        {
            "AbstractAggregateFactory",
            "DynamicAggregateRoot",
            "DynamicAnemicModel",
            "DynamicValueObject",
            "DynamicBoundedContextScope",
            "DynamicCollectionValueObject",
            "MetadataBusinessOperation",
            "ModelMerger"
        };

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeBaseType, SyntaxKind.SimpleBaseType);
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
        }

        private static string RootName(string typeText)
        {
            var idx = typeText.IndexOf('<');
            return idx >= 0 ? typeText.Substring(0, idx) : typeText;
        }

        private static bool IsForbidden(string typeText)
        {
            var root = RootName(typeText);
            if (MetadataDrivenTypes.Contains(root))
            {
                return true;
            }

            // Generic-оболочка SeedWorks Aggregate<TBC>. Голый 'Aggregate' (обычный класс) не трогаем.
            return root == "Aggregate" && typeText.Contains("<");
        }

        private static void AnalyzeBaseType(SyntaxNodeAnalysisContext ctx)
        {
            var baseType = (SimpleBaseTypeSyntax)ctx.Node;
            var name = baseType.Type.ToString();
            if (IsForbidden(name))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, baseType.GetLocation(), name));
            }
        }

        private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext ctx)
        {
            var oce = (ObjectCreationExpressionSyntax)ctx.Node;
            var name = oce.Type.ToString();
            if (IsForbidden(name))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, oce.GetLocation(), name));
            }
        }
    }
}
