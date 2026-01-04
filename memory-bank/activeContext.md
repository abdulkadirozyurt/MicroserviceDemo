# Active Context

## Current Work Focus
- Completed project review and documentation update.

## Recent Changes
- Initial project setup with Product and Cart services.
- Added Ocelot Gateway with **Load Balancing** and **Rate Limiting**.
- Integrated Consul and Polly.
- Updated `README.md` to include Gateway features, correct endpoints, and Docker run instructions.
- Refined Docker configuration with educational comments and added Dockerfiles for all services.
- Configured Gateway for containerized environments using service hostnames.

## Next Steps
- Consider adding integration tests for the gateway and services.
- Enhance error handling in microservices.
- Refactor synchronous calls in `CartWebAPI` to use proper async/await patterns.

## Active Decisions and Considerations
- The gateway is currently configured for container-to-container communication (`product-container`, etc.).
- Rate limiting is set to a strict 1 request per minute for demonstration purposes.
- `CartWebAPI` currently uses `.Result` on an async call, which should be addressed in future refactoring.
