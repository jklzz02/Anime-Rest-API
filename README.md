# Anime REST API

ASP.NET Core Web API implementing Clean Architecture principles for anime data management with integrated machine learning recommendations and Single Page Application client.

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blueviolet)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-8.0-512BD4)](https://learn.microsoft.com/ef/core/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-00758f)](https://www.mysql.com/)
[![FluentValidation](https://img.shields.io/badge/FluentValidation-11.x-brightgreen)](https://docs.fluentvalidation.net/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D)](https://swagger.io/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-orange)](https://jwt.io/)
[![xUnit](https://img.shields.io/badge/xUnit-Testing-5c2d91)](https://xunit.net/)
[![Moq](https://img.shields.io/badge/Moq-Mocking-yellowgreen)](https://github.com/moq/moq4)

## System Overview

```mermaid
graph TB
    A[Angular SPA<br/><em>In Development</em>]
    B[YARP Proxy<br/><em>Planned</em>]
    C[ASP.NET Core API<br/><strong>This Solution</strong>]
    E[FastAPI Recommender<br/><em>In Development</em>]
    D[(MySQL Database)]

    %% Connections
    A -.->|HTTP Requests| B
    B -.->|Proxied Requests| C
    C -->|SQL Queries| D
    C -.->|REST API Calls| E

    %% Styling
    classDef planned fill:#9e9e9e,stroke:#757575,stroke-width:2px,color:#fff,stroke-dasharray: 5 5
    classDef backend fill:#5c2d91,stroke:#4a2372,stroke-width:3px,color:#fff
    classDef database fill:#00758f,stroke:#005d73,stroke-width:2px,color:#fff

    class A planned
    class B planned
    class C backend
    class E planned
    class D database
```

**Current Status**: The Angular frontend, YARP reverse proxy, and FastAPI recommender service are in development. This repository contains the complete, production-ready REST API solution.

## Solution Architecture

This solution follows **Clean Architecture** principles with clear separation of concerns across four projects

```mermaid
graph TB
    Core["AnimeApi.Server.Core
• Abstractions & Interfaces
• Models & DTOs
• Shared Objects"]

    DataAccess["AnimeApi.Server.DataAccess
• EF Core DbContext
• Repository Implementations
• Migrations"]

    Business["AnimeApi.Server.Business
• Business Logic Services
• Validation (FluentValidation)
• Mapping Extensions
• Authentication (JWT)"]

    Server["AnimeApi.Server (API)
• Controllers & Endpoints
• Middleware (Auth, Logging)
• Swagger / OpenAPI
• DI & Configuration"]

    Test["AnimeApi.Server.Test
• Unit Tests (xUnit)
• Mocking (Moq)"]

    Core --> |Models - interfaces| DataAccess
    Core --> |Dto - interfaces - Object wrappers| Business

    DataAccess --> |Entities| Business

    Business --> |Dto| Server

    Test --> |Business logic unit tests| Business

    classDef core fill:#5c2d91,stroke:#4a2372,stroke-width:3px,color:#fff
    classDef data fill:#00758f,stroke:#005d73,stroke-width:2px,color:#fff
    classDef business fill:#9e9e9e,stroke:#757575,stroke-width:2px,color:#fff
    classDef api fill:#ff9800,stroke:#e68900,stroke-width:2px,color:#fff
    classDef test fill:#4caf50,stroke:#357a38,stroke-width:2px,color:#fff

    class Core core
    class DataAccess data
    class Business business
    class Server api
    class Test test

```

### [AnimeApi.Server.Core](./AnimeApi.Server.Core/)

**Purpose**: Defines contracts and shared objects across all layers

- **Interfaces**: Repository abstractions (`IRepository<T>`, domain service contracts)
- **Models**: Domain entities and data models
- **DTOs**: Data transfer objects for API communication
- **Shared Objects**: Search parameters, constants, enumerations
- **Dependencies**: Completely independent from other projects to ensure loose coupling

### [AnimeApi.Server.DataAccess](./AnimeApi.Server.DataAccess/)

**Purpose**: Implements data persistence using Entity Framework Core

- **Repository Implementation**: Concrete implementations of Core interfaces
- **Database Context**: EF Core `DbContext` configuration
- **Dependency Injection**: `ServiceCollectionExtensions.cs` for DI registration
- **Dependencies**: `AnimeApi.Server.Core`

### [AnimeApi.Server.Business](./AnimeApi.Server.Business/)

**Purpose**: Encapsulates business logic and orchestration

- **Business Services**: Domain logic implementation
- **Mapping Extensions**: Entity to DTO transformation methods
- **Validators**: Input validation using FluentValidation
- **Authentication**: JWT token services and identity management
- **Dependency Injection**: Service registration extensions
- **Dependencies**: `AnimeApi.Server.Core` only (no DataAccess reference)

### [AnimeApi.Server](./AnimeApi.Server/)

**Purpose**: Web API layer and application entry point

- **Controllers**: RESTful API endpoints
- **Middleware**: Authentication, logging, exception handling
- **Configuration**: Dependency injection wiring and application setup
- **Swagger**: API documentation and testing interface
- **Dependencies**: All other projects for DI container configuration

### [AnimeApi.Server.Test](./AnimeApi.Server.Test)

**Purpose**: Comprehensive unit testing suite

- **Business Logic Tests**: Validation of business layer functionality
- **Test Framework**: xUnit with Moq for mocking
- **Coverage**: Business layer methods and service implementations

## Clean Architecture Benefits

- **Independence**: Core layer has zero dependencies, enabling flexibility
- **Testability**: Business logic isolated from infrastructure concerns
- **Maintainability**: Clear separation allows independent evolution of layers
- **Dependency Inversion**: Higher-level modules don't depend on lower-level modules

## Technology Stack

| Layer              | Technologies                    |
| ------------------ | ------------------------------- |
| **Web API**        | ASP.NET Core, Swagger/OpenAPI   |
| **Business Logic** | FluentValidation, Google Oauth2 |
| **Data Access**    | Entity Framework Core, MySQL    |
| **Authentication** | JWT Bearer tokens               |
| **Testing**        | xUnit, Moq                      |

## Database Integration

- **Provider**: MySQL with Entity Framework Core (Pomelo)
- **Migrations**: EF migrations
- **Connection Management**: Managed through ASP.NET Core DI

## API Features

- **RESTful Design**: Standard HTTP methods and status codes
- **Authentication**: JWT-based security
- **Validation**: Input validation through FluentValidation
- **Documentation**: Swagger UI
- **Error Handling**: Consistent error response format

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- MySQL Server 8.0+
- IDE (Visual Studio, VS Code, or Rider)

### Setup

```bash
git clone https://github.com/jklzz02/Anime-Rest-API
cd Anime-Rest-API
```

**Restore dependencies & update database:**

```bash
dotnet restore

dotnet ef database update --project AnimeApi.Server.DataAccess --startup-project AnimeApi.Server
```

**Setup secrets & config:**

There's a comprehensive [sample file](./AnimeApi.Server/appsettings.Sample.json) to setup the web project secrets such as

- Google client secret
- Secret key

and also domains for external services and the client, to avoid `cors` issues and to hard code domains.

- Recommender domain
- Client domain

**Run the Web project:**

```bash
dotnet run --project AnimeApi.Server
```

### Testing

```bash
dotnet test AnimeApi.Server.Test

```

- Generators are used to create testing data
- Theories and interface mocks are the preferred approach

## Development Workflow

1. **Domain Changes**: Start with Core project if needed (abstractions)
2. **Data Layer**: Implement repositories in DataAccess project
3. **Business Logic**: Add services or repositories to helpers in Business project
4. **API Endpoints**: Create controllers in Server project
5. **Testing**: Write unit tests in Test project

---
