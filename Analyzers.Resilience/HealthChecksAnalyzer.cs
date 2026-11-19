using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Resilience
{
    /// <summary>QG-614: ASP.NET-приложение должно регистрировать health-checks и мапить endpoint /hc.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class HealthChecksAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG614";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Health-checks обязательны",
            "ASP.NET-приложение должно регистрировать AddHealthChecks и мапить MapHealthChecks/UseHealthChecks (QG-614)",
            "DigiTFactory",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(OnCompilationStart);
        }

        private static void OnCompilationStart(CompilationStartAnalysisContext ctx)
        {
            var state = new HealthCheckState();
            ctx.RegisterSyntaxNodeAction(state.Analyze, SyntaxKind.InvocationExpression);
            ctx.RegisterCompilationEndAction(state.Report);
        }

        private sealed class HealthCheckState
        {
            public bool HasWebApp;
            public bool HasAddHealthChecks;
            public bool HasMapHealthChecks;
            public Location? WebAppLocation;

            public void Analyze(SyntaxNodeAnalysisContext ctx)
            {
                var inv = (InvocationExpressionSyntax)ctx.Node;
                var name = (inv.Expression as MemberAccessExpressionSyntax)?.Name.Identifier.ValueText;
                if (name == "CreateBuilder")
                {
                    HasWebApp = true;
                    WebAppLocation ??= inv.GetLocation();
                }
                else if (name == "AddHealthChecks")
                {
                    HasAddHealthChecks = true;
                }
                else if (name == "MapHealthChecks" || name == "UseHealthChecks")
                {
                    HasMapHealthChecks = true;
                }
            }

            public void Report(CompilationAnalysisContext ctx)
            {
                if (HasWebApp && (!HasAddHealthChecks || !HasMapHealthChecks) && WebAppLocation != null)
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(Rule, WebAppLocation));
                }
            }
        }
    }
}
