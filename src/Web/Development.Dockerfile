FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

# Allow dotnet watch
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
# Used in Directory.Build.Props
ENV DOTNET_RUNNING_IN_CONTAINER true
EXPOSE 80

# Looks like this is the way to install node into the final .net container.
# It doesn't work even if i copy the entire node build.
# I'm open to suggestion on this, i don't like it. 
# This allow to run npm command in the dotnet container.
RUN curl --silent --location https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install --yes nodejs

# Moving csproj files to the container, for cache purpose.
WORKDIR /app
COPY ./Directory.Build.props src/Directory.Build.props
COPY src/Web/Gwenael.Web.csproj src/Web/Gwenael.Web.csproj
COPY src/Persistence/Gwenael.Persistence.csproj src/Persistence/Gwenael.Persistence.csproj
COPY src/Domain/Gwenael.Domain.csproj src/Domain/Gwenael.Domain.csproj
COPY src/Application/Gwenael.Application.csproj src/Application/Gwenael.Application.csproj
RUN dotnet restore src/Web/Gwenael.Web.csproj

# Build app assets, we need to do this on the container to have the linux node-sass bindings + cache purpose.
FROM node:14.17.0 AS node-build
COPY src/Web/package.json package.json
COPY src/Web/package-lock.json package-lock.json
RUN npm install

FROM base AS final
COPY --from=node-build node_modules src/Web/node_modules
WORKDIR /app/src/Web
ENTRYPOINT npm run start