using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>QG-006: иммутабельная реализация домена — нет публичных сеттеров в Implementation/.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoPublicSetterAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG006";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Иммутабельность домена",
            "Публичный setter в реализации домена запрещён (QG-006)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
        }

        private static void AnalyzeProperty(SyntaxNodeAnalysisContext ctx)
        {
            var prop = (PropertyDeclarationSyntax)ctx.Node;

            if (!prop.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
            {
                return;
            }

            var hasSetter = prop.AccessorList != null &&
                            prop.AccessorList.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration));
            if (!hasSetter)
            {
                return;
            }

            var path = (prop.SyntaxTree.FilePath ?? string.Empty).Replace('\\', '/');
            if (path.Contains("/Implementation/"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, prop.Identifier.GetLocation()));
            }
        }
    }
}
