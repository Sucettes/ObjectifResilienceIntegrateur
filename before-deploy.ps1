if($env:APPVEYOR_REPO_BRANCH -eq "master"){
    choco install -yr terraform
    terraform init -backend-config "arm_client_secret=$env:TF_VAR_service_principal_secret"
    terraform apply -auto-approve
}
else{
    Write-Host "Skipping 'terraform apply' when not on 'master' branch" -ForegroundColor Black -BackgroundColor Yellow
}