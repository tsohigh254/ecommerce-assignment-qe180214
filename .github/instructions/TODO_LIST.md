# 📋 TODO LIST - E-Commerce Assignment 2 - QE180214

**Project Status:** 🟢 In Progress  
**Last Updated:** October 14, 2025  
**Completion:** 10% (3/32 tasks completed)

---

## ✅ PHASE 1: Authentication Foundation (COMPLETED ✓)

### Task 1: ✅ Set up User Authentication Models
**Status:** ✅ COMPLETED  
**Priority:** HIGH  
**Files Created:**
- ✅ `src/ECommerce.Core/Models/User.cs` - User model extending IdentityUser
- ✅ `src/ECommerce.Core/Models/AuthDtos.cs` - RegisterDto, LoginDto, AuthResponseDto
- ✅ `src/ECommerce.Core/ECommerce.Core.csproj` - Added Identity package
- ✅ `src/ECommerce.API/Data/ECommerceDbContext.cs` - Updated to use IdentityDbContext<User>
- ✅ `src/ECommerce.API/ECommerce.API.csproj` - Added JWT and Identity packages

**Details:**
- User model với email, password hash, FirstName, LastName
- Navigation properties cho Cart và Order
- DTOs cho Register và Login với validation

---

### Task 2: ✅ Implement JWT Authentication API
**Status:** ✅ COMPLETED  
**Priority:** HIGH  
**Files Created:**
- ✅ `src/ECommerce.API/Configuration/JwtSettings.cs` - JWT configuration model
- ✅ `src/ECommerce.API/Controllers/AuthController.cs` - Auth endpoints
- ✅ `src/ECommerce.API/appsettings.json` - Added JwtSettings
- ✅ `src/ECommerce.API/Program.cs` - Configured JWT authentication

**Endpoints Implemented:**
- ✅ POST `/api/auth/register` - User registration
- ✅ POST `/api/auth/login` - User login
- ✅ POST `/api/auth/logout` - User logout
- ✅ GET `/api/auth/me` - Get current user info

**Details:**
- JWT token generation với claims
- Identity configuration với UserManager
- Token expiration: 60 minutes
- Password requirements configured

---

### Task 3: ✅ Add Authorization to ProductsController
**Status:** ✅ COMPLETED  
**Priority:** HIGH  
**Files Modified:**
- ✅ `src/ECommerce.API/Controllers/ProductsController.cs`

**Changes:**
- ✅ Added `[Authorize]` to POST `/api/products` (Create)
- ✅ Added `[Authorize]` to PUT `/api/products/{id}` (Update)
- ✅ Added `[Authorize]` to DELETE `/api/products/{id}` (Delete)
- ✅ GET endpoints remain public (unauthenticated access)

---

## 🔄 PHASE 2: Data Models (IN PROGRESS)

### Task 4: ⏳ Update Product Model
**Status:** ⏳ PENDING  
**Priority:** LOW (Already complete from previous assignment)  
**Action Required:**
- [ ] Verify Product model has all required fields
- [ ] Product model already has: Name, Description, Price, ImageUrl ✓

---

### Task 5: 🔴 Create Cart Model and DbContext
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Core/Models/Cart.cs`
- [ ] `src/ECommerce.Core/Models/CartItem.cs`

**Required Properties:**
```csharp
// Cart.cs
- CartId (int, PK)
- UserId (string, FK)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
- Navigation: List<CartItem>

// CartItem.cs
- CartItemId (int, PK)
- CartId (int, FK)
- ProductId (int, FK)
- Quantity (int)
- DateAdded (DateTime)
- Navigation: Product, Cart
```

**Files to Modify:**
- [ ] `src/ECommerce.API/Data/ECommerceDbContext.cs` - Add DbSet<Cart>, DbSet<CartItem>

---

### Task 7: 🔴 Create Order Models
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Core/Models/Order.cs`
- [ ] `src/ECommerce.Core/Models/OrderItem.cs`

