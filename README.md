
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

## Configuration de la base de données locale
- Ouvrez SSMS (SQL Server Management Studio)
- Connectez-vous à votre serveur de bd local
- Créez une nouvelle bd
    - Clic droit sur ``Databases`` > ``New database``
    - Donnez-lui le même nom que celui qui se trouve dans ``appsettings.Development.json``
- Maintenant, il faut donner accès à un user à la bd qu'on vient de créer
    - Ouvrez le dossier ``Security``
    - Clic droit sur ``Logins`` > ``New login``
    - Sélectionnez SQL Authentication
    - Dans ``appsettings.Development.json``, j'ai défini le username à dev et le password à dev, mais vous pouvez choisir le username et mot de passe de votre choix, il suffit de changer ensuite les valeurs dans le ``appsettings.Development.json``

## Build and run the app

Ouvez deux invites de commandes.
Dans la première exécutez les commande suivantes :
```
cd .\src\Web
dotnet watch run
```
L'application va maintenant rouler à l'adresse ``http://localhost:5000/``

Dans la deuxième invite de commande, exécutez les commandes suivantes :
```
cd .\src\Web
npm run sync
```
Cette commande appelle ``gulp sync`` qui va mettre à jour les fichiers ``site.css`` et ``site.js`` lorsqu'un fichier ``.scss`` ou ``.js`` est changé.

## Azure database
When booting the app with Azure SQL database connection string, if you have this error
```
Cannot open server '[your-server-name]' requested by the login.
Client with IP address '161.184.136.100' is not allowed to access the server. 
```
- Go to your SQL database in Azure
- Click on ``Set server firewall``
- Add the IP address mentionned in the error message
- Click on ``Save``
