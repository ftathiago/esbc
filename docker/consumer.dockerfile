FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
RUN mkdir /app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet_restore
WORKDIR /app
COPY "Esbc.sln" "Esbc.sln"
COPY "__tests__/EsbcConsumer/EsbcConsumer.csproj" "__tests__/EsbcConsumer/EsbcConsumer.csproj"
COPY "__tests__/EsbcProducer/EsbcProducer.csproj" "__tests__/EsbcProducer/EsbcProducer.csproj"
COPY "src/EsbcConsumer/EsbcConsumer.csproj" "src/EsbcConsumer/EsbcConsumer.csproj"
COPY "src/EsbcProducer/EsbcProducer.csproj" "src/EsbcProducer/EsbcProducer.csproj"
RUN dotnet restore "Esbc.sln"

FROM dotnet_restore AS dotnet_code
WORKDIR /app
COPY . . 

FROM dotnet_code AS test
WORKDIR /app
RUN dotnet test

FROM dotnet_code as publish
WORKDIR /app
RUN ls
RUN dotnet build
RUN dotnet publish -c Release -o dist

FROM base AS final
WORKDIR /dist
COPY --from=publish /app/dist .
ENTRYPOINT ["dotnet", "EsbcConsumer.dll"]