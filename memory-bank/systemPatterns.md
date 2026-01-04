# System Patterns

## Architecture
The project follows a microservices architecture with an API Gateway.

### Components
- **API Gateway (MicroserviceDemo.Gateway)**: Built with Ocelot, it acts as the entry point for all client requests.
- **Product Service (MicroserviceDemo.ProductWebAPI)**: Manages product data.
- **Cart Service (MicroserviceDemo.CartWebAPI)**: Manages shopping carts and aggregates data from the Product Service.
- **Service Registry (Consul)**: Stores service locations and health status.

## Key Technical Decisions
- **Minimal APIs**: Used for lightweight and high-performance service implementation.
- **Service Discovery**: Steeltoe.Discovery.Consul is used for registration and discovery.
- **Resilience**: Polly Resilience Pipelines are used for retries and timeouts.
- **In-Memory Database**: EF Core with In-Memory provider is used for simplicity in this demo.

## Design Patterns
- **Gateway Pattern**: Centralized entry point for microservices.
- **Service Discovery Pattern**: Dynamic location of service instances.
- **Retry Pattern**: Automatic retry of failed operations.
- **Timeout Pattern**: Preventing long-running requests from blocking resources.
- **Aggregator Pattern**: Cart service aggregates data from Product service.
- **Sidecar/Service Registry Pattern**: Services register themselves with Consul for discovery.
- **Containerization Pattern**: Multi-stage Docker builds for optimized production images.
- **Load Balancing Pattern**: Round-robin distribution of requests across multiple service instances at the gateway.
- **Rate Limiting Pattern**: Throttling client requests to prevent service abuse.
