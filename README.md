[![SeedWorks](https://github.com/Ekstrem/DigiTFactory.Libraries/actions/workflows/seedworks.yml/badge.svg)](https://github.com/Ekstrem/DigiTFactory.Libraries/actions/workflows/seedworks.yml)

# DigiTFactory.Libraries

![](https://repository-images.githubusercontent.com/214590577/b4967dcf-97e8-4ba2-a4df-d6f59b96899a)

Фреймворк быстрой разработки событийно-управляемых микросервисов, декомпозированных по субдомену. Построен на принципах **DDD**, **CQRS**, **Event Sourcing** и **гексагональной архитектуры**.

Цель — дать продуктовым командам готовую инфраструктуру, чтобы разработчики сосредоточились на моделировании предметной области, а не на технических деталях.

## Пакеты

Все пакеты опубликованы на [NuGet.org](https://www.nuget.org/profiles/Ekstrem) и [GitHub Packages](https://github.com/Ekstrem?tab=packages).

### Ядро

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.SeedWorks` | 0.8.0 | Тактические паттерны DDD: интерфейсы, монады, инварианты |

### Command Side (Event Store)

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.CommandRepository.Postgres` | 0.2.2 | Event Store на PostgreSQL (EF Core) |
| `DigiTFactory.Libraries.CommandRepository.Mongo` | 0.1.1 | Event Store на MongoDB |

### Event Bus

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.EventBus.InMemory` | 0.2.0 | In-memory шина для dev/тестов |
| `DigiTFactory.Libraries.EventBus.Kafka` | 0.1.1 | Apache Kafka для продакшена |
| `DigiTFactory.Libraries.EventBus.Postgres` | 0.1.1 | Outbox-паттерн на PostgreSQL |

### Query Side (Read Store)

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.ReadRepository.Postgres` | 0.1.2 | Проекции на PostgreSQL (EF Core) |
| `DigiTFactory.Libraries.ReadRepository.Redis` | 0.1.1 | Кэш проекций на Redis |
| `DigiTFactory.Libraries.ReadRepository.Scylla` | 0.1.1 | Высокопроизводительные проекции на ScyllaDB |

### Качество (Roslyn-анализаторы, QG-метод)

Анализаторы превращают метод EDM в проверки компилятора. Подключать с `PrivateAssets="all"` и настраивать severity в `.editorconfig` — иначе правило «не проверить локально» не работает.

| Пакет | Версия | Правила |
|-------|--------|---------|
| `DigiTFactory.Libraries.Analyzers.Domain` | 0.6.2 | QG-001..022 |
| `DigiTFactory.Libraries.Analyzers.DomainServices` | 0.6.3 | QG-101..110 |
| `DigiTFactory.Libraries.Analyzers.Application` | 0.1.7 | QG-201..212 |
| `DigiTFactory.Libraries.Analyzers.Storage` | 0.1.2 | QG-301..306 |
| `DigiTFactory.Libraries.Analyzers.CrossCutting` | 0.4.1 | QG-602/603 |
| `DigiTFactory.Libraries.Analyzers.Resilience` | 0.1.1 | QG-609..614 |
| `DigiTFactory.Libraries.Analyzers.Testing` | 0.1.1 | QG-701 |

### Отказоустойчивость и тестирование

| Пакет | Версия | Описание |
|-------|--------|----------|
| `DigiTFactory.Libraries.CrossCutting` | 0.1.1 | Polly-политики (retry+jitter, circuit-breaker, timeout), resilient HttpClient, health-checks |
| `DigiTFactory.Libraries.Testing` | 0.2.1 | Coverage-assertion: покрытие бизнес-методов агрегата и инвариантов тестами |

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

### Доставка событий (подход)

События распространяются через **долговечный лог** (`EventBus.Kafka`) — он и есть источник
истины. `EventBus.Postgres` (outbox) — только **dev/Single-фолбэк**, когда Kafka недоступна;
это не основной паттерн.

Нотификатор делает **одну** операцию — публикует событие в лог — и **не ждёт**. Event Store и
материализованное представление — **асинхронные консьюмеры** лога: читают событие, применяют
идемпотентно по `Version`, отставшую проекцию пере-выводят через `IRebuildService`.
«Сохранить» и «опубликовать» — это НЕ два шага (save + publish), а один: запись в лог.

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
<PackageReference Include="DigiTFactory.Libraries.SeedWorks" Version="0.8.0" />

<!-- Api: шина событий (выбрать один) -->
<PackageReference Include="DigiTFactory.Libraries.EventBus.InMemory" Version="0.2.0" />

<!-- Storage: Event Store + Read Store (выбрать по профилю) -->
<PackageReference Include="DigiTFactory.Libraries.CommandRepository.Postgres" Version="0.2.2" />
<PackageReference Include="DigiTFactory.Libraries.ReadRepository.Postgres" Version="0.1.2" />

<!-- Отказоустойчивость: Polly + resilient HttpClient + health-checks -->
<PackageReference Include="DigiTFactory.Libraries.CrossCutting" Version="0.1.1" />

<!-- Тесты: coverage-assertion (в тест-проект) -->
<PackageReference Include="DigiTFactory.Libraries.Testing" Version="0.2.1" />

<!-- Качество: анализаторы QG-метода (в каждый проект, PrivateAssets=all) -->
<PackageReference Include="DigiTFactory.Libraries.Analyzers.Domain" Version="0.6.2" PrivateAssets="all" />
<PackageReference Include="DigiTFactory.Libraries.Analyzers.DomainServices" Version="0.6.3" PrivateAssets="all" />
<PackageReference Include="DigiTFactory.Libraries.Analyzers.Application" Version="0.1.7" PrivateAssets="all" />
<PackageReference Include="DigiTFactory.Libraries.Analyzers.Storage" Version="0.1.2" PrivateAssets="all" />
<PackageReference Include="DigiTFactory.Libraries.Analyzers.CrossCutting" Version="0.4.1" PrivateAssets="all" />
<PackageReference Include="DigiTFactory.Libraries.Analyzers.Resilience" Version="0.1.1" PrivateAssets="all" />
<PackageReference Include="DigiTFactory.Libraries.Analyzers.Testing" Version="0.1.1" PrivateAssets="all" />
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

## Лицензия

[MIT](LICENSE) &copy; Evgenei Voitov
