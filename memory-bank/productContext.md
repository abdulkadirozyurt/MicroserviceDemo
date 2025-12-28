# Product Context

## Why this project exists
This project serves as a reference implementation for building microservices with .NET 10.0. It addresses the complexities of service discovery, resilience, and centralized routing in a distributed system.

## Problems it solves
- **Service Discovery**: How services find each other without hardcoded IP addresses.
- **Resilience**: How to handle service unavailability or slow responses.
- **Centralized Routing**: How to provide a single API endpoint for multiple microservices.

## How it should work
1. **Consul** runs as the service registry.
2. **ProductWebAPI** and **CartWebAPI** register themselves with Consul on startup.
3. **CartWebAPI** discovers **ProductWebAPI** via Consul to fetch product details.
4. **Gateway** (Ocelot) routes external requests to the appropriate microservice.
5. **Polly** ensures that calls between services are resilient to failures.

## User Experience Goals
- Developers can easily understand the microservices pattern.
- The system remains functional even if some services are temporarily slow or unavailable (within limits).
- Clients interact with a single gateway instead of multiple service endpoints.
