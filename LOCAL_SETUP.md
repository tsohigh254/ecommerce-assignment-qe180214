# 🔐 Local Development Setup Guide

## Quick Start

### 1. Prerequisites
- Docker Desktop installed and running
- Git

### 2. Clone & Setup

```bash
# Clone repository
git clone https://github.com/tsohigh254/ecommerce-assignment-qe180214.git
cd ecommerce-assignment-qe180214

# Optional: Create custom environment variables
# (Skip this step to use default values)
cp .env.example .env
# Edit .env if you want to change ports or passwords
```

### 3. Start Application

```bash
# Start all services
docker-compose up -d

# Check status
docker-compose ps

# View logs
docker-compose logs -f
```

### 4. Access Application

- **Web UI:** http://localhost:5173
- **API:** http://localhost:7246
- **Swagger Documentation:** http://localhost:7246/swagger
- **Database:** localhost:5432

### 5. Stop Application

```bash
# Stop services
docker-compose down

# Stop and remove volumes (reset database)
docker-compose down -v
```

---

## 🔧 Configuration

### Environment Variables

All sensitive configuration is managed through environment variables. Default values are provided for convenience:

| Variable | Default | Description |
|----------|---------|-------------|
| `DB_NAME` | `ecommerce` | PostgreSQL database name |
| `DB_USER` | `postgres` | PostgreSQL username |
| `DB_PASSWORD` | `postgres` | PostgreSQL password |
| `DB_PORT` | `5432` | PostgreSQL port |
| `API_PORT` | `7246` | API service port |
| `WEB_PORT` | `5173` | Web UI port |
| `ASPNETCORE_ENV` | `Development` | ASP.NET Core environment |

### Custom Configuration

To override defaults, create a `.env` file:

```bash
# Copy template
cp .env.example .env

# Edit values (example)
DB_PASSWORD=my_secure_password
API_PORT=8080
WEB_PORT=3000
```

---

## 🧪 Testing

### Test API Endpoints

```bash
# Get all products
curl http://localhost:7246/api/products

# Get single product
curl http://localhost:7246/api/products/1

# Create product
curl -X POST http://localhost:7246/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Test Product","description":"Test","price":99.99}'
```

### Test Web Interface

1. Open browser: http://localhost:5173
2. Browse products
3. Create new product
4. Edit/Delete products

---

## 🐛 Troubleshooting

### Port Already in Use

If ports are occupied, change them in `.env`:

```bash
DB_PORT=5433
API_PORT=7247
WEB_PORT=5174
```

Then restart:

```bash
docker-compose down
docker-compose up -d
```

### Database Connection Issues

```bash
# Check postgres is healthy
docker-compose ps

# View postgres logs
docker-compose logs postgres

# Restart postgres
docker-compose restart postgres
```

### Reset Everything

```bash
# Stop all services and remove data
docker-compose down -v

# Remove all containers and images
docker-compose down --rmi all -v

# Start fresh
docker-compose up -d
```

---

## 📁 Project Structure

```
ecommerce-assignment-qe180214/
├── src/
│   ├── ECommerce.API/        # Backend API
│   ├── ECommerce.Web/        # Frontend Web
│   └── ECommerce.Core/       # Shared Models
├── docker-compose.yml        # Docker configuration
├── .env.example             # Environment template
├── .gitignore               # Git ignore rules
└── README.md                # Main documentation
```

---

## 🔒 Security Notes

### For Instructors/Reviewers:

This project uses **environment variables** for configuration:

- ✅ **No hardcoded passwords** in code
- ✅ **Default values** for quick local setup
- ✅ **.env files** are git-ignored
- ✅ **Production uses** separate secure configuration

### Default Credentials (Local Only):

- **Database User:** `postgres`
- **Database Password:** `postgres` (can be changed in .env)

These defaults are **ONLY for local development** and are **NOT used in production**.

### Production Configuration:

Production deployment (Render.com) uses:
- ✅ Separate environment variables
- ✅ Strong passwords from Neon.tech
- ✅ HTTPS encryption
- ✅ No exposed database ports

---

## 📚 Additional Resources

- **Full Documentation:** See `CONSOLIDATED_DOCUMENTATION.md`
- **Live Demo:** https://ecommerce-web-qe180214.onrender.com
- **API Documentation:** https://ecommerce-assignment-qe180214.onrender.com/swagger (dev only)
- **GitHub Repository:** https://github.com/tsohigh254/ecommerce-assignment-qe180214

---

## 👨‍🎓 Student Information

- **Student ID:** QE180214
- **Assignment:** CRUD REST API + UI
- **Framework:** .NET 8
- **Database:** PostgreSQL

---

**Note:** This setup is optimized for ease of grading and local testing. For production deployment instructions, see `CONSOLIDATED_DOCUMENTATION.md`.
