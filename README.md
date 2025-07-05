# Layered Storage Backend - Home Assignment

This project implements a modular .NET-based backend system deployed via Docker Swarm. It features authentication, data caching, file storage, and database access, and demonstrates a scalable microservice architecture suitable for production-ready DevOps environments.

---

##  Services Overview

### 1. **UserManagment.API**
- JWT-based authentication and token issuance
- Configuration-based JWT settings (key, issuer, audience, expiration)
- Intended to manage login logic, token generation, and possibly authorization.

### 2. **LayeredStorage.API**
- Core data retrieval layer
- Multi-tiered storage access:
  -  **Redis** cache (10 minutes TTL)
  -  **File** storage as JSON (30 minutes TTL)
  -  **MariaDB** fallback if cache/file missing
- Custom logic for data freshness and fallbacks
- Uses configuration from `appsettings_layered_storage.json`

---

##  Infrastructure

Each service is deployed as a **Docker Swarm stack**. Services are connected using a **shared overlay network** named `shared-backend`.

### External Dependencies:
- **Redis**: In-memory caching
- **MariaDB (Bitnami)**: Persistent relational database
- **SeaweedFS**: Distributed file system for storing uploaded files

```bash
docker network create --driver overlay --attachable shared-backend
docker stack deploy -c mariadb-stack.yml mariadb-stack
docker stack deploy -c redis-stack.yml redis-stack
docker stack deploy -c seaweedfs-stack.yml seaweedfs-stack
docker stack deploy -c backend-stack.yml backend-stack
```

---

##  Configuration Management

This project uses Docker **configs** to mount service-specific `appsettings.json` files during Swarm deploy:

```yaml
configs:
  usermanagment-config:
    file: ./appsettings_user_managment.json

  layeredstorage-config:
    file: ./appsettings_layered_storage.json
```

---

##  File Server Support (Bonus)

The `LayeredStorage.API` has support for file upload using SeaweedFS:

- Upload endpoint available
- SeaweedFS configured via:
```json
"FileService": {
  "SeaweedUrl": "http://seaweedfs",
  "BasePath": "Data"
}
```

 *Download functionality (`GET`) is not yet implemented, but can be added as a next step.*

---

##  Swagger Documentation

Once the services are running, you can explore the APIs using Swagger:

- [http://localhost:5001/swagger](http://localhost:5001/swagger) → UserManagment API
- [http://localhost:5002/swagger](http://localhost:5002/swagger) → LayeredStorage API

---

##  Future Improvements

- Add `GET` download support for files via SeaweedFS
- Add API Gateway (e.g. Traefik or Ocelot)
- Integrate Serilog + centralized logging
- Use Docker Secrets for sensitive configuration
- Add healthchecks and readiness probes

---

##  Author

**Alex Kovalyov** — July 2025  

