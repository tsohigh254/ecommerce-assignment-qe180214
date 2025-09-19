# 🔒 SECURITY BEST PRACTICES

## ⚠️ QUAN TRỌNG: Không commit sensitive data!

### ❌ KHÔNG BAO GIỜ commit những thứ này lên Git:
- Database passwords
- API keys  
- Connection strings có password
- Private keys
- JWT secrets
- OAuth client secrets

### ✅ Cách sử dụng an toàn:

#### 1. Environment Variables (Local Development)
```bash
# File .env (đã được ignore bởi .gitignore)
DATABASE_CONNECTION_STRING=Host=your-host;Database=your-db;Username=user;Password=secret

# Load và chạy:
source ./load-env.sh
cd src/ECommerce.API
dotnet run
```

#### 2. Production Deployment 
```bash
# Render.com - Set Environment Variables trong dashboard:
DATABASE_CONNECTION_STRING=your-production-connection-string
```

#### 3. Code Setup
```csharp
// Program.cs - Đọc từ Environment Variables trước
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "fallback-connection";
```

### 🛡️ Files Structure:
```
├── .env                     # ❌ IGNORED - chứa secrets
├── .gitignore              # ✅ COMMITTED - ignore .env
├── appsettings.json        # ✅ COMMITTED - không có secrets
├── appsettings.Production.json # ✅ COMMITTED - empty connection string
└── load-env.sh            # ✅ COMMITTED - script load .env
```

### 🔍 Kiểm tra Git History:
```bash
git log --oneline --grep="password"
git show HEAD
```

Nếu đã commit secrets, cần:
1. Reset commit hoặc remove sensitive data
2. Force push: `git push --force-with-lease`
3. Rotate/change các secrets đã bị expose