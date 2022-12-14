### 0. Domain introduction
Imagine that there are two concepts: `Transaction` and `Refund` that are related to each other.

What's more:
* each `Transaction` is unique
* each `Refund` related to a given `Transaction` is unique
* each `Transaction` has a specific amount associated with it
* each `Refund` has a specific amount associated with it
* `Transaction` might be a target of a request for refunding with a given amount
* each `Refund` for a given `Transaction` might be a rejected
* each `Refund` for a given `Transaction` might be a approved
* requested `Refund`'s amount and existing non-rejected `Refund`s **cannot** exceed `Transaction` amount
* rejected `Refund` cannot be approved
* rejected `Refund` cannot be rejected again
* approved `Refund` cannot be rejected
* approved `Refund` cannot be approved again

### 1. How to design an aggregate?
This repository aims to answer the question: *how to design an aggregate satisfying all the domain knowledge from [this](#0-domain-introduction) section?*

#### 1.1 Additional modeling questions
* based on the responsibilities described in [this](#0-domain-introduction) section - how would you name it?
* what operations it provides?
* how it can be used?
* what is forbidden?

It is just a suggestion with minimal implementation to show how it shapes the surroundings.
### 2. How to run locally

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

### 3. How to run using `docker-compose`
1. Build API using the following command:
```sh
sudo docker build -t lf/transaction-balance-api:latest -t lf/transaction-balance-api:v0.0.0 .
```
2. Run by executing:
```sh
sudo docker-compose up
```

### 4. How to play with API from `VSCode`
Install [Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) `VSCode` extension to send requests by using `api.rest`.

### 5. How to play with API with `Swagger`
Run API by using either [local setup](#1-how-to-run-locally) or [docker setup](#2-how-to-run-using-docker-compose) and go to
```sh
http://localhost:8080/api-docs
```

