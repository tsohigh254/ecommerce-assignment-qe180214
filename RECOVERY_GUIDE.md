# 🔄 RECOVERY GUIDE - QE180214

## Scenario 1: Máy mới / Reinstall

### Step 1: Clone Repository
```bash
git clone https://github.com/tsohigh254/ecommerce-assignment-qe180214.git
cd ecommerce-assignment-qe180214
```

### Step 2: Install Dependencies
```bash
# Install .NET 8 SDK
sudo apt update
sudo apt install -y dotnet-sdk-8.0

# Install tools
dotnet tool install --global dotnet-ef
```

### Step 3: Setup Local Environment (Optional)
```bash
# Create .env file if need local development
echo "DATABASE_CONNECTION_STRING=Host=ep-bitter-heart-a1fstxow-pooler.ap-southeast-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_DuZ63vKLnYzH;SslMode=Require" > .env

# Make script executable
chmod +x load-env.sh
```

### Step 4: Test Local (Optional)
```bash
# Load environment and run
source ./load-env.sh
cd src/ECommerce.API
dotnet run
```

## Scenario 2: Code Changes & Redeploy

### Step 1: Make Changes
```bash
# Edit code files
# Test locally if needed
```

### Step 2: Commit & Push
```bash
git add .
git commit -m "Your changes description"
git push
```

### Step 3: Auto-Deploy
```
✅ Render.com automatically detects changes
✅ Rebuilds both API and Frontend services  
✅ Live URLs updated within 5-10 minutes
```

## Scenario 3: Complete Redeploy

### If services deleted from Render:
1. Go to Render.com dashboard
2. Create new Web Services
3. Connect same GitHub repository
4. Use same configuration:
   - API: Dockerfile = src/ECommerce.API/Dockerfile
   - Frontend: Dockerfile = src/ECommerce.Web/Dockerfile
   - Environment variables as documented

### If database deleted from Neon:
1. Create new Neon.tech database
2. Update environment variables in Render
3. Migration will run automatically

## Scenario 4: Assignment Submission

### For teacher evaluation:
```
✅ Live URLs: Always accessible
✅ GitHub: Always accessible  
✅ Documentation: In repository
✅ No local setup required for evaluation
```

## 🔒 Backup Strategy

### Critical Files Already Backed Up:
- ✅ All source code → GitHub
- ✅ Database → Neon.tech cloud
- ✅ Deployment config → Repository
- ✅ Documentation → Repository

### Local Files to Backup:
- 📁 .env (if used for local dev)
- 📁 Any custom configurations

## 🎯 Key Points

1. **Cloud Applications:** Never affected by local machine
2. **Source Code:** Always safe on GitHub
3. **Database:** Always safe on Neon.tech  
4. **Re-deployment:** Fully automated via git push
5. **Assignment:** Perpetually accessible for grading

## 🚨 Emergency Recovery

If everything fails:
1. Clone from GitHub ✅
2. Redeploy to Render ✅  
3. Database still intact ✅
4. Assignment URLs restored ✅

**Total recovery time: ~15 minutes**