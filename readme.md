### 1. How to run locally

1. To prepare DB (it creates basic structure), run 
```sh
dotnet run --project ./infrastructure/Lf.Slack.Infrastructure.Setup/Lf.Slack.Infrastructure.Setup.csproj
``` 
- **Can be executed only once**
2. Run API:
```sh
dotnet run --project ./Lf.Slack.TransactionBalance.Api/Lf.Slack.TransactionBalance.Api.csproj
```

Use your favorite tool for sending requests to API. If you want to use `VSCode`, please check [this](#1-how-to-run) section.

### 2. How to run using `docker-compose`
1. Build API using the following command:
```sh
sudo docker build -t lf/transaction-balance-api:latest -t lf/transaction-balance-api:v0.0.0 .
```
2. Run by executing:
```sh
sudo docker-compose up
```

### 3. How to play with API from `VSCode`
Install [Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) `VSCode` extension to send requests by using `api.rest`.

### 4. How to play with API with `Swagger`
Run API by using either [local setup](#1-how-to-run-locally) or [docker setup](#2-how-to-run-using-docker-compose) and go to
```sh
http://localhost:8080/api-docs
```

