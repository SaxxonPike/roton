#!/bin/sh

dotnet publish -c Release -f net10.0 -o ./../../Deploy ./Source/Lyon/Lyon.csproj
chmod -v +x Deploy/lyon.sh
