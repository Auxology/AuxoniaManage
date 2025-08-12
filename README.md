# 🚀 AuxoniaManage - Project Management Backend System

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-purple.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-latest-blue.svg)](https://postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-containerized-blue.svg)](https://docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

**AuxoniaManage** is a comprehensive, enterprise-grade project management backend system built with **ASP.NET Core 9.0** and **C#**. It implements **Clean Architecture principles** with **Domain-Driven Design (DDD)** patterns and features **CQRS (Command Query Responsibility Segregation)** with **Event-Driven Architecture**.

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                 AuxoniaManage.Presentation                 │  
│                 (Controllers, DTOs, API)                   │
├─────────────────────────────────────────────────────────────┤
│                AuxoniaManage.Application                   │
│           (Features, Commands, Queries, Services)          │
├─────────────────────────────────────────────────────────────┤
│               AuxoniaManage.Infrastructure                 │
│           (Data Access, External Services, Email)          │
├─────────────────────────────────────────────────────────────┤
│                  AuxoniaManage.Domain                     │
│             (Entities, Events, ReadModels, Enums)         │
└─────────────────────────────────────────────────────────────┘
│                AuxoniaManage.SharedKernel                 │
│           (Common Abstractions, Base Types, DTOs)         │
└─────────────────────────────────────────────────────────────┘
```

## ✨ Key Features

### 🏢 **Workspace Management**
- Multi-tenant workspaces with invitation-based onboarding
- Role-based access control (Owner, Admin, Member)
- Workspace-specific settings and branding

### 📋 **Project Management**
- Project organization within workspaces
- File upload and storage integration
- Project-specific configurations and metadata

### ✅ **Task Management**
- Project-specific task tracking and management
- Task assignment and status management
- Priority levels and due dates

### 👥 **User Management**
- Complete authentication system with JWT
- User registration and email verification
- Password reset and change functionality
- User profile management

### 📧 **Event-Driven Notifications**
- Real-time email notifications for system events
- Audit trail for security events
- Automated cleanup and maintenance tasks

## 🛠️ Technology Stack

### **Core Framework**
- **.NET 9.0** - Latest .NET version with performance improvements
- **ASP.NET Core** - High-performance web API framework
- **C# 12** - Latest language features and syntax improvements

### **Architecture & Patterns**
- **Clean Architecture** - Clear separation of concerns
- **CQRS** - Command Query Responsibility Segregation
- **Event-Driven Architecture** - Loose coupling through domain events
- **Domain-Driven Design** - Rich domain models with business logic
- **Repository Pattern** - Data access abstraction

### **Data & Persistence**
- **Entity Framework Core 9.0** - Modern ORM with advanced features
- **PostgreSQL** - Robust, scalable relational database
- **Microsoft Identity** - Comprehensive authentication/authorization

### **Messaging & Communication**
- **MassTransit** - Enterprise service bus implementation
- **RabbitMQ** - Reliable message broker
- **SignalR** - Real-time web functionality

### **External Services**
- **AWS SES** - Scalable email service
- **AWS S3** - Object storage for file uploads
- **AWS CloudFront** - CDN for static asset delivery

### **Development & Quality**
- **FluentValidation** - Elegant validation library
- **MediatR** - Simple mediator pattern implementation
- **Ardalis GuardClauses** - Clean input validation
- **AutoMapper** - Object-to-object mapping
- **OpenAPI/Swagger** - API documentation

## 🚀 Quick Start

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/install/)
- [PostgreSQL](https://postgresql.org/) (if running locally)

### 🐳 Docker Development Setup (Recommended)

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/AuxoniaManage.git
   cd AuxoniaManage/backend
   ```

2. **Start all services**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   - API: http://localhost:5000
   - RabbitMQ Management: http://localhost:15672 (auxonia/auxonia)
   - Aspire Dashboard: http://localhost:18888

4. **Run database migrations**
   ```bash
   docker exec backend dotnet ef database update --project AuxoniaManage.Infrastructure --startup-project AuxoniaManage.Presentation
   ```

### 💻 Local Development Setup

1. **Install dependencies**
   ```bash
   dotnet restore
   ```

2. **Set up database connection**
   ```bash
   # Update appsettings.Development.json with your PostgreSQL connection
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update --project AuxoniaManage.Infrastructure --startup-project AuxoniaManage.Presentation
   ```

4. **Start the application**
   ```bash
   dotnet run --project AuxoniaManage.Presentation
   ```

## 🔧 Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | - |
| `RabbitMQ__HostName` | RabbitMQ host | rabbitmq |
| `AWS_REGION` | AWS region for services | eu-central-1 |
| `AWS_ACCESS_KEY_ID` | AWS access key | - |
| `AWS_SECRET_ACCESS_KEY` | AWS secret key | - |

### Application Settings

Key configuration sections in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=auxonia;Username=auxonia;Password=auxonia"
  },
  "RabbitMQ": {
    "Host": "rabbitmq",
    "User": "auxonia",
    "Password": "auxonia"
  },
  "StorageSettings": {
    "BucketName": "auxoniabucket",
    "CdnUrl": "https://d1cykq3p6li75z.cloudfront.net"
  }
}
```

## 📚 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | User registration |
| POST | `/api/auth/login` | User authentication |
| POST | `/api/auth/logout` | User logout |
| POST | `/api/auth/forgot-password` | Password reset request |
| PATCH | `/api/auth/reset-password` | Password reset confirmation |
| PATCH | `/api/auth/change-password` | Change user password |
| POST | `/api/auth/verify-email` | Email verification |

### Workspace Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workspace/create` | Create new workspace |
| PATCH | `/api/workspace/update` | Update workspace details |
| GET | `/api/workspace/get` | Get workspace information |
| DELETE | `/api/workspace/delete` | Delete workspace |
| POST | `/api/workspace/rotate-invitation` | Rotate invitation token |

