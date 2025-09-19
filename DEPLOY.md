# 🚀 DEPLOY TO RENDER.COM

## Step 1: Tạo tài khoản Render.com
1. Đi đến [render.com](https://render.com)
2. Sign up with GitHub account
3. Authorize Render to access GitHub repositories

## Step 2: Deploy API
1. Vào Render dashboard
2. Click "New +" → "Web Service"
3. Connect GitHub repository: `tsohigh254/ecommerce-assignment-qe180214`
4. Cấu hình:
   - **Name:** `ecommerce-api-qe180214`
   - **Branch:** `main`
   - **Root Directory:** `/` (để trống)
   - **Runtime:** `Docker`
   - **Dockerfile Path:** `src/ECommerce.API/Dockerfile`
   - **Instance Type:** `Free`

## Step 3: Set Environment Variables
Trong Render dashboard, đi đến "Environment" và thêm:

```
ASPNETCORE_ENVIRONMENT=Production
DATABASE_CONNECTION_STRING=Host=ep-bitter-heart-a1fstxow-pooler.ap-southeast-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_DuZ63vKLnYzH;SslMode=Require
```

## Step 4: Deploy
1. Click "Create Web Service"
2. Render sẽ auto-deploy từ GitHub
3. Đợi build complete (~5-10 phút)
4. Test API endpoint: `https://your-service-name.onrender.com/api/products`

## 🔧 Troubleshooting
- Check build logs nếu failed
- Verify environment variables
- Test database connection từ Render logs

## 📋 Expected Results
- ✅ API deployed tại: `https://ecommerce-api-qe180214.onrender.com`
- ✅ Endpoint: `/api/products` returns 3 products
- ✅ All CRUD operations working
- ✅ Connected to Neon.tech PostgreSQL