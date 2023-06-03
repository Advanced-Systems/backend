FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
ARG PORT
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /App
EXPOSE $PORT

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AdvancedSystems.Backend/AdvancedSystems.Backend.csproj", "AdvancedSystems.Backend/nuget.config", "AdvancedSystems.Backend/"]
RUN dotnet restore "AdvancedSystems.Backend/AdvancedSystems.Backend.csproj" --configfile "AdvancedSystems.Backend/nuget.config" --verbosity minimal
COPY . .
WORKDIR "/src/AdvancedSystems.Backend"
RUN dotnet build "AdvancedSystems.Backend.csproj" --configuration Release --output "/App/Build"

FROM build AS publish
RUN dotnet publish "AdvancedSystems.Backend.csproj" --configuration Release --output "/App/Publish" --no-restore --nologo

FROM base AS final
WORKDIR /App
COPY --from=publish /App/Publish .
RUN echo "Publish AdvancedSystems.Backend"
ENTRYPOINT ["dotnet", "AdvancedSystems.Backend.dll"]
