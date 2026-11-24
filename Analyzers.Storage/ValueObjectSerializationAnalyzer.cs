using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Storage
{
    /// <summary>
    /// QG-311: сериализация Value Objects должна идти через IValueObjectSerializer
    /// (ValueObjectSerializerFactory.Create), а не голый JsonSerializer.Serialize по интерфейсному типу.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ValueObjectSerializationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG311";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Сериализация VO не через фабрику",
            "Сериализуй Value Objects через ValueObjectSerializerFactory.Create + IValueObjectSerializer.Serialize, а не JsonSerializer.Serialize напрямую (QG-311): прямой вызов сериализует по интерфейсному типу и даёт {{}}",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext ctx)
        {
            if (ctx.Node.SyntaxTree.FilePath.Contains("Tests"))
            {
                return;
            }

            var invocation = (InvocationExpressionSyntax)ctx.Node;
            if (invocation.Expression is not MemberAccessExpressionSyntax member)
            {
                return;
            }

            if (member.Expression.ToString() != "JsonSerializer" || member.Name.Identifier.ValueText != "Serialize")
            {
                return;
            }

            var arguments = invocation.ArgumentList.Arguments;
            if (arguments.Count == 0)
            {
                return;
            }

            var payload = arguments[0].Expression.ToString();
            if (payload.Contains("ChangedValueObjects") || payload.Contains("ValueObjects") || payload.Contains("GetValueObjects"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, invocation.GetLocation()));
            }
        }
    }
}
