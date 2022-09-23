FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY src/Gitlab.Sidekick.Domain/Gitlab.Sidekick.Domain.csproj ./src/Gitlab.Sidekick.Domain/
COPY src/Gitlab.Sidekick.Application/Gitlab.Sidekick.Application.csproj ./src/Gitlab.Sidekick.Application/
COPY src/Gitlab.Sidekick.Infrastructure/Gitlab.Sidekick.Infrastructure.csproj ./src/Gitlab.Sidekick.Infrastructure/
COPY src/Gitlab.Sidekick.Api/Gitlab.Sidekick.Api.csproj ./src/Gitlab.Sidekick.Api/
RUN dotnet restore ./src/Gitlab.Sidekick.Api/Gitlab.Sidekick.Api.csproj

COPY . ./
RUN dotnet publish ./src/Gitlab.Sidekick.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
EXPOSE 5432
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Gitlab.Sidekick.Api.dll"]
