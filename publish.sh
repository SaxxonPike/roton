#!/bin/sh

dotnet publish -c Release -f netcoreapp2.2 -o ./../../Deploy ./Source/Lyon/Lyon.csproj
chmod -v +x Deploy/lyon.sh
