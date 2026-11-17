## 2. Доменные сервисы (`{Context}.DomainServices`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-101 | `AggregateProvider` загружает агрегат и **декорирует** его `Notifier` | grep `PipeTo(Aggregate.Create` + `PipeNotifierToContract` | 🔴 | статья 7 |  |
| QG-102 | При промахе провайдер собирает агрегат из `DefaultAnemicModel` (пустой модели) | grep `?? DefaultAnemicModel.Create` | 🔴 | статья 7 |  |
| QG-103 | `Notifier` — декоратор бизнес-методов, реактивно рассылает результат подписчикам | grep `class Notifier.*I{Context}Aggregate` | 🔴 | статья 7 |  |
| QG-104 | `BusAdapter` **и** сохраняет событие (`IEventStoreRepository.SaveEventAsync`), **и** публикует (`IEventBus.Publish`) | grep `SaveEventAsync` + `Publish` в BusAdapter | 🔴 | ceqrs-data-flows |  |
| QG-105 | `RegisterDependencies` (Autofac) регистрирует провайдера и наблюдателя | grep `RegisterType<.*Provider` + `IObserver<AggregateResult` | 🔴 | skill |  |
| QG-106 | Нет **самодельных** реализаций `IEventBus`/`IAnemicModelRepository`/`IReadRepository` | grep `class .* : IEventBus` / `: IAnemicModelRepository` → fail | 🔴 | принцип #6 | применён |
| QG-107 | DomainServices зависит только от Domain + InternalContracts (не Api/Application/Storage) | csproj: нет этих ссылок | 🔴 | skill (граф) |  |
| QG-108 | Нет «мёртвого» кода (невызываемые `SaveEventAsync`, `DomainCommandExecutor`, MassTransit-обёртки) | grep `NotImplementedException` / вызов не найден | 🟠 | отклонение Chat |  |
| QG-109 | `AggregateProvider` инжектит в конструктор только библиотечные зависимости (`IAnemicModelRepository`, `IEventBus`), не кастомные (`CommandDbContext`/`*Repository`) | grep `IAnemicModelRepository` в ctor + нет `CommandDbContext` | 🔴 | статья 7 | применён |
