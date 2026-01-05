# MicroserviceDemo

A demonstration of microservices architecture built with .NET 10.0, showcasing service discovery, resilience patterns, and communication using **Consul**, **Steeltoe**, **Polly**, **YARP**, and **Minimal APIs**.

## Tech Stack

- **Framework**: .NET 10.0
- **Service Discovery**: [Consul](https://www.consul.io/)
- **Client Library**: [Steeltoe](https://steeltoe.io/)
- **Resilience**: [Polly](https://www.thepollyproject.org/) (Retry, Timeout & Circuit Breaker policies)
- **API Gateway**:
  - [Ocelot](https://ocelot.readthedocs.io/)
  - [YARP (Yet Another Reverse Proxy)](https://microsoft.github.io/reverse-proxy/)
- **Database**: Entity Framework Core (In-Memory)
- **API Style**: Minimal APIs
- **Documentation**: [Scalar](https://scalar.com/) (Integrated in YARP Gateway)
- **Communication**: HttpClient with Service Discovery (Inter-service) & Reverse Proxy (Gateway)

## Project Structure

The solution consists of four main components. The microservices communicate via Consul Service Discovery (Cart -> Product), while the Gateways route external traffic to the services.

### 1. MicroserviceDemo.ProductWebAPI
The core service responsible for managing product data.
- **Port**: `6001` (Instance 1), `6002` (Instance 2 via Docker)
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

### 3. MicroserviceDemo.OcelotGateway
The classic entry point using Ocelot.
- **Port**: `5001`
- **Responsibilities**:
  - Routes requests to the appropriate microservice.
  - **Load Balancing**: Round Robin strategy across `product-container` and `product-container2`.
  - **Rate Limiting**: Configured for 10 requests per minute.
  - **QoS**: Implements Circuit Breaker (Breaks after 3 failures for 30s).
  - **CORS**: Enabled for all origins.

### 4. MicroserviceDemo.YarpGateway
A modern reverse proxy entry point using YARP.
- **Port**: `5002`
- **Responsibilities**:
  - High-performance routing to microservices.
  - **Rate Limiting**: Fixed Window strategy (5 permits per 5 seconds).
  - **Load Balancing**: Round Robin strategy.
  - **API Documentation**: Integrates Scalar for unified API exploration.

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

You need to run Consul, the microservices, and at least one gateway.

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
Open terminals for each service:

**Product Service:**
```bash
dotnet run --project MicroserviceDemo.ProductWebAPI
```
*Runs on http://localhost:6001 and registers with Consul.*

**Cart Service:**
```bash
dotnet run --project MicroserviceDemo.CartWebAPI
```
*Runs on http://localhost:6010 and discovers Product Service via Consul.*

**Ocelot Gateway:**
```bash
dotnet run --project MicroserviceDemo.OcelotGateway
```
*Runs on http://localhost:5001.*

**YARP Gateway:**
```bash
dotnet run --project MicroserviceDemo.YarpGateway
```
*Runs on http://localhost:5002.*

## Docker Configuration

The project is fully containerized. The `docker-compose.yml` orchestrates the entire stack, including two instances of the Product Service for load balancing demonstration.

### Multi-Stage Builds
The Dockerfiles use **multi-stage builds** to optimize image size and cache dependencies.

### Running with Docker

To run the entire stack using Docker Compose (Recommended):

1. **Build and Run:**
   ```bash
   docker-compose up -d --build
   ```

   This will start:
   - Consul
   - Product Service (2 Instances)
   - Cart Service
   - Ocelot Gateway (Port 5001)
   - YARP Gateway (Port 5002)

2. **Verify Containers:**
   ```bash
   docker ps
   ```

## API Endpoints

### Gateways (Recommended Entry Points)

You can use either Gateway. They route to container hostnames (`product-container`, `cart-container`).

| Endpoint | Description | Ocelot (5001) | YARP (5002) |
|----------|-------------|---------------|-------------|
| **Get Products** | List all products | `GET /api/products/getall` | `GET /api/products/getall` |
| **Get Carts** | List carts with details | `GET /api/carts/getall` | `GET /api/carts/getall` |

### Resilience & Behavior

- **Load Balancing**: Repeated requests to `/api/products/getall` will cycle between the two Product Service instances (check logs or headers if available).
- **Rate Limiting**:
  - **Ocelot**: Returns `418 I'm a teapot` if > 10 req/min.
  - **YARP**: Returns `503 Service Unavailable` (default) or `429` if > 5 req/5s.
- **Circuit Breaker (Ocelot)**: If the Product Service fails 3 times, Ocelot will open the circuit for 30s.

### Service Direct Access (Localhost)

- **Product Service**: `http://localhost:6001/getall`
- **Cart Service**: `http://localhost:6010/getall`
- **Consul UI**: `http://localhost:8500`
