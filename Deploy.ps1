# Server
$imageName = "hangfire-app"
$imagePath = "./$imageName.tar"
$ip = "159.65.201.136"

# Docker
$composeFile = "docker-compose.yml"
$remote_dockerPath = "/root/Docker/hangfire"

# --------------------- MAIN ---------------------

Write-Host "Deploying to IP: $ip" -ForegroundColor Green

# Confirm docker is running, else exit
$dockerInfo = docker info 2>&1
if ($dockerInfo -match "docker daemon is not running") {
    Write-Host "Docker is not running, please start docker and try again" -ForegroundColor Red
    exit
}
else {
    Write-Host "Docker is running" -ForegroundColor Green
}


Write-Host "--------- (DOCKER DEPLOYMENT) ---------" -ForegroundColor Cyan

Write-Host "Building project: mk_hangfire" -ForegroundColor Green
dotnet build "./mk_hangfire.csproj" -c Release -o ./app/build

Write-Host "Publishing project: mk_hangfire" -ForegroundColor Green
dotnet publish "./mk_hangfire.csproj" -c Release -o ./app/publish /p:UseAppHost=false

Write-Host "Building image: $imageName" -ForegroundColor Green
docker compose build

Write-Host "Saving image: $imageName" -ForegroundColor Green
docker save -o $imagePath $imageName

Write-Host "Copying image to server" -ForegroundColor Green
Invoke-Expression "scp $imagePath root@$ip`:$remote_dockerPath/"

Write-Host "Copying $composeFile to server" -ForegroundColor Green
Invoke-Expression "scp $composeFile root@$ip`:$remote_dockerPath/$composeFile"

Write-Host "Loading image on server" -ForegroundColor Green
Invoke-Expression "ssh root@$ip 'docker load -i $remote_dockerPath/$imageName.tar'"

Write-Host "Compose down" -ForegroundColor Green
Invoke-Expression "ssh root@$ip 'docker compose -f $remote_dockerPath/$composeFile down'"

Write-Host "Compose up" -ForegroundColor Green
Invoke-Expression "ssh root@$ip 'docker compose -f $remote_dockerPath/$composeFile up -d'"
