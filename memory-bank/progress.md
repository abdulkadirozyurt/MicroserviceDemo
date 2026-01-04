# Progress

## What Works
- **ProductWebAPI**: Basic CRUD (getall) and Consul registration.
- **CartWebAPI**: Cart management, Consul discovery, and Polly integration.
- **Gateway**: Ocelot routing with **Load Balancing** (Round Robin) and **Rate Limiting**.
- **Service Discovery**: Consul integration via Steeltoe.
- **Documentation**: `README.md` is accurate, includes all components, and provides Docker run instructions.
- **Containerization**: Dockerfiles for all services with optimized multi-stage builds.

## What's Left to Build
- More robust error handling in services.
- Integration tests for the gateway and services.
- Fix blocking async calls in `CartWebAPI`.

## Current Status
- Project is functional and demonstrates advanced gateway patterns.

## Known Issues
- `CartWebAPI` uses `.Result` on an async call, which can cause deadlocks in some environments.
- Gateway hostnames in `ocelot.json` are container-specific.
