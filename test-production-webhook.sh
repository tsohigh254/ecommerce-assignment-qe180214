#!/bin/bash

# Test Production Webhook
# Usage: ./test-production-webhook.sh https://your-api.onrender.com

API_URL="${1:-https://ecommerce-api-qe180214.onrender.com}"

echo "========================================="
echo "Testing Production Webhook"
echo "========================================="
echo ""

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Test 1: Check if API is reachable
echo -e "${YELLOW}Test 1: Checking API health${NC}"
response=$(curl -s -o /dev/null -w "%{http_code}" "$API_URL/api/webhook/test")

if [ "$response" -eq 200 ]; then
    echo -e "${GREEN}✅ API is reachable${NC}"
else
    echo -e "${RED}❌ API not reachable (Status: $response)${NC}"
    echo "Make sure your API is deployed and running on Render"
    exit 1
fi

echo ""

# Test 2: Get webhook endpoint URL
echo -e "${YELLOW}Test 2: Webhook Endpoint${NC}"
WEBHOOK_URL="$API_URL/api/webhook/stripe"
echo -e "${GREEN}Webhook URL: $WEBHOOK_URL${NC}"
echo ""
echo "Copy this URL to Stripe Dashboard:"
echo -e "${GREEN}$WEBHOOK_URL${NC}"
echo ""

# Test 3: Instructions for Stripe Dashboard
echo -e "${YELLOW}Test 3: Setup in Stripe Dashboard${NC}"
echo "1. Go to: https://dashboard.stripe.com/webhooks"
echo "2. Click 'Add endpoint'"
echo "3. Enter Endpoint URL: $WEBHOOK_URL"
echo "4. Select events:"
echo "   - payment_intent.succeeded"
echo "   - payment_intent.payment_failed"
echo "   - payment_intent.canceled"
echo "5. Click 'Add endpoint'"
echo "6. Copy the Signing Secret (whsec_...)"
echo "7. Add to Render environment variables:"
echo "   StripeSettings__WebhookSecret=whsec_..."
echo ""

# Test 4: Test webhook with curl (will fail without proper signature)
echo -e "${YELLOW}Test 4: Testing webhook endpoint accessibility${NC}"
response=$(curl -s -w "\nHTTP Status: %{http_code}\n" -X POST "$WEBHOOK_URL" \
  -H "Content-Type: application/json" \
  -d '{"test": "data"}')

echo "$response"
echo ""
echo "Note: This should return 400 (Missing Stripe signature) - that's expected!"
echo "The webhook will work properly when called by Stripe."
echo ""

echo -e "${GREEN}=========================================${NC}"
echo -e "${GREEN}Setup Complete!${NC}"
echo -e "${GREEN}=========================================${NC}"
echo ""
echo "Next steps:"
echo "1. Configure webhook in Stripe Dashboard"
echo "2. Add webhook secret to Render environment variables"
echo "3. Test with real payment in your app"
echo "4. Check webhook events in Stripe Dashboard"
