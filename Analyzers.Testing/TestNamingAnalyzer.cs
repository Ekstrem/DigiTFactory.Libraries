using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Testing
{
    /// <summary>
    /// QG-701: имя теста — Method_When_Should (минимум 2 подчёркивания), чтобы из имени
    /// был виден бизнес-метод и сценарий. Coverage-проверку (QG-702) делает библиотека
    /// DigiTFactory.Libraries.Testing рефлексией, а не анализатором.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class TestNamingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG701";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Имя теста не по конвенции",
            "Имя теста должно быть Method_When_Should (минимум 2 подчёркивания), чтобы показывать бизнес-метод и сценарий (QG-701)",
            "DigiTFactory",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeMethod(SyntaxNodeAnalysisContext ctx)
        {
            var method = (MethodDeclarationSyntax)ctx.Node;

            var isTest = method.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(a =>
                {
                    var name = a.Name.ToString();
                    return name == "Fact" || name == "Theory" ||
                           name.EndsWith(".Fact") || name.EndsWith(".Theory");
                });
            if (!isTest)
            {
                return;
            }

            var name = method.Identifier.ValueText;
            if (name.Count(c => c == '_') < 2)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, method.Identifier.GetLocation()));
            }
        }
    }
}
