"before-deploy.ps1"

$sitePath = (get-website -Name $env:APPLICATION_SITE_NAME).physicalPath
$destination = "$sitePath\app_offline.htm";
"Exrtacting app_offline to target website to destination : $destination"

Add-Type -Assembly System.IO.Compression.FileSystem
$zip = [IO.Compression.ZipFile]::OpenRead($env:ARTIFACT_LOCALPATH)
$zip.Entries | Where-Object {$_.Name -like '*app_offline*'} | ForEach-Object {[System.IO.Compression.ZipFileExtensions]::ExtractToFile($_, $destination, $true)}
$zip.Dispose()

"Starting 10 sec sleep to allow app to shutdown gracefully..."
Start-Sleep -s 10
"Done! Proceeding to deploy"