#!/bin/bash

# QE180214 E-Commerce Assignment - Quick Start Script

echo "🚀 Starting E-Commerce Assignment QE180214..."
echo "=============================================="

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    exit 1
fi

# Check if Docker Compose is installed
if ! command -v docker-compose &> /dev/null; then
    echo "❌ Docker Compose is not installed. Please install Docker Compose first."
    exit 1
fi

echo "✅ Docker and Docker Compose are installed"

# Start the application
echo "🐳 Starting containers..."
docker-compose up -d

# Wait for services to be ready
echo "⏳ Waiting for services to start..."
sleep 30

# Check if services are running
if [ "$(docker-compose ps -q | wc -l)" -eq 3 ]; then
    echo "✅ All services are running!"
    echo ""
    echo "🌐 Application URLs:"
    echo "📱 Frontend:  http://localhost:5173"
    echo "🔗 API:       http://localhost:7246"
    echo "📚 Swagger:   http://localhost:7246/swagger"
    echo "🗄️  Database:  localhost:5432"
    echo ""
    echo "🎯 Assignment QE180214 is ready!"
    echo "Open your browser and navigate to http://localhost:5173"
else
    echo "❌ Some services failed to start. Check logs with:"
    echo "   docker-compose logs"
fi