# Anime Rest API

```mermaid

graph TB
A[Angular SPA]
B[YARP Proxy]
C[ASP.NET Core API]
E[FastAPI Recommender]
D[(MySQL Database)]

    %% Connections
    A -->|HTTP Requests| B
    B -->|Proxied Requests| C
    C -->|SQL Queries| D
    C -->|REST API Calls| E

    %% Styling
    classDef frontend fill:#dd0031,stroke:#c50025,stroke-width:2px,color:#fff
    classDef proxy fill:#512bd4,stroke:#4527a0,stroke-width:2px,color:#fff
    classDef backend fill:#5c2d91,stroke:#4a2372,stroke-width:2px,color:#fff
    classDef ml fill:#009688,stroke:#00796b,stroke-width:2px,color:#fff
    classDef database fill:#00758f,stroke:#005d73,stroke-width:2px,color:#fff

    class A frontend
    class B proxy
    class C backend
    class E ml
    class D database
```

This repository is part of a larger project and contains only the main backend.

- [Client side application](https://github.com/jklzz02/Anime-client)

- [Recommender API](https://github.com/jklzz02/Anime-recommender)
