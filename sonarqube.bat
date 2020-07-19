dotnet tool install --global dotnet-sonarscanner --version 4.8.0
dotnet sonarscanner begin /k:"Esbc" /d:sonar.host.url=http://localhost:9000/
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet sonarscanner end