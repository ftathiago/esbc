﻿FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
RUN mkdir /app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet_restore
WORKDIR /app
COPY "Esbc.sln" "Esbc.sln"
COPY "__tests__/EsbcConsumer/EsbcConsumer.csproj" "__tests__/EsbcConsumer/EsbcConsumer.csproj"
COPY "__tests__/EsbcProducer/EsbcProducer.csproj" "__tests__/EsbcProducer/EsbcProducer.csproj"
COPY "src/EsbcConsumer/EsbcConsumer.csproj" "src/EsbcConsumer/EsbcConsumer.csproj"
COPY "src/EsbcProducer/EsbcProducer.csproj" "src/EsbcProducer/EsbcProducer.csproj"
RUN dotnet restore --packages ./pkg "Esbc.sln"

FROM dotnet_restore AS dotnet_code
WORKDIR /app
COPY . . 
RUN dotnet publish -c Release -o dist

FROM dotnet_code AS test
WORKDIR /app
RUN dotnet test

FROM base AS final
WORKDIR /app
COPY --from=dotnet_code /app/dist .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "EsbcProducer.dll"]

# COPY --from=dotnet_code /app/idle.sh .
# ENTRYPOINT ["sh","/app/idle.sh"]


