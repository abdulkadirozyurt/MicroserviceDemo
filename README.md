# MicroserviceDemo

A demonstration of microservices architecture built with .NET 10.0, showcasing communication between services using Minimal APIs, Entity Framework Core (In-Memory), and HttpClient.

## Tech Stack

- **Framework**: .NET 10.0
- **Database**: Entity Framework Core (In-Memory)
- **API Style**: Minimal APIs
- **Communication**: HttpClient

## Project Structure

The solution consists of two microservices:

### 1. MicroserviceDemo.ProductWebAPI
The core service responsible for managing product data.
- **Port**: `6001`
- **Responsibilities**:
  - Provides product information.
  - Seeds demo data.

### 2. MicroserviceDemo.CartWebAPI
A service that manages user carts and aggregates product details.
- **Port**: `6010`
- **Responsibilities**:
  - Manages cart items.
  - Communicates with `ProductWebAPI` to fetch product names.

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

You need to run both microservices simultaneously. Open two terminal windows:

**Terminal 1 - Product Service:**
```bash
dotnet run --project MicroserviceDemo.ProductWebAPI
```
*Runs on http://localhost:6001*

**Terminal 2 - Cart Service:**
```bash
dotnet run --project MicroserviceDemo.CartWebAPI
```
*Runs on http://localhost:6010*

## API Endpoints

### Product Service (Port 6001)

#### `GET /products`
Retrieves a list of all products.
- **Response**: JSON array of products.

### Cart Service (Port 6010)

#### `GET /carts`
Retrieves a list of carts with enriched product details.
- **Response**: JSON array of cart items including product names fetched from the Product Service.

**Example Response:**
```json
[
  {
    "id": "0193630f-5e88-724d-b039-019310287532",
    "productId": "6a18b9d2-9537-4c12-86de-70bb61192ee0",
    "name": "Smartphone",
    "quantityPerUnit": 1
  }
]
```