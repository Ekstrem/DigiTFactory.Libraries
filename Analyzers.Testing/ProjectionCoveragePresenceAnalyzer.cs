using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Testing
{
    /// <summary>
    /// QG-707: *.IntegrationTests обязан вызывать ProjectionCoverage.VerifyRebuildEquivalence
    /// и VerifyIdempotency (поведенческие проверки проекций).
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ProjectionCoveragePresenceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG707";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Нет поведенческих проверок проекций",
            "Добавь в {0} вызовы ProjectionCoverage.VerifyRebuildEquivalence и VerifyIdempotency (QG-707): rebuild == incremental и повторная доставка не меняет MV",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(AnalyzeCompilation);
        }

        private static void AnalyzeCompilation(CompilationAnalysisContext ctx)
        {
            var assemblyName = ctx.Compilation.AssemblyName ?? string.Empty;
            if (!assemblyName.EndsWith(".IntegrationTests"))
            {
                return;
            }

            var hasRebuild = false;
            var hasIdempotency = false;

            foreach (var invocation in ctx.Compilation.SyntaxTrees
                .SelectMany(t => t.GetRoot().DescendantNodes())
                .OfType<InvocationExpressionSyntax>())
            {
                if (invocation.Expression is not MemberAccessExpressionSyntax member ||
                    member.Expression is not IdentifierNameSyntax owner ||
                    owner.Identifier.ValueText != "ProjectionCoverage")
                {
                    continue;
                }

                var method = member.Name.Identifier.ValueText;
                hasRebuild |= method == "VerifyRebuildEquivalence";
                hasIdempotency |= method == "VerifyIdempotency";
            }

            if (!hasRebuild || !hasIdempotency)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, Location.None, assemblyName));
            }
        }
    }
}
