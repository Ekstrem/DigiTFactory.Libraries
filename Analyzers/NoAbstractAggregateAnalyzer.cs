using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>QG-004/QG-602: агрегат — обычный класс, не наследует IAggregate&lt;T&gt;/Aggregate&lt;T&gt;.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoAbstractAggregateAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG602";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Абстрактный агрегат запрещён",
            "Тип наследует '{0}' — агрегат должен быть обычным классом (QG-004/QG-602)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeBaseType, SyntaxKind.SimpleBaseType);
        }

        private static void AnalyzeBaseType(SyntaxNodeAnalysisContext ctx)
        {
            var baseType = (SimpleBaseTypeSyntax)ctx.Node;
            var name = baseType.Type.ToString();
            if (name == "IAggregate" || name.StartsWith("IAggregate<") || name == "Aggregate" || name.StartsWith("Aggregate<"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, baseType.GetLocation(), name));
            }
        }
    }
}
