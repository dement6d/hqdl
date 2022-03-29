rm -r builds
mkdir builds

cd src
dotnet publish -c Release -r win-x64 --self-contained true
mv bin/Release/net6.0/win-x64/publish ../builds/win-x64

dotnet publish -c Release -r linux-x64 --self-contained true
mv bin/Release/net6.0/linux-x64/publish ../builds/linux-x64
