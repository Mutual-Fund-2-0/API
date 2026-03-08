#* ------------------------------------------------------------
#* 🧹 CLEAN STEP
#* ------------------------------------------------------------
# Removes previous build artifacts such as bin/ and obj/ folders
# to ensure a fresh build. Diagnostic verbosity prints detailed
# logs which are useful for debugging CI/CD issues.

Write-Host "🧹 Cleaning project..." -ForegroundColor Green
dotnet clean --verbosity diagnostic

# Capture the exit code of the last command executed.
$buildExitCode = $LASTEXITCODE

# If clean failed (exit code != 0), stop script execution.
if ($buildExitCode -ne 0) {
    Write-Host "❌ Clean failed." -ForegroundColor Red
    exit $buildExitCode
}
Write-Host "✅ Clean completed (Exit: $buildExitCode)" -ForegroundColor Gray

#? ------------------------------------------------------------
#? 📦 RESTORE STEP
#? ------------------------------------------------------------
# Restores NuGet packages required by the solution/project.
# This downloads any missing dependencies defined in csproj.

Write-Host "📦 Restoring packages..." -ForegroundColor Green
dotnet restore --verbosity diagnostic
$buildExitCode = $LASTEXITCODE
if ($buildExitCode -ne 0) {
    Write-Host "❌ Restore failed." -ForegroundColor Red
    exit $buildExitCode
}

#TODO ------------------------------------------------------------
#TODO 🔨 BUILD STEP
#TODO ------------------------------------------------------------
# Compiles all projects. The --no-restore flag improves performance
# because packages were already restored in the previous step.

Write-Host "🔨 Building project..." -ForegroundColor Green
dotnet build --no-restore --verbosity diagnostic
$buildExitCode = $LASTEXITCODE
if ($buildExitCode -ne 0) {
    Write-Host "❌ Build failed." -ForegroundColor Red
    exit $buildExitCode 
}

#! ------------------------------------------------------------
#! 🧪 TEST DETECTION AND EXECUTION
#! ------------------------------------------------------------
# Search recursively for .csproj files and detect test projects
# by checking if the project references NUnit.
Write-Host "🔍 Checking for test projects..." -ForegroundColor Green
$testProjects = Get-ChildItem -Recurse -Filter "*.csproj" | Where-Object {
    $content = Get-Content $_.FullName -Raw
    $content -match "NUnit"
}

# If test projects are found, run tests
if ($testProjects) {
    Write-Host "🧪 Found test projects. Running tests..." -ForegroundColor Yellow
    foreach ($project in $testProjects) {
        Write-Host "📋 Testing: $($project.Name)" -ForegroundColor Cyan
        dotnet test $project.FullName --no-build --no-restore --verbosity diagnostic
        $testExitCode = $LASTEXITCODE
        if ($testExitCode -ne 0) {
            Write-Host "❌ Tests failed in $($project.Name)." -ForegroundColor Red
            exit $testExitCode
        }
    }
    Write-Host "✅ All tests passed!" -ForegroundColor Green
}
else {
    Write-Host "ℹ️ No test projects found. Skipping tests." -ForegroundColor Gray
}

#! ------------------------------------------------------------
#! 🚀 APPLICATION RUN STEP
#! ------------------------------------------------------------
# Detect runnable projects (non-test projects) and run them.
# Test projects are filtered out based on naming conventions.
Write-Host "🔍 Checking for Projects..." -ForegroundColor Green
$projects = Get-ChildItem -Recurse -Filter "*.csproj" | Where-Object { 
    $_.Name -notmatch "Test|Tests|UT|IT"
}
if ($projects) {
    foreach ($project in $projects)
    {
        Write-Host "🚀 Running project: $($project.FullName)" -ForegroundColor Green
        dotnet run --project $project.FullName --no-build --no-restore --verbosity diagnostic
        $buildExitCode = $LASTEXITCODE
        if ($buildExitCode -ne 0) {
            Write-Host "❌ Run failed." -ForegroundColor Red
            exit $buildExitCode
        }
    }
}
else {
    Write-Host "ℹ️ No projects found." -ForegroundColor Gray
}
