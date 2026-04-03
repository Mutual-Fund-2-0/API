# ------------------------------------------------------------
# 🧪 TEST DETECTION AND EXECUTION
# ------------------------------------------------------------

. Join-Path $PSScriptRoot "\common.ps1"

# Run the build
Invoke-ProjectBuild

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
        dotnet test $project.FullName `
            --no-build `
            --settings Join-Path $PSScriptRoot "\..\.runsettings" `
            --collect:"XPlat Code Coverage" `
            --results-directory Join-Path $PSScriptRoot "\..\artifacts\coverage\" $($project.FullName) `
            --verbosity diagnostic
        Assert-Success "Tests in $($project.Name)"
    }

    # ------------------------------------------------------------
    # 📊 REPORT GENERATION
    # ------------------------------------------------------------
    Write-Host "📊 Generating Coverage Report..." -ForegroundColor Magenta
    
    # This combines all individual test results into one HTML dashboard
    dotnet reportgenerator `
        -reports: Join-Path $PSScriptRoot "\..\artifacts\coverage\" $($project.Name) "\*\coverage.cobertura.xml" `
        -targetdir: Join-Path $PSScriptRoot "\..\artifacts\report" `
        -reporttypes:Html
        
    Assert-Success "Report Generation"
    
    Write-Host "✅ All tests passed and report generated at ./artifacts/report/index.html" -ForegroundColor Green
} else {
    Write-Host "ℹ️ No test projects found. Skipping tests." -ForegroundColor Gray
}