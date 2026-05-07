## 🚀 Project Overview: Mutual Fund Schemes API

This is a modern, high-performance **ASP.NET Core Web API** built using **.NET 10**. The project follows a robust architectural pattern with a clear separation between business logic, data access, and automated testing. It is designed to work seamlessly with **Supabase (PostgreSQL)** for production while supporting high-speed **In-Memory** execution for testing environments.

---

## 🛠️ Tech Stack & Prerequisites

| Component | Technology | Version |
| :--- | :--- | :--- |
| **Runtime** | .NET SDK | 10.0.201 |
| **Framework** | ASP.NET Core | 10.0.x |
| **Database** | PostgreSQL (Supabase) | 16+ |
| **ORM** | EF Core | 10.0.2 |
| **Documentation** | OpenAPI | v10 |
| **Testing** | NUnit & Reqnroll (BDD) | 4.3.2 / 3.3.2 |

### 📋 Prerequisites
1. **Install .NET 10 SDK** (Pinned in `global.json`).
2. **Install local tools**: Run `dotnet tool restore` to install `dotnet-ef` and `reportgenerator`.
3. Install Docker CLI & Colima for containerized execution.
4. **Supabase Account**: Ensure you have access to your PostgreSQL instance.

---

## 🏗️ Project Architecture

* **API**: The core Web API containing Controllers, Services, and Repositories.
* **UT (Unit Tests)**: Isolated tests using **Moq** and **NUnit**.
* **IT (Integration Tests)**: End-to-end BDD tests using **Reqnroll** and **WebApplicationFactory**.
* **Centralized Configuration**: Uses `Directory.Build.props` for global settings and `Directory.Packages.props` for centralized version management.

---

## ⚙️ Setup & Installation

### 1️⃣ Database Scaffolding
The project uses a database-first approach. To sync your C# models with the Supabase schema:
* Ensure your connection string is set in `appsettings.Development.json`.
* Run the scaffolding script:
    ```powershell
    ./scafold.ps1
    ```
    This generates the `DbContext` and Models in the `Datas` and `Models` directories.

### 2️⃣ Restore & Build
The project uses **Package Source Mapping** for security. To set up dependencies:
```powershell
dotnet restore
dotnet build
```

ere is the completely updated README.md with the new pull and run commands added to the Deployment Note section at the bottom.

🚀 Project Overview: Mutual Fund Schemes API
This is a modern, high-performance ASP.NET Core Web API built using .NET 10. The project follows a robust architectural pattern with a clear separation between business logic, data access, and automated testing. It is designed to work seamlessly with Supabase (PostgreSQL) for production while supporting high-speed In-Memory execution for testing environments. The application is fully containerized using highly optimized, secure Docker images.

🛠️ Tech Stack & Prerequisites
Component	Technology	Version
Runtime	.NET SDK	10.0.201
Framework	ASP.NET Core	10.0.x
Database	PostgreSQL (Supabase)	16+
ORM	EF Core	10.0.2
Documentation	Scalar	v10
Testing	NUnit & Reqnroll (BDD)	4.3.2 / 3.3.2
📋 Prerequisites

Install .NET 10 SDK (Pinned in global.json).

Install Docker (Docker Desktop, Colima, etc.) for containerized execution.

Install local tools: Run dotnet tool restore to install dotnet-ef and reportgenerator.

Supabase Account: Ensure you have access to your PostgreSQL instance.

🏗️ Project Architecture
API: The core Web API containing Controllers, Services, and Repositories.

UT (Unit Tests): Isolated tests using Moq and NUnit.

IT (Integration Tests): End-to-end BDD tests using Reqnroll and WebApplicationFactory.

Centralized Configuration: Uses Directory.Build.props for global settings and Directory.Packages.props for centralized version management.

⚙️ Setup & Installation
1️⃣ Database Scaffolding

The project uses a database-first approach. To sync your C# models with the Supabase schema:

Ensure your connection string is set in appsettings.Development.json.

Run the scaffolding script:

PowerShell
./scafold.ps1
This generates the DbContext and Models in the Datas and Models directories.

2️⃣ Restore & Build (Local)

The project uses Package Source Mapping for security. To set up dependencies locally:

PowerShell
dotnet restore
dotnet build

3️⃣ Running the Application (Dockerized)

The application is optimized for Docker using a multi-stage build and an ultra-secure Ubuntu Chiseled runtime environment.

