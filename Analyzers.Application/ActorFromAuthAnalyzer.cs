using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-209: владелец/actor — из авторизации (JWT sub), не из тела команды.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ActorFromAuthAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG209";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Владелец — из JWT sub, не из тела команды",
            "Команда '{0}' не должна нести ClientId — владелец/actor берётся из авторизации (JWT sub)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext ctx)
        {
            var type = (INamedTypeSymbol)ctx.Symbol;
            if (type.TypeKind != TypeKind.Class)
            {
                return;
            }

            if (!type.Name.EndsWith("Command"))
            {
                return;
            }

            var hasClientId = type.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == "ClientId");
            if (hasClientId)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