**Required Properties:**
```csharp
// Order.cs
- OrderId (int, PK)
- UserId (string, FK)
- TotalAmount (decimal)
- Status (string) - "Pending", "Processing", "Completed", "Cancelled"
- OrderDate (DateTime)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
- Navigation: List<OrderItem>

// OrderItem.cs
- OrderItemId (int, PK)
- OrderId (int, FK)
- ProductId (int, FK)
- Quantity (int)
- Price (decimal) - Price at time of order
- Navigation: Product, Order
```

**Files to Modify:**
- [ ] `src/ECommerce.API/Data/ECommerceDbContext.cs` - Add DbSet<Order>, DbSet<OrderItem>

---

### Task 18: 🔴 Run Database Migrations
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH (Do after Tasks 5 & 7)  
**Commands to Run:**
```bash
cd src/ECommerce.API
dotnet ef migrations add AddAuthenticationAndCartAndOrder
dotnet ef database update
```

**Expected Migrations:**
- [ ] Identity tables (AspNetUsers, AspNetRoles, etc.)
- [ ] Cart and CartItem tables
- [ ] Order and OrderItem tables

---

## 📡 PHASE 3: API Controllers

### Task 6: 🔴 Implement Cart API Controller
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.API/Controllers/CartController.cs`

**Endpoints to Implement:**
- [ ] POST `/api/cart/add` - Add product to cart (requires auth)
- [ ] GET `/api/cart` - Get current user's cart (requires auth)
- [ ] PUT `/api/cart/item/{cartItemId}` - Update quantity (requires auth)
- [ ] DELETE `/api/cart/item/{cartItemId}` - Remove item (requires auth)
- [ ] DELETE `/api/cart/clear` - Clear entire cart (requires auth)

---

### Task 8: 🔴 Implement Order API Controller
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.API/Controllers/OrderController.cs`

**Endpoints to Implement:**
- [ ] POST `/api/orders` - Place order from cart (requires auth)
- [ ] GET `/api/orders` - Get user's orders (requires auth)
- [ ] GET `/api/orders/{id}` - Get order by ID (requires auth)
- [ ] PUT `/api/orders/{id}/status` - Update order status (optional)

**Logic Required:**
- [ ] Create order from cart items
- [ ] Copy cart items to order items with current price
- [ ] Calculate total amount
- [ ] Clear cart after order placement
- [ ] Set initial status to "Pending"

---

## 🎨 PHASE 4: Web Authentication UI

### Task 9: 🔴 Create Register and Login Views
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Web/Controllers/AccountController.cs`
- [ ] `src/ECommerce.Web/Views/Account/Register.cshtml`
- [ ] `src/ECommerce.Web/Views/Account/Login.cshtml`
- [ ] `src/ECommerce.Web/Models/RegisterViewModel.cs`
- [ ] `src/ECommerce.Web/Models/LoginViewModel.cs`

**Features:**
- [ ] Registration form with validation
- [ ] Login form with validation
- [ ] Error message display
- [ ] Success message display
- [ ] Redirect after successful login

---

### Task 10: 🔴 Add Authentication to Web UI
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Modify:**
- [ ] `src/ECommerce.Web/Program.cs` - Add session, authentication
- [ ] `src/ECommerce.Web/Controllers/AccountController.cs` - Implement login/logout

**Implementation:**
- [ ] Add session configuration
- [ ] Store JWT token in session/cookie
- [ ] Add authentication middleware
- [ ] Implement logout functionality
- [ ] Add JWT token to API requests from ProductService

**Packages to Add:**
- [ ] `Microsoft.AspNetCore.Session`
- [ ] Add session to services

---

## 🖼️ PHASE 5: Web Features

### Task 11: 🔴 Update Navigation Menu
**Status:** 🔴 NOT STARTED  
**Priority:** MEDIUM  
**Files to Modify:**
- [ ] `src/ECommerce.Web/Views/Shared/_Layout.cshtml`

**Features to Add:**
- [ ] Show "Login" and "Register" links when not authenticated
- [ ] Show "Logout" button when authenticated
- [ ] Show user email when authenticated
- [ ] Add "Cart" link with item count badge
- [ ] Add "My Orders" link
- [ ] Conditional display of "Create Product" link

---

### Task 16: 🔴 Create CartService in Web Project
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Web/Services/CartService.cs`
- [ ] `src/ECommerce.Web/Services/ICartService.cs`