### Project Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/project/create` | Create new project |
| PATCH | `/api/project/update` | Update project details |
| GET | `/api/project/get` | Get single project |
| GET | `/api/project/get-all` | Get all projects in workspace |
| DELETE | `/api/project/delete` | Delete project |

### Task Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/task/create` | Create new task |
| PATCH | `/api/task/edit` | Update task details |
| GET | `/api/task/get-all` | Get all tasks in project |
| DELETE | `/api/task/delete` | Delete task |

### Example API Usage

```bash
# Register a new user
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePassword123!",
    "firstName": "John",
    "lastName": "Doe"
  }'

# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePassword123!",
    "rememberMe": false
  }'

# Create workspace (requires authentication)
curl -X POST http://localhost:5000/api/workspace/create \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <your-jwt-token>" \
  -d '{
    "name": "My Workspace",
    "description": "A workspace for project management"
  }'
```

## 🏗️ Architecture Deep Dive

### Clean Architecture Layers

1. **Presentation Layer** (`AuxoniaManage.Presentation`)
   - REST API controllers
   - Request/response DTOs
   - Global exception handling
   - Authentication middleware

2. **Application Layer** (`AuxoniaManage.Application`)
   - CQRS command and query handlers
   - Business logic orchestration
   - Validation with FluentValidation
   - Event consumers for domain events

3. **Infrastructure Layer** (`AuxoniaManage.Infrastructure`)
   - Entity Framework Core implementation
   - Repository pattern implementation
   - External service integrations (AWS, Email)
   - Database configurations

4. **Domain Layer** (`AuxoniaManage.Domain`)
   - Core business entities
   - Domain events
   - Business rules and invariants
   - Read models for queries

5. **Shared Kernel** (`AuxoniaManage.SharedKernel`)
   - Common abstractions
   - CQRS base types
   - Shared DTOs and interfaces

### Key Design Patterns

- **CQRS**: Separate command and query models for optimal performance
- **Event Sourcing**: Domain events drive business process automation
- **Repository Pattern**: Clean data access abstraction
- **Unit of Work**: Transactional data operations
- **Pipeline Behaviors**: Cross-cutting concerns like logging and validation

## 🔍 Performance Optimizations

The system includes several performance optimizations documented in [`PERFORMANCE_PATTERNS.md`](PERFORMANCE_PATTERNS.md):

- **Batch + Dictionary Lookup**: Eliminates N+1 query problems
- **HashSet Operations**: Efficient set operations for data filtering
- **Static Collections**: Optimized constant lookups
- **Async/Await**: Non-blocking I/O operations throughout

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Database Migrations

```bash
# Add new migration
dotnet ef migrations add <MigrationName> --project AuxoniaManage.Infrastructure --startup-project AuxoniaManage.Presentation

# Update database
dotnet ef database update --project AuxoniaManage.Infrastructure --startup-project AuxoniaManage.Presentation

# Remove last migration
dotnet ef migrations remove --project AuxoniaManage.Infrastructure --startup-project AuxoniaManage.Presentation
```

## 🔒 Security Features

- **JWT Authentication** with ASP.NET Core Identity
- **Role-based Authorization** for workspace access control
- **Input Validation** with FluentValidation and guard clauses
- **SQL Injection Protection** through Entity Framework parameterized queries
- **CORS Configuration** for cross-origin requests
- **Audit Trail** through domain events and logging

## 🚀 Deployment

### Docker Production Deployment

1. **Build production image**
   ```bash
   docker build -f AuxoniaManage.Presentation/Dockerfile -t auxoniamanage:latest .
   ```

2. **Run with production configuration**
   ```bash
   docker run -p 80:80 \
     -e ASPNETCORE_ENVIRONMENT=Production \
     -e ConnectionStrings__DefaultConnection="<production-connection-string>" \
     auxoniamanage:latest
   ```

### Cloud Deployment

The application is ready for deployment to:
- **AWS ECS/Fargate** with RDS PostgreSQL
- **Azure Container Apps** with Azure Database for PostgreSQL
- **Google Cloud Run** with Cloud SQL
- **Kubernetes** clusters with Helm charts

## 📈 Monitoring & Observability

- **Structured Logging** with Serilog integration
- **Health Checks** for database and external services
- **OpenTelemetry** integration for distributed tracing
- **Aspire Dashboard** for development monitoring

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines

- Follow Clean Architecture principles
- Write comprehensive tests for new features
- Update documentation for API changes
- Use conventional commit messages
- Ensure all CI checks pass

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙋‍♂️ Support

For questions and support:

- Create an [issue](https://github.com/yourusername/AuxoniaManage/issues) on GitHub
- Check the [documentation](ARCHITECTURE_DOCUMENTATION.md) for detailed architecture information
- Review [performance patterns](PERFORMANCE_PATTERNS.md) for optimization techniques

---

**Built with ❤️ using .NET 9.0 and Clean Architecture principles**