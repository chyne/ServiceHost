# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/ServiceHost/*.csproj ./src/ServiceHost/
RUN dotnet restore

# copy everything else and build app
COPY src/ServiceHost/. ./src/ServiceHost/
WORKDIR /source/src/ServiceHost
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "ServiceHost.dll"]