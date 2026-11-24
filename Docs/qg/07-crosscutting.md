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
| QG-609 | `HttpClient` создаётся только через `IHttpClientFactory`/`AddHttpClient`, не `new HttpClient()` | анализатор QG609 | 🔴 | статья «IHttpClientFactory» | применён |
| QG-610 | EF-подключение устойчиво: `UseNpgsql`/`UseSqlServer` с `EnableRetryOnFailure` | анализатор QG610 | 🔴 | статья «EF SQL connections» | применён |
| QG-611 | `AddHttpClient` несёт политику отказоустойчивости (retry/circuit-breaker) | анализатор QG611 | 🟠 | статья «Polly» | применён |
| QG-612 | Нет sync-over-async (`.Result`/`.Wait()`/`.GetAwaiter().GetResult()`) | анализатор QG612 | 🔴 | статья «partial failure» | применён |
| QG-613 | Нет самописного retry (цикл + `Sleep`/`Delay`) — только Polly | анализатор QG613 | 🟠 | статья «retry+backoff» | применён |
| QG-614 | Health-checks: `AddHealthChecks` + `MapHealthChecks`/`UseHealthChecks` | анализатор QG614 | 🟠 | статья «monitor app health» | применён |
| QG-701 | Имя теста — `Method_When_Should` (виден бизнес-метод и сценарий) | анализатор QG701 | 🟡 | отклонение Journal | применён |
| QG-702 | Каждый бизнес-метод агрегата и каждый инвариант покрыт тестом (happy + отказ); `*.Domain.Tests` обязан содержать оба coverage-gate | `AggregateCoverage.Verify<T>()` + `VerifyInvariants()` + анализатор QG702 | 🔴 | отклонение Journal | применён |
