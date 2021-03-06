﻿FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN mkdir /app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet_restore
WORKDIR /app
COPY "Esbc.sln" "Esbc.sln"
COPY "__tests__/EsbcProducerTest/EsbcProducerTest.csproj" "__tests__/EsbcProducerTest/EsbcProducerTest.csproj"
COPY "src/EsbcProducer/EsbcProducer.csproj" "src/EsbcProducer/EsbcProducer.csproj"
RUN dotnet restore "Esbc.sln"

FROM dotnet_restore AS dotnet_code
WORKDIR /app
COPY . . 
RUN dotnet publish -c Release -o dist --no-restore

FROM base AS final
WORKDIR /app
COPY --from=dotnet_code /app/dist .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "EsbcProducer.dll"]

# COPY --from=dotnet_code /app/idle.sh .
# ENTRYPOINT ["sh","/app/idle.sh"]


