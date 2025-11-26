# MicroserviceDemo

A demonstration of microservices architecture built with .NET 10.0, showcasing service discovery, resilience patterns, and communication using **Consul**, **Steeltoe**, **Polly**, and **Minimal APIs**.

## Tech Stack

- **Framework**: .NET 10.0
- **Service Discovery**: [Consul](https://www.consul.io/)
- **Client Library**: [Steeltoe](https://steeltoe.io/)
- **Resilience**: [Polly](https://www.thepollyproject.org/) (Retry & Timeout policies)
- **Database**: Entity Framework Core (In-Memory)
- **API Style**: Minimal APIs
- **Communication**: HttpClient with Service Discovery

## Project Structure

The solution consists of two microservices that communicate via Consul Service Discovery and use Polly for fault tolerance:

### 1. MicroserviceDemo.ProductWebAPI
The core service responsible for managing product data.
- **Port**: `6001`
- **Service Name**: `ProductWebAPI`
- **Responsibilities**:
  - Registers with Consul.
  - Provides product information.
  - Exposes health checks at `/health`.

### 2. MicroserviceDemo.CartWebAPI
A service that manages user carts and aggregates product details.
- **Port**: `6010`
- **Service Name**: `CartWebAPI`
- **Responsibilities**:
  - Uses Consul to discover `ProductWebAPI`.
  - Uses **Polly Resilience Pipelines** (Retry & Timeout) for robust service-to-service communication.
  - Manages cart items.
  - Aggregates data from the Product Service.

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Consul](https://www.consul.io/downloads) (or Docker)

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

You need to run Consul and both microservices.

#### 1. Start Consul
If you have Docker installed, you can run Consul with:
```bash
docker run -d -p 8500:8500 --name consul consul
```
Or run the binary directly:
```bash
consul agent -dev
```

#### 2. Start Microservices
Open two terminal windows:

**Terminal 1 - Product Service:**
```bash
dotnet run --project MicroserviceDemo.ProductWebAPI
```
*Runs on http://localhost:6001 and registers with Consul.*

**Terminal 2 - Cart Service:**
```bash
dotnet run --project MicroserviceDemo.CartWebAPI
```
*Runs on http://localhost:6010 and discovers Product Service via Consul.*

## API Endpoints

### Product Service (Port 6001)

#### `GET /`
Returns a welcome message.

#### `GET /health`
Health check endpoint used by Consul.

#### `GET /products`
Retrieves a list of all products.
- **Response**: JSON array of products.

### Cart Service (Port 6010)

#### `GET /carts`
Retrieves a list of carts with enriched product details (fetched dynamically from Product Service).
- **Response**: JSON array of cart items including product names.
- **Resilience**: This endpoint uses a Polly pipeline with:
  - **Retry**: 3 attempts with a 10-second delay.
  - **Timeout**: 30 seconds.

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