**Methods to Implement:**
```csharp
- Task<CartDto> GetCartAsync()
- Task AddToCartAsync(int productId, int quantity)
- Task UpdateQuantityAsync(int cartItemId, int quantity)
- Task RemoveItemAsync(int cartItemId)
- Task ClearCartAsync()
```

**Files to Modify:**
- [ ] `src/ECommerce.Web/Program.cs` - Register CartService

---

### Task 12: 🔴 Implement Cart View and UI
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Web/Controllers/CartController.cs`
- [ ] `src/ECommerce.Web/Views/Cart/Index.cshtml`
- [ ] `src/ECommerce.Web/Models/CartViewModel.cs`

**Features:**
- [ ] Display all cart items
- [ ] Show product image, name, price
- [ ] Quantity input with update button
- [ ] Remove item button
- [ ] Calculate and display subtotal for each item
- [ ] Calculate and display cart total
- [ ] "Proceed to Checkout" button
- [ ] "Clear Cart" button
- [ ] Empty cart message

---

### Task 13: 🔴 Create Checkout Page
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Web/Views/Cart/Checkout.cshtml`
- [ ] `src/ECommerce.Web/Models/CheckoutViewModel.cs`

**Features:**
- [ ] Order summary with all items
- [ ] Total amount display
- [ ] User information display
- [ ] "Place Order" button
- [ ] Confirmation modal (optional)
- [ ] Redirect to order confirmation page

---

### Task 17: 🔴 Create OrderService in Web Project
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Web/Services/OrderService.cs`
- [ ] `src/ECommerce.Web/Services/IOrderService.cs`

**Methods to Implement:**
```csharp
- Task<OrderDto> PlaceOrderAsync()
- Task<List<OrderDto>> GetOrdersAsync()
- Task<OrderDto> GetOrderByIdAsync(int orderId)
```

**Files to Modify:**
- [ ] `src/ECommerce.Web/Program.cs` - Register OrderService

---

### Task 14: 🔴 Create Order History Page
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.Web/Controllers/OrdersController.cs`
- [ ] `src/ECommerce.Web/Views/Orders/Index.cshtml`
- [ ] `src/ECommerce.Web/Views/Orders/Details.cshtml`
- [ ] `src/ECommerce.Web/Models/OrderViewModel.cs`

**Features:**
- [ ] List all user orders
- [ ] Display order ID, date, status, total
- [ ] "View Details" link for each order
- [ ] Order details page showing all items
- [ ] Status badge styling (Pending, Processing, Completed)

---

### Task 15: 🔴 Update Product Views with Auth
**Status:** 🔴 NOT STARTED  
**Priority:** MEDIUM  
**Files to Modify:**
- [ ] `src/ECommerce.Web/Views/Products/Index.cshtml`
- [ ] `src/ECommerce.Web/Views/Products/Details.cshtml`
- [ ] `src/ECommerce.Web/Views/Shared/_Layout.cshtml`

**Changes:**
- [ ] Show "Create New Product" only when authenticated
- [ ] Show "Edit" button only when authenticated
- [ ] Show "Delete" button only when authenticated
- [ ] Add "Add to Cart" button on Details page
- [ ] Add quantity input on Details page

---

## 🧪 PHASE 6: Testing

### Task 19: 🔴 Test All API Endpoints
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Files to Create:**
- [ ] `src/ECommerce.API/Tests.http` or use Postman

