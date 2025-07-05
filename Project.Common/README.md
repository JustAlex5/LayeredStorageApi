# LayeredStorageApi

This project is a demonstration .NET 8 Web API implementing multi-layered data storage.

## Features

- Retrieves data from:
  1. Redis Cache (10 min TTL)
  2. File Storage (30 min expiration)
  3. Fallback to Database (SQL/NoSQL)
- JWT-based Authentication and Authorization
- Swagger UI documentation
- Design Patterns: Repository, Factory
- Uses:
  - Automapper
  - FluentValidation
  - Polly
- Dockerized setup

## Author

Alex Kovalyov — for home assignment submission.
