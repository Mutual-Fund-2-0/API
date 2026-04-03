$ErrorActionPreference = "Stop"

# ------------------------------------------------------------
# 🔧 HELPER FUNCTION
# ------------------------------------------------------------
function Assert-Success([string]$StepName) {
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ $StepName failed with exit code $LASTEXITCODE." -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

# ------------------------------------------------------------
# 🧹 CLEAN & 🔨 BUILD (Build inherently does Restore)
# ------------------------------------------------------------
Write-Host "🧹 Cleaning project..." -ForegroundColor Green
dotnet clean --verbosity diagnostic
Assert-Success "Clean"

Write-Host "🔨 Restoring & Building project..." -ForegroundColor Green
# dotnet build automatically runs dotnet restore first!
dotnet build --verbosity diagnostic
Assert-Success "Build"

# ------------------------------------------------------------
# 🧪 TEST DETECTION AND EXECUTION
# ------------------------------------------------------------
Write-Host "🔍 Checking for test projects..." -ForegroundColor Green
$testProjects = Get-ChildItem -Recurse -Filter "*.csproj" | Where-Object {
    $content = Get-Content $_.FullName -Raw
    # Universal check for all .NET Test frameworks (NUnit, xUnit, MSTest)
    $content -match "Microsoft.NET.Test.Sdk" 
}

if ($testProjects) {
    Write-Host "🧪 Found test projects. Running tests..." -ForegroundColor Yellow
    foreach ($project in $testProjects) {
        Write-Host "📋 Testing: $($project.Name)" -ForegroundColor Cyan
        dotnet test $project.FullName --no-build --verbosity diagnostic --collect:"XPlat Code Coverage" --results-directory "../artifacts/coverage/$($project.FullName)/"
        Assert-Success "Tests in $($project.Name)"
    }
    Write-Host "✅ All tests passed!" -ForegroundColor Green
} else {
    Write-Host "ℹ️ No test projects found. Skipping tests." -ForegroundColor Gray
}

# ------------------------------------------------------------
# 🚀 APPLICATION RUN STEP
# ------------------------------------------------------------
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