**Test Cases:**
- [ ] Register new user
- [ ] Login with valid credentials
- [ ] Login with invalid credentials
- [ ] Get current user (with token)
- [ ] Get products (without token)
- [ ] Create product (with token)
- [ ] Create product (without token) - should fail
- [ ] Update product (with token)
- [ ] Delete product (with token)
- [ ] Add to cart
- [ ] Get cart
- [ ] Update cart item quantity
- [ ] Remove cart item
- [ ] Place order
- [ ] Get orders
- [ ] Get order by ID

---

### Task 20: 🔴 Test Complete User Flow in UI
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  

**Test Flow:**
1. [ ] Open website (unauthenticated)
2. [ ] View products (should work)
3. [ ] Try to create product (should redirect to login)
4. [ ] Register new account
5. [ ] Login with new account
6. [ ] Create a new product
7. [ ] Edit a product
8. [ ] Browse products
9. [ ] Add multiple products to cart
10. [ ] View cart
11. [ ] Update quantity in cart
12. [ ] Remove item from cart
13. [ ] Proceed to checkout
14. [ ] Place order
15. [ ] View order history
16. [ ] View order details
17. [ ] Logout
18. [ ] Verify cannot create/edit/delete products

---

## ⭐ PHASE 7: Optional Features (BONUS)

### Task 21: 🔴 Optional: Implement Payment Integration
**Status:** 🔴 NOT STARTED  
**Priority:** LOW (BONUS)  
**Options:** Stripe or Payos Checkout

**Files to Create:**
- [ ] `src/ECommerce.API/Controllers/PaymentController.cs`
- [ ] `src/ECommerce.API/Services/PaymentService.cs`

**Features:**
- [ ] Payment endpoint
- [ ] Success callback
- [ ] Failure callback
- [ ] Update order status to "Paid"

---

### Task 22: 🔴 Optional: Add Image Upload Feature
**Status:** 🔴 NOT STARTED  
**Priority:** LOW (BONUS)  
**Service:** Cloudinary

**Changes:**
- [ ] Add Cloudinary package
- [ ] Update ProductsController to handle file uploads
- [ ] Store image URL in Product model
- [ ] Update Create/Edit forms with file input

---

### Task 23: 🔴 Optional: Implement Search and Filter
**Status:** 🔴 NOT STARTED  
**Priority:** LOW (BONUS)  

**Features:**
- [ ] Search by product name
- [ ] Filter by price range
- [ ] Update API endpoint
- [ ] Update Products/Index view with search form

---

### Task 24: 🔴 Optional: Add Pagination to Products
**Status:** 🔴 NOT STARTED  
**Priority:** LOW (BONUS)  

**Features:**
- [ ] Implement pagination in API
- [ ] Add page size and page number parameters
- [ ] Update Products/Index view with pagination controls
- [ ] Previous/Next buttons
- [ ] Page numbers display

---

## 🚀 PHASE 8: Deployment

### Task 25: 🔴 Configure Production Database
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  

**Steps:**
- [ ] Choose provider (Supabase/ElephantSQL/Render)
- [ ] Create PostgreSQL database
- [ ] Get connection string
- [ ] Update `appsettings.Production.json`
- [ ] Add environment variable for connection string

---

### Task 26: 🔴 Update Docker Configuration
**Status:** 🔴 NOT STARTED  
**Priority:** MEDIUM  

**Files to Modify:**
- [ ] `docker-compose.yml`
- [ ] `src/ECommerce.API/Dockerfile`
- [ ] `src/ECommerce.Web/Dockerfile`

**Updates:**
- [ ] Add JWT environment variables
- [ ] Add production database connection
- [ ] Ensure CORS is configured

---

### Task 27: 🔴 Deploy to Hosting Platform
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Platforms:** Render, Railway, Azure Free Tier

**Steps:**
- [ ] Deploy API project
- [ ] Deploy Web project
- [ ] Configure environment variables
- [ ] Set up CORS for production URLs
- [ ] Test deployed endpoints

---

### Task 28: 🔴 Run Production Migrations
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  

