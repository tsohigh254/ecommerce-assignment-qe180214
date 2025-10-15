# 🚀 Hướng Dẫn Deployment - E-Commerce Application

## 📋 Chuẩn Bị Trước Khi Deploy

### 1. Kiểm Tra Code Đã Push Lên GitHub
```bash
git status
git push origin main
```

### 2. Chuẩn Bị Database (PostgreSQL)

**Option 1: Sử dụng Render PostgreSQL (Recommended)**
- Miễn phí, dễ tích hợp với Render services
- URL: https://render.com/

**Option 2: Sử dụng Supabase**
- Miễn phí, có UI quản lý database
- URL: https://supabase.com/

**Option 3: Sử dụng ElephantSQL**
- Miễn phí 20MB
- URL: https://www.elephantsql.com/

---

## 🗄️ BƯỚC 1: Tạo PostgreSQL Database

### Sử Dụng Render PostgreSQL:

1. Đăng nhập vào https://render.com/
2. Click **"New +"** → **"PostgreSQL"**
3. Điền thông tin:
   - **Name**: `ecommerce-db-qe180214`
   - **Database**: `ecommerce`
   - **User**: `ecommerce_user` (tự động)
   - **Region**: `Singapore`
   - **PostgreSQL Version**: `16`
   - **Plan**: **Free**
4. Click **"Create Database"**
5. Đợi database được tạo (1-2 phút)
6. **Copy Connection String** (Internal Database URL):
   ```
   postgres://username:password@hostname/database
   ```
   hoặc
   ```
   postgresql://username:password@hostname/database
   ```

### Lấy Connection String:
- Vào tab **"Info"** của database
- Copy **"Internal Database URL"** (dùng cho services trong Render)
- Hoặc **"External Database URL"** (dùng cho local development)

---

## 🚀 BƯỚC 2: Deploy API Lên Render

### 2.1. Tạo API Service

1. Đăng nhập Render: https://render.com/
2. Click **"New +"** → **"Web Service"**
3. Chọn **"Build and deploy from a Git repository"**
4. Click **"Connect"** GitHub repository của bạn
5. Chọn repo: `ecommerce-assignment-qe180214`

### 2.2. Cấu Hình API Service

**Basic Settings:**
- **Name**: `ecommerce-api-qe180214`
- **Region**: `Singapore`
- **Branch**: `main`
- **Root Directory**: Để trống
- **Runtime**: `Docker`
- **Dockerfile Path**: `./src/ECommerce.API/Dockerfile`

**Instance Type:**
- **Plan**: `Free`

### 2.3. Environment Variables cho API

Click **"Advanced"** → **"Add Environment Variable"**:

```env
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080

# Database Connection
ConnectionStrings__DefaultConnection=postgres://your-database-connection-string

# JWT Settings
JwtSettings__Secret=YOUR-SUPER-SECRET-KEY-CHANGE-THIS-IN-PRODUCTION-MIN-32-CHARS
JwtSettings__Issuer=https://ecommerce-api-qe180214.onrender.com
JwtSettings__Audience=https://ecommerce-web-qe180214.onrender.com
JwtSettings__ExpiryMinutes=60

# CORS (sẽ update sau khi có Web URL)
CorsSettings__AllowedOrigins=https://ecommerce-web-qe180214.onrender.com

# Cloudinary (nếu dùng image upload)
CloudinarySettings__CloudName=your_cloud_name
CloudinarySettings__ApiKey=your_api_key
CloudinarySettings__ApiSecret=your_api_secret

# Stripe (nếu dùng payment)
StripeSettings__SecretKey=your_stripe_secret_key
StripeSettings__PublishableKey=your_stripe_publishable_key
StripeSettings__WebhookSecret=your_webhook_secret
```

**⚠️ QUAN TRỌNG:**
- Thay `YOUR-SUPER-SECRET-KEY-...` bằng key thật (ít nhất 32 ký tự)
- Thay database connection string
- Update URL sau khi deploy xong

### 2.4. Deploy API

1. Click **"Create Web Service"**
2. Đợi build và deploy (5-10 phút)
3. Kiểm tra logs để đảm bảo không có lỗi
4. **Copy API URL**: `https://ecommerce-api-qe180214.onrender.com`

### 2.5. Chạy Database Migrations

**Option 1: Từ Local (Recommended)**
```bash
# Update connection string trong appsettings.Production.json hoặc dùng biến môi trường
export ConnectionStrings__DefaultConnection="postgres://your-external-database-url"

cd src/ECommerce.API
dotnet ef database update --connection "postgres://your-external-database-url"
```

**Option 2: Từ Render Shell**
1. Vào API service trên Render
2. Click **"Shell"**
3. Chạy:
```bash
cd /app
dotnet ef database update
```

---

## 🌐 BƯỚC 3: Deploy Web UI Lên Render

### 3.1. Tạo Web Service

1. Click **"New +"** → **"Web Service"**
2. Chọn repository: `ecommerce-assignment-qe180214`
3. Click **"Connect"**

### 3.2. Cấu Hình Web Service

**Basic Settings:**
- **Name**: `ecommerce-web-qe180214`
- **Region**: `Singapore`
- **Branch**: `main`
- **Root Directory**: Để trống
- **Runtime**: `Docker`
- **Dockerfile Path**: `./src/ECommerce.Web/Dockerfile`

**Instance Type:**
- **Plan**: `Free`

### 3.3. Environment Variables cho Web

