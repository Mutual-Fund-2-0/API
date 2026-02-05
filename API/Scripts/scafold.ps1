Write-Host "🛠️  Scaffolding Supabase DbContext..." -ForegroundColor Green

# Read appsettings.json from the project root
$appsettingsPath = Join-Path $PSScriptRoot "..\appsettings.Development.json"

$json = Get-Content $appsettingsPath | Out-String | ConvertFrom-Json
$connectionString = $json.ConnectionStrings.Supabase

$contextName = Read-Host "Enter Context name (default: SupabaseDbContext)"
if (-not $contextName) { $contextName = "SupabaseDbContext" }

dotnet ef dbcontext scaffold $connectionString Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context $contextName --context-dir Datas --force

Write-Host "✅ Scaffolding complete!" -ForegroundColor Green
