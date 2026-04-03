# 🔧 common.ps1
$ErrorActionPreference = "Stop"

# Shared Helper Function
function Assert-Success([string]$StepName) {
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ $StepName failed with exit code $LASTEXITCODE." -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

# Shared Build Logic
function Invoke-ProjectBuild {
    Write-Host "🧹 Cleaning project..." -ForegroundColor Green
    dotnet clean --verbosity diagnostic
    Assert-Success "Clean"

    Write-Host "🔨 Restoring & Building project..." -ForegroundColor Green
    dotnet build --verbosity diagnostic
    Assert-Success "Build"
}