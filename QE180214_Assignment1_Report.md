# ASSIGNMENT 1 - CRUD REST API + UI
## Student: QE180214
## Date: September 19, 2025

---

## 📋 ASSIGNMENT OVERVIEW

**Subject:** Enterprise Application Development  
**Assignment:** CRUD REST API + Frontend UI  
**Student ID:** QE180214  
**Submission Date:** September 19, 2025  

---

## 🎯 COMPLETED REQUIREMENTS

### ✅ REST API (Backend)
- **Framework:** ASP.NET Core 8.0 Web API
- **Database:** PostgreSQL (Neon.tech cloud)
- **ORM:** Entity Framework Core
- **Architecture:** Clean Architecture (Core, API layers)

### ✅ Frontend UI
- **Framework:** ASP.NET Core 8.0 MVC
- **UI Library:** Bootstrap 5
- **Icons:** Font Awesome 6
- **Responsive:** Mobile-friendly design

### ✅ CRUD Operations
1. **CREATE:** Add new products
2. **READ:** List all products + Get single product
3. **UPDATE:** Edit existing products
4. **DELETE:** Remove products

---

## 🌐 DEPLOYED APPLICATIONS

### 🔗 Live URLs:
- **API Endpoint:** https://ecommerce-assignment-qe180214.onrender.com/api
- **Frontend Website:** https://ecommerce-web-qe180214.onrender.com
- **GitHub Repository:** https://github.com/tsohigh254/ecommerce-assignment-qe180214

### 🗄️ Database:
- **Provider:** Neon.tech PostgreSQL
- **Region:** AWS ap-southeast-1
- **Features:** SSL connection, connection pooling

---

## 🔧 TECHNICAL IMPLEMENTATION

### API Endpoints:
```
GET    /api/products        - Get all products
GET    /api/products/{id}   - Get product by ID
POST   /api/products        - Create new product
PUT    /api/products/{id}   - Update existing product
DELETE /api/products/{id}   - Delete product
```

### Data Model:
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### Key Features:
- ✅ Input validation with Data Annotations
- ✅ Error handling and logging
- ✅ CORS configuration
- ✅ Environment-based configuration
- ✅ Docker containerization
- ✅ Production deployment
- ✅ Security best practices (no secrets in code)

---

## 🏗️ ARCHITECTURE & DEPLOYMENT

### Project Structure:
```
├── src/
│   ├── ECommerce.Core/      # Domain models and interfaces
│   ├── ECommerce.API/       # REST API implementation
│   └── ECommerce.Web/       # MVC Frontend
├── Dockerfile               # Multi-stage Docker builds
├── render.yaml             # Deployment configuration
└── README.md               # Documentation
```

### Deployment Stack:
- **API Hosting:** Render.com (Docker)
- **Frontend Hosting:** Render.com (Docker)
- **Database:** Neon.tech PostgreSQL
- **Source Control:** GitHub
- **CI/CD:** Auto-deploy on git push

---

## 📊 SAMPLE DATA

The application includes 3 pre-loaded products:

1. **Classic T-Shirt** - $29.99
   - Comfortable cotton t-shirt perfect for everyday wear
   
2. **Denim Jacket** - $89.99
   - Stylish denim jacket for a casual look
   
3. **Summer Dress** - $59.99
   - Light and breezy summer dress in floral pattern

---

## 🧪 TESTING INSTRUCTIONS

### API Testing:
```bash
# Get all products
curl https://ecommerce-assignment-qe180214.onrender.com/api/products

# Get specific product
curl https://ecommerce-assignment-qe180214.onrender.com/api/products/1

# Create new product (POST)
curl -X POST https://ecommerce-assignment-qe180214.onrender.com/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Test Product","description":"Test Description","price":99.99}'
```

### Frontend Testing:
1. Visit: https://ecommerce-web-qe180214.onrender.com
2. View product list (homepage)
3. Click "Create New Product" to test CREATE
4. Click "Edit" on any product to test UPDATE
5. Click "Details" to view single product
6. Click "Delete" to test DELETE operation

---

## 🔒 SECURITY CONSIDERATIONS

### Implemented Security Features:
- ✅ Environment variables for sensitive data
- ✅ Database connection strings not in source code
- ✅ HTTPS enforcement in production
- ✅ Input validation and sanitization
- ✅ Error handling without information disclosure
- ✅ CORS properly configured

### File: .env (local development only - not committed)
```
DATABASE_CONNECTION_STRING=actual_connection_string_here
```

---

## 📚 TECHNOLOGIES USED

### Backend:
- ASP.NET Core 8.0 Web API
- Entity Framework Core 9.0
- Npgsql (PostgreSQL provider)
- Microsoft.Extensions.Logging

### Frontend:
- ASP.NET Core 8.0 MVC
- Bootstrap 5.3
- Font Awesome 6.0
- jQuery (for enhanced UX)

### Database:
- PostgreSQL 17
- Neon.tech cloud hosting
- SSL connections

### DevOps:
- Docker containerization
- Render.com deployment
- GitHub source control
- Automated CI/CD

---

## 🚀 DEPLOYMENT PROCESS

### Step 1: Database Setup
1. Created Neon.tech PostgreSQL database
2. Configured connection string in environment variables
3. Applied Entity Framework migrations

### Step 2: API Deployment
1. Built Docker image from src/ECommerce.API/Dockerfile
2. Deployed to Render.com with environment variables
3. Verified API endpoints working

### Step 3: Frontend Deployment
1. Built Docker image from src/ECommerce.Web/Dockerfile
2. Configured to connect to deployed API
3. Deployed to Render.com
4. Verified complete application functionality

---

## 📝 SUBMISSION CHECKLIST

- ✅ REST API with 5 CRUD endpoints
- ✅ Frontend UI with Bootstrap styling
- ✅ Create, Read, Update, Delete operations
- ✅ Database integration (PostgreSQL)
- ✅ Cloud deployment (Render.com)
- ✅ Source code on GitHub
- ✅ Working live demo URLs
- ✅ Documentation and README
- ✅ Security best practices
- ✅ Error handling and validation

---

## 🎉 CONCLUSION

This assignment demonstrates a complete full-stack e-commerce application with:
- Modern .NET 8 architecture
- Cloud-native deployment
- Production-ready security
- Responsive user interface
- RESTful API design
- Proper database integration

**Both applications are live and fully functional at the provided URLs.**

---

**Submitted by:** Student QE180214  
**Date:** September 19, 2025  
**GitHub:** https://github.com/tsohigh254/ecommerce-assignment-qe180214