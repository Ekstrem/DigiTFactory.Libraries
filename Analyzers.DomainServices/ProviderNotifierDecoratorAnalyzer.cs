using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>QG-101: AggregateProvider загружает агрегат и декорирует его Notifier'ом.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ProviderNotifierDecoratorAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG101";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "AggregateProvider декорирует агрегат Notifier'ом",
            "AggregateProvider должен декорировать загруженный агрегат Notifier'ом (PipeTo/PipeNotifierToContract)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClass(SyntaxNodeAnalysisContext ctx)
        {
            var cls = (ClassDeclarationSyntax)ctx.Node;
            if (cls.Identifier.ValueText != "AggregateProvider")
            {
                return;
            }

            var hasNotifier = cls.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Any(id => id.Identifier.ValueText is "Notifier" or "PipeNotifier" or "PipeNotifierToContract");

            if (!hasNotifier)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation()));
            }
        }
    }
}
