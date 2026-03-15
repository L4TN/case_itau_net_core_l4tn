FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/CaseItau.Domain/CaseItau.Domain.csproj src/CaseItau.Domain/
COPY src/CaseItau.Application/CaseItau.Application.csproj src/CaseItau.Application/
COPY src/CaseItau.Infra/CaseItau.Infra.csproj src/CaseItau.Infra/
COPY src/CaseItau.API/CaseItau.API.csproj src/CaseItau.API/

RUN dotnet restore src/CaseItau.API/CaseItau.API.csproj

COPY src/ src/

RUN dotnet build src/CaseItau.API/CaseItau.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish src/CaseItau.API/CaseItau.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaseItau.API.dll"]
