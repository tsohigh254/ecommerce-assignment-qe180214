# 🔐 Local Development Setup

## Quick Start

1. **Copy environment template:**
   ```bash
   cp .env.example .env
   ```

2. **Add your Stripe Test Keys to `.env`:**
   
   Get your keys from: https://dashboard.stripe.com/test/apikeys
   
   ```bash
   STRIPE_SECRET_KEY=sk_test_your_actual_key_here
   STRIPE_PUBLISHABLE_KEY=pk_test_your_actual_key_here
   ```

3. **Start the application:**
   ```bash
   docker-compose up -d
   ```

4. **Access the app:**
   - Web: http://localhost:5173
   - API: http://localhost:8080

## 🧪 Testing with Stripe Test Cards

Use these test card numbers:
- **Success**: 4242 4242 4242 4242
- **Declined**: 4000 0000 0000 0002
- **Insufficient funds**: 4000 0000 0000 9995

Any future expiry date and any 3-digit CVV will work.

## 📚 Full Documentation

See [SETUP_AND_TESTING_GUIDE.md](./SETUP_AND_TESTING_GUIDE.md) for complete instructions.
