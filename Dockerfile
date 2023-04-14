FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY . ./

WORKDIR /src/Velocity.Api

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /src/Velocity.Api/out .
ENTRYPOINT ["dotnet", "Velocity.Api.dll"]