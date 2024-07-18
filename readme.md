<p align="center">
  <a title="Project Logo">
    <img height="150" style="margin-top:15px" src="https://raw.githubusercontent.com/Advanced-Systems/assets/master/logos/svg/min/adv-logo.svg">
  </a>
</p>

<h1 align="center">Advanced Systems Backend</h1>

[![Unit Tests](https://github.com/Advanced-Systems/backend/actions/workflows/unit-tests.yml/badge.svg?branch=master)](https://github.com/Advanced-Systems/backend/actions/workflows/unit-tests.yml)
[![CodeQL](https://github.com/Advanced-Systems/backend/actions/workflows/codeql.yml/badge.svg?branch=master)](https://github.com/Advanced-Systems/backend/actions/workflows/codeql.yml)

## About

TODO

## Developer Notes

### Prerequisites

Install and build the project on a developer machine:

```powershell
.\scripts\install.ps1 -Environment Development
```

### Commands

Launch the backend in Development mode:

```powershell
setx ASPNETCORE_ENVIRONMENT "Development"
dotnet run --project="AdvancedSystems.Backend"
```

Open swagger:

```powershell
start https://localhost:5001/swagger/index.html
```

Run the test suite:

```powershell
dotnet test AdvancedSystems.Backend.Tests
```

Containerize the development environment:

```powershell
docker build --rm -t adv-sys/backend:latest . --build-arg PORT=5000
```

Run the container and expose port 5000 over HTTP:

```powershell
docker run --rm -p 5000:5000 adv-sys/backend
```
