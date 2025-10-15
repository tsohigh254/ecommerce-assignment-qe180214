# 🔧 Hướng Dẫn Khắc Phục Sự Cố - E-Commerce Assignment

## Các Vấn Đề Thường Gặp và Cách Giải Quyết

### 1. ❌ Lỗi Đăng Nhập Sau Khi Deploy

**Triệu chứng:** Không thể đăng nhập trên trang web đã deploy, nhưng hoạt động bình thường ở local.

**Nguyên nhân có thể:**
- JWT settings chưa được cấu hình đúng
- Database không thể truy cập
- Environment variables chưa được thiết lập

**Giải pháp:**

1. **Kiểm tra Environment Variables trên Render:**
   - Vào Render Dashboard → API Service của bạn → Environment
   - Xác nhận các biến sau đã được thiết lập:
     ```
     ASPNETCORE_ENVIRONMENT=Production
     ConnectionStrings__DefaultConnection=postgresql://user:pass@host/db
     JwtSettings__SecretKey=your-32-char-secret-key
     JwtSettings__Issuer=ECommerceAPI
     JwtSettings__Audience=ECommerceWebApp
     JwtSettings__ExpirationInMinutes=60
     ```

2. **Kiểm tra kết nối Database:**
   - Xác nhận database PostgreSQL đang chạy
   - Kiểm tra format connection string: `postgresql://username:password@host:port/database`

3. **Kiểm tra API Logs:**
   ```bash
   # Trong Render Dashboard:
   # Vào API service → tab Logs
   # Tìm các lỗi như:
   # - "Database connection failed"
   # - "JWT settings not configured"
   ```

---

### 2. ❌ API Trả Về Dữ Liệu Rỗng (Không Có Sản Phẩm)

**Triệu chứng:** `/api/products` trả về `{"products":[],"totalCount":0}` hoặc response rỗng.

**Nguyên nhân có thể:**
- Database migrations chưa được áp dụng
- Seed data chưa được insert
- Đang dùng `EnsureCreated()` thay vì `Migrate()`

**Giải pháp:**

✅ **Đã sửa trong commit này** - Code hiện sử dụng `context.Database.Migrate()` để:
- Áp dụng tất cả migrations
- Insert seed data từ `ECommerceDbContext.OnModelCreating()`

**Để xác nhận fix đã hoạt động:**
1. Redeploy API lên Render
2. Kiểm tra logs có dòng: `"Database migration completed successfully"`
3. Phải thấy: `"Database contains X products"`
4. Nếu hiển thị 0 products, kiểm tra seed data trong `ECommerceDbContext.cs`

**Cách fix thủ công (nếu vẫn không hoạt động):**

```bash
# Kết nối tới Render PostgreSQL database
# Cách 1: Dùng Render Shell
# Vào Render Dashboard → Database → Shell

# Cách 2: Dùng psql ở local
psql "postgresql://username:password@host:port/database"

# Kiểm tra sản phẩm có tồn tại không
SELECT COUNT(*) FROM "Products";

# Nếu = 0, insert seed data thủ công:
INSERT INTO "Products" ("Id", "Name", "Description", "Price", "ImageUrl", "CreatedAt", "UpdatedAt") VALUES
(1, 'Classic T-Shirt', 'Comfortable cotton t-shirt perfect for everyday wear', 29.99, 'https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400', '2024-01-01', '2024-01-01'),
(2, 'Denim Jacket', 'Stylish denim jacket for a casual look', 89.99, 'https://images.unsplash.com/photo-1544966503-7cc5ac882d5f?w=400', '2024-01-01', '2024-01-01'),
(3, 'Summer Dress', 'Light and breezy summer dress in floral pattern', 59.99, 'https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400', '2024-01-01', '2024-01-01');
```

---

### 3. ❌ Render Service Bị Spin Down / Phản Hồi Chậm

**Triệu chứng:** Request đầu tiên mất 30-60 giây, sau đó hoạt động bình thường.

**Nguyên nhân:** Render free tier tự động tắt service sau 15 phút không hoạt động.

**Giải pháp:**
- **Đây là hành vi bình thường** trên free tier
- Request đầu tiên sẽ "đánh thức" service
- Nâng cấp lên paid plan để hoạt động 24/7
- Hoặc dùng service như UptimeRobot để ping site mỗi 5 phút

---

### 4. ❌ Lỗi CORS Trên Browser

**Triệu chứng:** Browser console hiển thị `CORS policy: No 'Access-Control-Allow-Origin' header`

**Giải pháp:**

Kiểm tra `Program.cs` đã enable CORS (đã được cấu hình):
```csharp
// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Enable CORS
app.UseCors("AllowAll");
```

---

### 5. ❌ Thanh Toán Stripe Thất Bại

**Triệu chứng:** Nút thanh toán không hoạt động hoặc trả về lỗi.

**Giải pháp:**

1. **Kiểm tra Stripe Keys:**
   ```bash
   # Trong Render Dashboard → API Service → Environment
   StripeSettings__SecretKey=sk_test_... hoặc sk_live_...
   StripeSettings__PublishableKey=pk_test_... hoặc pk_live_...
   ```

2. **Xác nhận Webhook:**
   - Stripe Dashboard → Webhooks
   - Endpoint URL: `https://your-api.onrender.com/api/webhook/stripe`
   - Events: `checkout.session.completed`, `payment_intent.succeeded`
   - Copy Webhook Secret → Thêm vào Render env: `StripeSettings__WebhookSecret`

3. **Test Mode vs Live Mode:**
   - Dùng `sk_test_` và `pk_test_` keys để test
   - Đặt `StripeSettings__TestMode=true` trong development
   - Dùng `sk_live_` và `pk_live_` cho production

