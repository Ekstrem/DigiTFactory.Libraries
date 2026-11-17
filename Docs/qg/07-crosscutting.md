## 7. Сквозные правила (cross-cutting)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-601 | Корреляция имён по всей цепочке: команда → метод → событие → проекция → колонка MV → поле DTO | сверка канвасов/кода | 🔴 | принцип #4 |  |
| QG-602 | Нет metadata-driven `AbstractAggregate` (`DynamicAggregateRoot`/`AbstractAggregateFactory`/…) и generic `Aggregate<TBC>`; `IAggregate<TBC>` разрешён | grep `AbstractAggregateFactory\|DynamicAggregateRoot\|: Aggregate<` по всему решению → fail | 🔴 | BE-007 (без AbstractAggregate) | применён |
| QG-603 | Нет `NotImplementedException` в продуктовом коде (нет заглушек) | grep `NotImplementedException` → fail | 🔴 | отклонение Cart | применён |
| QG-604 | Сборка зелёная (`dotnet build`) + тесты зелёные (`dotnet test`) | CI-шаг | 🔴 | принцип #5 |  |
| QG-605 | Тесты домена: happy-path **каждого** метода + провал **каждого** инварианта + порядок (state machine) | покрытие в Domain.Tests | 🔴 | skill |  |
| QG-606 | Событие — источник истины: состояние восстанавливается из событий (ES), а не mutable-таблица | наличие event store + `GetById` replay | 🟠 | статья 10 |  |
| QG-607 | Агрегат — точка согласованности: инварианты только в агрегате, не в контроллере/хендлере | grep валидации вне Domain → fail | 🔴 | статья 10 |  |
| QG-608 | Нет дублей инфраструктурных интерфейсов (один `I{Context}AggregateProvider`, без `IRepository`+`IChatAggregateRepository`+…) | grep дублей интерфейсов | 🟡 | отклонение Chat |  |
