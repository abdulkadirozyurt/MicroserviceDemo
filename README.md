# MicroserviceDemo

A simple Microservice demonstration project built with .NET 10.0, showcasing a Product Web API using Minimal APIs and Entity Framework Core with an In-Memory database.

## Tech Stack

- **Framework**: .NET 10.0
- **Database**: Entity Framework Core (In-Memory)
- **API Style**: Minimal APIs

## Project Structure

The solution consists of a single microservice:

- **MicroserviceDemo.ProductWebAPI**: The main Web API project handling product operations.
  - `Models/Product.cs`: Defines the Product entity.
  - `Context/ApplicationDbContext.cs`: EF Core database context.
  - `Program.cs`: Application entry point and API endpoint definitions.

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/abdulkadirozyurt/MicroserviceDemo.git
   ```
2. Navigate to the project directory:
   ```bash
   cd MicroserviceDemo
   ```

### Running the Application

Run the Product Web API project:

```bash
dotnet run --project MicroserviceDemo.ProductWebAPI
```

The application will start, and you can access the endpoints.

## API Endpoints

### GET /products

Retrieves a list of all products.

- **URL**: `/products`
- **Method**: `GET`
- **Response**: JSON array of products.

**Example Response:**

```json
[
  {
    "id": "01935e62-8975-7312-9226-c220ef3533fb",
    "name": "Smartphone",
    "stockAmount": 50
  }
]
```

*Note: The application seeds a demo "Smartphone" product every time the `/products` endpoint is called for demonstration purposes.*