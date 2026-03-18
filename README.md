[![SeedWorks](https://github.com/Ekstrem/DigiTFactory.Libraries/actions/workflows/seedworks.yml/badge.svg)](https://github.com/Ekstrem/DigiTFactory.Libraries/actions/workflows/seedworks.yml)
[![AbstractAggregate](https://github.com/Ekstrem/DigiTFactory.Libraries/actions/workflows/abstractaggregate.yml/badge.svg)](https://github.com/Ekstrem/DigiTFactory.Libraries/actions/workflows/abstractaggregate.yml)

# DigiTFactory.Libraries

Фреймворк быстрой разработки событийно-управляемых микросервисов, декомпозированных по субдомену. Построен на принципах **DDD**, **CQRS**, **Event Sourcing** и **гексагональной архитектуры**.

Цель — дать продуктовым командам готовую инфраструктуру, чтобы разработчики сосредоточились на моделировании предметной области, а не на технических деталях.

## Пакеты

Все пакеты опубликованы на [NuGet.org](https://www.nuget.org/profiles/Ekstrem) и [GitHub Packages](https://github.com/Ekstrem?tab=packages).

### Ядро

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.SeedWorks` | 0.5.0 | Тактические паттерны DDD: интерфейсы, монады, инварианты |
| `DigiTFactory.Libraries.AbstractAggregate` | 0.1.0 | Metadata-driven агрегаты из Aggregate Design Canvas |
| `DigiTFactory.Libraries.AbstractAggregate.Postgres` | 0.1.0 | PostgreSQL-хранилище метаданных агрегатов |

### Command Side (Event Store)

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.CommandRepository.Postgres` | 0.1.0 | Event Store на PostgreSQL (EF Core) |
| `DigiTFactory.Libraries.CommandRepository.Mongo` | 0.1.0 | Event Store на MongoDB |

### Event Bus

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.EventBus.InMemory` | 0.1.0 | In-memory шина для dev/тестов |
| `DigiTFactory.Libraries.EventBus.Kafka` | 0.1.0 | Apache Kafka для продакшена |
| `DigiTFactory.Libraries.EventBus.Postgres` | 0.1.0 | Outbox-паттерн на PostgreSQL |

### Query Side (Read Store)

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.ReadRepository.Postgres` | 0.1.0 | Проекции на PostgreSQL (EF Core) |
| `DigiTFactory.Libraries.ReadRepository.Redis` | 0.1.0 | Кэш проекций на Redis |
| `DigiTFactory.Libraries.ReadRepository.Scylla` | 0.1.0 | Высокопроизводительные проекции на ScyllaDB |

## Архитектура

Гексагональная архитектура с шиной доменных событий как центральным портом:

```
                          ┌─────────────────┐
                          │   Event Bus     │
                          │ (центральный    │
                          │     порт)       │
                          └──┬─────┬─────┬──┘
                             │     │     │
                    ┌────────┘     │     └────────┐
                    ▼              ▼              ▼
             Command DB      Read Store      Outbound
             (Event Store)   (Projections)   (другие сервисы)
```

### Потоки данных (CEQRS)

```
Command ──► Aggregate ──► Event Bus ──► Command DB
                                   ├──► Read Store
                                   └──► Outbound

Query ──► Read Store ──► Response
```

### Строение модели

```
Ограниченный контекст (IBoundedContext)
└── Агрегат (IAggregate<T>)
    ├── Анемичная модель (IAnemicModel<T>)
    │   ├── Корень агрегата (IAggregateRoot<T>) — версионированная сущность
    │   └── Объекты-значения (IValueObject) — иммутабельные данные
    ├── Границы области (IBoundedContextScope<T>)
    │   ├── Бизнес-операции (IAggregateBusinessOperation<T>)
    │   └── Валидаторы (IBusinessOperationValidator<T>)
    └── Доменные события (IDomainEvent<T>)
```

## SeedWorks — ключевые интерфейсы

```csharp
// Ограниченный контекст
public interface IBoundedContext { }

// Агрегат: бизнес-операции + валидаторы
public interface IAggregate<TBoundedContext>

// Анемичная модель: Id, Version, ValueObjects
public interface IAnemicModel<TBoundedContext> : IHasKey, IHasVersion, IHasValueObjects

// Бизнес-операция: принимает модель, возвращает результат
public interface IAggregateBusinessOperation<TBoundedContext>
{
    AggregateResult Handle(IAnemicModel model, CommandToAggregate command, IBoundedContextScope scope);
}

// Шина событий: публикация + подписка
public interface IEventBus : IEventBusProducer, IEventBusConsumer

// Валидатор: спецификация бизнес-правила
public interface IBusinessOperationValidator<TBoundedContext>
{
    bool IsSatisfiedBy(BusinessOperationData obj);
    string Reason { get; }
}
```

## AbstractAggregate — metadata-driven домен

Метаданные агрегата (Value Objects, операции, инварианты, переходы состояний) загружаются из конфигурации вместо хардкода. Основано на [Aggregate Design Canvas](https://github.com/ddd-crew/aggregate-design-canvas).

```csharp
// Метаданные агрегата
AggregateMetadata
├── ValueObjects        // Список VO с их свойствами
├── Operations          // Бизнес-операции с merge-стратегиями
│   └── Invariants      // DSL-правила: IsNewEntity, PropertyEquals, StateIs, ...
└── StateTransitions    // Допустимые переходы состояний
```

### Динамические типы

| Класс | Заменяет |
|-------|----------|
| `DynamicValueObject` | Типизированные Value Objects |
| `DynamicAnemicModel<T>` | Типизированную AnemicModel |
| `DynamicAggregateRoot<T>` | Типизированный AggregateRoot |

### DSL инвариантов

```
IsNewEntity                        — новый агрегат (Version == 0)
IsNotNewEntity                     — существующий агрегат
VOExists:Actor                     — Value Object "Actor" существует
PropertyEquals:Actor.Type:Operator — свойство VO равно значению
PropertyNotEquals:Actor.Type:Bot   — свойство VO НЕ равно значению
PropertyNotNull:Root.SessionId     — свойство не null
CollectionNotEmpty:Messages        — коллекция не пуста
StateIs:Active                     — текущее состояние агрегата
```

## Профили деплоймента

Микросервис собирается с разным набором адаптеров в зависимости от профиля:

| Профиль | Event Bus | Command Store | Read Store | Сценарий |
|---------|-----------|---------------|------------|----------|
| **Single** | InMemory | Postgres | Postgres | Dev, тесты |
| **BudgetCqrs** | Postgres Outbox | Postgres | Postgres | Бюджетный CQRS |
| **Cqrs** | Kafka | Postgres | Redis | Полный CQRS |
| **Ceqrs** | Kafka | Mongo | ScyllaDB | Полный CEQRS |

## Быстрый старт

### Подключение пакетов

```xml
<!-- Domain: только SeedWorks -->
<PackageReference Include="DigiTFactory.Libraries.SeedWorks" Version="0.5.0" />

<!-- Domain с metadata-driven агрегатами -->
<PackageReference Include="DigiTFactory.Libraries.AbstractAggregate" Version="0.1.0" />

<!-- Api: шина событий (выбрать один) -->
<PackageReference Include="DigiTFactory.Libraries.EventBus.InMemory" Version="0.1.0" />

<!-- Storage: Event Store + Read Store (выбрать по профилю) -->
<PackageReference Include="DigiTFactory.Libraries.CommandRepository.Postgres" Version="0.1.0" />
<PackageReference Include="DigiTFactory.Libraries.ReadRepository.Postgres" Version="0.1.0" />
```

### Граф зависимостей микросервиса

```
{Context}.Domain
    → SeedWorks

{Context}.DomainServices
    → SeedWorks
    → {Context}.Domain

{Context}.Storage
    → CommandRepository.Postgres (или .Mongo)
    → ReadRepository.Postgres (или .Redis / .Scylla)

{Context}.Api
    → EventBus.InMemory (или .Kafka / .Postgres)
    → {Context}.Application
```

## Структура репозитория

```
DigiTFactory.Libraries/
├── Source/                          # SeedWorks — ядро фреймворка
│   ├── TacticalPatterns/            #   IAggregate, IAnemicModel, IValueObject, ...
│   ├── Definition/                  #   IBoundedContext
│   ├── Events/                      #   IDomainEvent, IEventBus
│   ├── Invariants/                  #   IBusinessOperationValidator
│   ├── Monads/                      #   PipeTo, Do, Either, Result<T,E>
│   └── Characteristics/             #   IHasKey, IHasVersion, ...
├── AbstractAggregate/               # Metadata-driven агрегаты
│   ├── DynamicTypes/                #   DynamicValueObject, DynamicAnemicModel
│   ├── Metadata/                    #   AggregateMetadata, OperationMetadata
│   ├── Factory/                     #   AbstractAggregateFactory
│   ├── Operations/                  #   MetadataBusinessOperation, ModelMerger
│   ├── Invariants/Rules/            #   IsNewEntity, PropertyEquals, StateIs, ...
│   └── Cache/                       #   IMetadataCache, InMemoryMetadataCache
├── AbstractAggregate.Postgres/      # PostgreSQL-хранилище метаданных
├── CommandRepository.Postgres/      # Event Store на PostgreSQL
├── CommandRepository.Mongo/         # Event Store на MongoDB
├── EventBus.InMemory/               # In-memory шина
├── EventBus.Kafka/                  # Apache Kafka шина
├── EventBus.Postgres/               # Outbox-паттерн на PostgreSQL
├── ReadRepository.Postgres/         # Read Store на PostgreSQL
├── ReadRepository.Redis/            # Read Store на Redis
├── ReadRepository.Scylla/           # Read Store на ScyllaDB
└── Examples/Hive.Employee/          # Пример: агрегат Employee
```

## CI/CD

Каждый пакет имеет собственный GitHub Actions workflow. При push в `master` с изменениями в директории пакета автоматически запускается сборка и публикация на NuGet.org и GitHub Packages.

## Ключевые принципы

1. **Домен в центре.** Domain зависит только от SeedWorks — никакой инфраструктуры.
2. **Иммутабельность.** Никаких setter'ов. Изменения только через бизнес-операции.
3. **Событие как источник истины.** Шина событий — центральный порт. БД — адаптер.
4. **Трёхуровневая валидация.** Assertions → Validators → Success/Exception/Warnings.
5. **Plug & Play инфраструктура.** Сменить Postgres на Mongo или Redis на Scylla — замена одного пакета.

## Reference-проект

[AbstactMicroservice](https://github.com/Ekstrem/AbstactMicroservice) — полноценный микросервис на базе DigiTFactory.Libraries с metadata-driven доменом.

## Лицензия

[MIT](LICENSE) &copy; Evgenei Voitov
