#!/bin/sh

dotnet ef migrations add $1 -p ./src/Velocity.Infrastructure/Velocity.Infrastructure.csproj -s ./src/Velocity.Api/Velocity.Api.csproj