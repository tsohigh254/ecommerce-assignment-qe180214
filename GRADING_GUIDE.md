# 👨‍🏫 Hướng Dẫn Chấm Bài - Assignment QE180214

## 📋 Thông Tin Sinh Viên
- **Mã SV:** QE180214
- **Bài tập:** Assignment 1 - CRUD REST API + UI
- **Công nghệ:** .NET 8, PostgreSQL, Docker

---

## 🚀 Cách Chạy Nhanh (3 Lệnh)

### 1️⃣ Clone Project
```bash
git clone https://github.com/tsohigh254/ecommerce-assignment-qe180214.git
cd ecommerce-assignment-qe180214
```

### 2️⃣ Khởi Động (Docker Desktop phải đang chạy)
```bash
docker-compose up -d
```

### 3️⃣ Đợi 30 giây và truy cập:
- **Giao diện Web:** http://localhost:5173
- **API + Swagger:** http://localhost:7246/swagger
- **Database:** localhost:5432 (postgres/postgres)

### ⏹️ Dừng ứng dụng:
```bash
docker-compose down
```

---

## ✅ Các Tính Năng Cần Kiểm Tra

### 1. API Endpoints (Swagger UI)
Truy cập: http://localhost:7246/swagger

✅ **GET** `/api/products` - Lấy danh sách sản phẩm  
✅ **GET** `/api/products/{id}` - Lấy 1 sản phẩm  
✅ **POST** `/api/products` - Tạo sản phẩm mới  
✅ **PUT** `/api/products/{id}` - Cập nhật sản phẩm  
✅ **DELETE** `/api/products/{id}` - Xóa sản phẩm  

### 2. Giao Diện Web
Truy cập: http://localhost:5173

✅ Hiển thị danh sách sản phẩm  
✅ Xem chi tiết sản phẩm  
✅ Tạo sản phẩm mới  
✅ Chỉnh sửa sản phẩm  
✅ Xóa sản phẩm  
✅ Responsive design (mobile/tablet/desktop)  

### 3. Database
- PostgreSQL tự động được tạo và seed data
- 3 sản phẩm mẫu có sẵn khi khởi động

---

## 🌐 Ứng Dụng Online (Đã Deploy)

Nếu không muốn chạy local, có thể test online:

- **Frontend:** https://ecommerce-web-qe180214.onrender.com
- **API:** https://ecommerce-assignment-qe180214.onrender.com/api/products

⚠️ **Lưu ý:** Lần đầu truy cập có thể chậm (~30s) do free tier sleep mode.

---

## 🔒 Bảo Mật

### Cấu hình Local Development:
- ✅ **Không có hardcoded passwords** trong code
- ✅ Sử dụng **environment variables** với default values
- ✅ File `.env` được git-ignore (nếu tạo)
- ✅ Có `.env.example` làm template

### Mật khẩu mặc định (chỉ cho local):
- Database: `postgres/postgres`
- Có thể thay đổi bằng cách tạo file `.env` (không bắt buộc)

### Production (Online):
- Sử dụng environment variables riêng từ hosting platform
- Mật khẩu mạnh từ Neon.tech PostgreSQL
- HTTPS encryption
- KHÔNG sử dụng cấu hình local

---

## 📁 Cấu Trúc Project

```
ecommerce-assignment-qe180214/
├── src/
│   ├── ECommerce.API/         # Backend REST API
│   │   ├── Controllers/       # API Controllers
│   │   ├── Data/             # Database Context
│   │   └── Dockerfile        # API Container
│   ├── ECommerce.Web/        # Frontend MVC
│   │   ├── Controllers/      # MVC Controllers
│   │   ├── Views/            # Razor Views
│   │   ├── Services/         # HTTP Client
│   │   └── Dockerfile        # Web Container
│   └── ECommerce.Core/       # Shared Models
│       └── Models/           # Product Entity
├── docker-compose.yml        # Docker orchestration
├── .env.example             # Env variables template
├── README.md                # Main documentation
└── LOCAL_SETUP.md          # Detailed setup guide
```

---

## 🛠️ Troubleshooting

### Lỗi: Port đã được sử dụng
```bash
# Kiểm tra port 5173, 7246, 5432 có đang được dùng không
netstat -ano | findstr "5173"
netstat -ano | findstr "7246"
netstat -ano | findstr "5432"

# Hoặc thay đổi port trong .env
```

### Lỗi: Docker không khởi động
```bash
# Kiểm tra Docker Desktop đã chạy chưa
docker ps

# Xem logs
docker-compose logs

# Khởi động lại
docker-compose down
docker-compose up -d
```

### Reset toàn bộ database
```bash
docker-compose down -v
docker-compose up -d
```

---

## 📊 Công Nghệ Sử Dụng

| Layer | Technology |
|-------|------------|
| **Backend API** | ASP.NET Core 8.0 Web API |
| **Frontend** | ASP.NET Core 8.0 MVC + Razor |
| **Database** | PostgreSQL 13 |
| **ORM** | Entity Framework Core 9.0 |
| **UI Framework** | Bootstrap 5 + Font Awesome 6 |
| **Containerization** | Docker + Docker Compose |
| **Cloud Hosting** | Render.com + Neon.tech |

---

## 📚 Tài Liệu Bổ Sung

- **README.md** - Tài liệu tổng quan
- **LOCAL_SETUP.md** - Hướng dẫn setup chi tiết
- **CONSOLIDATED_DOCUMENTATION.md** - Tài liệu đầy đủ về project

---

## 📞 Liên Hệ

**Student ID:** QE180214  
**GitHub:** https://github.com/tsohigh254/ecommerce-assignment-qe180214  
**Live Demo:** https://ecommerce-web-qe180214.onrender.com

---

## ✅ Checklist Chấm Bài

- [ ] Backend API có đầy đủ 5 CRUD endpoints
- [ ] Frontend có giao diện đầy đủ cho CRUD
- [ ] Database được tích hợp và hoạt động
- [ ] Code structure rõ ràng, dễ đọc
- [ ] Có documentation đầy đủ
- [ ] Ứng dụng chạy thành công bằng docker-compose
- [ ] UI responsive và professional
- [ ] Deploy thành công lên cloud
- [ ] Bảo mật: không có hardcoded credentials
- [ ] Error handling được implement

---

**🎓 Cảm ơn thầy đã chấm bài!**
