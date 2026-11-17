using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>QG-105: RegisterDependencies (Autofac) регистрирует провайдера и наблюдателя.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class RegisterDependenciesAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG105";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "RegisterDependencies регистрирует провайдера и наблюдателя",
            "RegisterDependencies должен регистрировать и провайдера (RegisterType<...Provider>), и наблюдателя (IObserver<AggregateResult>)",
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
            if (cls.Identifier.ValueText != "RegisterDependencies")
            {
                return;
            }

            var ids = cls.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Select(id => id.Identifier.ValueText)
                .Distinct().ToList();

            var hasProvider = ids.Any(i => i.EndsWith("Provider"));
            var hasObserver = ids.Contains("IObserver");

            if (!(hasProvider && hasObserver))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation()));
            }
        }
    }
}
