using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Testing
{
    /// <summary>
    /// QG-702: каждый *.Domain.Tests обязан вызывать AggregateCoverage.Verify и VerifyInvariants.
    /// Анализатор гарантирует наличие behavior-gate в CI; фактическое покрытие проверяется при dotnet test.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AggregateCoveragePresenceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG702";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Нет обязательной проверки покрытия агрегата",
            "Добавь AggregateCoverage.Verify<TAggregate> и AggregateCoverage.VerifyInvariants в {0}: каждый бизнес-метод и каждый инвариант должны иметь тест (QG-702)",
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
            if (!assemblyName.EndsWith(".Domain.Tests"))
            {
                return;
            }

            var hasVerify = false;
            var hasVerifyInvariants = false;

            foreach (var tree in ctx.Compilation.SyntaxTrees)
            {
                foreach (var invocation in tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    if (invocation.Expression is not MemberAccessExpressionSyntax member ||
                        member.Expression is not IdentifierNameSyntax owner ||
                        owner.Identifier.ValueText != "AggregateCoverage")
                    {
                        continue;
                    }

                    var methodName = member.Name switch
                    {
                        GenericNameSyntax generic => generic.Identifier.ValueText,
                        IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
                        _ => string.Empty,
                    };

                    hasVerify |= methodName == "Verify";
                    hasVerifyInvariants |= methodName == "VerifyInvariants";
                }
            }

            if (!hasVerify || !hasVerifyInvariants)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, Location.None, assemblyName));
            }
        }
    }
}
