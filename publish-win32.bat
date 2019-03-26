dotnet publish -c Release -f net472 -o ./../../Deploy ./Source/Lyon/Lyon.csproj
del Deploy\Lyon.bat
del Deploy\lyon.sh