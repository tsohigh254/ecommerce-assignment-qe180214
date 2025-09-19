# Dockerfile for ECommerce.Web
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/ECommerce.Web/ECommerce.Web.csproj", "src/ECommerce.Web/"]
COPY ["src/ECommerce.Core/ECommerce.Core.csproj", "src/ECommerce.Core/"]

# Restore dependencies
RUN dotnet restore "src/ECommerce.Web/ECommerce.Web.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/src/ECommerce.Web"
RUN dotnet build "ECommerce.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "ECommerce.Web.dll"]