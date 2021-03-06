﻿FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet_restore
WORKDIR /app
COPY "Esbc.sln" "Esbc.sln"
COPY "__tests__/EsbcProducerTest/EsbcProducerTest.csproj" "__tests__/EsbcProducerTest/EsbcProducerTest.csproj"
COPY "src/EsbcProducer/EsbcProducer.csproj" "src/EsbcProducer/EsbcProducer.csproj"
RUN dotnet restore

FROM dotnet_restore AS dotnet_code
WORKDIR /app
COPY . . 

FROM dotnet_code AS test
WORKDIR /app
RUN dotnet test --no-restore 

# COPY --from=dotnet_code /app/idle.sh .
# ENTRYPOINT ["sh","/app/idle.sh"]


