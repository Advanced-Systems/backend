<p align="center">
  <a title="Project Logo">
    <img height="150" style="margin-top:15px" src="https://raw.githubusercontent.com/Advanced-Systems/vector-assets/master/advanced-systems-logo-annotated.svg">
  </a>
</p>

<h1 align="center">Advanced Systems Backend Repository</h1>

## About

TODO

## Developer Notes

Install the NET HTTP REPL command-line tool for making HTTP requests to the Web
API.

```powershell
dotnet tool install -g Microsoft.dotnet-httprepl
```

Configure your system to trust the dev certificate:

```powershell
dotnet dev-certs https --trust
```

Launch the backend:

```powershell
$project = "AdvancedSystems.Backend"
dotnet build $project
dotnet run --project=$project
```

Open swagger:

```powershell
start https://localhost:5001/swagger/
```

Or connect to the web API with `httprepl`:

```powershell
httprepl https://localhost:5001
```

## Further Reading

- [Test web APIs with the HttpRepl](https://learn.microsoft.com/en-us/aspnet/core/web-api/http-repl/?view=aspnetcore-7.0&tabs=windows)
