<p align="center">
  <a title="Project Logo">
    <img height="150" style="margin-top:15px" src="https://raw.githubusercontent.com/Advanced-Systems/vector-assets/master/advanced-systems-logo-annotated.svg">
  </a>
</p>

<h1 align="center">Advanced Systems Backend Repository</h1>

## About

TODO

## Developer Notes

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

## Further Reading

- [Test web APIs with the HttpRepl](https://learn.microsoft.com/en-us/aspnet/core/web-api/http-repl/?view=aspnetcore-7.0&tabs=windows)
