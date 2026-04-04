$ErrorActionPreference = "Stop"

Write-Host "🛠️  Scaffolding Supabase DbContext..." -ForegroundColor Cyan

# Safely resolve appsettings path
$appsettingsPath = Join-Path $PSScriptRoot "..\API\appsettings.Development.json"

if (-not (Test-Path $appsettingsPath)) {
    Write-Host "❌ Error: Could not find $appsettingsPath. Make sure the file exists." -ForegroundColor Red
    exit 1
}

# Efficient JSON parsing using -Raw
try {
    $json = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
    $connectionString = $json.ConnectionStrings.Supabase

    if ([string]::IsNullOrWhiteSpace($connectionString)) {
        throw "ConnectionStrings.Supabase is missing or empty in appsettings.Development.json"
    }
} catch {
    Write-Host "❌ Error reading connection string: $_" -ForegroundColor Red
    exit 1
}

$contextName = Read-Host "Enter Context name (default: SupabaseDbContext)"
if ([string]::IsNullOrWhiteSpace($contextName)) { 
    $contextName = "SupabaseDbContext"
}

Write-Host "⏳ Generating models from database..." -ForegroundColor Yellow

# Run EF Core scaffold command securely without hardcoding credentials in the output
dotnet ef dbcontext scaffold $connectionString Npgsql.EntityFrameworkCore.PostgreSQL `
    --project ..\API\API.csproj `
    --output-dir ..\API\Models `
    --context $contextName `
    --context-dir ..\API\Datas `
    --no-onconfiguring `
    --force

Write-Host "✅ Scaffolding complete!" -ForegroundColor Green
