using System.Linq;
using Microsoft.CodeAnalysis;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>Проверка реализации маркера по имени — учитывает и прямые, и транзитивные интерфейсы.</summary>
    internal static class SymbolExtensions
    {
        public static bool Implements(this INamedTypeSymbol type, string interfaceName)
            => type.Interfaces.Any(i => i.Name == interfaceName)
               || type.AllInterfaces.Any(i => i.Name == interfaceName);
    }
}
