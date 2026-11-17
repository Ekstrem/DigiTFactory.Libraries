## 5. API (`{Context}.Api`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-401 | Все мутирующие операции — **POST** (без PUT/DELETE) | grep `[HttpPut]`/`[HttpDelete]` → fail | 🔴 | статья 7 | спорный |
| QG-402 | Маршрут `/api/v1/{context}/{resource}/{id}/{action}` | grep `[Route("api/v1/` | 🟡 | Chat |  |
| QG-403 | Действие контроллера = один `IMediator.Send(command)` | grep `_mediator.Send` | 🔴 | skill |  |
| QG-404 | `Program.cs` использует библиотечные extensions (`AddEventStore*`, `AddEventBus*`, `AddReadStore*`) | grep `AddEventStore`/`AddEventBus`/`AddReadStore` | 🔴 | принцип #6 |  |
| QG-405 | Шина — библиотечная; **нет** MassTransit/RabbitMQ | grep `MassTransit` → fail | 🔴 | принцип #6 |  |
| QG-406 | Подписка проекции на шину: `IEventBus.Subscribe<TBC>(handler)` | grep `.Subscribe<` | 🔴 | skill |  |
| QG-407 | HTTP-статусы: `200` (Success/Warnings), `400` (Exception), `404`, `401` | сверка контроллера | 🟠 | command-api ref |  |
| QG-408 | JWT-авторизация + health check (`/health`) | grep `AddAuthentication(JwtBearer` + `AddHealthChecks` | 🟠 | BC-Journal |  |
| QG-409 | `DeploymentRole` условная регистрация (Command/Event/Query/All) | grep `DeploymentRole` | 🟠 | статья 10 |  |
