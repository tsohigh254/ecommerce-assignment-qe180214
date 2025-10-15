# 🚀 Hướng Dẫn Deploy với Neon PostgreSQL và Render

## ⚠️ Sửa Lỗi Exit Status 139 (Segmentation Fault)

Lỗi này xảy ra do Render Free tier chỉ có **512MB RAM** trong khi .NET 8.0 mặc định tốn nhiều bộ nhớ hơn.

### ✅ Đã Tối Ưu:
1. **Alpine Linux**: Dùng image nhẹ hơn (từ ~210MB xuống ~110MB)
2. **Memory Limits**: Giới hạn GC Heap ở 50MB
3. **Disable Diagnostics**: Tắt các tính năng chẩn đoán tốn RAM
4. **Workstation GC**: Dùng GC mode cho client thay vì server

---

## 📋 BƯỚC 1: Tạo Database trên Neon

### 1.1. Đăng ký Neon
1. Truy cập: https://neon.tech/
2. Đăng nhập bằng GitHub
3. Click **"Create a project"**

### 1.2. Cấu hình Database
- **Project Name**: `ecommerce-qe180214`
- **Database Name**: `ecommerce`
- **Region**: `AWS Asia Pacific (Singapore)` hoặc gần bạn nhất
- **Compute Size**: Để mặc định (Free tier)

### 1.3. Lấy Connection String
Sau khi tạo xong, copy **Connection String**:
```
postgresql://[user]:[password]@[host]/[database]?sslmode=require
```

**Lưu ý:**
- Neon cung cấp 2 loại connection: **Pooled** và **Direct**
- Dùng **Pooled connection** cho production (tối ưu hơn)

---

## 📦 BƯỚC 2: Push Code Lên GitHub

```bash
# Commit các thay đổi Dockerfile mới
git add .
git commit -m "chore: optimize Dockerfile for Render free tier"
git push origin main
```

---

## 🚀 BƯỚC 3: Deploy API lên Render

### 3.1. Tạo Web Service
1. Đăng nhập https://render.com/
2. Click **"New +"** → **"Web Service"**
3. Click **"Connect account"** để kết nối GitHub (nếu chưa)
4. Chọn repository: `ecommerce-assignment-qe180214`
5. Click **"Connect"**

### 3.2. Cấu hình API Service

**Thông tin cơ bản:**
```
Name:              ecommerce-api-qe180214
Region:            Singapore
Branch:            main
Root Directory:    (để trống)
Runtime:           Docker
Dockerfile Path:   ./src/ECommerce.API/Dockerfile
```

**Instance:**
```
Instance Type:     Free (512MB RAM, shared CPU)
```

### 3.3. Environment Variables

Click **"Advanced"** → **"Add Environment Variable"**:

```env
# ASP.NET Core Configuration
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080

# Memory optimization (quan trọng!)
DOTNET_EnableDiagnostics=0
DOTNET_GCHeapHardLimit=100000000
COMPlus_GCHeapHardLimit=100000000
DOTNET_gcServer=0

# Database Connection (thay bằng Neon connection string)
ConnectionStrings__DefaultConnection=postgresql://[user]:[password]@[host]/[database]?sslmode=require

# JWT Settings (thay YOUR_SECRET bằng chuỗi ít nhất 32 ký tự)
JwtSettings__Secret=YOUR_SUPER_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG
JwtSettings__Issuer=https://ecommerce-api-qe180214.onrender.com
JwtSettings__Audience=https://ecommerce-web-qe180214.onrender.com,https://localhost:5001
JwtSettings__ExpiryMinutes=60

# CORS (sẽ update sau)
CorsSettings__AllowedOrigins=https://ecommerce-web-qe180214.onrender.com,http://localhost:5001

# Optional: Cloudinary (nếu dùng image upload)
CloudinarySettings__CloudName=your_cloud_name
CloudinarySettings__ApiKey=your_api_key
CloudinarySettings__ApiSecret=your_api_secret

# Optional: Stripe (nếu dùng payment)
StripeSettings__SecretKey=sk_test_...
StripeSettings__PublishableKey=pk_test_...
StripeSettings__WebhookSecret=whsec_...
```

### 3.4. Deploy
1. Click **"Create Web Service"**
2. Đợi build (7-10 phút lần đầu)
3. Theo dõi logs để đảm bảo không có lỗi
4. Lưu lại **API URL**: `https://ecommerce-api-qe180214.onrender.com`

---

## 🗄️ BƯỚC 4: Chạy Database Migrations

### Option 1: Từ Local Machine (Khuyến nghị)

```bash
# Đặt connection string vào biến môi trường
export ConnectionStrings__DefaultConnection="postgresql://[user]:[password]@[host]/[database]?sslmode=require"

# Chạy migrations
cd src/ECommerce.API
dotnet ef database update

# Hoặc chỉ định trực tiếp connection string
dotnet ef database update --connection "postgresql://[user]:[password]@[host]/[database]?sslmode=require"
```

### Option 2: Từ Render Shell

1. Vào API service trên Render dashboard
2. Click **"Shell"** ở menu bên trái
3. Chạy:
```bash
cd /app
dotnet ef database update
```

### Xác nhận Migrations thành công:
- Vào Neon dashboard → Tables
- Kiểm tra có các bảng: `Users`, `Products`, `Carts`, `Orders`, v.v.

---

## 🌐 BƯỚC 5: Deploy Web UI lên Render

### 5.1. Tạo Web Service mới
1. Click **"New +"** → **"Web Service"**
2. Chọn repository: `ecommerce-assignment-qe180214`
3. Click **"Connect"**

### 5.2. Cấu hình Web Service

