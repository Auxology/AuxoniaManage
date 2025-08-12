# AuxoniaManage Backend Architecture Documentation

## Project Overview

**AuxoniaManage** is a comprehensive project management backend system built with **ASP.NET Core 9.0** and **C#**. The application follows **Clean Architecture principles** with **Domain-Driven Design (DDD)** patterns and implements **CQRS (Command Query Responsibility Segregation)** with **Event-Driven Architecture**.

### Core Business Domain
- **Workspace Management**: Multi-tenant workspaces with invitation-based onboarding
- **Project Management**: Project organization within workspaces
- **Task Management**: Project-specific task tracking and management
- **User Management**: Authentication, authorization, and profile management
- **Membership Management**: Role-based workspace access control

---

## Architecture Overview

### 🏗️ Clean Architecture Implementation

The solution follows **Clean Architecture** with clear separation of concerns across four distinct layers:

```
┌─────────────────────────────────────────────────────────────┐
│                    AuxoniaManage.Presentation              │
│                    (Controllers, DTOs, API)                │
├─────────────────────────────────────────────────────────────┤
│                    AuxoniaManage.Application               │
│              (Features, Commands, Queries, Services)        │
├─────────────────────────────────────────────────────────────┤
│                    AuxoniaManage.Infrastructure            │
│              (Data Access, External Services, Email)       │
├─────────────────────────────────────────────────────────────┤
│                    AuxoniaManage.Domain                    │
│                (Entities, Events, ReadModels, Enums)       │
└─────────────────────────────────────────────────────────────┘
│                    AuxoniaManage.SharedKernel              │
│              (Common Abstractions, Base Types, DTOs)       │
└─────────────────────────────────────────────────────────────┘
```

### 🎯 Key Architectural Patterns

1. **CQRS (Command Query Responsibility Segregation)**
2. **Event-Driven Architecture** with MassTransit
3. **Repository Pattern** with Unit of Work
4. **Domain-Driven Design** with Rich Domain Models
5. **Pipeline Behaviors** for cross-cutting concerns
6. **Clean Architecture** with dependency inversion

---

## Layer-by-Layer Architecture

### 1. Domain Layer (`AuxoniaManage.Domain`)

**Purpose**: Core business logic and domain models with no external dependencies.

#### 🔹 Core Entities
Rich domain models with encapsulated business logic:

- **`Workspace`**: Multi-tenant workspace entity
- **`Project`**: Project management within workspaces  
- **`ProjectTask`**: Task tracking and management
- **`UserProfile`**: User profile information
- **`Membership`**: Workspace membership and roles

#### 🔹 Domain Events
Event-driven architecture for business logic communication:

```
Events/
├── Auth/                    # Authentication lifecycle events
│   ├── EmailVerifiedEvent
│   ├── SuccessfulLoginEvent
│   ├── PasswordChangedEvent
│   └── UserRegisteredEvent
├── Membership/              # Membership management events
│   ├── MembershipCreatedEvent
│   ├── OwnershipTransferredEvent
│   └── UserMadeAdminEvent
├── Profile/                 # Profile lifecycle events
├── Project/                 # Project lifecycle events
├── ProjectTask/             # Task lifecycle events
└── Workspace/               # Workspace lifecycle events
```

#### 🔹 Read Models
Optimized data structures for query operations:

- **`ProfileReadModel`**: Optimized user profile data
- **`WorkspaceReadModel`**: Optimized workspace data
- **`ProjectReadModel`**: Optimized project data

#### 🔹 Design Patterns

**Encapsulation**: Private setters with public methods for state changes
```csharp
public sealed class Workspace
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public void UpdateWorkspace(string name, string description, DateTime timeStamp)
    {
        Name = name;
        Description = description;
        UpdatedAt = timeStamp;
    }
}
```

**Immutable Events**: Record types for domain events
```csharp
public sealed record SuccessfulLoginEvent
(
    string Id,
    string Email,
    string IpAddress,
    string UserAgent,
    DateTime LoginTime
);
```

### 2. Application Layer (`AuxoniaManage.Application`)

**Purpose**: Application business logic, use cases, and orchestration.

#### 🔹 CQRS Implementation

**Feature-Based Organization**: Commands and Queries organized by business capability:

```
Features/
├── Auth/
│   ├── Login/               # Command pattern
│   │   ├── LoginCommand.cs
│   │   ├── LoginCommandHandler.cs
│   │   └── LoginCommandValidator.cs
│   └── GetProfile/          # Query pattern
├── Workspace/
├── Project/
└── Membership/
```

**Command Pattern Implementation**:
```csharp
public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Business logic implementation
        // Event publishing
        // Response generation
    }
}
```

