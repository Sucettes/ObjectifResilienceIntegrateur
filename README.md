# SPK Web Application Template

# Sentry
Dans `appsettings.json` assurez-vous d'aller mettre � jour le *projectId*.

# Papertrail
Dans `appsettings.json` assurez-vous d'aller modifier le *port* et *appName*.

# Mailing
Dans `appsettings.json` assurez-vous d'aller indiquer votre *accessKeyId* valide.
Dans `appsettings.local.json` assurez-vous d'aller ajouter votre *accessKeySecret* valide.

# Docker

Nic: Poker moi si vous avez des questions/commentaires/améliorations du setup docker.

## Windows
Install Docker Desktop (Windows) if you don't already have it. For now we are still using Hyper-V, so disable WSL 2 docker integration. Also disable docker-compose V2.

**Build and run the app**

If it is the first time you run the project, please run the /docker/build/dev.ps1 script to generate/trust the required certificate for https.

```bash
powershell /docker/build/dev.ps1
# Then execute this command to start app and db container. 
docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" up --build
```

## Mac/Linux

You will need to generate the required certificate and change the 'Dev_Certificate_Volume_Path' is the .env file.

See: https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-6.0

**Build and run the project**

```bash
docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" up --build
```
---

**The entrypoint for the dockerfile.development is `npm run start`, running gulp and dotnet watch run. After running the docker-compose command, your app will be running at https://localhost:5001**

**You can also run the app via Visual Studio, just set the "Startup Project" to "docker-compose".**

## Troubleshooting

In case you run into this error:
```
ERROR: for your-new-project-app  Cannot create container for service app: user declined directory sharing [...]
```

Add `C:\Users\[YourUser]` in Settings → Resources → File Sharing, then Apply & Restart.

## Exploring images and build layers

You can inspect each step of your dockerfile and see the final folder stucture using Dive

https://github.com/wagoodman/dive

## .NET 6.0 Hot Reload

As of now, hot reload is not supported for .NET6.0 running in docker container.

See:

https://github.com/dotnet/aspnetcore/issues/31908

https://github.com/dotnet/sdk/pull/17072

https://github.com/dotnet/aspnetcore/issues/33823

## Debugging

You can easily attach your docker process in Visual Studio.

`Debug -> Attach To Process -> Connection type (Docker Linux Container) -> Find (Wait for your running container to appear) then select your container -> Find your process and Attach -> Done`

