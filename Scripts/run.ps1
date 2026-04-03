# ------------------------------------------------------------
# 🚀 APPLICATION RUN STEP
# ------------------------------------------------------------

. Join-Path $PSScriptRoot "\common.ps1"

# Run the build
Invoke-ProjectBuild

Write-Host "🔍 Checking for runnable APIs..." -ForegroundColor Green
# Filter out test projects and common library folders
$runnableProjects = Get-ChildItem -Recurse -Filter "*.csproj" | Where-Object { 
    $content = Get-Content $_.FullName -Raw
    $content -notmatch "Microsoft.NET.Test.Sdk" -and
    # Only pick up executable projects or Web APIs, not class libraries
    ($content -match 'Sdk="Microsoft.NET.Sdk.Web"' -or $content -match "<OutputType>Exe</OutputType>")
}

if ($runnableProjects) {
    # If there is only one API, run it directly (blocking)
    if ($runnableProjects.Count -eq 1) {
        Write-Host "🚀 Running project: $($runnableProjects[0].Name)" -ForegroundColor Green
        dotnet run --project $runnableProjects[0].FullName --no-build
        Assert-Success "Run"
    }
    # If there are multiple runnable projects, launch them in parallel detached windows
    else {
        Write-Host "🚀 Multiple APIs found. Launching in parallel..." -ForegroundColor Yellow
        foreach ($project in $runnableProjects) {
            Write-Host "   -> Starting $($project.Name)..." -ForegroundColor Cyan
            Start-Process "dotnet" -ArgumentList "run --project `"$($project.FullName)`" --no-build"
        }
        Write-Host "✅ All projects launched!" -ForegroundColor Green
    }
} else {
    Write-Host "ℹ️ No runnable application projects found." -ForegroundColor Gray
}
