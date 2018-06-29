FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 60447
EXPOSE 44348

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY SiteChecker.WebApplication/SiteChecker.WebApplication.csproj SiteChecker.WebApplication/
RUN dotnet restore SiteChecker.WebApplication/SiteChecker.WebApplication.csproj
COPY . .
WORKDIR /src/SiteChecker.WebApplication
RUN dotnet build SiteChecker.WebApplication.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SiteChecker.WebApplication.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SiteChecker.WebApplication.dll"]
