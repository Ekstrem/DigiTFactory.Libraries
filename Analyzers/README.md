# DigiTFactory.Libraries.Analyzers

Roslyn-анализаторы, превращающие Quality Gates (`QG.md`) из текста в проверяемое условие.
Один анализатор — один `QG-NNN`; diagnostic ID = номер правила, severity = «северность»
(🔴 → Error, 🟠 → Warning, 🟡 → Info).

## Реализовано (v0.3)

| Diagnostic | Правило | Тип |
|---|---|---|
| `QG603` | нет `NotImplementedException` в продуктовом коде | syntax |
| `QG602` | нет metadata-driven `AbstractAggregate` + generic `Aggregate<TBC>` | syntax |
| `QG006` | нет публичных сеттеров в `Implementation/` | syntax + path |
| `QG106` | нет своих `IRepository`/`IReadRepository`/`IEventBus`/… (вне DigiTFactory) | symbol |
| `QG005` | агрегат: приватный ctor + статическая фабрика `Create`/`CreateInstance` | symbol |
| `QG011` | `AnemicModel` иммутабельна (без публичных сеттеров) | symbol |
| `QG015` | Value Object иммутабелен (без публичных сеттеров) | symbol |

## Сборка и прогон по 6-msa

```bash
dotnet build DigiTFactory.Libraries.Analyzers.Runner/DigiTFactory.Libraries.Analyzers.Runner.csproj
dotnet run --project DigiTFactory.Libraries.Analyzers.Runner -- <путь-к-6-msa> <путь-к-ref-pack>
```

Без аргументов `DtfScan` сканирует `XDiary/6-msa` и ref pack .NET 8.

## Подключение как гейта (следующий шаг)

1. Запаковать пакет: `dotnet pack` (анализатор уходит в `analyzers/dotnet/cs`).
2. В `Directory.Build.props` сервиса: `<PackageReference Include="DigiTFactory.Libraries.Analyzers" Version="0.3.0" PrivateAssets="all" />`.
3. В CI: `TreatWarningsAsErrors` по `QG*` (или `WarningsAsErrors=QG603;QG602;…`).

Severity каждого `QG-NNN` правится в `.editorconfig` — это точка, где «gap» объявляется
явно, а не молча отключается.

## Публикация

Пакет `DigiTFactory.Libraries.Analyzers` публикуется в GitHub Packages и NuGet.org через
**Trusted Publishing** (OIDC, без долгоживущего ключа `NUORG`). Для NuGet.org нужна политика
Trusted Publishing: `Ekstrem / DigiTFactory.Libraries / analyzers.yml`.
