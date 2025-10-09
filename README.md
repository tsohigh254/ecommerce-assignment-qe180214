# ECommerce Platform - QE180214

## 🎯 Overview
Complete e-commerce platform built with .NET 8, featuring REST API backend and MVC frontend for product management.

**Live Demo:**
- 🌐 **Frontend:** https://ecommerce-web-qe180214.onrender.com
- 🔗 **API:** https://ecommerce-assignment-qe180214.onrender.com/api/products
- 📚 **Repository:** https://github.com/tsohigh254/ecommerce-assignment-qe180214

## 🛠️ Technology Stack
- **.NET 8** - Web API & MVC
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **Bootstrap 5** - UI Framework
- **Docker** - Containerization

## 🏗️ Project Structure
```
src/
├── ECommerce.API/          # REST API Backend
├── ECommerce.Core/         # Shared Models  
└── ECommerce.Web/          # MVC Frontend
```

## 🚀 Quick Start

### Using Docker (Recommended)
```bash
git clone https://github.com/tsohigh254/ecommerce-assignment-qe180214.git
cd ecommerce-assignment-qe180214
docker-compose up -d
```
**Access:** http://localhost:5173

### Manual Setup
1. **Start PostgreSQL:**
   ```bash
   docker run --name postgres-dev -e POSTGRES_DB=ecommerce -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=dev123 -p 5432:5432 -d postgres:13
   ```

2. **Run API:**
   ```bash
   cd src/ECommerce.API
   dotnet run
   ```

3. **Run Web:**
   ```bash
   cd src/ECommerce.Web
   dotnet run
   ```

## 🔧 Configuration

### Environment Variables
```bash
# Production
DATABASE_CONNECTION_STRING=Host=host;Database=db;Username=user;Password=pass
API_BASE_URL=https://your-api-url.com/api

# Development (automatic fallback)
# No configuration needed - uses localhost defaults
```

### Local Development Files
```bash
# Copy templates and configure
cp src/ECommerce.API/appsettings.Template.json src/ECommerce.API/appsettings.Development.json
cp src/ECommerce.Web/appsettings.Template.json src/ECommerce.Web/appsettings.Development.json
```

## 📋 API Endpoints
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

## ✅ Features
- [x] Complete CRUD operations
- [x] Responsive UI with Bootstrap 5
- [x] Image URL support with preview
- [x] Form validation (client & server)
- [x] Error handling & notifications
- [x] Mobile-responsive design
- [x] PostgreSQL database integration
- [x] Docker containerization
- [x] Cloud deployment ready

## 🔒 Security
- Environment variables for sensitive data
- No hardcoded credentials in source code
- Input validation & SQL injection protection
- HTTPS enforcement in production

## 🧪 Testing
```bash
# Test API
curl https://ecommerce-assignment-qe180214.onrender.com/api/products

# Test Frontend
# Visit: https://ecommerce-web-qe180214.onrender.com
```

---
**Student:** QE180214 | **Framework:** .NET 8 | **Database:** PostgreSQL