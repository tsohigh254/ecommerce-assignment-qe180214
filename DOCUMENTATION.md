# ECommerce Platform - QE180214 - Complete Documentation

## ?? Overview
Complete e-commerce platform built with .NET 8, featuring REST API backend and MVC frontend for product management.

**Live Demo:**
- ?? **Frontend:** https://ecommerce-web-qe180214.onrender.com
- ?? **API:** https://ecommerce-assignment-qe180214.onrender.com/api/products
- ?? **Repository:** https://github.com/tsohigh254/ecommerce-assignment-qe180214

## ??? Technology Stack
- **.NET 8** - Web API & MVC
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **Bootstrap 5** - UI Framework
- **Docker** - Containerization

## ??? Project Structure
```
src/
??? ECommerce.API/          # REST API Backend
??? ECommerce.Core/         # Shared Models  
??? ECommerce.Web/          # MVC Frontend
```

---

# ?? Quick Start Guide

## Option 1: Using Docker (Recommended)
```bash
git clone https://github.com/tsohigh254/ecommerce-assignment-qe180214.git
cd ecommerce-assignment-qe180214
docker-compose up -d
```
**Access:** http://localhost:5173

## Option 2: Manual Setup

### Prerequisites
- .NET 8 SDK
- PostgreSQL 13+
- Git

### Step 1: Clone Repository
```bash
git clone https://github.com/tsohigh254/ecommerce-assignment-qe180214.git
cd ecommerce-assignment-qe180214
```

### Step 2: Start PostgreSQL
```bash
docker run --name postgres-dev \
  -e POSTGRES_DB=ecommerce \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -p 5432:5432 \
  -d postgres:13
```

### Step 3: Run API Backend
```bash
cd src/ECommerce.API
dotnet restore
dotnet run
```
API will be available at: http://localhost:7246

### Step 4: Run Web Frontend
```bash
cd src/ECommerce.Web
dotnet restore
dotnet run
```
Web will be available at: http://localhost:5173

---

# ?? Configuration Guide

## Environment Variables

All sensitive configuration is managed through environment variables with sensible defaults:

| Variable | Default | Description |
|----------|---------|-------------|
| `DB_NAME` | `ecommerce` | PostgreSQL database name |
| `DB_USER` | `postgres` | PostgreSQL username |
| `DB_PASSWORD` | `postgres` | PostgreSQL password |
| `DB_PORT` | `5432` | PostgreSQL port |
| `API_PORT` | `7246` | API service port |
| `WEB_PORT` | `5173` | Web UI port |
| `ASPNETCORE_ENV` | `Development` | ASP.NET Core environment |

## Custom Configuration

To override defaults, create a `.env` file:

```bash
# Copy template
cp .env.example .env

# Edit values (example)
DB_PASSWORD=my_secure_password
API_PORT=8080
WEB_PORT=3000
```

## Development Configuration Files

```bash
# Copy templates and configure
cp src/ECommerce.API/appsettings.Template.json src/ECommerce.API/appsettings.Development.json
cp src/ECommerce.Web/appsettings.Template.json src/ECommerce.Web/appsettings.Development.json
```

---

# ?? API Documentation

## Endpoints
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

## Testing API

### Get All Products
```bash
curl http://localhost:7246/api/products
```

### Get Single Product
```bash
curl http://localhost:7246/api/products/1
```

### Create Product
```bash
curl -X POST http://localhost:7246/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Product",
    "description": "Test Description", 
    "price": 99.99,
    "imageUrl": "https://example.com/image.jpg"
  }'
```

### Update Product
```bash
curl -X PUT http://localhost:7246/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "Updated Product",
    "description": "Updated Description",
    "price": 149.99,
    "imageUrl": "https://example.com/updated.jpg"
  }'
```

### Delete Product
```bash
curl -X DELETE http://localhost:7246/api/products/1
```

---

# ?? Frontend Features & UI Design

## Modern Design Features

### EliteShop Brand Identity
- **Name**: EliteShop (Premium E-Commerce Platform)
- **Tagline**: "Premium E-Commerce Platform"
- **Icon**: Diamond/Gem (??)
- **Color Scheme**: Purple gradient (premium, luxury feel)
- **Design Language**: Minimalist with elegant touches

### Visual Design Elements
```css
Primary Gradient: #667eea ? #764ba2
Success Gradient: #4facfe ? #00f2fe
Warning Gradient: #fa709a ? #fee140
Danger Gradient: #ff6b6b ? #ee5a6f
```

### Typography
- **Headings**: Poppins (600-800 weight)
- **Body**: Inter (300-700 weight)
- **Responsive**: 14px base ? 16px on tablet+

### Interactive Elements
- ? Fade-in effects using Animate.css
- ?? Hover lift animations on cards
- ?? Smooth transitions (0.3s ease)
- ?? Staggered card animations
- ?? Image scale effects on hover
- ?? Glassmorphism cards with backdrop blur

