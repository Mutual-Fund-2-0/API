Write-Host "🧹 Cleaning project..." -ForegroundColor Green
dotnet clean
$buildExitCode = $LASTEXITCODE
if ($buildExitCode -ne 0) {
    Write-Host "❌ Clean failed." -ForegroundColor Red
    exit $buildExitCode
}

Write-Host "📦 Restoring packages..." -ForegroundColor Green
dotnet restore
$buildExitCode = $LASTEXITCODE
if ($buildExitCode -ne 0) {
    Write-Host "❌ Restore failed." -ForegroundColor Red
    exit $buildExitCode
}

Write-Host "🔨 Building project..." -ForegroundColor Green
dotnet build --no-restore
$buildExitCode = $LASTEXITCODE
if ($buildExitCode -ne 0) {
    Write-Host "❌ Build failed." -ForegroundColor Red
    exit $buildExitCode
}

Write-Host "🚀 Running application..." -ForegroundColor Green
dotnet run --no-build
$buildExitCode = $LASTEXITCODE
if ($buildExitCode -ne 0) {
    Write-Host "❌ Run failed." -ForegroundColor Red
    exit $buildExitCode
}
