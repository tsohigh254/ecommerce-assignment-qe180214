# 🚀 Production Deployment Guide

## Deploy to Render (Free Tier)

### Prerequisites
- GitHub account with this repo
- Render account (https://render.com - sign up free)
- Stripe account (https://stripe.com)

---

## 📦 Step 1: Deploy Database (PostgreSQL)

1. Go to https://dashboard.render.com
2. Click **"New +"** → **"PostgreSQL"**
3. Configure:
   - **Name**: `ecommerce-db-qe180214`
   - **Database**: `ecommerce`
   - **User**: `postgres` (auto-generated)
   - **Region**: `Singapore`
   - **Plan**: `Free` (0.1GB storage)
4. Click **"Create Database"**
5. Wait 2-3 minutes for provisioning
6. **Copy the Internal Database URL** (starts with `postgresql://`)

---

## 🔧 Step 2: Deploy API (Backend)

1. Click **"New +"** → **"Web Service"**
2. Connect your GitHub repo: `tsohigh254/ecommerce-assignment-qe180214`
3. Configure:
   - **Name**: `ecommerce-api-qe180214`
   - **Region**: `Singapore`
   - **Branch**: `main`
   - **Root Directory**: `src/ECommerce.API`
   - **Environment**: `Docker`
   - **Dockerfile Path**: `./src/ECommerce.API/Dockerfile`
   - **Plan**: `Free`
4. **Environment Variables** (click "Add Environment Variable"):
   ```bash
   ASPNETCORE_ENVIRONMENT=Production
   
   # Database (paste from Step 1)
   ConnectionStrings__DefaultConnection=postgresql://user:pass@host/dbname
   
   # JWT Settings
   JwtSettings__SecretKey=your-super-secret-key-min-32-chars
   JwtSettings__Issuer=ECommerceAPI
   JwtSettings__Audience=ECommerceWebApp
   JwtSettings__ExpirationInMinutes=60
   
   # Stripe (get from https://dashboard.stripe.com/apikeys)
   StripeSettings__SecretKey=sk_live_YOUR_LIVE_KEY
   StripeSettings__PublishableKey=pk_live_YOUR_LIVE_KEY
   StripeSettings__WebhookSecret=whsec_WILL_ADD_LATER
   StripeSettings__Currency=usd
   StripeSettings__TestMode=false
   
   # Cloudinary (optional)
   CloudinarySettings__CloudName=your_cloud_name
   CloudinarySettings__ApiKey=your_api_key
   CloudinarySettings__ApiSecret=your_api_secret
   ```
5. Click **"Create Web Service"**
6. Wait 5-10 minutes for build & deploy
7. **Copy the API URL** (e.g., `https://ecommerce-api-qe180214.onrender.com`)

---

## 🌐 Step 3: Deploy Web (Frontend)

1. Click **"New +"** → **"Web Service"**
2. Select same repo: `tsohigh254/ecommerce-assignment-qe180214`
3. Configure:
   - **Name**: `ecommerce-web-qe180214`
   - **Region**: `Singapore`
   - **Branch**: `main`
   - **Root Directory**: `src/ECommerce.Web`
   - **Environment**: `Docker`
   - **Dockerfile Path**: `./src/ECommerce.Web/Dockerfile`
   - **Plan**: `Free`
4. **Environment Variables**:
   ```bash
   ASPNETCORE_ENVIRONMENT=Production
   
   # API URL (from Step 2)
   ApiSettings__BaseUrl=https://ecommerce-api-qe180214.onrender.com
   ```
5. Click **"Create Web Service"**
6. Wait 5-10 minutes for deployment
7. **Your app is live!** 🎉

---

## 🔔 Step 4: Setup Stripe Webhook (Production)

### 4.1 Create Webhook in Stripe Dashboard

1. Go to https://dashboard.stripe.com/webhooks
2. Click **"Add endpoint"**
3. Configure:
   - **Endpoint URL**: `https://ecommerce-api-qe180214.onrender.com/api/webhook/stripe`
   - **Description**: `E-Commerce Payment Webhook`
   - **Events to send**:
     - ✅ `payment_intent.succeeded`
     - ✅ `payment_intent.payment_failed`
     - ✅ `payment_intent.canceled`
4. Click **"Add endpoint"**
5. **Copy the Signing Secret** (starts with `whsec_`)

### 4.2 Update API Environment Variables

1. Go back to Render Dashboard → Your API service
2. Click **"Environment"** tab
3. Find `StripeSettings__WebhookSecret`
4. Update with the signing secret from Stripe
5. Click **"Save Changes"**
6. Service will auto-redeploy (1-2 minutes)

### 4.3 Test Webhook

1. In Stripe Dashboard → Webhooks → Your endpoint
2. Click **"Send test webhook"**
3. Select `payment_intent.succeeded`
4. Click **"Send test webhook"**
5. Check response status should be `200 OK` ✅

---

## ✅ Step 5: Verify Deployment

### Test the Application:

1. **Open Web App**: `https://ecommerce-web-qe180214.onrender.com`
2. **Register a new user**
3. **Add products to cart**
4. **Proceed to checkout**
5. **Use Stripe test card**: `4242 4242 4242 4242`
6. **Complete payment**
7. **Verify webhook**: Order status should auto-update to "Succeeded"

### Check Logs:

**API Logs:**
```bash
# In Render Dashboard → API Service → Logs
[info] Webhook signature verified successfully
[info] Successfully updated order 123 for PaymentIntent pi_xxx
```

**Webhook Logs in Stripe:**
- Go to Stripe Dashboard → Webhooks → Your endpoint
- Check "Recent Events" - should show successful deliveries

---

## 🔧 Troubleshooting

### Database Connection Failed
- Check `ConnectionStrings__DefaultConnection` is correct
- Ensure database is running (green status in Render)
- Try internal database URL instead of external

### Webhook Not Working
1. Check webhook URL is correct: `https://your-api.onrender.com/api/webhook/stripe`
2. Verify signing secret in environment variables
3. Check API logs for webhook errors
4. Test endpoint manually: `curl https://your-api.onrender.com/api/webhook/test`

### API Service Not Starting
- Check Dockerfile syntax
- Review build logs for errors
- Verify all required environment variables are set

### Payment Not Processing
1. Switch Stripe from test mode to live mode
2. Update API keys to live keys (starts with `sk_live_` and `pk_live_`)
3. Update `StripeSettings__TestMode=false`

---

## 🎯 Production URLs

After deployment, you'll have:

- **Web App**: `https://ecommerce-web-qe180214.onrender.com`
- **API**: `https://ecommerce-api-qe180214.onrender.com`
- **Database**: Internal Render PostgreSQL
- **Webhook**: `https://ecommerce-api-qe180214.onrender.com/api/webhook/stripe`

---

## 💰 Cost

**Render Free Tier:**
- ✅ Web Services: Free (sleeps after 15min inactivity)
- ✅ PostgreSQL: Free (0.1GB, 90 days)
- ✅ Bandwidth: 100GB/month
- ✅ Build minutes: 500 minutes/month

**Stripe:**
- ✅ No monthly fees
- ✅ 2.9% + $0.30 per successful charge

---

## 📚 Resources

- Render Docs: https://render.com/docs
- Stripe Webhooks: https://stripe.com/docs/webhooks
- .NET on Render: https://render.com/docs/deploy-dotnet

---

## 🆘 Need Help?

Check logs in Render Dashboard or Stripe Dashboard for detailed error messages.
