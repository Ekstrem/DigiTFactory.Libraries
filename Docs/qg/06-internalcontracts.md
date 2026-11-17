## 6. Внутренние контракты (`{Context}.InternalContracts`)

| ID | Правило | Проверка | Север. | Источник | Статус |
|---|---|---|---|---|---|
| QG-501 | Read-модели и контракты событий живут в InternalContracts | grep `IReadModel`/`ReadModel` в InternalContracts | 🟠 | Chat/BC-Journal |  |
| QG-502 | InternalContracts не зависит от Api/Application/Storage | csproj: нет этих ссылок | 🔴 | skill (граф) |  |
