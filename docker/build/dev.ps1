Param([string] $rootPath)
$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path

$password = 'Vt8EG!Sh'

# generate dev certificate, you can change this for your app name if you wants. Don't forget to change it also in the .env file.
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Gwenael.Web.pfx -p $password
dotnet dev-certs https --trust

Write-Host "Development certificate successfully installed." -ForegroundColor Yellow