using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-203: обработчик не содержит доменной логики (валидация/переходы — только в агрегате).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class HandlerNoDomainLogicAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG203";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Обработчик без доменной логики",
            "Обработчик '{0}' содержит доменную валидацию (*Validator) — валидация/переходы только в агрегате (IBusinessOperationValidator/Assertion)",
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
            if (!cls.Identifier.ValueText.EndsWith("Handler"))
            {
                return;
            }

            var hasValidator = cls.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Any(id => id.Identifier.ValueText.EndsWith("Validator"));

            if (hasValidator)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation(), cls.Identifier.ValueText));
            }
        }
    }
}