```env
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080

# API URL (thay bằng API URL thật từ bước 2)
ApiSettings__BaseUrl=https://ecommerce-api-qe180214.onrender.com
```

### 3.4. Deploy Web

1. Click **"Create Web Service"**
2. Đợi build và deploy (5-10 phút)
3. **Copy Web URL**: `https://ecommerce-web-qe180214.onrender.com`

---

## 🔄 BƯỚC 4: Update CORS Settings

Sau khi có cả API và Web URL, cần update CORS cho API:

1. Vào **API Service** trên Render
2. Click **"Environment"**
3. Tìm `CorsSettings__AllowedOrigins`
4. Update value:
   ```
   https://ecommerce-web-qe180214.onrender.com
   ```
5. Click **"Save Changes"**
6. Service sẽ tự động redeploy

---

## ✅ BƯỚC 5: Kiểm Tra Deployment

### 5.1. Test API Endpoints

```bash
# Test Health Check (nếu có)
curl https://ecommerce-api-qe180214.onrender.com/api/health

# Test Get Products
curl https://ecommerce-api-qe180214.onrender.com/api/products

# Test Register
curl -X POST https://ecommerce-api-qe180214.onrender.com/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test@123456",
    "firstName": "Test",
    "lastName": "User"
  }'
```

### 5.2. Test Web UI

1. Mở browser: `https://ecommerce-web-qe180214.onrender.com`
2. Kiểm tra:
   - ✅ Trang chủ hiển thị
   - ✅ Products list hiển thị
   - ✅ Register form hoạt động
   - ✅ Login form hoạt động
   - ✅ Cart functionality
   - ✅ Order placement

### 5.3. Test Complete Flow

1. **Register** tài khoản mới
2. **Login** với tài khoản vừa tạo
3. **Create Product** (chỉ khi đã login)
4. **Add to Cart**
5. **View Cart**
6. **Place Order**
7. **View Order History**
8. **Logout**

---

## 🐛 Troubleshooting

### Lỗi: "Failed to connect to database"
**Giải pháp:**
- Kiểm tra connection string có đúng không
- Đảm bảo database đã được tạo
- Kiểm tra firewall/network của database
- Dùng **Internal Database URL** cho services trong Render

### Lỗi: "CORS policy"
**Giải pháp:**
- Kiểm tra `CorsSettings__AllowedOrigins` trong API có đúng Web URL không
- Đảm bảo không có trailing slash trong URL
- Redeploy API sau khi update CORS

### Lỗi: "JWT token invalid"
**Giải pháp:**
- Kiểm tra `JwtSettings__Secret` có giống nhau giữa API và Web không
- Đảm bảo secret key đủ dài (>= 32 ký tự)
- Check `JwtSettings__Issuer` và `JwtSettings__Audience`

### Lỗi: Build failed
**Giải pháp:**
- Kiểm tra Dockerfile có đúng không
- Check logs để xem lỗi cụ thể
- Đảm bảo tất cả packages đã được restore

### Service Sleep (Free Plan)
**Lưu ý:**
- Free plan trên Render sẽ **sleep sau 15 phút** không hoạt động
- Request đầu tiên sau khi sleep sẽ **mất 30-60 giây** để wake up
- Đây là hạn chế của free plan

---

## 📊 Monitoring & Logs

### Xem Logs

1. Vào service trên Render dashboard
2. Click tab **"Logs"**
3. Xem real-time logs
4. Filter logs nếu cần

### Metrics

1. Click tab **"Metrics"**
2. Xem:
   - CPU usage
   - Memory usage
   - Response time
   - Request count

---

## 🔐 Security Best Practices

### ✅ Đã Làm:
- ✅ JWT authentication
- ✅ HTTPS (tự động bởi Render)
- ✅ CORS configuration
- ✅ Environment variables cho secrets

### 🔒 Nên Làm Thêm:
- [ ] Thay đổi JWT secret key mạnh hơn
- [ ] Enable rate limiting
- [ ] Add input validation
- [ ] Implement refresh tokens
- [ ] Add logging và monitoring

---

## 📝 Checklist Deploy

### Pre-Deployment
- [x] Code đã push lên GitHub
- [x] Dockerfile đã test locally
- [x] Database schema đã sẵn sàng
- [x] Environment variables đã chuẩn bị

### Database
- [ ] PostgreSQL database đã tạo
- [ ] Connection string đã copy
- [ ] Migrations đã chạy thành công

### API Deployment
- [ ] API service đã tạo
- [ ] Environment variables đã set
- [ ] API đã deploy thành công
- [ ] API endpoints đã test

### Web Deployment
- [ ] Web service đã tạo
- [ ] API URL đã set đúng
- [ ] Web đã deploy thành công
- [ ] Web UI đã test

### Post-Deployment
- [ ] CORS đã update
- [ ] Complete flow đã test
- [ ] Logs không có error
- [ ] Documentation đã update

---

## 📦 URLs Sau Khi Deploy

```
API URL:  https://ecommerce-api-qe180214.onrender.com
Web URL:  https://ecommerce-web-qe180214.onrender.com
Database: (Internal - không public)
```

---

## 🎯 Next Steps

1. ✅ Deploy thành công
2. Test toàn bộ tính năng
3. Fix bugs nếu có
4. Update documentation
5. Chuẩn bị submission document
6. Submit assignment

---

**Created:** October 15, 2025  
**Student:** QE180214  
**Project:** E-Commerce Assignment 2