**Thông tin cơ bản:**
```
Name:              ecommerce-web-qe180214
Region:            Singapore
Branch:            main
Root Directory:    (để trống)
Runtime:           Docker
Dockerfile Path:   ./src/ECommerce.Web/Dockerfile
Instance Type:     Free
```

### 5.3. Environment Variables

```env
# ASP.NET Core Configuration
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080

# Memory optimization
DOTNET_EnableDiagnostics=0
DOTNET_GCHeapHardLimit=100000000
COMPlus_GCHeapHardLimit=100000000
DOTNET_gcServer=0

# API Configuration (thay bằng API URL thật)
ApiSettings__BaseUrl=https://ecommerce-api-qe180214.onrender.com
```

### 5.4. Deploy
1. Click **"Create Web Service"**
2. Đợi build và deploy
3. Lưu lại **Web URL**: `https://ecommerce-web-qe180214.onrender.com`

---

## 🔄 BƯỚC 6: Update CORS Settings

Sau khi có cả API và Web URL, update CORS cho API:

1. Vào **API Service** trên Render
2. Click **"Environment"** → Tìm `CorsSettings__AllowedOrigins`
3. Update thành:
   ```
   https://ecommerce-web-qe180214.onrender.com
   ```
4. Click **"Save Changes"** → Service tự động redeploy

---

## ✅ BƯỚC 7: Kiểm Tra & Test

### 7.1. Test API Health

```bash
# Test API endpoint
curl https://ecommerce-api-qe180214.onrender.com/api/products

# Test với POST request
curl -X POST https://ecommerce-api-qe180214.onrender.com/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test@123456",
    "firstName": "Test",
    "lastName": "User"
  }'
```

### 7.2. Test Web UI

1. Mở browser: `https://ecommerce-web-qe180214.onrender.com`
2. Test flow:
   - ✅ Trang chủ load thành công
   - ✅ Register user mới
   - ✅ Login
   - ✅ Xem products
   - ✅ Add to cart
   - ✅ Place order
   - ✅ View order history

### 7.3. Check Logs

**API Logs:**
- Vào API service → **"Logs"**
- Kiểm tra không có error

**Web Logs:**
- Vào Web service → **"Logs"**
- Kiểm tra không có error

---

## 🐛 Troubleshooting

### ❌ Vẫn bị Exit Status 139

**Giải pháp 1: Giảm Memory Limit hơn nữa**
```env
DOTNET_GCHeapHardLimit=75000000
COMPlus_GCHeapHardLimit=75000000
```

**Giải pháp 2: Disable Response Compression**
Trong `Program.cs`:
```csharp
// Comment out hoặc xóa dòng này
// app.UseResponseCompression();
```

**Giải pháp 3: Sử dụng .NET 6 thay vì 8**
- .NET 6 nhẹ hơn và ổn định hơn trên free tier

### ❌ Database Connection Timeout

**Nguyên nhân:** Neon database bị suspend sau 5 phút không dùng (free tier)

**Giải pháp:**
- Request đầu tiên sẽ chậm (20-30s) để wake up database
- Đây là hạn chế của free tier, không thể tránh

### ❌ Build Failed

**Kiểm tra:**
```bash
# Test build local
docker build -f src/ECommerce.API/Dockerfile -t test-api .
docker build -f src/ECommerce.Web/Dockerfile -t test-web .
```

### ❌ CORS Error

**Kiểm tra:**
- API `CorsSettings__AllowedOrigins` có đúng Web URL không
- Không có trailing slash
- HTTPS chứ không phải HTTP

---

## 📊 Giới Hạn của Free Tier

### Render Free Tier:
- ✅ 512MB RAM
- ✅ Shared CPU
- ❌ **Sleep sau 15 phút** không hoạt động
- ❌ Request đầu tiên sau sleep: **30-60 giây**
- ✅ 750 giờ/tháng (đủ cho 1 tháng 24/7)

### Neon Free Tier:
- ✅ 512MB storage
- ✅ 1 project
- ✅ 10 branches
- ❌ **Autosuspend sau 5 phút** không hoạt động
- ❌ **100 giờ active time/tháng**
- ✅ Pooled connections

### Workaround cho Sleep Issue:
Dùng service ping miễn phí:
- https://uptimerobot.com/
- https://cron-job.org/
- Ping mỗi 10 phút để giữ service active

---

## 📝 Deployment Checklist

- [ ] Neon database đã tạo
- [ ] Connection string đã copy
- [ ] Code đã push lên GitHub
- [ ] API service đã tạo trên Render
- [ ] Environment variables đã set cho API
- [ ] API đã deploy thành công
- [ ] Database migrations đã chạy
- [ ] Web service đã tạo
- [ ] Environment variables đã set cho Web
- [ ] Web đã deploy thành công
- [ ] CORS đã update
- [ ] API test thành công (curl)
- [ ] Web test thành công (browser)
- [ ] Complete user flow test thành công
- [ ] Logs không có critical error

---

## 🎯 URLs Deployment

```
API:      https://ecommerce-api-qe180214.onrender.com
Web:      https://ecommerce-web-qe180214.onrender.com
Database: [Neon Internal - Không public]

GitHub:   https://github.com/tsohigh254/ecommerce-assignment-qe180214
```

---

## 📚 Tài Liệu Tham Khảo

- Render Docs: https://render.com/docs
- Neon Docs: https://neon.tech/docs
- .NET Memory Management: https://learn.microsoft.com/en-us/dotnet/core/runtime-config/garbage-collector
- Docker Multi-stage Builds: https://docs.docker.com/build/building/multi-stage/

---

**Last Updated:** October 15, 2025  
**Student:** QE180214  
**Assignment:** E-Commerce Web Application
