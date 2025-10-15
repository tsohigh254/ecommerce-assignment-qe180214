# Dockerfile for ECommerce.Web - Optimized for Render Free Tier
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Set environment variables to reduce memory usage
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_GCHeapHardLimit=50000000
ENV COMPlus_GCHeapHardLimit=50000000
ENV DOTNET_gcServer=0

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
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
RUN dotnet publish "ECommerce.Web.csproj" -c Release -o /app/publish \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false \
    /p:PublishSingleFile=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user (Alpine uses addgroup/adduser)
RUN addgroup -S appgroup && adduser -S appuser -G appgroup && chown -R appuser:appgroup /app
USER appuser

ENTRYPOINT ["dotnet", "ECommerce.Web.dll"]