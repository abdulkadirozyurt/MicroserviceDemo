# MicroserviceDemo

A demonstration of microservices architecture built with .NET 10.0, showcasing service discovery, resilience patterns, and communication using **Consul**, **Steeltoe**, **Polly**, and **Minimal APIs**.

## Tech Stack

- **Framework**: .NET 10.0
- **Service Discovery**: [Consul](https://www.consul.io/)
- **Client Library**: [Steeltoe](https://steeltoe.io/)
- **Resilience**: [Polly](https://www.thepollyproject.org/) (Retry & Timeout policies)
- **API Gateway**: [Ocelot](https://ocelot.readthedocs.io/)
- **Database**: Entity Framework Core (In-Memory)
- **API Style**: Minimal APIs
- **Communication**: HttpClient with Service Discovery

## Project Structure

The solution consists of three main components that communicate via Consul Service Discovery and use Polly for fault tolerance:

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

### 3. MicroserviceDemo.Gateway
The entry point for the microservices architecture.
- **Port**: `5001`
- **Responsibilities**:
  - Routes requests to the appropriate microservice using Ocelot.
  - Provides a unified API surface for clients.

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
docker run -d --name consul -p 8500:8500 hashicorp/consul:latest
```
Or run the binary directly:
```bash
consul agent -dev
```

#### 2. Start Microservices
Open three terminal windows:

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

**Terminal 3 - Gateway:**
```bash
dotnet run --project MicroserviceDemo.Gateway
```
*Runs on http://localhost:5001 and routes requests to services.*

## Docker Configuration

The project includes a `Dockerfile` for each service to demonstrate containerization concepts.

### Multi-Stage Builds
The Dockerfiles use **multi-stage builds** to optimize image size:
1. **Base**: A lightweight runtime image.
2. **Build**: A full SDK image for compiling the code.
3. **Publish**: Prepares the binaries for deployment.
4. **Final**: Copies only the necessary files back to the base image.

### Layer Caching
We copy the `.csproj` files and run `dotnet restore` before copying the rest of the source code. This allows Docker to cache the dependencies layer, making subsequent builds much faster if only the code changes.

### Running with Docker

To run the entire stack using Docker:

1. **Build the images:**
   ```bash
   docker build -t product-api -f MicroserviceDemo.ProductWebAPI/Dockerfile .
   docker build -t cart-api -f MicroserviceDemo.CartWebAPI/Dockerfile .
   docker build -t gateway-api -f MicroserviceDemo.OcelotGateway/Dockerfile .
   ```

2. **Run the containers:**
   *(Note: You may need to create a docker network or adjust hostnames in `ocelot.json` and `appsettings.json` for full container-to-container communication)*
   ```bash
   docker run -d --name product -p 6001:8080 product-api
   docker run -d --name cart -p 6010:8080 cart-api
   docker run -d --name gateway -p 5001:8080 gateway-api
   ```

## API Endpoints

### Gateway (Port 5001) - Recommended Entry Point

#### `GET /api/products/getall`
Routes to Product Service `GET /getall`.

#### `GET /api/carts/getall`
Routes to Cart Service `GET /getall`.

### Product Service (Port 6001)

#### `GET /`
Returns a welcome message.

#### `GET /health`
Health check endpoint used by Consul.

#### `GET /getall`
Retrieves a list of all products.
- **Response**: JSON array of products.

### Cart Service (Port 6010)

#### `GET /getall`
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