Build the image:

Bash
docker buildx build -t mutual-fund-api .
Run the container:
To access the Scalar API documentation, you must pass the Development environment variable so the container knows it is safe to expose the UI:

Bash
docker run -d \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  mutual-fund-api
API Base URL: http://localhost:8080

Scalar UI: http://localhost:8080/scalar/v1 (Active in Development mode)

Note: You can still run the app locally without Docker using ./run.ps1 or dotnet run --project API/API.csproj.

You can use the automated lifecycle script which handles cleaning, building, and running:
```powershell
./run.ps1
```
Alternatively, run the API manually:
```powershell
dotnet run --project API/API.csproj
```
* **Development URL**: `http://localhost:5291`
* **Swagger UI**: `http://localhost:5291/swagger` (Active in Development mode)

---

## 🧪 Testing Strategy

### 🧪 Running Tests
The `run.ps1` script automatically detects and executes all NUnit-based test projects.
```powershell
# Run all tests via script
./run.ps1
```

### 🔍 Integration Testing (IT)
* **Environment**: Uses a dedicated `Testing` environment.
* **Database**: Automatically switches to `UseInMemoryDatabase` when the environment is set to "Testing".
* **BDD**: Uses **Reqnroll**. Ensure `Reqnroll.json` exists in the IT project root before building.

To make your **README.md** professional and helpful for other developers (or your future self), it’s best to wrap these commands in a section that explains **why** they are being used and **what** the expected output is.

Here is the content you can drop directly into your README. It explains the purpose of the `.runsettings` and the value of the HTML dashboard.

---

### 🧪 Automated Testing & Code Coverage
The project uses a standardized testing pipeline to ensure code quality and logical correctness. We use **Coverlet** for data collection and **ReportGenerator** to visualize the results.

#### 1. Execute Tests with Coverage
To run all unit tests while applying global exclusions (defined in `.runsettings`) and collecting coverage data, use the following command:

```powershell
dotnet test --settings ../.runsettings --collect:"XPlat Code Coverage" --results-directory "../artifacts/coverage/UT"
```
* **`--settings`**: Points to the configuration file that excludes third-party libraries and auto-generated boilerplate from coverage stats.
* **`--collect`**: Enables the cross-platform "XPlat Code Coverage" collector.
* **`--results-directory`**: Centralizes all test output into a single `artifacts` folder for easier cleanup and CI/CD integration.

#### 2. Generate Visual HTML Report
Raw `.xml` coverage files can be difficult to read. Use the following command to transform those files into a searchable, interactive HTML dashboard:

```powershell
dotnet reportgenerator -reports:"./artifacts/coverage/UT/*/coverage.cobertura.xml" -targetdir:"./artifacts/report" -reporttypes:Html
```
* **`-reports`**: Uses a wildcard pattern to find all Cobertura files generated by the test projects.
* **`-targetdir`**: Specifies where the website files will be generated.
* **`-reporttypes:Html`**: Creates a full dashboard. You can view this by opening `./artifacts/report/index.html` in any browser.

---

## 🌐 Environment Configurations

| Environment | Database Type | Config File |
| :--- | :--- | :--- |
| **Development** | PostgreSQL (Supabase) | `appsettings.Development.json` |
| **Testing** | In-Memory DB | `appsettings.Testing.json` |
| **Staging** | PostgreSQL (Supabase) | `appsettings.Staging.json` |
| **Production** | PostgreSQL (Supabase) | `appsettings.json` |

---

## 📦 Deployment Note
Container Orchestration: The primary deployment method is via the included Dockerfile, which uses a 10.0-noble-chiseled image to run the app securely as a non-root user on port 8080.

Pull the latest image from a registry (replace your-registry with your actual repository like DockerHub, AWS ECR, etc.):

Bash
docker pull your-registry/mutual-fund-api:latest
Run the pulled image in a Development Environment (enables Scalar UI for testing deployments):

Bash
docker run -d \
  --name api-dev \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  your-registry/mutual-fund-api:latest

* The project includes a `web.config` for **IIS/Windows Hosting** using the `AspNetCoreModuleV2` in-process model.
* The production environment is explicitly set to `Production` within the `web.config` variables.
* **Security**: HTTPS redirection and Static Files are currently flagged for production-use only in `Program.cs`.

---
**Happy Coding!** 💻✨
