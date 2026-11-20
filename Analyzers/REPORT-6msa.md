# DtfScan — первый прогон по 6-msa (2026-11-16)

Прогон `DtfScan` (Roslyn-анализаторы DigiTFactory) по `XDiary/6-msa`.

| Показатель | Значение |
|---|---|
| Файлов `.cs` (без bin/obj) | 502 |
| Нарушений всего | 156 |
| Гейтов «чисто» | `QG401` (POST-only) — 0, `QG602` (нет `IAggregate<T>`) — 0 |

## Сводка по правилам

| Gate | Нарушений | Что это значит |
|---|---|---|
| `QG006` (публичные сеттеры в `Implementation/`) | **145** | системный долг: анемичные модели и корни мутабельны |
| `QG603` (`NotImplementedException`) | **10** | все — в `*ControllerIsolationTests.cs` (заглушки в тестах) |
| `QG106` (своя инфраструктура) | **1** | `Shared/XDiary.Contracts/IRepository.cs` — «последний самописный интерфейс» (BE-056/E3) |
| `QG401` (`[HttpPut]`/`[HttpDelete]`) | 0 | чисто |
| `QG602` (`IAggregate<T>`/`Aggregate<T>`) | 0 | чисто |

## Главная находка: мутабельные анемичные модели (QG-006, 145)

Фреймворк уже даёт **иммутабельный** базовый класс `AnemicModel<TBoundedContext>`
(`SeedWorks/TacticalPatterns/AnemicModel.cs`: только `readonly`-поля `_id`/`_version`/
`_correlationToken`/`_invariants`, ни одного сеттера). А в `6-msa` каждый BC вручную
пишет **свой** плоский `AnemicModel` с `public ... { get; set; }` на каждое поле:

```csharp
// BC-Journal/src/Journal.Domain/Implementation/AnemicModel.cs
public sealed class AnemicModel : IJournalAnemicModel   // ← не наследует AnemicModel<T>
{
    public Guid Id { get; set; } = Guid.NewGuid();      // ← мутабельно
    public IRecordRoot Root { get; set; } = null!;
    public string Place { get; set; } = string.Empty;
    ...
}
```

Это нарушает одновременно **QG-006** (агрегат/корень) и **QG-011** (анемичная модель
иммутабельна). Тот же паттерн — во всех 10 BC (`AppleHealthGateway`, `Catalog`, `Client`,
`DailyMetrics`, `GarminGateway`, `Journal`, `Partners`, `Portability`, `Privacy`,
`SomaticSeries`).

**Что это доказывает для DR:** анализатор ловит не «косметику», а системное отклонение
от метода, которое в текстовом аудите (`edm-verification.md`) было зафиксировано как
пункт «8. Мутабельные anemic-модели» — но тогда без машины его было видно только точечно.

## Оговорка к QG-603

10 вхождений — только в тестовых файлах `*IntegrationTests/*ControllerIsolationTests.cs`
(`throw new NotImplementedException()` как заглушка теста). Правило QG-603 формально
«grep по всему решению», но дух — «нет заглушек в **продуктовом** коде». Для точности
анализатору нужен фильтр «исключить `*.Tests` проекты», либо заглушки в тестах тоже
чинятся отдельной задачей.

## Следующие шаги

1. Подключить `DigiTFactory.Libraries.Analyzers` в `Directory.Build.props` одного BC +
   `WarningsAsErrors` по `QG*` — гейт «краснеет» прямо в `dotnet build`.
2. Закрыть `QG006`: перевести `AnemicModel` на базовый `AnemicModel<TBoundedContext>`
   (задача @BE, эталон — фреймворк).
3. Убрать `IRepository.cs` (QG-106) — закрывает BE-056/E3.
4. Решить судьбу `QG603` в тестах (исключить `*Tests` или доделать заглушки).
5. Упаковать анализатор в NuGet и добавить карту `QG ↔ анализатор` как метрику DR.