**Steps:**
- [ ] Connect to production database
- [ ] Run migrations
- [ ] Verify all tables created
- [ ] Seed initial data (if needed)
- [ ] Test database connection from deployed app

---

### Task 29: 🔴 Test Deployed Application
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  

**Test Items:**
- [ ] Registration works
- [ ] Login works
- [ ] Product CRUD works
- [ ] Cart functionality works
- [ ] Order placement works
- [ ] Order history displays correctly
- [ ] Authentication redirects work
- [ ] All images load correctly

---

## 📚 PHASE 9: Documentation & Submission

### Task 30: 🔴 Update DOCUMENTATION.md
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  

**Sections to Add/Update:**
- [ ] Architecture overview
- [ ] Authentication flow diagram
- [ ] API endpoints documentation
- [ ] Setup instructions
- [ ] Environment variables list
- [ ] Deployment steps
- [ ] Deployed URLs (API and Web)
- [ ] Screenshots of features

---

### Task 31: 🔴 Prepare Submission Document
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  

**File to Create:** `QE180214_Ass2.docx`

**Content:**
- [ ] Student information
- [ ] GitHub repository link (public)
- [ ] Deployed website URL
- [ ] Deployed API URL
- [ ] Brief description of features implemented
- [ ] Technologies used
- [ ] Bonus features implemented (if any)
- [ ] Screenshots of:
  - [ ] Registration page
  - [ ] Login page
  - [ ] Product listing
  - [ ] Product create/edit (authenticated)
  - [ ] Cart page
  - [ ] Checkout page
  - [ ] Order history

---

### Task 32: 🔴 Final Review and Submission
**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  

**Checklist:**
- [ ] All requirements met
- [ ] All features work on deployed site
- [ ] GitHub repo is public
- [ ] README.md is complete
- [ ] DOCUMENTATION.md is complete
- [ ] Code is clean and commented
- [ ] No sensitive data in repo
- [ ] Submission document complete
- [ ] Submit assignment

---

## 📊 PROGRESS SUMMARY

### Overall Progress: 10% Complete

| Phase | Status | Tasks | Completed |
|-------|--------|-------|-----------|
| Phase 1: Authentication Foundation | ✅ DONE | 3 | 3/3 |
| Phase 2: Data Models | 🔴 NOT STARTED | 3 | 0/3 |
| Phase 3: API Controllers | 🔴 NOT STARTED | 2 | 0/2 |
| Phase 4: Web Authentication UI | 🔴 NOT STARTED | 2 | 0/2 |
| Phase 5: Web Features | 🔴 NOT STARTED | 6 | 0/6 |
| Phase 6: Testing | 🔴 NOT STARTED | 2 | 0/2 |
| Phase 7: Optional Features | 🔴 NOT STARTED | 4 | 0/4 |
| Phase 8: Deployment | 🔴 NOT STARTED | 5 | 0/5 |
| Phase 9: Documentation | 🔴 NOT STARTED | 3 | 0/3 |
| **TOTAL** | | **32** | **3/32** |

---

## 🎯 NEXT PRIORITIES

### Immediate Next Steps (Do in order):
1. ✅ Task 5: Create Cart Model and DbContext
2. ✅ Task 7: Create Order Models  
3. ✅ Task 18: Run Database Migrations
4. ✅ Task 6: Implement Cart API Controller
5. ✅ Task 8: Implement Order API Controller

### Estimated Time Remaining: 25-30 hours

---

## 💡 NOTES

- JWT Secret Key cần được thay đổi cho production
- Cần test kỹ authentication flow trước khi làm cart/order
- Cart phải liên kết với UserId
- Order phải được tạo từ Cart items
- Giá sản phẩm trong Order phải lưu giá tại thời điểm đặt hàng (không reference Product.Price)

---

**Last Updated:** October 14, 2025  
**Created By:** GitHub Copilot  
**Project:** E-Commerce Assignment 2 - QE180214
