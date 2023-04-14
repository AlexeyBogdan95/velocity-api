#!/bin/sh

dotnet ef database update $1 -s ./src/SoccerAnalysis.Api/SoccerAnalysis.Api.csproj