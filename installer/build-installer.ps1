[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$versionFile = Join-Path $repositoryRoot 'Version.props'
$bundleProject = Join-Path $PSScriptRoot 'InfluxDBStudio.Bundle\InfluxDBStudio.Bundle.wixproj'

if ([string]::IsNullOrWhiteSpace($env:SYNCFUSION_LICENSE_KEY)) {
    throw 'SYNCFUSION_LICENSE_KEY must be configured before building a public release.'
}

[xml]$versionDocument = Get-Content -LiteralPath $versionFile
$currentVersion = [Version]$versionDocument.Project.PropertyGroup.Version
$nextVersion = '{0}.{1}.{2}' -f $currentVersion.Major, $currentVersion.Minor,
    ($currentVersion.Build + 1)

$versionDocument.Project.PropertyGroup.Version = $nextVersion
$versionDocument.Project.PropertyGroup.AssemblyVersion = "$nextVersion.0"
$versionDocument.Project.PropertyGroup.FileVersion = "$nextVersion.0"
$versionDocument.Save($versionFile)

Write-Host "Building InfluxDB Studio $nextVersion..."
dotnet build $bundleProject --configuration Release
if ($LASTEXITCODE -ne 0) {
    throw "Installer build failed with exit code $LASTEXITCODE."
}

$installerDirectory = Join-Path $repositoryRoot 'artifacts\installer'
$setupPath = Join-Path $installerDirectory "InfluxDBStudio-$nextVersion-Setup-win-x64.exe"
$msiPath = Join-Path $installerDirectory "InfluxDBStudio-$nextVersion-win-x64.msi"

Write-Host "Setup created: $setupPath"
Write-Host "MSI created:   $msiPath"
