
## Requirements
- Installer .Net Core 6 https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- Installer Docker [Docker Desktop for Mac and Windows \| Docker](https://www.docker.com/products/docker-desktop)
    - Décochez « Install required Windows components for WSL 2 »
    - Après l'installation, ouvrir Docker
    - Accepter les termes d'utilisation
    - Si vous avez un message d'erreur indiquant "WSL 2 installation is incomplete" cliquez sur Cancel
    - Dans Docker, allez dans Réglages -> Général et décochez
        - Use the WSL 2 based engine
        - Use Docker Compose V2
        - Cliquez sur Apply & Restart
    - Si vous avez un message d'erreur indiquant que Hyper-V n'est pas activé "Required Windows feature(s) not enabled : Hyper-V"
        - Cliquez sur Quit
        - Ouvrez Powershell **en tant d'administrateur**
        - Roulez cette commande dans Powershell `Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All`
        - Redémarrez votre ordinateur en tappant Y dans la fenêtre Powershell
        - D'autres méthodes sont proposées ici : [Enable Hyper-V on Windows 10 \| Microsoft Docs](https://docs.microsoft.com/en-us/virtualization/hyper-v-on-windows/quick-start/enable-hyper-v)
- Installer npm v6.14.15 ``npm install npm@6.14.15 -g``
- Installer nvm pour gérer les version de node
- Installer node v14.17.6
```
nvm install 14.17.6
nvm use 14.17.6
```

## Installation du projet
- Cloner le repo
- Ouvrir l'invite de commande et effectuer les commandes suivantes
```
cd path/to/my-new-app
cd ./src/Web
dotnet build
npm install
npm run build
```
- Renouveller le certificat Docker
```
cd path/to/my-new-app
powershell ./docker/build/dev.ps1
```

## Build and run the app
Avant de rouler cette commande qui est assez longue, suivez la procédure de la section ``Troubleshooting`` afin de donner le droit à Docker d'accéder au dossier
```
cd path/to/my-new-app
docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" up --build
```
L'application va ensuite rouler à l'adresse ``http://localhost:5000/``
## Troubleshooting
In case you run into this error:

``ERROR: for your-new-project-app  Cannot create container for service app: user declined directory sharing [...]``
- Open Docker Desktop
- Go to Settings → Resources → File Sharing
- Add ``C:\Users\[YourUser]``
- Add ``path/to/my-new-app``
- Click on Apply & Restart
- Try running docker-compose command again

In case you run into this error during docker compose command:
``Cannot start service db: Ports are not available: listen tcp 0.0.0.0:1433``
Run these commands in a command prompt as Administrator
```
net stop winnat
docker start container_name
net start winnat
```