## User Interface Features
- ?? **Responsive Design**: Mobile-first approach with breakpoints
- ?? **Touch-Friendly**: Optimized for mobile interactions
- ?? **Fast Loading**: Optimized CSS and minimal dependencies
- ? **Accessible**: Proper ARIA labels and semantic HTML
- ?? **Modern Aesthetics**: 2025 design trends with gradients
- ?? **Smooth Animations**: GPU-accelerated transitions

## Page Layouts
1. **Home Page**: Hero section with featured products
2. **Product Catalog**: Grid layout with search and filters
3. **Product Details**: Full product information with image gallery
4. **Create/Edit Forms**: Enhanced form inputs with validation
5. **Responsive Navigation**: Mobile hamburger menu

---

# ?? Security & Best Practices

## Security Implementation

### Development Security
- ? **No hardcoded passwords** in source code
- ? **Environment variables** for sensitive data
- ? **Git-ignored** configuration files (.env)
- ? **Template files** provided for setup
- ? **Input validation** (client & server side)
- ? **SQL injection protection** via Entity Framework

### Default Credentials (Local Development Only)
- **Database User:** `postgres`
- **Database Password:** `postgres`
- ?? These are **ONLY for local development** and can be changed in `.env`

### Production Security
- ? **Separate environment variables** from hosting platform
- ? **Strong passwords** from managed database service (Neon.tech)
- ? **HTTPS enforcement** in production
- ? **No exposed database ports** publicly
- ? **Secure connection strings** in environment variables

## Best Practices Implemented
1. **Clean Architecture**: Separation of concerns with Core/API/Web layers
2. **Repository Pattern**: Data access abstraction
3. **Dependency Injection**: Built-in .NET DI container
4. **Error Handling**: Try-catch blocks with proper error responses
5. **Validation**: Data annotations and model validation
6. **Logging**: Structured logging with different levels

---

# ?? Testing & Quality Assurance

## Manual Testing Procedures

### Web Interface Testing
1. **Navigation**: Test all menu links and breadcrumbs
2. **Product Listing**: Verify grid layout and responsiveness
3. **CRUD Operations**: Create, Read, Update, Delete products
4. **Form Validation**: Test required fields and validation messages
5. **Image Display**: Verify image URLs render correctly
6. **Mobile Experience**: Test on different screen sizes

### API Testing
```bash
# Test all endpoints
curl http://localhost:7246/api/products
curl http://localhost:7246/api/products/1
curl -X POST http://localhost:7246/api/products -H "Content-Type: application/json" -d '{"name":"Test","description":"Test","price":99.99}'
```

### Online Demo Testing
- **Frontend**: https://ecommerce-web-qe180214.onrender.com
- **API**: https://ecommerce-assignment-qe180214.onrender.com/api/products
- ?? **Note**: First access may be slow (~30s) due to free tier sleep mode

---

# ?? Troubleshooting Guide

## Common Issues & Solutions

### Port Already in Use
```bash
# Check if ports are occupied
netstat -ano | findstr "5173"
netstat -ano | findstr "7246"
netstat -ano | findstr "5432"

# Change ports in .env if needed
echo "WEB_PORT=5174" >> .env
echo "API_PORT=7247" >> .env
echo "DB_PORT=5433" >> .env

# Restart services
docker-compose down
docker-compose up -d
```

### Docker Issues
```bash
# Check Docker is running
docker ps

# View logs
docker-compose logs

# Restart everything
docker-compose down
docker-compose up -d

# Reset database
docker-compose down -v
docker-compose up -d
```

### Database Connection Problems
```bash
# Check postgres health
docker-compose ps postgres

# View postgres logs
docker-compose logs postgres

# Restart postgres only
docker-compose restart postgres
```

### API Not Responding
1. Check if API is running: `curl http://localhost:7246/api/products`
2. Verify database connection in API logs
3. Check appsettings.json configuration
4. Ensure database migrations are applied

### Frontend Issues
1. Verify API_BASE_URL in appsettings.json
2. Check browser developer console for errors
3. Ensure API is accessible from frontend
4. Test API endpoints directly first

## Complete Reset
```bash
# Nuclear option - reset everything
docker-compose down -v --rmi all
docker system prune -f
git clean -fdx
docker-compose up -d
```

---

# ?? Project Structure Details

```
ecommerce-assignment-qe180214/
??? src/
?   ??? ECommerce.API/              # Backend REST API
?   ?   ??? Controllers/            # API Controllers
?   ?   ?   ??? ProductsController.cs
?   ?   ??? Data/                   # Database Context
?   ?   ?   ??? ApplicationDbContext.cs
?   ?   ??? appsettings.json        # API Configuration
?   ?   ??? appsettings.Template.json
?   ?   ??? Program.cs              # API Startup
?   ?   ??? Dockerfile              # API Container
?   ??? ECommerce.Web/              # Frontend MVC
?   ?   ??? Controllers/            # MVC Controllers
?   ?   ?   ??? HomeController.cs
?   ?   ?   ??? ProductsController.cs
?   ?   ??? Views/                  # Razor Views
?   ?   ?   ??? Home/
?   ?   ?   ??? Products/
?   ?   ?   ??? Shared/
?   ?   ??? Services/               # HTTP Client Services
?   ?   ?   ??? ProductService.cs
?   ?   ??? wwwroot/                # Static Assets
?   ?   ?   ??? css/
?   ?   ?   ??? js/
?   ?   ?   ??? lib/
?   ?   ??? appsettings.json        # Web Configuration
?   ?   ??? appsettings.Template.json
?   ?   ??? Program.cs              # Web Startup
?   ?   ??? Dockerfile              # Web Container
?   ??? ECommerce.Core/             # Shared Models
?       ??? Models/                 # Domain Models
?       ?   ??? Product.cs
?       ??? ECommerce.Core.csproj
??? docker-compose.yml              # Docker Orchestration
??? .env.example                    # Environment Template
??? .gitignore                      # Git Ignore Rules
??? DOCUMENTATION.md                # This File (Consolidated)
??? README.md                       # Quick Overview
```

