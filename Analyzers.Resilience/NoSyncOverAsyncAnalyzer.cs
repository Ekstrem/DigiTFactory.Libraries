using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Resilience
{
    /// <summary>QG-612: sync-over-async запрещён — .Result/.Wait()/.GetAwaiter().GetResult() блокирует поток.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoSyncOverAsyncAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG612";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "sync-over-async запрещён",
            "Синхронное ожидание асинхронной операции (.Result/.Wait()/.GetAwaiter().GetResult()) блокирует поток. Используйте await (QG-612)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext ctx)
        {
            var mae = (MemberAccessExpressionSyntax)ctx.Node;
            if (mae.Name.Identifier.ValueText != "Result")
            {
                return;
            }

            var type = ctx.SemanticModel.GetTypeInfo(mae.Expression).Type;
            var typeName = type?.ToDisplayString() ?? string.Empty;
            if (typeName.StartsWith("System.Threading.Tasks.Task") ||
                typeName.StartsWith("System.Threading.Tasks.ValueTask"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, mae.GetLocation()));
            }
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext ctx)
        {
            var inv = (InvocationExpressionSyntax)ctx.Node;
            if (ctx.SemanticModel.GetSymbolInfo(inv).Symbol is not IMethodSymbol method)
            {
                return;
            }

            var containing = method.ContainingType?.ToDisplayString() ?? string.Empty;
            if ((method.Name == "GetResult" && containing.StartsWith("System.Runtime.CompilerServices.TaskAwaiter")) ||
                (method.Name == "Wait" && containing.StartsWith("System.Threading.Tasks.Task")))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, inv.GetLocation()));
            }
        }
    }
}
