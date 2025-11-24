FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Ordering.API/Ordering.API.csproj", "Ordering.API/"]
COPY ["Ordering.Application/Ordering.Application.csproj", "Ordering.Application/"]
COPY ["Ordering.Domain/Ordering.Domain.csproj", "Ordering.Domain/"]
COPY ["Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Ordering.Infrastructure/"]

RUN dotnet restore "Ordering.API/Ordering.API.csproj"

COPY . .

WORKDIR "/src/Ordering.API"
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]