#### 🔹 Pipeline Behaviors

Cross-cutting concerns implemented as MediatR pipeline behaviors:

- **`ValidationPipelineBehavior`**: FluentValidation integration
- **`LoggingPipelineBehavior`**: Request/response logging
- **`ExceptionHandlingPipelineBehavior`**: Centralized error handling
- **`TransactionalPipelineBehavior`**: Database transaction management

#### 🔹 Event-Driven Architecture

**Event Consumers** for handling domain events:
```
Consumers/
├── Email/                   # Email notification consumers
│   └── Auth/
│       ├── EmailVerifiedEmailConsumer
│       ├── SuccessfulLoginEmailConsumer
│       └── PasswordChangedEmailConsumer
├── Profile/                 # Read model maintenance
├── Project/
└── Workspace/
```

#### 🔹 Service Layer

**Application Services**:
- **`IEmailService`**: Email communication abstraction
- **`IStorageService`**: File storage abstraction  
- **`IWorkspacePermissionService`**: Authorization logic
- **`ICleanUpService`**: Data cleanup operations

### 3. Infrastructure Layer (`AuxoniaManage.Infrastructure`)

**Purpose**: External concerns and data access implementation.

#### 🔹 Data Access Pattern

**Entity Framework Core** with Repository pattern:

```csharp
public sealed class WorkspaceRepository : IWorkspaceRepository
{
    private readonly ApplicationDbContext _context;
    
    public async Task<Workspace?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Workspaces
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken: cancellationToken);
    }
}
```

**DbContext Design**:
- **Identity Integration**: `IdentityDbContext<IdentityUser>`
- **Domain Entities**: Direct DbSet mapping
- **Read Models**: Separate DbSets for query optimization
- **Configuration**: Entity configurations in separate classes

#### 🔹 External Service Integration

**Service Implementations**:
- **Email Service**: AWS SES integration
- **Storage Service**: AWS S3 integration
- **Message Bus**: MassTransit with RabbitMQ

#### 🔹 Configuration Pattern

Strongly-typed configuration classes:
```csharp
public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
}
```

### 4. Presentation Layer (`AuxoniaManage.Presentation`)

**Purpose**: HTTP API endpoints and request/response handling.

#### 🔹 Controller Design

**RESTful API Controllers** with MediatR integration:

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var loginCommand = new LoginCommand(/* parameters */);
        var response = await _mediator.Send(loginCommand);
        return Ok(response);
    }
}
```

#### 🔹 Request/Response Pattern

**DTOs** for API contracts:
- Input DTOs: `LoginRequest`, `CreateProjectRequest`
- Response DTOs: Generated by command/query handlers
- Separation from domain models

#### 🔹 Cross-Cutting Concerns

**Global Exception Handling**:
```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    // Centralized exception handling and logging
}
```

**API Configuration**:
- CORS policy configuration
- Authentication/Authorization setup
- SignalR integration
- OpenAPI/Swagger documentation

### 5. SharedKernel (`AuxoniaManage.SharedKernel`)

**Purpose**: Common abstractions and shared types across layers.

#### 🔹 CQRS Abstractions

```csharp
public interface ICommand : IRequest { }
public interface ICommand<out TResponse> : IRequest<TResponse> { }
public interface ITransactionalCommand : ICommand { }
```

#### 🔹 Repository Abstractions

```csharp
public interface IReadModelRepository<T>
{
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
}
```

---

## Code Style and Conventions

### 🎨 Naming Conventions

#### **C# Language Standards**
- **PascalCase**: Classes, methods, properties, public members
- **camelCase**: Private fields with underscore prefix (`_fieldName`)
- **PascalCase**: Constants and static readonly fields
- **Interfaces**: Prefixed with `I` (e.g., `IWorkspaceRepository`)

#### **File Organization**
- **Feature Folders**: Organized by business capability
- **Single Responsibility**: One class per file
- **Consistent Naming**: File names match class names
- **Namespace Alignment**: Folder structure mirrors namespace structure

### 🏗️ Architectural Conventions

#### **Domain Layer Patterns**
```csharp
// ✅ Encapsulated entities with private setters
public sealed class Workspace
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    // Private constructor for EF Core
    private Workspace() { }
    
    // Business logic in methods
    public void UpdateWorkspace(string name, DateTime timestamp)
    {
        Name = name;
        UpdatedAt = timestamp;
    }
}
```

#### **CQRS Implementation Patterns**
```csharp
// ✅ Command/Query separation
public sealed class LoginCommand : ICommand<LoginResponse>
{
    // Immutable command properties
}

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    // Handler implementation with dependency injection
}
```

#### **Repository Pattern**
```csharp
// ✅ Interface segregation and async patterns
public interface IWorkspaceRepository
{
    Task<Workspace?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> AddAsync(Workspace workspace, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Workspace workspace, CancellationToken cancellationToken);
}
```

### 🔒 Security Patterns

#### **Guard Clauses**
Consistent input validation using Ardalis.GuardClauses:
```csharp
public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
{
    Guard.Against.Null(request, nameof(request));
    Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
    Guard.Against.NullOrEmpty(request.Password, nameof(request.Password));
}
```

#### **Authorization Patterns**
- **[Authorize]** attributes on controllers
- **[AllowAnonymous]** for public endpoints
- **Workspace Permission Service** for domain-specific authorization

### ⚡ Performance Patterns

#### **Async/Await Usage**
- **Consistent async patterns** throughout the application
- **CancellationToken support** in all async operations
- **ConfigureAwait(false)** avoided (not needed in ASP.NET Core)

#### **Entity Framework Optimization**
```csharp
// ✅ AsNoTracking for read-only operations
return await _context.Workspaces
    .AsNoTracking()
    .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
