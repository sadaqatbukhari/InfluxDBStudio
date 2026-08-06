[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$installerProject = Join-Path $PSScriptRoot 'InfluxDBStudio.Setup\InfluxDBStudio.Setup.wixproj'
$installerPath = Join-Path $repositoryRoot 'artifacts\installer\InfluxDBStudio-3.0.0-win-x64.msi'

dotnet build $installerProject --configuration Release
if ($LASTEXITCODE -ne 0) {
    throw "Installer build failed with exit code $LASTEXITCODE."
}

Write-Host "Installer created: $installerPath"
