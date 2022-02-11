# Checkout.com Challenge
## Recruitment challenge to create an API for a Payment Gateway.

See [API design & architectural approach](approach.md) for details & discussions.


## Prerequisites
* [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Docker](https://docs.docker.com/get-docker/) (if running with docker)
    
## Build, test, run locally


### Option 1 - use Docker image

e.g. to map port 8081 to the container:
```
docker run -d -p 127.0.0.1:8081:80 roblascelles/checkoutchallengewebapi
```

Open browser to swagger UI: e.g http://localhost:8081/swagger

### Option 2 - use Visual Studio

Using Visual Studio 2019/2022, set [CheckoutChallenge.WebAPI](https://github.com/roblascelles/CKOChallenge/tree/master/src/CheckoutChallenge.WebAPI) as startup project & run.

### Option 3 - use command line

Using command line (from root of repository):

Run tests:
```
dotnet test
```

Run Web API:
```
dotnet run --project .\src\CheckoutChallenge.WebAPI\CheckoutChallenge.WebAPI.csproj
```

Open browser to swagger UI: e.g https://localhost:7019/swagger

## Explore the API

### Set authorize API Key

Without authorisation (so a valid API key pass in the `Authorization` header), the endpoints should return 401 (unauthorised).

In Swagger UI, set the Authorization header (click "Authorize" button top right):

Allowed values are:
* `secret_key_test_a`
* `secret_key_test_b`

Test API Keys [hardcoded here](//github.com/roblascelles/CKOChallenge/blob/master/src/CheckoutChallenge.WebAPI/Auth/TestAPIKeyStore.cs#L9)

### Process a payment:

Using Swagger UI, POST a payment request to `/api/payments/` - sample request:

```
{
  "merchantRef": "ref#1",
  "amount": 2199,
  "currency": "GBP",
  "expiry": "2030/12",
  "cvv": "737",
  "pan": "4111111111111111",
  "cardHolderName": "Mr Test"
}
```

Validation rules [defined here](//github.com/roblascelles/CKOChallenge/blob/master/src/CheckoutChallenge.WebAPI/Models/ProcessPaymentRequest.cs#L10) - e.g. posting with an empty CVV value, will return 400.

Sample successful response:
```
{
  "paymentId": "e10e3dcd06024a0c8ffb5234dce8ca8b",
  "approved": true,
  "merchantRef": "ref#1",
  "status": "Authorized",
  "amount": 2199,
  "currency": "GBP",
  "authCode": "fe0f095581ce4cec99414627eb645568"
}
```

To make the fake acquirer return a response that isn't authorised, there are [magic CVV values defined](//github.com/roblascelles/CKOChallenge/blob/master/src/CheckoutChallenge.Acquirers.Faked/FakeAcquirer.cs#L25-L28) e.g. using 400 as the CVV will return a declined authorisation from the acquirer.



### Retrieve a payment:

Using Swagger UI, GET the payment status at `/api/payments/{paymentId}` - where `paymentId` is the value returned from the process payment endpoint.

Sample response:
```
{
  "paymentId": "e10e3dcd06024a0c8ffb5234dce8ca8b",
  "status": "Authorized",
  "authCode": "3615bf9dfb0b41fba58584802a02da05",
  "amount": 2199,
  "currency": "GBP",
  "merchantRef": "ref#1",
  "cardNumber": "411111******1111",
  "cardExpiry": "2030/12",
  "cardHolder": "Mr Test"
}
```

