# Tech Context

## Technologies Used
- **Framework**: .NET 10.0
- **Service Discovery**: Consul
- **Client Library**: Steeltoe.Discovery.Consul
- **Resilience**: Polly
- **API Gateway**: Ocelot
- **Database**: Entity Framework Core (In-Memory)
- **Communication**: HttpClient

## Development Setup
- **OS**: Windows (current environment)
- **IDE**: VS Code
- **Tools**: Docker (used for Consul and service containerization), .NET 10.0 SDK

## Technical Constraints
- Services must be registered with Consul to be discoverable.
- Polly pipelines must be configured for inter-service communication.
- Ocelot configuration (`ocelot.json`) must match the service ports and paths.

## Dependencies
- `Steeltoe.Discovery.Consul`
- `Polly`
- `Ocelot`
- `Microsoft.EntityFrameworkCore.InMemory`
