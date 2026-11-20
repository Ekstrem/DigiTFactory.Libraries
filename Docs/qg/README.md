# Quality Gates — метод EDM (DigiTFactory)

> Набор проверяемых правил метода событийно-управляемого микросервиса на `.NET / DigiTFactory.Libraries`.
> Сформулировано из сравнения реализаций: `Chat`, `Cart` (EShop), `Catalog`, `BC-Journal`, `Journal` —
> против статей «Тактические паттерны» (7) и «От моделей до EDM» (10), принципов skill `edm-net`
> и фактического API `SeedWorks` (актуальная версия с nuget.org).
>
> **Назначение QG** — превратить «метод» из описания в автоматически проверяемое условие:
> каждая проверка должна выражаться grep-паттерном / правилом csproj / тестом / AST-правилом и
> висеть в CI как гейт. Нарушение = блок сборки.

## Как читать

- **ID** — уникальный номер `QG-NNN`.
- **Северность**: 🔴 обязательно (блокирующее), 🟠 желательно, 🟡 опционально.
- **Проверка** — как механически проверить (grep / csproj / тест).
- **Источник** — статья (7/10), принцип skill (#N), SeedWorks, или конкретное отклонение версии.

## Слои (отдельные файлы)

- [1. Домен](01-domain.md) — QG-001..022
- [2. Доменные сервисы](02-domainservices.md) — QG-101..109
- [3. Слой приложения](03-application.md) — QG-201..212
- [4. Хранилище](04-storage.md) — QG-301..310
- [5. API](05-api.md) — QG-401..409
- [6. Внутренние контракты](06-internalcontracts.md) — QG-501..502
- [7. Сквозные правила](07-crosscutting.md) — QG-601..608

## Как это запускать как гейт (пример)

```bash
# 1. Зависимости домена — только SeedWorks
grep -R "ProjectReference" src/{Context}.Domain/*.csproj          # не должно быть ничего кроме SeedWorks-пакета

# 2. Нет абстрактного агрегата
grep -R ": IAggregate<" src/                                     # пусто

# 3. Нет самодельной инфраструктуры
grep -R ": IAnemicModelRepository" src/                          # пусто (интерфейс библиотеки — ок, реализация — нет)
grep -R ": IEventBus" src/                                       # пусто
grep -R "class .*Repository : IRepository" src/                  # пусто

# 4. Нет заглушек
grep -R "NotImplementedException" src/                           # пусто

# 5. Нет PUT/DELETE
grep -R "\[HttpPut\]\|\[HttpDelete\]" src/                       # пусто

# 6. Библиотечная инфраструктура зарегистрирована
grep -R "AddEventStorePostgres\|AddEventBus\|AddReadStorePostgres" src/{Context}.Api/Program.cs

# 7. Сборка и тесты
dotnet build {Context}.sln && dotnet test {Context}.sln
```

## Легенда источников

- **статья 7** — «Тактические паттерны» (habr.com/ru/articles/521884).
- **статья 10** — «От моделей до EDM» (habr.com/ru/articles/781062).
- **принцип #N** — ключевые принципы skill `edm-net` (1–7).
- **SeedWorks** — фактический API `DigiTFactory.Libraries.SeedWorks`.
- **отклонение X** — конкретный дефект/упрощение, найденный в Chat / Cart / Catalog / BC-Journal / Journal.

## Примечания (уточнения после внедрения)

- **QG-012** — `DefaultAnemicModel`/`Create(Guid id)` это **начальное (пустое) состояние агрегата**. Полный паттерн: `AggregateProvider` при отсутствии записи в репозитории возвращает `DefaultAggregate`, построенный на `DefaultAnemicModel.Create(id)` — нужен для сложных случаев (новый агрегат, восстановление, интеграции).
- **QG-013** — `IAggregateRoot<TBC> : IValueObject` это **маркер корня** агрегата, а не «просто VO».
- **QG-014** — повтор с компилятором осознанный: компилятор ловит пропуск `IValueObject` в точке использования, QG-014 — в точке объявления (fail-fast для агента).
- **QG-004/QG-602** — запрещён metadata-driven `AbstractAggregate`; `IAggregate<T>` SeedWorks — законен.
- **QG-401** — спорный: внедрено, но мотивация «иммутабельность через POST в REST» неверна.
- **QG-003** — `BoundedContextDescription` имеет смысл при нескольких агрегатах на микросервис; как Roslyn-гейт не внедрён (проверка «существования»).
- **QG-301/302** — хранение агрегатов/событий в **JSON** (единый репозиторий, без лишних проецирований); кастомный колоночный `CommandDbContext` и ручной `*ReadRepository` — долг к миграции.
- **QG-305/309** — `Version` и имя контекста (`$type`) — не «просто поля», а ключ **миграции** и полиморфной десериализации.
- **QG-302** — репозиторий материализованных представлений — **платформенный** (`IReadModelStore`/`AddReadStorePostgres`) с точками расширения, не своя реализация.
- **QG-307/308/309 — «ожидает библиотеку»** (владелец 2026-11-17): интерфейс `IRebuildService` есть, но нет реализации; `ProjectFromEntryAsync` (JSON-replay) и `$type`-дискриминатор для полиморфной сериализации `IValueObject` в библиотеках DTF отсутствуют. Внедрять гейт раньше инструмента — значит гнать BE на нереализованное.
- **IAnemicModelRepository vs IEventStoreRepository** (владелец 2026-11-17): первый хранит **агрегат** (снапшот/стрим — экономия диска), второй — **доменные события**. Выбор от домена: снапшот / события / комбинация (снапшот раз в N событий + replay свежих до актуального состояния). Нужны оба интерфейса; стратегия «снапшот + события» — техдолг библиотеки CommandRepository.Postgres.
- **Notifier → IEventBus; IEventStoreRepository — подписчик (профиль BudgetCqrs)** (владелец 2026-11-17): нотифаер **только публикует** `IDomainEvent` в `IEventBus`; дальше — реакция подписчиков (как в Kafka: продюсер → брокер → консьюмеры). `IEventStoreRepository` — подписчик, который **сохраняет состояние события** (`SaveEventAsync`, persistence), а не параллельный шаг; проекция MV — другой подписчик (`UpsertAsync`). Это вариант **BudgetCqrs**: `EventBus.InMemory` эмулирует приём из Kafka в процессе (чистая диспетчеризация `Publish → OnNext`, сам не сохраняет); на профиле **Cqrs** вместо InMemory регистрируется `EventBus.Kafka`. Уточняет QG-104 (формулировку «и сохраняет, и публикует» — правьте в методе).
- **ICommandHandler/IQueryHandler — маркеры** (владелец 2026-11-17): в SeedWorks добавлены маркер-интерфейсы `ICommandHandler` (мутация через агрегат; QG-210 требует `IAggregateProvider` в ctor) и `IQueryHandler` (чтение из Read Store; QG-204). Исключение для командного хендлера без агрегата — `[CommandHandlerNoAggregate]` → QG-210W warning.
- **IQueryableReadRepository** (владелец 2026-11-17): SeedWorks — `IQueryableReadRepository<TBC, TReadModel> : IReadRepository<TBC, TReadModel>` добавляет `GetQueryable()` для хранилищ с IQueryable (Postgres/EF Core); Redis/Scylla реализуют только `IReadRepository`. Специфичный поиск (`GetByPhone`) = `GetQueryable().Where(l => l.Phone == value).FirstOrDefault()`, без кастомных `I{Context}QueryRepository`.
- **QG-205/207/208 — конвенции** (владелец 2026-11-17): не внедрены как Roslyn-анализаторы — это дисциплина (сверка полей DTO↔ReadModel, идемпотентность через CorrelationToken, оптимистичная блокировка ExpectedVersion), а не синтаксис. Фиксируются ревью/канвасом.
