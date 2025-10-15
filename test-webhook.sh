#!/bin/bash

# Test Stripe Webhook Integration
# This script tests the webhook endpoint functionality

echo "==================================="
echo "Testing Stripe Webhook Integration"
echo "==================================="
echo ""

# Colors for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

API_URL="http://localhost:8080"

# Test 1: Check if webhook endpoint is reachable
echo -e "${YELLOW}Test 1: Checking webhook endpoint${NC}"
response=$(curl -s -o /dev/null -w "%{http_code}" "$API_URL/api/webhook/test")

if [ "$response" -eq 200 ]; then
    echo -e "${GREEN}✅ Webhook endpoint is reachable${NC}"
else
    echo -e "${RED}❌ Webhook endpoint not reachable (Status: $response)${NC}"
    exit 1
fi

echo ""

# Test 2: Check Stripe CLI installation
echo -e "${YELLOW}Test 2: Checking Stripe CLI installation${NC}"
if command -v stripe &> /dev/null; then
    echo -e "${GREEN}✅ Stripe CLI is installed${NC}"
    stripe --version
else
    echo -e "${RED}❌ Stripe CLI not found${NC}"
    echo "Install it with: brew install stripe/stripe-cli/stripe"
    echo "Or download from: https://stripe.com/docs/stripe-cli"
    exit 1
fi

echo ""

# Test 3: Instructions for webhook forwarding
echo -e "${YELLOW}Test 3: Webhook Forwarding Instructions${NC}"
echo "To test webhooks locally, run:"
echo ""
echo -e "${GREEN}stripe listen --forward-to http://localhost:8080/api/webhook/stripe${NC}"
echo ""
echo "Then in another terminal, trigger a test event:"
echo -e "${GREEN}stripe trigger payment_intent.succeeded${NC}"
echo ""

# Test 4: Sample webhook payload
echo -e "${YELLOW}Test 4: Sample Webhook Payload${NC}"
echo "A successful payment webhook will contain:"
echo '{
  "id": "evt_xxx",
  "type": "payment_intent.succeeded",
  "data": {
    "object": {
      "id": "pi_xxx",
      "amount": 5000,
      "currency": "usd",
      "status": "succeeded"
    }
  }
}'
echo ""

# Test 5: Check if orders exist in database
echo -e "${YELLOW}Test 5: Checking for orders with PaymentIntentId${NC}"
echo "To verify webhook updates, create an order with payment first:"
echo "1. Login to the web app: http://localhost:3000"
echo "2. Add items to cart"
echo "3. Proceed to checkout"
echo "4. Complete payment with test card: 4242 4242 4242 4242"
echo "5. Watch the webhook logs for automatic status update"
echo ""

echo -e "${GREEN}==================================="
echo "Webhook Testing Guide Complete"
echo "===================================${NC}"
echo ""
echo "Next steps:"
echo "1. Start Stripe webhook forwarding: stripe listen --forward-to http://localhost:8080/api/webhook/stripe"
echo "2. Copy the webhook secret (whsec_...) to your .env file"
echo "3. Restart docker-compose: docker-compose down && docker-compose up -d"
echo "4. Test with: stripe trigger payment_intent.succeeded"
echo "5. Check API logs: docker logs ecommerce-api-dev -f"
echo ""
