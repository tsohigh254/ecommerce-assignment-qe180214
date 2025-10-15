# ✅ Deployment Checklist

## Pre-deployment
- [x] Code committed to GitHub
- [x] Environment variables documented
- [x] Deployment guide created
- [ ] Render account created (https://render.com)
- [ ] Stripe account verified (https://stripe.com)

## Step 1: Database (5 mins)
- [ ] Create PostgreSQL on Render
- [ ] Copy Internal Database URL
- [ ] Test connection

## Step 2: API Service (10 mins)
- [ ] Create Web Service from GitHub repo
- [ ] Set root directory: `src/ECommerce.API`
- [ ] Configure environment variables:
  - [ ] Database connection string
  - [ ] JWT secret key
  - [ ] Stripe SECRET key (sk_live_...)
  - [ ] Stripe PUBLISHABLE key (pk_live_...)
- [ ] Wait for deployment
- [ ] Copy API URL
- [ ] Test: `https://your-api.onrender.com/api/webhook/test`

## Step 3: Web Service (10 mins)
- [ ] Create Web Service from GitHub repo
- [ ] Set root directory: `src/ECommerce.Web`
- [ ] Set API URL in environment
- [ ] Wait for deployment
- [ ] Open web app in browser
- [ ] Test registration & login

## Step 4: Stripe Webhook (5 mins)
- [ ] Go to https://dashboard.stripe.com/webhooks
- [ ] Click "Add endpoint"
- [ ] Enter webhook URL: `https://your-api.onrender.com/api/webhook/stripe`
- [ ] Select events:
  - [ ] payment_intent.succeeded
  - [ ] payment_intent.payment_failed
  - [ ] payment_intent.canceled
- [ ] Copy webhook signing secret (whsec_...)
- [ ] Add to Render API environment variables:
  - [ ] StripeSettings__WebhookSecret=whsec_...
- [ ] Wait for service restart
- [ ] Test webhook in Stripe Dashboard
- [ ] Check "Recent Events" shows 200 OK

## Step 5: End-to-End Test (10 mins)
- [ ] Register new user
- [ ] Browse products
- [ ] Add items to cart
- [ ] Proceed to checkout
- [ ] Use test card: 4242 4242 4242 4242
- [ ] Complete payment
- [ ] Verify order created
- [ ] Check order status auto-updated to "Succeeded"
- [ ] View webhook logs in Stripe Dashboard

## Post-deployment
- [ ] Share live URL with instructor
- [ ] Monitor Render logs for errors
- [ ] Test on mobile device
- [ ] Add SSL certificate (automatic with Render)

---

## 🎯 Your Live URLs

After deployment, update these:

- **Production Web**: https://ecommerce-web-qe180214.onrender.com
- **Production API**: https://ecommerce-api-qe180214.onrender.com
- **Webhook Endpoint**: https://ecommerce-api-qe180214.onrender.com/api/webhook/stripe

---

## 🆘 Quick Troubleshooting

**API won't start:**
```bash
# Check build logs in Render Dashboard
# Verify all environment variables are set
# Check Dockerfile syntax
```

**Webhook not working:**
```bash
# Test endpoint manually
curl https://your-api.onrender.com/api/webhook/test

# Should return 200 OK with message
```

**Database connection error:**
```bash
# Use INTERNAL database URL (starts with postgresql://)
# Not the external URL
# Format: postgresql://user:pass@host:5432/dbname
```

---

## 📊 Expected Timeline

Total deployment time: ~30-40 minutes

- Database setup: 5 mins
- API deployment: 10 mins (first build takes longer)
- Web deployment: 10 mins
- Stripe webhook: 5 mins
- Testing: 10 mins

---

## 💡 Tips

1. **First deploy is slowest** - Subsequent deploys are faster
2. **Free tier sleeps** - First request after inactivity takes 30-60 seconds
3. **Logs are your friend** - Check Render logs for debugging
4. **Test locally first** - Ensure everything works before deploying
5. **Keep secrets safe** - Never commit real API keys to Git

---

## ✨ Success Criteria

Your deployment is successful when:
✅ Web app loads without errors
✅ Users can register and login
✅ Products display correctly
✅ Cart functionality works
✅ Stripe checkout form appears
✅ Payment processes successfully
✅ Order status auto-updates via webhook
✅ Webhook shows 200 OK in Stripe Dashboard

---

Good luck! 🚀
