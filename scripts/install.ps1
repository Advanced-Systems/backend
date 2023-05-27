param(
    [ValidateSet("Development", "Production")]
    [string] $Environment = "Development"
)
begin {
    Push-Location -Path $(git rev-parse --show-toplevel)
    $Project = "AdvancedSystems.Backend"
    $ConfigFile = "$Project/nuget.config"
}
process {
    if ($Environment -eq "Development") {
        Write-Host "Install tools" -ForegroundColor Yellow
        dotnet tool install --global microsoft.dotnet-httprepl --configfile $ConfigFile

        Write-Host "Configure project" -ForegroundColor Yellow
        git config --local core.autocrl=true

        # Note: the --trust option is only available on Windows and macOS
        dotnet dev-certs https --trust
    }
    else {
        Write-Host "Disabling automatic development certificate generation in redistributable production-like environments" -ForegroundColor Blue
        [Environment]::SetEnvironmentVariable("DOTNET_GENERATE_ASPNET_CERTIFICATE", $false, [EnvironmentVariableTarget]::Process)
    }

    Write-Host "Build $Project" -ForegroundColor Yellow
    dotnet restore $Project --configfile $ConfigFile --verbosity minimal
    dotnet build $Project --nologo
}
clean {
    Pop-Location
}
