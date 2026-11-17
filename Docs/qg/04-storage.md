## 4. Хранилище (`{Context}.Storage`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-301 | Event Store — библиотечный `AddEventStorePostgres` (нет кастомного репозитория) | grep `AddEventStorePostgres`; grep `class .*Repository : IRepository` → fail | 🔴 | принцип #6 | применён |
| QG-302 | Read Store — библиотечный `AddReadStorePostgres` | grep `AddReadStorePostgres` | 🔴 | принцип #6 | применён |
| QG-303 | Read-модель реализует `IReadModel<TBC>` | grep `: IReadModel<` | 🔴 | SeedWorks | применён |
| QG-304 | Проекция — `IDomainEventHandler<TBC>` + `IObserver<IDomainEvent<TBC>>` | grep `IDomainEventHandler` | 🔴 | skill | применён |
| QG-305 | Проекция **идемпотентна** (проверка `Version`) | grep `Version >=` в Projection | 🔴 | skill (idempotency) | применён |
| QG-306 | Проекция пишет через `IReadModelStore.UpsertAsync` / читает `IReadRepository.GetByIdAsync` | grep `UpsertAsync` | 🔴 | skill | применён |
| QG-307 | Rebuild — `IRebuildService<TBC>`, читает события из библиотечного `EventStoreDbContext` | grep `IRebuildService` + `EventStoreDbContext` | 🔴 | статья 10 (регидротация) | ожидает библиотеку |
| QG-308 | Есть JSON-путь проекции (`ProjectFromEntryAsync`) для replay из `DomainEventEntry` | grep `ProjectFromEntryAsync` | 🟠 | статья 10 | ожидает библиотеку |
| QG-309 | `$type`-дискриминатор (или иной полиморфный конвертер) для сериализации `IValueObject` | grep `JsonConverter`/`$type` | 🟠 | отклонение Chat/BC-Journal | ожидает библиотеку |
| QG-310 | Command DB и Read DB разделены (отдельные DbContext/схемы/БД) | наличие `CommandDbContext`/`EventStoreDbContext` + `ReadDbContext` | 🟠 | статья 10 (CQRS) |  |
