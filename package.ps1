[CmdletBinding()]
param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$root = $PSScriptRoot

$dll = Join-Path $root "bin\$Configuration\RPGSkillsMod.dll"

if (-not (Test-Path $dll)) {
    Write-Error "`"$dll`" introuvable. Compile le projet en configuration $Configuration avant de lancer ce script."
    exit 1
}

$manifest = Get-Content (Join-Path $root "manifest.json") -Raw | ConvertFrom-Json
$version = $manifest.version_number
$versionUnderscored = $version -replace '\.', '_'

$zipName = "RPGSkillsMod_$versionUnderscored.zip"
$zipPath = Join-Path $root $zipName

if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

$zip = [System.IO.Compression.ZipFile]::Open($zipPath, [System.IO.Compression.ZipArchiveMode]::Create)

try {
    # Files shipped as-is at the root of the package.
    $rootFiles = @(
        "icon.png",
        "manifest.json",
        "README.md",
        "CHANGELOG.md"
    )

    foreach ($file in $rootFiles) {
        $path = Join-Path $root $file

        if (-not (Test-Path $path)) {
            Write-Warning "Fichier manquant, ignore : $file"
            continue
        }

        [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $path, $file) | Out-Null
    }

    # The compiled plugin goes under plugins/ inside the package.
    [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $dll, "plugins/RPGSkillsMod.dll") | Out-Null
}
finally {
    $zip.Dispose()
}

Write-Host "OK : $zipName cree avec succes." -ForegroundColor Green
