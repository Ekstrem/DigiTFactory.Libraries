using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>QG-401: мутирующие операции — только POST. [HttpPut]/[HttpDelete]/[HttpPatch] запрещены.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoPutDeleteAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG401";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Мутация только через POST",
            "Операция объявлена через '{0}' — по канону EDM мутирующие операции только POST",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeAttribute, SyntaxKind.Attribute);
        }

        private static void AnalyzeAttribute(SyntaxNodeAnalysisContext ctx)
        {
            var attr = (AttributeSyntax)ctx.Node;
            var name = attr.Name.ToString();
            if (name == "HttpPut" || name == "HttpDelete" || name == "HttpPatch")
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, attr.GetLocation(), name));
            }
        }
    }
}