---

# ? Features Checklist

## Core Features Implemented
- [x] **Complete CRUD Operations**: Create, Read, Update, Delete products
- [x] **REST API Backend**: Full API with all endpoints
- [x] **MVC Frontend**: Web interface for product management
- [x] **Database Integration**: PostgreSQL with Entity Framework Core
- [x] **Responsive UI**: Bootstrap 5 with custom CSS
- [x] **Form Validation**: Client-side and server-side validation
- [x] **Error Handling**: Proper error pages and messages
- [x] **Image Support**: Product image URLs with preview
- [x] **Docker Support**: Complete containerization
- [x] **Cloud Deployment**: Production deployment on Render.com

## Technical Features
- [x] **Clean Architecture**: Layered project structure
- [x] **Dependency Injection**: .NET built-in DI container
- [x] **Environment Configuration**: Template and production configs
- [x] **Security Best Practices**: No hardcoded secrets
- [x] **API Documentation**: RESTful endpoints with examples
- [x] **Responsive Design**: Mobile-first approach
- [x] **Modern UI/UX**: Glassmorphism design with animations
- [x] **Professional Branding**: EliteShop identity

## Quality Assurance
- [x] **Code Structure**: Clear, maintainable codebase
- [x] **Documentation**: Comprehensive setup and usage guides
- [x] **Testing**: Manual testing procedures documented
- [x] **Troubleshooting**: Common issues and solutions
- [x] **Production Ready**: Deployed and accessible online
- [x] **Mobile Optimized**: Touch-friendly interface
- [x] **Performance**: Optimized loading and animations

---

# ?? Academic Information

## Assignment Details
- **Student ID**: QE180214
- **Assignment**: CRUD REST API + UI Implementation
- **Framework**: .NET 8 (ASP.NET Core)
- **Database**: PostgreSQL
- **Architecture**: Clean Architecture with API + MVC

## Grading Criteria Addressed
- [ ] **Backend API**: Complete 5 CRUD endpoints ?
- [ ] **Frontend UI**: Full CRUD interface ?
- [ ] **Database Integration**: Working PostgreSQL setup ?
- [ ] **Code Quality**: Clean, readable structure ?
- [ ] **Documentation**: Comprehensive guides ?
- [ ] **Docker Support**: Working docker-compose ?
- [ ] **Responsive UI**: Professional, mobile-friendly ?
- [ ] **Cloud Deployment**: Live demo available ?
- [ ] **Security**: No hardcoded credentials ?
- [ ] **Error Handling**: Proper error management ?

## Technology Specifications Met
| Requirement | Implementation | Status |
|-------------|----------------|---------|
| .NET 8 | ASP.NET Core 8.0 | ? |
| REST API | 5 CRUD endpoints | ? |
| Database | PostgreSQL + EF Core | ? |
| Frontend | MVC with Razor Views | ? |
| Responsive UI | Bootstrap 5 + Custom CSS | ? |
| Containerization | Docker + Docker Compose | ? |
| Documentation | Complete setup guides | ? |
| Deployment | Cloud hosting (Render.com) | ? |

---

# ?? Contact & Resources

## Student Information
- **Student ID**: QE180214
- **GitHub**: https://github.com/tsohigh254/ecommerce-assignment-qe180214
- **Email**: Available in assignment submission

## Live Resources
- **Frontend Demo**: https://ecommerce-web-qe180214.onrender.com
- **API Demo**: https://ecommerce-assignment-qe180214.onrender.com/api/products
- **Repository**: https://github.com/tsohigh254/ecommerce-assignment-qe180214

## Additional Documentation
This consolidated document replaces:
- `README.md` (overview)
- `LOCAL_SETUP.md` (setup guide)
- `GRADING_GUIDE.md` (grading information)
- `CONSOLIDATED_DOCUMENTATION.md` (technical details)

## Support
For technical issues or questions:
1. Check the troubleshooting section above
2. Review the setup steps carefully
3. Verify Docker and .NET 8 are properly installed
4. Test with the online demo first to confirm expected behavior

---

**?? Built with ?? for Academic Excellence | Assignment 1 - 2025**
**© 2025 E-Commerce Platform QE180214 - All Documentation Consolidated**