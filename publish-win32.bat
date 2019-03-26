dotnet publish -c Release -f net472 -o ./../../Deploy-Win32 ./Source/Lyon/Lyon.csproj
del Deploy-Win32\Lyon.bat
del Deploy-Win32\lyon.sh