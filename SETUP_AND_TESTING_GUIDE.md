# E-Commerce Application - Setup and Testing Guide

## 📋 Table of Contents
- [System Requirements](#system-requirements)
- [Quick Start](#quick-start)
- [Environment Configuration](#environment-configuration)
- [Running the Application](#running-the-application)
- [Testing All Features](#testing-all-features)
- [Troubleshooting](#troubleshooting)

---

## 🔧 System Requirements

- Docker Desktop or Docker + Docker Compose
- Git
- Web browser (Chrome/Firefox recommended)
- Internet connection (for downloading images)

---

## 🚀 Quick Start

### 1. Clone and Setup

```bash
# Clone the repository
git clone <repository-url>
cd ecommerce-assignment-qe180214

# Create environment file from example
cp .env.example .env
```

### 2. Configure Environment Variables

Edit the `.env` file with your credentials:

```env
# Database Configuration
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_secure_password
POSTGRES_DB=ecommerce

# JWT Configuration
JWT_SECRET=your_secure_jwt_secret_key_at_least_32_characters
JWT_ISSUER=ECommerceAPI
JWT_AUDIENCE=ECommerceClients

# Stripe Payment Configuration (REQUIRED)
STRIPE_SECRET_KEY=sk_test_your_test_secret_key
STRIPE_PUBLISHABLE_KEY=pk_test_your_test_publishable_key
STRIPE_WEBHOOK_SECRET=whsec_your_webhook_secret

# Cloudinary Image Upload Configuration (OPTIONAL)
CLOUDINARY_CLOUD_NAME=your_cloud_name
CLOUDINARY_API_KEY=your_api_key
CLOUDINARY_API_SECRET=your_api_secret
```

#### How to Get Stripe Test Keys:

1. Go to [Stripe Dashboard](https://dashboard.stripe.com/test/dashboard)
2. Click **Developers** → **API keys**
3. Copy your **Publishable key** (starts with `pk_test_`)
4. Reveal and copy your **Secret key** (starts with `sk_test_`)
5. Paste them into your `.env` file

#### How to Setup Stripe Webhook (for Auto Payment Status Update):

See detailed guide: [STRIPE_WEBHOOK_SETUP.md](./STRIPE_WEBHOOK_SETUP.md)

**Quick Setup with Stripe CLI:**
```bash
# Install Stripe CLI
brew install stripe/stripe-cli/stripe

# Login
stripe login

# Forward webhooks to local server
stripe listen --forward-to http://localhost:8080/api/webhook/stripe

# Copy the webhook secret (whsec_...) and add to .env file
```

### 3. Start the Application

```bash
# Start all containers (postgres, api, web)
docker-compose up -d

# Check container status
docker ps

# You should see 3 containers running:
# - ecommerce-postgres-dev (database)
# - ecommerce-api-dev (backend API)
# - ecommerce-web-dev (frontend)
```

### 4. Access the Application

Open your browser and navigate to:

**🌐 Web Application: http://localhost:5173**

---

## 🧪 Testing All Features

### 1. User Registration & Authentication

#### Test User Registration

1. Navigate to http://localhost:5173
2. Click **"Register"** in the navigation menu
3. Fill in the registration form:
   - **Email**: test@example.com
   - **Password**: Test123!@#
   - **Confirm Password**: Test123!@#
4. Click **"Register"** button
5. ✅ **Expected**: Successfully registered, redirected to login page

#### Test User Login

1. On the login page, enter:
   - **Email**: test@example.com
   - **Password**: Test123!@#
2. Click **"Login"** button
3. ✅ **Expected**: Successfully logged in, redirected to home page
4. ✅ **Verify**: User email displayed in navigation bar

### 2. Product Management (Authenticated Users Only)

#### Create a Product

1. Ensure you're logged in
2. Click **"Products"** → **"Create New Product"**
3. Fill in the product form:
   - **Name**: Classic T-Shirt
   - **Description**: High-quality cotton t-shirt
   - **Price**: 29.99
   - **Image URL**: https://via.placeholder.com/400x400.png?text=T-Shirt
4. Click **"Create"**
5. ✅ **Expected**: Product created successfully, redirected to product list

#### View Products

1. Navigate to **"Products"** page
2. ✅ **Expected**: See list of all products with images, names, prices
3. Click **"Details"** on a product
4. ✅ **Expected**: View full product information

#### Update a Product

1. On the product details page, click **"Edit"**
2. Change the **Price** to: 24.99
3. Click **"Save"**
4. ✅ **Expected**: Product updated, price reflects new value

#### Delete a Product

1. On the product details page, click **"Delete"**
2. Confirm the deletion
3. ✅ **Expected**: Product removed from list

### 3. Shopping Cart Functionality

#### Add Products to Cart

1. Browse the **Products** page
2. Click **"Add to Cart"** on a product
3. ✅ **Expected**: Success message displayed
4. Add 2-3 different products to cart

#### View Cart

1. Click **"Cart"** in the navigation menu
2. ✅ **Expected**: See all added products with:
   - Product name
   - Price
   - Quantity selector
   - Subtotal
   - Total amount

#### Update Cart Quantity

1. In the cart, change the quantity of a product
2. Click **"Update"**
3. ✅ **Expected**: Subtotal and total updated

#### Remove from Cart

1. Click **"Remove"** next to a product
2. ✅ **Expected**: Product removed from cart

### 4. Checkout & Payment (Stripe Integration)

#### Process Test Payment

1. Ensure you have products in your cart
2. Click **"Proceed to Checkout"**
3. On the checkout page, you'll see:
   - Order summary
   - Total amount
   - Stripe payment form

#### Use Stripe Test Cards

Enter one of these test card numbers:

| Card Number | Description | Expected Result |
|-------------|-------------|-----------------|
| `4242 4242 4242 4242` | Successful payment | ✅ Payment succeeds |
| `4000 0000 0000 0002` | Card declined | ❌ Payment fails |
| `4000 0025 0000 3155` | Requires authentication | 🔐 3D Secure popup |

**Complete Payment Form:**
- **Card Number**: 4242 4242 4242 4242
- **Expiry Date**: 12/34 (any future date)
- **CVC**: 123 (any 3 digits)
- **ZIP**: 12345 (any 5 digits)

4. Click **"Pay Now"**
5. ✅ **Expected**: 
   - Payment processing message
   - Redirected to success page
   - Order confirmation displayed

### 5. Order History

#### View Your Orders

1. Click **"Orders"** in the navigation menu
2. ✅ **Expected**: See list of all your orders with:
   - Order ID
   - Order date
   - Total amount
   - Status (Pending/Processing/Completed)

#### View Order Details

1. Click **"Details"** on an order
2. ✅ **Expected**: View complete order information:
   - All items in the order
   - Individual item prices
   - Total amount
   - Payment status
   - Stripe Payment Intent ID

---

## 🔍 API Testing (Optional - For Developers)

### Test API Endpoints with curl

The API runs on the **internal Docker network** but you can test it via localhost:7246 (if exposed) or from inside the api container:

```bash
# Register a new user
curl -X POST http://localhost:7246/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "apitest@example.com",
    "password": "Test123!@#",
    "confirmPassword": "Test123!@#"
  }'

# Login
curl -X POST http://localhost:7246/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "apitest@example.com",
    "password": "Test123!@#"
  }'

# Get all products (no auth required)
curl http://localhost:7246/api/products

# Create a product (requires JWT token)
curl -X POST http://localhost:7246/api/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "name": "API Test Product",
    "description": "Created via API",
    "price": 49.99,
    "imageUrl": "https://via.placeholder.com/400"
  }'
```

---

## 🐛 Troubleshooting

### Issue: Containers Won't Start

**Solution:**
```bash
# Stop all containers
docker-compose down

# Remove volumes (WARNING: This deletes database data)
docker-compose down -v

# Rebuild and start fresh
docker-compose up -d --build
```

### Issue: "Connection Refused" Errors

**Cause**: API not reachable from Web container

**Solution:**
```bash
# Check all containers are running
docker ps

# Check Web container logs
docker logs ecommerce-web-dev

# Check API container logs
docker logs ecommerce-api-dev

# Verify environment variables
docker exec ecommerce-web-dev env | grep ApiSettings
```

### Issue: Login/Registration Not Working

**Solution:**
```bash
# Check API container logs for errors
docker logs ecommerce-api-dev --tail 50

# Verify database is healthy
docker ps | grep postgres

# Check if migrations ran successfully
docker exec ecommerce-api-dev ls -la
```

### Issue: Payment Form Not Loading

**Cause**: Missing or invalid Stripe keys

**Solution:**
1. Verify your `.env` file has correct Stripe keys
2. Restart the API container:
   ```bash
   docker-compose restart api
   ```
3. Check API logs for Stripe initialization:
   ```bash
   docker logs ecommerce-api-dev | grep Stripe
   ```

### Issue: "Payment Failed" Even with Test Card

**Checklist:**
1. ✅ Using Stripe test keys (starting with `sk_test_` and `pk_test_`)
2. ✅ Card number is `4242 4242 4242 4242`
3. ✅ Expiry date is in the future
4. ✅ Check browser console for JavaScript errors
5. ✅ Check API logs: `docker logs ecommerce-api-dev`

### Issue: Images Not Uploading

**Note**: Image upload requires Cloudinary configuration. If not configured:
- Use direct image URLs instead
- Or configure Cloudinary credentials in `.env`

---

## 📊 Database Access (Optional)

Connect to PostgreSQL database:

```bash
# Using docker exec
docker exec -it ecommerce-postgres-dev psql -U postgres -d ecommerce

# Or use any PostgreSQL client with:
# Host: localhost
# Port: 5432
# Username: postgres
# Password: (from your .env file)
# Database: ecommerce
```

### Useful SQL Queries:

```sql
-- View all users
SELECT * FROM "AspNetUsers";

-- View all products
SELECT * FROM "Products";

-- View all orders
SELECT * FROM "Orders";

-- View order items
SELECT * FROM "OrderItems";

-- View cart items
SELECT * FROM "CartItems";
```

---

## 🛠️ Development Commands

```bash
# View logs from all containers
docker-compose logs -f

# View logs from specific container
docker logs -f ecommerce-web-dev
docker logs -f ecommerce-api-dev

# Restart a specific service
docker-compose restart web
docker-compose restart api

# Rebuild and restart a service
docker-compose up -d --build web

# Stop all containers
docker-compose down

# Stop and remove volumes (deletes data)
docker-compose down -v

# Access container shell
docker exec -it ecommerce-web-dev bash
docker exec -it ecommerce-api-dev bash
```

---

## ✅ Complete Testing Checklist

Use this checklist to verify all features work correctly:

### Authentication
- [ ] User registration with valid credentials
- [ ] Registration validation (password confirmation)
- [ ] User login with correct credentials
- [ ] Login fails with wrong password
- [ ] Logout functionality
- [ ] Protected routes redirect to login

### Products
- [ ] View all products (unauthenticated)
- [ ] View product details
- [ ] Create product (authenticated only)
- [ ] Update product (authenticated only)
- [ ] Delete product (authenticated only)
- [ ] Product validation (required fields)

### Shopping Cart
- [ ] Add product to cart
- [ ] View cart items
- [ ] Update quantity in cart
- [ ] Remove item from cart
- [ ] Cart persists after logout/login
- [ ] Cart total calculation correct

### Checkout & Payment
- [ ] Proceed to checkout with items in cart
- [ ] Stripe payment form loads
- [ ] Successful payment with test card (4242...)
- [ ] Payment decline with declined card (4000 0000 0000 0002)
- [ ] Order created after successful payment
- [ ] Cart cleared after successful order

### Orders
- [ ] View order history
- [ ] View order details
- [ ] Order shows correct items and total
- [ ] Order displays payment status
- [ ] Order displays Stripe Payment Intent ID

---

## 🎯 Quick Test Scenario

**Complete E-Commerce Flow (5 minutes):**

1. ✅ Register new user → Login
2. ✅ Create 2-3 products (or use existing)
3. ✅ Add products to cart
4. ✅ View cart, update quantity
5. ✅ Proceed to checkout
6. ✅ Complete payment with card `4242 4242 4242 4242`
7. ✅ Verify order in order history
8. ✅ Logout → Login → Verify order still visible

---

## 📞 Support

If you encounter issues:

1. Check container logs: `docker logs ecommerce-web-dev`
2. Verify environment variables in `.env`
3. Ensure all containers are running: `docker ps`
4. Try rebuilding: `docker-compose up -d --build`

---

## 🔐 Security Notes

**For Production Deployment:**

- ⚠️ Change all default passwords
- ⚠️ Use strong JWT secret (32+ characters)
- ⚠️ Switch to Stripe live keys (starts with `sk_live_`)
- ⚠️ Enable HTTPS/SSL
- ⚠️ Configure proper CORS settings
- ⚠️ Set up proper database backups
- ⚠️ Use secrets management (not .env files)

---

## 🏁 Summary

This application demonstrates a complete e-commerce solution with:

- ✅ User authentication (JWT)
- ✅ Product CRUD operations
- ✅ Shopping cart management
- ✅ Stripe payment integration
- ✅ Order tracking and history
- ✅ Dockerized deployment

**Access**: http://localhost:5173

**Test Payment**: Use card `4242 4242 4242 4242` with any future expiry date

**Enjoy testing!** 🎉
