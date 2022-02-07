FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

WORKDIR /app
EXPOSE 80

# Build application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY src/Web/Gwenael.Web.csproj src/Web/
COPY src/Persistence/Gwenael.Persistence.csproj src/Persistence/
COPY src/Domain/Gwenael.Domain.csproj src/Domain/
COPY src/Application/Gwenael.Application.csproj src/Application/
RUN dotnet restore src/Web/Gwenael.Web.csproj
COPY . .
WORKDIR /src/src/Web
RUN dotnet build Gwenael.Web.csproj -c Release -o /app

# Build app assets
FROM node:14.17.0 as node-build
COPY src/Web/ .
RUN npm set progress=false
RUN npm install

FROM build AS publish
COPY --from=node-build . .
RUN dotnet publish Gwenael.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gwenael.Web.dll"]
