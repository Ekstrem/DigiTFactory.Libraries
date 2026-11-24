using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>
    /// QG-213: Command Handler не собирает AnemicModel/Root/State и не вызывает доменный builder.
    /// Он загружает агрегат и передаёт в бизнес-метод только входные VO/примитивы.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class HandlerModelBuildingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG213";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Handler собирает доменную модель",
            "Handler не должен создавать AnemicModel/Root/State или вызывать доменный builder; новое состояние создаёт бизнес-метод агрегата (QG-213)",
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
            if (ctx.Node.SyntaxTree.FilePath.Contains("Tests"))
            {
                return;
            }

            var cls = (ClassDeclarationSyntax)ctx.Node;
            if (!cls.Identifier.ValueText.EndsWith("Handler"))
            {
                return;
            }

            foreach (var invocation in cls.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                if (invocation.Expression is not MemberAccessExpressionSyntax member)
                {
                    continue;
                }

                var method = member.Name.Identifier.ValueText;
                var owner = member.Expression.ToString();

                var forbidden =
                    method == "WithRoot" ||
                    method == "WithState" ||
                    (method == "Build" && owner.IndexOf("builder", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((method == "Create" || method == "CreateInstance") && IsModelType(owner));

                if (forbidden)
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(Rule, invocation.GetLocation()));
                }
            }

            foreach (var creation in cls.DescendantNodes().OfType<ObjectCreationExpressionSyntax>())
            {
                if (IsModelType(creation.Type.ToString()))
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(Rule, creation.GetLocation()));
                }
            }
        }

        private static bool IsModelType(string typeName)
        {
            var name = typeName.Split('.').Last();
            return name.EndsWith("AnemicModel") || name.EndsWith("AggregateRoot") || name.EndsWith("Root");
        }
    }
}