---

### 6. ❌ Upload Ảnh Không Hoạt Động

**Triệu chứng:** Không thể upload ảnh sản phẩm.

**Giải pháp:**

1. **Kiểm tra Cloudinary Settings:**
   ```bash
   CloudinarySettings__CloudName=your_cloud_name
   CloudinarySettings__ApiKey=your_api_key
   CloudinarySettings__ApiSecret=your_api_secret
   ```

2. **Lấy Cloudinary Credentials:**
   - Đăng ký tại https://cloudinary.com (free tier: 25 GB)
   - Dashboard → Account Details
   - Copy Cloud Name, API Key, API Secret

---

### 7. ❌ Lỗi Kết Nối Database

**Triệu chứng:** API logs hiển thị "Cannot connect to database" hoặc "Connection refused"

**Giải pháp:**

1. **Kiểm tra Format Connection String:**
   ```
   # Format Render PostgreSQL Internal URL:
   postgresql://username:password@hostname:5432/database_name
   
   # Lỗi thường gặp:
   ❌ postgres:// (sai protocol)
   ❌ Thiếu port :5432
   ❌ Sai tên database
   ```

2. **Dùng Internal Database URL:**
   - Render Dashboard → Database → Internal Database URL
   - Dùng URL này cho API service (nhanh hơn, miễn phí)
   - Không dùng External Database URL trừ khi kết nối từ bên ngoài Render

3. **Kiểm tra Database Đang Chạy:**
   - Render Dashboard → Database → Phải hiển thị "Available"
   - Nếu bị suspended, nâng cấp plan hoặc kiểm tra giới hạn free tier

---

### 8. ❌ Lỗi Migration

**Triệu chứng:** Logs hiển thị "Migration failed" hoặc "Table already exists"

**Giải pháp:**

**Cách 1: Reset Database (⚠️ Xóa toàn bộ dữ liệu)**
```bash
# Trong Render Database Shell:
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;

# Sau đó redeploy API để chạy migrations mới
```

**Cách 2: Migration Thủ Công**
```bash
# Ở local, tạo migration bundle:
cd src/ECommerce.API
dotnet ef migrations bundle --output migrations-bundle

# Upload bundle lên Render và chạy qua SSH hoặc startup command
```

---

## 🛠️ Checklist Debug

Khi có vấn đề, kiểm tra theo thứ tự sau:

1. ✅ **Trạng Thái Render Service**
   - Dashboard hiển thị "Available" (không phải "Suspended" hoặc "Failed")

2. ✅ **Environment Variables**
   - Tất cả biến cần thiết đã được set
   - Không có lỗi đánh máy trong tên biến
   - Giá trị đúng (không phải placeholder text)

3. ✅ **Kết Nối Database**
   - Database đang chạy
   - Connection string đúng
   - Đang dùng Internal URL (không phải External)

4. ✅ **API Logs**
   - Kiểm tra lỗi khi startup
   - Tìm message về kết nối database
   - Kiểm tra "Migration completed successfully"

5. ✅ **Network/CORS**
   - API có thể truy cập: `curl https://your-api.onrender.com/api/products`
   - CORS headers có trong response

6. ✅ **Cấu Hình Frontend**
   - Web service có đúng `ApiSettings__BaseUrl`
   - Trỏ tới API service (không phải localhost)

---

## 🔍 Log Messages Thường Gặp

### ✅ Dấu Hiệu Tốt:
```
✓ "Database migration completed successfully"
✓ "Database contains 3 products"
✓ "Application started. Press Ctrl+C to shut down."
✓ "Now listening on: http://0.0.0.0:10000"
```

### ❌ Dấu Hiệu Lỗi:
```
✗ "Database connection string not configured"
   → Fix: Thêm ConnectionStrings__DefaultConnection env variable

✗ "No products found in database"
   → Fix: Seed data chưa được áp dụng, kiểm tra migration

✗ "Cannot connect to database"
   → Fix: Kiểm tra format connection string và trạng thái database

✗ "JWT settings not configured"
   → Fix: Thêm JwtSettings__* environment variables
```

---

## 📞 Vẫn Còn Vấn Đề?

1. **Kiểm Tra Thay Đổi Gần Đây:**
   - So sánh với phiên bản local đang hoạt động
   - Review các commit gần đây có thể gây lỗi

2. **Test Ở Local Trước:**
   ```bash
   cd src/ECommerce.API
   dotnet run
   # Test tại http://localhost:5000/api/products
   ```

3. **Kiểm Tra Render Status Page:**
   - https://status.render.com
   - Tìm các sự cố đang diễn ra

4. **Xem Lại Documentation:**
   - `DEPLOYMENT_GUIDE.md` - Hướng dẫn setup đầy đủ
   - `SETUP_AND_TESTING_GUIDE.md` - Test ở local
   - `README.md` - Tổng quan project

---

## 🎯 Bảng Tóm Tắt Quick Fixes

| Vấn Đề | Cách Fix Nhanh |
|--------|----------------|
| Không có sản phẩm | Redeploy API (đã dùng Migrate()) |
| Login thất bại | Kiểm tra JWT environment variables |
| Load chậm lần đầu | Bình thường trên free tier (service spin-up) |
| Lỗi CORS | Xác nhận CORS policy trong Program.cs |
| Lỗi kết nối DB | Dùng Internal Database URL từ Render |
| Stripe payment lỗi | Xác nhận webhook secret và endpoint |
| Upload ảnh lỗi | Thêm Cloudinary credentials |

---

**Cập Nhật Lần Cuối:** 15 Tháng 10, 2025  
**Phiên Bản:** 1.0
