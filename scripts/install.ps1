param(
    [ValidateSet("Development", "Production")]
    [string] $Environment = "Development"
)
begin {
    Push-Location -Path $(git rev-parse --show-toplevel)
    $Project = "AdvancedSystems.Backend"
}
process {
    Write-Host "Install tools" -ForegroundColor Yellow
    dotnet tool install --global microsoft.dotnet-httprepl

    if ($Environment -eq "Development") {
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
    dotnet restore $Project
    dotnet build $Project
}
clean {
    Pop-Location
}
