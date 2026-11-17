## 1. Домен (`{Context}.Domain`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-001 | Domain зависит **только** от `DigiTFactory.Libraries.SeedWorks` | в csproj нет других `ProjectReference` и пакетов | 🔴 | принцип #1 | применён |
| QG-002 | Есть маркер контекста `I{Context} : IBoundedContext` | grep `interface I.* : IBoundedContext` | 🔴 | статья 7 |  |
| QG-003 | Есть `{Context}BoundedContextDescription : IBoundedContextDescription` с `ContextName`/`MicroserviceVersion` | grep `IBoundedContextDescription` | 🔴 | SeedWorks |  |
| QG-004 | Агрегат — **обычный класс**, реализующий `IAggregate<TBC>` (SeedWorks); без наследования metadata-driven `AbstractAggregate` (`DynamicAggregateRoot`/`AbstractAggregateFactory`) | grep `AbstractAggregateFactory\|DynamicAggregateRoot\|: Aggregate<` в Domain → fail | 🔴 | BE-007 (без AbstractAggregate) |  |
| QG-005 | У агрегата приватный конструктор + статическая фабрика `Create`/`CreateInstance` | grep `private Aggregate(` + `static .* Create` | 🔴 | статья 7 | применён |
| QG-006 | У агрегата **нет** публичных сеттеров/изменяемых полей | grep `public .* set` в Implementation → fail | 🔴 | статья 7 (иммутабельность) | применён |
| QG-007 | Бизнес-метод возвращает `AggregateResult<TBC, TAM>` | grep `AggregateResult<` в сигнатурах | 🔴 | статья 7 | применён |
| QG-008 | Бизнес-метод **не мутирует** состояние — порождает новую `AnemicModel` | grep `= model.X` / мутация в методе → fail | 🔴 | статья 7, вар. 3 |  |
| QG-009 | Результат несёт **старую и новую** анемичную модель (`BusinessOperationData.Commit(old, new)`) | grep `BusinessOperationData.*Commit` | 🔴 | статья 7 |  |
| QG-010 | `I{Context}AnemicModel : IAnemicModel<I{Context}>` | grep `: IAnemicModel<` | 🔴 | SeedWorks | применён |
| QG-011 | `AnemicModel` иммутабельна (readonly-поля, без сеттеров) | grep `set;` в AnemicModel → fail | 🔴 | статья 7 | применён |
| QG-012 | Есть `DefaultAnemicModel` (пустая) с `Create(Guid id)` | grep `class DefaultAnemicModel` | 🔴 | статья 7 | применён |
| QG-013 | Корень агрегата реализует `IAggregateRoot<TBC>` | grep `: IAggregateRoot<` | 🔴 | SeedWorks | применён |
| QG-014 | Каждый VO реализует `IValueObject` | grep `: IValueObject` | 🔴 | SeedWorks | применён |
| QG-015 | VO: приватный конструктор + фабрика `CreateInstance`, без сеттеров | grep `private .*(` + `CreateInstance` | 🔴 | статья 7 | применён |
| QG-016 | Инварианты реализованы как `IBusinessOperationValidator`/`IBusinessOperationAssertion`, **не** ad-hoc `IsSatisfiedBy(int)` | grep `IBusinessOperationValidator` | 🔴 | принцип #5 | применён |
| QG-017 | Есть слой **Assertions** (предусловия) отдельно от **Validators** (бизнес-правила) | наличие `Assertions/` + `Specifications/` | 🟠 | принцип #5 |  |
| QG-018 | Assertions → `DomainOperationResultEnum.Exception`; Validators → `WithWarnings` | grep `DomainResult =>` в классах | 🟠 | принцип #5 |  |
| QG-019 | State machine явно промоделирован и **охраняется** (переходы через assertion/validator) | есть `RecordState`/enum + валидаторы переходов | 🔴 | статья 7; отклонение BC-Journal |  |
| QG-020 | Реализация модели — `internal` + `InternalsVisibleTo` (наружу — только абстракции) | grep `public class Aggregate` → fail; есть AssemblyInfo | 🟠 | статья 7 | применён |
| QG-021 | Событие порождается **на каждый** бизнес-метод, имя коррелирует с методом | таблица метод→событие 1:1 | 🔴 | канвас агрегата §7 |  |
| QG-022 | Namespace: `{Context}.Domain` / `.Abstraction` / `.Implementation` / `.Specifications` | grep `namespace {Context}.Domain` | 🟡 | Chat/BC-Journal |  |
