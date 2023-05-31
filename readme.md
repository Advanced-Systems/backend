<p align="center">
  <a title="Project Logo">
    <img height="150" style="margin-top:15px" src="https://raw.githubusercontent.com/Advanced-Systems/vector-assets/master/advanced-systems-logo-annotated.svg">
  </a>
</p>

<h1 align="center">Advanced Systems Backend Repository</h1>

## About

TODO

## Developer Notes

Project requirements:

```powershell
winget install microsoft.dotnet.sdk.7
```

Install and build the project:

```powershell
.\scripts\install.ps1 -Environment Development
```

Launch the backend:

```powershell
dotnet run --project="AdvancedSystems.Backend"
```

Open swagger:

```powershell
start https://localhost:5001/
```

Or connect to the web API with `httprepl`:

```powershell
httprepl https://localhost:5001
```

Run the test suite:

```powershell
dotnet test .\AdvancedSystems.Backend.Tests\
```
