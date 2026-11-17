using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-201: команды/запросы — MediatR IRequest&lt;TResult&gt;.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CommandInterfaceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG201";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Команда/запрос реализует IRequest",
            "Тип '{0}' (имя *Command/*Query) должен реализовать MediatR IRequest<TResult>",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeType, SyntaxKind.ClassDeclaration, SyntaxKind.RecordDeclaration);
        }

        private static void AnalyzeType(SyntaxNodeAnalysisContext ctx)
        {
            var node = (TypeDeclarationSyntax)ctx.Node;
            var name = node.Identifier.ValueText;
            if (!(name.EndsWith("Command") || name.EndsWith("Query")))
            {
                return;
            }

            var hasIRequest = node.BaseList?.Types.Any(t => t.ToString().Contains("IRequest")) ?? false;
            if (!hasIRequest)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, node.Identifier.GetLocation(), name));
            }
        }
    }
}
