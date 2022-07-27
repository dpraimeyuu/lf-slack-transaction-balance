
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ./Lf.Slack.TransactionBalance.Api/Lf.Slack.TransactionBalance.Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build Lf.Slack.TransactionBalance.sln -c Release

FROM build AS publish
RUN dotnet run --project ./infrastructure/Lf.Slack.Infrastructure.Setup/Lf.Slack.Infrastructure.Setup.csproj
RUN dotnet publish ./Lf.Slack.TransactionBalance.Api/Lf.Slack.TransactionBalance.Api.csproj -c Release -o /publish
COPY ./infrastructure/lfslack.db /publish/lfslack.db


FROM base AS final
WORKDIR /app
COPY --from=publish /publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings:LF_SLACK_CONNECTION_STRING=lfslack.db
ENV WITH_SWAGGER=true
ENTRYPOINT ["dotnet", "Lf.Slack.TransactionBalance.Api.dll"]