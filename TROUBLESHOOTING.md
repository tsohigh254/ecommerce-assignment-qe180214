# 🔧 Troubleshooting Guide - E-Commerce Assignment

## Common Issues and Solutions

### 1. ❌ Login Error After Deployment

**Symptom:** Unable to login on deployed site, but works locally.

**Possible Causes:**
- JWT settings not configured correctly
- Database not accessible
- Environment variables not set

**Solution:**

1. **Check Render Environment Variables:**
   - Go to Render Dashboard → Your API Service → Environment
   - Verify these variables are set:
     ```
     ASPNETCORE_ENVIRONMENT=Production
     ConnectionStrings__DefaultConnection=postgresql://user:pass@host/db
     JwtSettings__SecretKey=your-32-char-secret-key
     JwtSettings__Issuer=ECommerceAPI
     JwtSettings__Audience=ECommerceWebApp
     JwtSettings__ExpirationInMinutes=60
     ```

2. **Check Database Connection:**
   - Verify the PostgreSQL database is running
   - Test connection string format: `postgresql://username:password@host:port/database`

3. **Check API Logs:**
   ```bash
   # In Render Dashboard:
   # Go to your API service → Logs tab
   # Look for errors like:
   # - "Database connection failed"
   # - "JWT settings not configured"
   ```

---

### 2. ❌ API Returns Empty Data (No Products)

**Symptom:** `/api/products` returns `{"products":[],"totalCount":0}` or empty response.

**Possible Causes:**
- Database migrations not applied
- Seed data not inserted
- Using `EnsureCreated()` instead of `Migrate()`

**Solution:**

✅ **Fixed in this commit** - The code now uses `context.Database.Migrate()` which properly:
- Applies all migrations
- Inserts seed data from `ECommerceDbContext.OnModelCreating()`

**To verify the fix worked:**
1. Redeploy your API to Render
2. Check logs for: `"Database migration completed successfully"`
3. Should see: `"Database contains X products"`
4. If it shows 0 products, check seed data in `ECommerceDbContext.cs`

**Manual Fix (if still not working):**

```bash
# Connect to your Render PostgreSQL database
# Option 1: Use Render Shell
# Go to Render Dashboard → Database → Shell

# Option 2: Use psql locally
psql "postgresql://username:password@host:port/database"

# Check if products exist
SELECT COUNT(*) FROM "Products";

# If 0, insert seed data manually:
INSERT INTO "Products" ("Id", "Name", "Description", "Price", "ImageUrl", "CreatedAt", "UpdatedAt") VALUES
(1, 'Classic T-Shirt', 'Comfortable cotton t-shirt perfect for everyday wear', 29.99, 'https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400', '2024-01-01', '2024-01-01'),
(2, 'Denim Jacket', 'Stylish denim jacket for a casual look', 89.99, 'https://images.unsplash.com/photo-1544966503-7cc5ac882d5f?w=400', '2024-01-01', '2024-01-01'),
(3, 'Summer Dress', 'Light and breezy summer dress in floral pattern', 59.99, 'https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400', '2024-01-01', '2024-01-01');
```

---

### 3. ❌ Render Service Spinning Down / Slow Response

**Symptom:** First request takes 30-60 seconds, then works fine.

**Cause:** Render free tier spins down services after 15 minutes of inactivity.

**Solution:**
- **Expected behavior** on free tier
- First request "wakes up" the service
- Upgrade to paid plan for 24/7 uptime
- Or use a service like UptimeRobot to ping your site every 5 minutes

---

### 4. ❌ CORS Errors in Browser

**Symptom:** Browser console shows `CORS policy: No 'Access-Control-Allow-Origin' header`

**Solution:**

Check `Program.cs` has CORS enabled (already configured):
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

### 5. ❌ Stripe Payment Fails

**Symptom:** Payment button doesn't work or returns error.

**Solutions:**

1. **Check Stripe Keys:**
   ```bash
   # In Render Dashboard → API Service → Environment
   StripeSettings__SecretKey=sk_test_... or sk_live_...
   StripeSettings__PublishableKey=pk_test_... or pk_live_...
   ```

2. **Verify Webhook:**
   - Stripe Dashboard → Webhooks
   - Endpoint URL: `https://your-api.onrender.com/api/webhook/stripe`
   - Events: `checkout.session.completed`, `payment_intent.succeeded`
   - Copy Webhook Secret → Add to Render env: `StripeSettings__WebhookSecret`

3. **Test Mode vs Live Mode:**
   - Use `sk_test_` and `pk_test_` keys for testing
   - Set `StripeSettings__TestMode=true` in development
   - Use `sk_live_` and `pk_live_` for production

