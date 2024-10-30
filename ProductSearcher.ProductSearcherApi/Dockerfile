FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductSearcher.ProductSearcherApi/ProductSearcher.ProductSearcherApi.csproj", "ProductSearcher.ProductSearcherApi/"]
COPY ["ProductSearcher.Models/ProductSearcher.Models.csproj", "ProductSearcher.Models/"]
COPY ["ProductSearcher.DTO/ProductSearcher.DTO.csproj", "ProductSearcher.DTO/"]
RUN dotnet restore "ProductSearcher.ProductSearcherApi/ProductSearcher.ProductSearcherApi.csproj"
COPY . .
WORKDIR "/src/ProductSearcher.ProductSearcherApi"
RUN dotnet build "ProductSearcher.ProductSearcherApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductSearcher.ProductSearcherApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductSearcher.ProductSearcherApi.dll"]
