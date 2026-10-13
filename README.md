# E-Commerce API

A RESTful E-Commerce API built with **ASP.NET Core 8** and **Entity Framework Core (SQL Server)**. It covers authentication, product catalog, shopping cart, orders, and online payment with **Stripe**, following SOLID principles and a layered architecture.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Payment Flow (Stripe)](#payment-flow-stripe)
- [Getting Started](#getting-started)
- [Configuration](#configuration)

---

## Features

- **Authentication:** register, login, email-exists check, current user, and saved shipping address (JWT-based)
- **Products:** paginated product listing, product details, brands, and types
- **Response caching:** custom `[Cache]` attribute (300 seconds) on product listing
- **Shopping cart:** create/update, get, and delete a cart by key (stored in Redis)
- **Orders:** create an order from the cart, list the user's orders, get an order by id, and list delivery methods
- **Payments:** Stripe payment intent per cart + webhook that updates the payment status (verified with the `Stripe-Signature` header)
- **Global error handling:** custom middleware that returns consistent error responses
- **Data seeding:** database is seeded automatically on startup
- **Swagger UI:** interactive API documentation

---

## Tech Stack

| Category        | Technology                     |
| --------------- | ------------------------------ |
| Framework       | ASP.NET Core 8 Web API         |
| ORM             | Entity Framework Core 8        |
| Database        | SQL Server (data + Identity), Redis (cart) |
| Auth            | ASP.NET Identity + JWT Bearer  |
| Payments        | Stripe (Payment Intents + Webhook) |
| Docs            | Swagger (Swashbuckle)          |

---

## Project Structure

```
E-Commerce/
├── Core/                          # Domain entities, contracts, services
├── Infrastructure/
│   ├── Persistence/               # DbContext, repositories, migrations, seeding
│   └── Presentation/              # Controllers, custom attributes
├── Shared/                        # DTOs and shared helpers
└── E-Commerce.Web/                # Startup project: Program.cs, middlewares, DI
```

---

## API Endpoints

### Authentication — `/api/Authentication`

| Method | Endpoint                         | Access | Description                     |
| ------ | -------------------------------- | ------ | ------------------------------- |
| POST   | `/api/Authentication/Register`   | Public | Create a new account            |
| POST   | `/api/Authentication/Login`      | Public | Login and receive a JWT         |
| GET    | `/api/Authentication/CheckEmail` | Public | Check if an email is registered |
| GET    | `/api/Authentication/CurrentUser`| Auth   | Get the logged-in user          |
| GET    | `/api/Authentication/Address`    | Auth   | Get the user's saved address    |
| PUT    | `/api/Authentication/Address`    | Auth   | Update the user's address       |

### Products — `/api/Products`

| Method | Endpoint               | Description                                     |
| ------ | ---------------------- | ----------------------------------------------- |
| GET    | `/api/Products`        | Paginated product list (filter via query params) |
| GET    | `/api/Products/{id}`   | Get a product by id                             |
| GET    | `/api/Products/brands` | Get all brands                                  |
| GET    | `/api/Products/types`  | Get all product types                           |

### Cart — `/api/Cart`

| Method | Endpoint           | Description                |
| ------ | ------------------ | -------------------------- |
| GET    | `/api/Cart?key=`   | Get a cart by key          |
| POST   | `/api/Cart`        | Create or update a cart    |
| DELETE | `/api/Cart/{key}`  | Delete a cart              |

### Orders — `/api/Orders` (requires authentication)

| Method | Endpoint                     | Description                       |
| ------ | ---------------------------- | --------------------------------- |
| POST   | `/api/Orders`                | Create an order                   |
| GET    | `/api/Orders`                | Get all orders of the current user |
| GET    | `/api/Orders/{id}`           | Get an order by id                |
| GET    | `/api/Orders/DeliveryMethods`| Get available delivery methods    |

### Payment — `/api/Payment`

| Method | Endpoint                 | Description                                   |
| ------ | ------------------------ | --------------------------------------------- |
| POST   | `/api/Payment/{cartId}`  | Create or update a Stripe payment intent      |
| POST   | `/api/Payment/WebHook`   | Stripe webhook that updates the payment status |

---

## Authentication

The API uses **JWT Bearer** authentication.

1. Register or log in to receive a token
2. Send it with every protected request:

```
Authorization: Bearer {your_token}
```

In Swagger UI, click **Authorize** and paste `Bearer {your_token}`.

---

## Payment Flow (Stripe)

1. The client builds a cart (`POST /api/Cart`)
2. The client calls `POST /api/Payment/{cartId}` to create or update a Stripe **payment intent** for that cart
3. The response contains the client secret used by the frontend to confirm the payment
4. Stripe calls `POST /api/Payment/WebHook` with the result
5. The API verifies the `Stripe-Signature` header, then updates the payment status

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or Express)
- Redis (`docker run -d -p 6379:6379 --name redis-local redis`)
- A Stripe account (test keys)

### Run locally

```bash
git clone https://github.com/eslam352005/E-Commerce-API.git
cd E-Commerce-API
dotnet restore
dotnet ef database update --project Infrastructure/Persistence --startup-project E-Commerce.Web
cd E-Commerce.Web
dotnet run
```

Open Swagger at `https://localhost:{port}/swagger`.

---

## Configuration

Keep secrets out of `appsettings.json`. Use **User Secrets** in development:

```bash
cd E-Commerce.Web
dotnet user-secrets init
dotnet user-secrets set "JwtOptions:SecretKey" "<long-random-key>"
dotnet user-secrets set "Stripe:Secretkey" "<sk_test_...>"
dotnet user-secrets set "Stripe:EndPointSecret" "<whsec_...>"
```

Non-secret values go in `appsettings.json`:

| Key                                  | Description                                   |
| ------------------------------------ | --------------------------------------------- |
| `ConnectionStrings:DefaultConnection`  | SQL Server connection string (store data)     |
| `ConnectionStrings:IdentityConnection` | SQL Server connection string (Identity data)  |
| `ConnectionStrings:RedisConnection`    | Redis, e.g. `localhost`                       |
| `JwtOptions:issuer` / `audience`       | JWT issuer and audience                       |
| `Stripe:Publishablekey`                | Stripe publishable key (`pk_test_...`)        |

In production use environment variables (e.g. `Stripe__Secretkey`, `JwtOptions__SecretKey`).

> Never commit real secret keys. Use your own Stripe test keys.
