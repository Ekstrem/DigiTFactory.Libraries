## 4. Хранилище (`{Context}.Storage`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-301 | Event Store — библиотечный `AddEventStorePostgres` (нет кастомного репозитория) | grep `AddEventStorePostgres`; grep `class .*Repository : IRepository` → fail | 🔴 | принцип #6 | применён |
| QG-302 | Read Store — библиотечный `AddReadStorePostgres` | grep `AddReadStorePostgres` | 🔴 | принцип #6 | применён |
| QG-303 | Read-модель реализует `IReadModel<TBC>` | grep `: IReadModel<` | 🔴 | SeedWorks | применён |
| QG-304 | Проекция — `IDomainEventHandler<TBC>` + `IObserver<IDomainEvent<TBC>>` | grep `IDomainEventHandler` | 🔴 | skill | применён |
| QG-305 | Проекция **идемпотентна**: принимает только более новую `Version`; повторное/старое событие не меняет MV | grep `Version` в Projection + integration test | 🔴 | skill (idempotency) | применён |
| QG-306 | Проекция пишет через `IReadModelStore.UpsertAsync` / читает `IReadRepository.GetByIdAsync` | grep `UpsertAsync` | 🔴 | skill | применён |
| QG-307 | Rebuild — `IRebuildService<TBC>`; integration test: incremental projection == rebuild из Full ES | `IRebuildService` + behavior test | 🔴 | статья 10 (регидротация) | порт готов |
| QG-308 | Есть JSON-путь проекции (`ProjectFromEntryAsync`) для replay из `DomainEventEntry` | grep `ProjectFromEntryAsync` | 🟠 | статья 10 | ожидает библиотеку |
| QG-309 | Типизированная сериализация `IValueObject` по runtime-типу (`$type`-дискриминатор) через `IValueObjectSerializer`; `{}` запрещён | `ValueObjectSerializerFactory` + round-trip тест | 🟠 | отклонение Chat/BC-Journal | применён |
| QG-310 | Command DB и Read DB разделены (отдельные DbContext/схемы/БД) | наличие `CommandDbContext`/`EventStoreDbContext` + `ReadDbContext` | 🟠 | статья 10 (CQRS) |  |
