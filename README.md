# FormulaOne API Project Setup Guide

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)
![EF Core](https://img.shields.io/badge/EF%20Core-8.0-orange)
![SQLite](https://img.shields.io/badge/SQLite-3-green)
![AutoMapper](https://img.shields.io/badge/AutoMapper-latest-yellow)

## Table of Contents

- [Project Setup](#project-setup)
- [Package Installation](#package-installation)
- [EF Core Tools Setup](#ef-core-tools-setup)
- [Database Configuration](#database-configuration)

## Project Setup

### 1. Initialize Project Structure

```bash
# Create project directory and initialize git
mkdir FormulaOne && cd FormulaOne
git init
dotnet new gitignore

# Set .NET SDK version
dotnet new globaljson --sdk-version 8.0.111

# Create solution
dotnet new sln -n FormulaOne
```

### 2. Create Projects

```bash
# API Project
dotnet new webapi -n FormulaOne.Api
dotnet sln FormulaOne.sln add FormulaOne.Api/FormulaOne.Api.csproj

# Entity Project
dotnet new classlib -n FormulaOne.Entites
dotnet sln FormulaOne.sln add FormulaOne.Entites/FormulaOne.Entites.csproj

# Data Service Project
dotnet new classlib -n FormulaOne.DataService
dotnet sln FormulaOne.sln add FormulaOne.DataService/FormulaOne.DataService.csproj
```

### 3. Configure Project References

```bash
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj reference FormulaOne.Entites/FormulaOne.Entites.csproj
dotnet add FormulaOne.Api/FormulaOne.Api.csproj reference FormulaOne.DataService/FormulaOne.DataService.csproj
```

### 4. Run the Application

```bash
dotnet run --project FormulaOne.Api/FormulaOne.Api.csproj
```

Access the API at: `http://localhost:5038`

## Package Installation

### DataService Project Dependencies

```bash
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore.Sqlite
```

### API Project Dependencies

```bash
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package AutoMapper
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
```

> **Note**: Ensure AutoMapper and AutoMapper.Extensions.Microsoft.DependencyInjection versions match.

## EF Core Tools Setup

### 1. Install Global Tools

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```

### 2. Configure Environment

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
source ~/.zshrc
dotnet ef --version
```

### 3. Verify Installation

```bash
ls -la $HOME/.dotnet/tools
```

## Database Configuration

### 1. Configure Connection String

Update `appsettings.json` in FormulaOne.Api:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=FormulaOne.db"
  }
}
```

### 2. Initialize Database

```bash
# Create initial migration
dotnet ef migrations add InitialCreate \
    --project FormulaOne.DataService/FormulaOne.DataService.csproj \
    --startup-project FormulaOne.Api/FormulaOne.Api.csproj

# Apply migration
dotnet ef database update \
    --project FormulaOne.DataService/FormulaOne.DataService.csproj \
    --startup-project FormulaOne.Api/FormulaOne.Api.csproj
```

---

For more details, check the [official .NET documentation](https://docs.microsoft.com/en-us/dotnet/).
