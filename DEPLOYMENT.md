# QE180214 Assignment 1 - Deployment Instructions

## 🎯 Quick Deployment Guide

### Step 1: Database Setup (Neon.tech)
1. Visit [neon.tech](https://neon.tech) and create free account
2. Create new project: "ecommerce-qe180214"
3. Copy connection string from dashboard
4. Format: `Host=ep-XXX.neon.tech;Database=neondb;Username=neondb_owner;Password=XXX;SSL Mode=Require`

### Step 2: GitHub Repository
```bash
# Initialize git in project root
cd ~/assignment-qe180214
git init
git add .
git commit -m "Initial commit - E-commerce QE180214"

# Create GitHub repo (replace with your username)
# Repository name: ecommerce-assignment-qe180214
git remote add origin https://github.com/YOUR_USERNAME/ecommerce-assignment-qe180214.git
git branch -M main
git push -u origin main
```

### Step 3: Deploy API (Render.com)
1. Go to [render.com](https://render.com) → Sign up/Login
2. **New** → **Web Service**
3. Connect GitHub repository
4. Settings:
   - **Name**: `ecommerce-api-qe180214`
   - **Root Directory**: `src/ECommerce.API`
   - **Environment**: `Docker`
   - **Plan**: Free
5. **Environment Variables**:
   ```
   DATABASE_URL=<your-neon-connection-string>
   ASPNETCORE_ENVIRONMENT=Production
   ```
6. **Deploy** → Wait 5-10 minutes

### Step 4: Deploy Frontend (Render.com)
1. **New** → **Web Service**
2. Connect same GitHub repository
3. Settings:
   - **Name**: `ecommerce-web-qe180214`
   - **Root Directory**: `src/ECommerce.Web`
   - **Environment**: Docker
   - **Plan**: Free
4. **Environment Variables**:
   ```
   ApiBaseUrl=https://ecommerce-api-qe180214.onrender.com/api
   ASPNETCORE_ENVIRONMENT=Production
   ```
5. **Deploy** → Wait 5-10 minutes

### Step 5: Test Deployment
1. **API**: https://ecommerce-api-qe180214.onrender.com/swagger
2. **Frontend**: https://ecommerce-web-qe180214.onrender.com
3. Test all CRUD operations

### Step 6: Document Submission
Create file: **QE180214_Ass1.docx**

```
E-COMMERCE PLATFORM ASSIGNMENT
Student ID: QE180214

1. GITHUB REPOSITORY
   https://github.com/YOUR_USERNAME/ecommerce-assignment-qe180214

2. DEPLOYED LINKS
   API: https://ecommerce-api-qe180214.onrender.com
   Frontend: https://ecommerce-web-qe180214.onrender.com

3. FEATURES IMPLEMENTED
   ✅ Complete CRUD API endpoints
   ✅ Responsive MVC frontend
   ✅ PostgreSQL database integration
   ✅ Professional UI with Bootstrap 5
   ✅ Docker containerization
   ✅ Cloud deployment

4. TECHNOLOGY STACK
   - Backend: .NET 8 Web API + Entity Framework
   - Frontend: .NET 8 MVC + Bootstrap 5
   - Database: PostgreSQL (Neon.tech)
   - Deployment: Render.com
```

## ⚠️ Important Notes

### Free Tier Limitations
- **Render.com**: Apps sleep after 15 minutes of inactivity
- **First request** after sleep takes 30-60 seconds (normal)
- **Neon.tech**: 0.5GB database limit (sufficient for assignment)

### If Deployment Fails
1. Check build logs in Render dashboard
2. Verify Dockerfile paths are correct
3. Ensure environment variables are set
4. Check connection string format

### Testing Checklist
- [ ] API responds to GET /api/products
- [ ] Swagger documentation loads
- [ ] Frontend displays product list
- [ ] Can create new product
- [ ] Can edit existing product
- [ ] Can delete product
- [ ] Mobile responsive design works

## 🎉 Success Indicators

✅ **API URL**: https://ecommerce-api-qe180214.onrender.com/swagger  
✅ **Frontend URL**: https://ecommerce-web-qe180214.onrender.com  
✅ **GitHub**: https://github.com/YOUR_USERNAME/ecommerce-assignment-qe180214  
✅ **Document**: QE180214_Ass1.docx with all links  

**Total Cost**: $0.00 (All free tiers)  
**Assignment Requirements**: 100% Completed ✅