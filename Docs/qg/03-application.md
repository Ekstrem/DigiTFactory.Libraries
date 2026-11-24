## 3. Слой приложения (`{Context}.Application`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-201 | Команды — MediatR `IRequest<TResult>` | grep `: IRequest<` | 🔴 | skill | применён |
| QG-202 | Обработчик: провайдер → бизнес-метод → маппинг результата | grep `_provider.GetAggregateAsync` + `aggregate.` | 🔴 | статья 7 | применён |
| QG-203 | Обработчик **не содержит** доменной логики (валидация/переходы — только в агрегате) | grep `if`/`Validator` в Handler → fail | 🔴 | статья 7 | применён |
| QG-204 | `IQueryHandler` читает **только** `IReadRepository` (Read Store), не Event Store/агрегат | grep `IReadRepository` в `IQueryHandler`; `IRepository.GetById`/агрегат → fail | 🔴 | CEQRS | применён |
| QG-205 | Response DTO поля 1:1 с колонками MV | сверка DTO ↔ ReadModel | 🟠 | канвас Query API | конвенция |
| QG-206 | Request DTO плоский, не повторяет доменные VO | нет `: IValueObject` в DTO | 🟠 | command-api ref | применён |
| QG-207 | Идемпотентность: команда несёт `CorrelationToken`, обработчик дедуплицирует через `GetByCorrelationToken` | grep `GetByCorrelationToken` | 🟠 | статья 10 (PACELC) | конвенция |
| QG-208 | Оптимистичная блокировка: `ExpectedVersion` + загрузка через `GetByIdAndVersion` | grep `GetByIdAndVersion` | 🟠 | статья 7 (версионирование) | конвенция |
| QG-209 | Владелец/actor берётся из авторизации (JWT `sub`), **не** из тела запроса | grep `clientId` в Request → fail | 🔴 | статья 7 + ADR | применён |
| QG-210 | `ICommandHandler` инжектит `IAggregateProvider` в конструктор (DI); исключение — `[CommandHandlerNoAggregate]` → warning | grep `IAggregateProvider` в ctor `ICommandHandler`; нет и без атрибута → error, с атрибутом → warning | 🔴 | статья 7 + владелец | применён |
| QG-211 | `IAggregateProvider` инжектится только `ICommandHandler` (кроме тестов) | grep `IAggregateProvider` в ctor вне `ICommandHandler` → fail | 🔴 | статья 7 + владелец | применён |
| QG-212 | `IQueryRepository` инжектится только `IQueryHandler` (кроме тестов) | grep `IQueryRepository` в ctor вне `IQueryHandler` → fail | 🔴 | статья 7 + владелец | применён |
| QG-213 | Handler не собирает AnemicModel/Root/State и не вызывает доменный builder; новое состояние создаёт бизнес-метод агрегата | анализатор QG213 | 🔴 | aggregate-first + отклонения генераций | применён |
