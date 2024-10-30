FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SomeOtherWorker/SomeOtherWorker.csproj", "SomeOtherWorker/"]
COPY ["ProductSearcher.BasicWorker/ProductSearcher.BasicWorker.csproj", "ProductSearcher.BasicWorker/"]
COPY ["ProductSearcher.Interfaces/ProductSearcher.Interfaces.csproj", "ProductSearcher.Interfaces/"]
COPY ["ProductSearcher.Models/ProductSearcher.Models.csproj", "ProductSearcher.Models/"]
RUN dotnet restore "SomeOtherWorker/SomeOtherWorker.csproj"
COPY . .
WORKDIR "/src/SomeOtherWorker"
RUN dotnet build "SomeOtherWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SomeOtherWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SomeOtherWorker.dll"]