---

### 6. ❌ Image Upload Not Working

**Symptom:** Product images won't upload.

**Solution:**

1. **Check Cloudinary Settings:**
   ```bash
   CloudinarySettings__CloudName=your_cloud_name
   CloudinarySettings__ApiKey=your_api_key
   CloudinarySettings__ApiSecret=your_api_secret
   ```

2. **Get Cloudinary Credentials:**
   - Sign up at https://cloudinary.com (free tier: 25 GB)
   - Dashboard → Account Details
   - Copy Cloud Name, API Key, API Secret

---

### 7. ❌ Database Connection Error

**Symptom:** API logs show "Cannot connect to database" or "Connection refused"

**Solutions:**

1. **Check Connection String Format:**
   ```
   # Render PostgreSQL Internal URL format:
   postgresql://username:password@hostname:5432/database_name
   
   # Common mistakes:
   ❌ postgres:// (wrong protocol)
   ❌ Missing port :5432
   ❌ Wrong database name
   ```

2. **Use Internal Database URL:**
   - Render Dashboard → Database → Internal Database URL
   - Use this for API service (faster, free)
   - Don't use External Database URL unless connecting from outside Render

3. **Check Database is Running:**
   - Render Dashboard → Database → Should show "Available"
   - If suspended, upgrade plan or check free tier limits

---

### 8. ❌ Migration Errors

**Symptom:** Logs show "Migration failed" or "Table already exists"

**Solutions:**

**Option 1: Reset Database (⚠️ Deletes all data)**
```bash
# In Render Database Shell:
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;

# Then redeploy API to run migrations fresh
```

**Option 2: Manual Migration**
```bash
# Locally, generate migration bundle:
cd src/ECommerce.API
dotnet ef migrations bundle --output migrations-bundle

# Upload bundle to Render and run via SSH or startup command
```

---

## 🛠️ Debugging Checklist

When something goes wrong, check these in order:

1. ✅ **Render Service Status**
   - Dashboard shows "Available" (not "Suspended" or "Failed")

2. ✅ **Environment Variables**
   - All required variables are set
   - No typos in variable names
   - Values are correct (not placeholder text)

3. ✅ **Database Connection**
   - Database is running
   - Connection string is correct
   - Using Internal URL (not External)

4. ✅ **API Logs**
   - Check for startup errors
   - Look for database connection messages
   - Check for "Migration completed successfully"

5. ✅ **Network/CORS**
   - API is accessible: `curl https://your-api.onrender.com/api/products`
   - CORS headers are present in response

6. ✅ **Frontend Configuration**
   - Web service has correct `ApiSettings__BaseUrl`
   - Points to API service (not localhost)

---

## 🔍 Common Log Messages

### ✅ Good Signs:
```
✓ "Database migration completed successfully"
✓ "Database contains 3 products"
✓ "Application started. Press Ctrl+C to shut down."
✓ "Now listening on: http://0.0.0.0:10000"
```

### ❌ Error Signs:
```
✗ "Database connection string not configured"
   → Fix: Add ConnectionStrings__DefaultConnection env variable

✗ "No products found in database"
   → Fix: Seed data not applied, check migration

✗ "Cannot connect to database"
   → Fix: Check connection string format and database status

✗ "JWT settings not configured"
   → Fix: Add JwtSettings__* environment variables
```

---

## 📞 Still Having Issues?

1. **Check Recent Changes:**
   - Compare with working local version
   - Review recent commits for breaking changes

2. **Test Locally First:**
   ```bash
   cd src/ECommerce.API
   dotnet run
   # Test at http://localhost:5000/api/products
   ```

3. **Check Render Status Page:**
   - https://status.render.com
   - Look for ongoing incidents

4. **Review Documentation:**
   - `DEPLOYMENT_GUIDE.md` - Full setup instructions
   - `SETUP_AND_TESTING_GUIDE.md` - Local testing
   - `README.md` - Project overview

---

## 🎯 Quick Fixes Summary

| Issue | Quick Fix Command/Action |
|-------|-------------------------|
| No products | Redeploy API (uses Migrate() now) |
| Login fails | Check JWT environment variables |
| Slow first load | Normal on free tier (service spin-up) |
| CORS error | Verify CORS policy in Program.cs |
| DB connection | Use Internal Database URL from Render |
| Stripe payment | Verify webhook secret and endpoint |
| Image upload | Add Cloudinary credentials |

---

**Last Updated:** October 15, 2025  
**Version:** 1.0
