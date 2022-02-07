FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ENV DOTNET_RUNNING_IN_CONTAINER true
EXPOSE 80

COPY ./Directory.Build.props src/Directory.Build.props
COPY src/Web/Gwenael.Web.csproj src/Web/Gwenael.Web.csproj
COPY src/Persistence/Gwenael.Persistence.csproj src/Persistence/Gwenael.Persistence.csproj
COPY src/Domain/Gwenael.Domain.csproj src/Domain/Gwenael.Domain.csproj
COPY src/Application/Gwenael.Application.csproj src/Application/Gwenael.Application.csproj
RUN dotnet restore src/Web/Gwenael.Web.csproj
COPY . .

FROM base AS final
ENTRYPOINT dotnet run --project src/Web/Gwenael.Web.csproj