# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Basket.API.csproj", "./"]
RUN dotnet restore "Basket.API.csproj"

COPY . .
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]