Param([string] $rootPath)
$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path

# $path = "$env:USERPROFILE\.aspnet\https\Gwenael.Web.pfx"
# $password = 'Vt8EG!Sh'

# Write-Host "Certificate path is $path"

# #generate dev certificate
# dotnet dev-certs https ––clean
# dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Gwenael.Web.pfx -p $password

# #trust the certificate
# Add-Type -AssemblyName System.Security
# $cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
# $cert.Import($path, $password, [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]"PersistKeySet")
# $store = new-object system.security.cryptography.X509Certificates.X509Store -argumentlist "MY", CurrentUser
# $store.Open([System.Security.Cryptography.X509Certificates.OpenFlags]"ReadWrite")
# $store.Add($cert)
# $store.Close()

#dotnet dev-certs https --trust

# certutil -f -p Vt8EG!Sh -importPFX Root .\src\Web\Gwenael.Web.pfx
# certutil -f -p Vt8EG!Sh -importPFX My .\src\Web\Gwenael.Web.pfx

# Write-Host "Current script directory is $scriptPath" -ForegroundColor Yellow

# if ([string]::IsNullOrEmpty($rootPath)) {
#     $rootPath = "$scriptPath\..\.."
# }
# Write-Host "Root path used is $rootPath" -ForegroundColor Yellow

# docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" up -d --build
docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" up -d --build