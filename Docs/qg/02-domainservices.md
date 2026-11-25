## 2. Доменные сервисы (`{Context}.DomainServices`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-101 | `AggregateProvider` загружает агрегат и **декорирует** его `Notifier` | grep `PipeTo(Aggregate.Create` + `PipeNotifierToContract` | 🔴 | статья 7 |  |
| QG-102 | При промахе провайдер собирает агрегат из `DefaultAnemicModel` (пустой модели) | grep `?? DefaultAnemicModel.Create` | 🔴 | статья 7 |  |
| QG-103 | `Notifier` — декоратор бизнес-методов: реализует интерфейс агрегата (`I{Context}Aggregate`/`IAggregate<T>`) и `IObservable<AggregateResult<...>>`, реактивно рассылает результат подписчикам | анализатор QG103 | 🔴 | статья 7 | применён |
| QG-104 | `BusAdapter` **только публикует** (`IEventBus.Publish`); сохранение события — в отдельном подписчике шины (`*EventStoreHandler`), а не в `BusAdapter` | grep `SaveEventAsync` в BusAdapter (должно отсутствовать) + `Publish` (должно быть) | 🔴 | статья 7 + ceqrs-data-flows | применён |
| QG-105 | `RegisterDependencies` (Autofac) регистрирует провайдера и наблюдателя | grep `RegisterType<.*Provider` + `IObserver<AggregateResult` | 🔴 | skill |  |
| QG-106 | Нет **самодельных** реализаций `IEventBus`/`IAnemicModelRepository`/`IReadRepository` | grep `class .* : IEventBus` / `: IAnemicModelRepository` → fail | 🔴 | принцип #6 | применён |
| QG-107 | DomainServices зависит только от Domain + InternalContracts (не Api/Application/Storage) | csproj: нет этих ссылок | 🔴 | skill (граф) |  |
| QG-108 | Нет «мёртвого» кода (невызываемые `SaveEventAsync`, `DomainCommandExecutor`, MassTransit-обёртки) | grep `NotImplementedException` / вызов не найден | 🟠 | отклонение Chat |  |
| QG-109 | `AggregateProvider` инжектит в конструктор только библиотечные зависимости (`IAnemicModelRepository`, `IEventBus`), не кастомные (`CommandDbContext`/`*Repository`) | grep `IAnemicModelRepository` в ctor + нет `CommandDbContext` | 🔴 | статья 7 | применён |
| QG-110 | Команда изменяет агрегат через `IAggregateProviderWithCompletion.GetAggregateWithCompletionAsync` + `await completion` (TCS-мост, долговечная доставка), а не через `GetAggregateAsync` без ожидания | анализатор QG110 (grep `GetAggregateAsync` вне Tests) | 🟠 | статья 7 + SeedWorks 0.7.0 | применён |
| QG-111 | Durable publish: `*EventStoreHandler` реализует `IAsyncDomainEventHandler<T>` и его `HandleAsync` ожидается до возврата `PublishAsync` (InMemoryEventBus await durable handlers); `completion` завершается только после durable-записи | `InMemoryEventBus` await `HandleAsync` + regression `*EventStoreDurableTests` | 🔴 | BE-145/148 + EventBus.InMemory 0.2.0 | применён |