```

#### **CQRS Query Optimization**
- **Separate Read Models** for optimized queries
- **Projection patterns** for selective data loading
- **Caching strategies** for frequently accessed data

### 🧪 Testing Patterns

#### **Validation Strategies**
- **FluentValidation** for command/query validation
- **Pipeline Behaviors** for cross-cutting validation concerns
- **Domain validation** within entity methods

#### **Error Handling**
```csharp
// ✅ Custom exception types
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Invalid credentials provided.") { }
}

// ✅ Global exception handling
public class GlobalExceptionHandler : IExceptionHandler
{
    // Centralized exception processing
}
```

---

## Technology Stack

### **Core Framework**
- **.NET 9.0**: Latest .NET version
- **ASP.NET Core**: Web API framework
- **C# 12**: Latest language features

### **Data & Persistence**
- **Entity Framework Core**: ORM with PostgreSQL
- **Microsoft.AspNetCore.Identity**: Authentication/Authorization
- **Repository Pattern**: Data access abstraction

### **Messaging & Events**
- **MassTransit**: Message bus implementation  
- **RabbitMQ**: Message broker
- **Domain Events**: Event-driven architecture

### **Validation & Cross-Cutting**
- **FluentValidation**: Command/Query validation
- **MediatR**: CQRS implementation
- **Ardalis.GuardClauses**: Input validation

### **External Services**
- **AWS SES**: Email service
- **AWS S3**: File storage
- **SignalR**: Real-time communication

### **Development & Quality**
- **OpenAPI/Swagger**: API documentation
- **Serilog**: Structured logging
- **AutoMapper**: Object mapping (inferred from dependencies)

---

## Design Principles & Best Practices

### **Clean Architecture Compliance**
1. **Dependency Inversion**: Outer layers depend on inner layers
2. **Separation of Concerns**: Each layer has distinct responsibilities  
3. **Independence**: Business logic independent of frameworks
4. **Testability**: Easy unit testing through abstractions

### **Domain-Driven Design**
1. **Rich Domain Models**: Business logic in domain entities
2. **Ubiquitous Language**: Consistent terminology across layers
3. **Bounded Contexts**: Clear service boundaries
4. **Event-Driven Communication**: Loose coupling through events

### **SOLID Principles**
1. **Single Responsibility**: Classes have single reasons to change
2. **Open/Closed**: Open for extension, closed for modification
3. **Liskov Substitution**: Subtypes substitutable for base types
4. **Interface Segregation**: Small, focused interfaces
5. **Dependency Inversion**: Depend on abstractions, not concretions

### **Security Best Practices**
1. **Authentication**: ASP.NET Core Identity integration
2. **Authorization**: Role-based and resource-based authorization
3. **Input Validation**: Multiple validation layers
4. **Audit Trail**: Comprehensive event logging
5. **Secure Communication**: HTTPS and secure headers

---

## Conclusion

**AuxoniaManage** demonstrates a mature, enterprise-grade .NET architecture with:

- ✅ **Clean separation** of business and technical concerns
- ✅ **CQRS implementation** for scalable read/write operations  
- ✅ **Event-driven architecture** for loose coupling
- ✅ **Rich domain models** with encapsulated business logic
- ✅ **Comprehensive validation** and error handling
- ✅ **Modern .NET practices** and performance optimization
- ✅ **Security-first design** with multiple protection layers
- ✅ **Maintainable codebase** with consistent conventions

The architecture provides a solid foundation for a scalable project management system with excellent separation of concerns, testability, and maintainability.