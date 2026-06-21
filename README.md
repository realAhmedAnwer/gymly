<div align="center">

# 🏋️ **Gymly**

**A modern, full-featured gym management platform built with ASP.NET Core MVC and Clean Architecture.**

[![Build](https://img.shields.io/badge/build-passing-brightgreen?style=for-the-badge)](#)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/license-MIT-blue?style=for-the-badge)](LICENSE.txt)

*Manage members, trainers, sessions, bookings, attendance, and membership plans — all from a single, responsive dashboard.*

---

</div>

## 📖 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Architecture & Design Patterns](#-architecture--design-patterns)
- [Default Credentials](#-default-credentials)
- [Endpoints](#-endpoints)
- [License](#-license)

---

## ✨ Features

- **Member Management** — Register, edit, deactivate/reactivate members with full profile and QR attendance cards.
- **Trainer Management** — Onboard trainers with specialization tracking and email uniqueness enforcement.
- **Class & Session Scheduling** — Define classes and schedule sessions with trainer assignment, overlap detection, and capacity management.
- **Booking System** — Book members into sessions with real-time capacity checks, duplicate prevention, and cancellation support.
- **Attendance Check-In** — Scan QR codes or manually check in members; validates active membership before granting access.
- **Membership Plans** — Create tiered plans with pricing, duration, and granular access rules (class restrictions, time windows, booking limits).
- **Role-Based Access Control** — Three-tier role hierarchy (Super Admin, Admin, Receptionist) with cookie-based authentication.
- **Dashboard** — Real-time overview with 8 stat cards, upcoming sessions, and quick-action links.
- **Responsive UI** — Dark sidebar navigation, Tailwind CSS design system, mobile-friendly layout.
- **Redis Distributed Caching** — Lookup data (trainers, classes, roles, plans) cached in Redis with automatic invalidation on mutations.

---

## 🛠 Tech Stack

| Layer            | Technology                                                                 |
| ---------------- | -------------------------------------------------------------------------- |
| **Runtime**      | .NET 10.0, ASP.NET Core MVC                                                |
| **UI**           | Razor Views, Tailwind CSS (CDN), Inter Font, jQuery, jQuery Validation     |
| **Mediator**     | MediatR 14.1.0                                                             |
| **ORM**          | Entity Framework Core 10.0.8                                               |
| **Database**     | Microsoft SQL Server 2022                                                   |
| **Auth**         | ASP.NET Core Cookie Authentication, BCrypt.Net-Next 4.2.0                   |
| **Validation**   | FluentValidation (server-side pipeline), Data Annotations (client-side)     |
| **Containerization** | Docker, Docker Compose                                                  |
| **Caching**          | Redis 8 (Microsoft.Extensions.Caching.StackExchangeRedis 10.0.8)       |

---

## 📂 Project Structure

```
Gymly/
├── Gymly.Domain/                    # Domain layer — entities and enums only (zero dependencies)
│   ├── Entities/
│   │   ├── Users/                   #   Member, Trainer, SystemUser, SystemRole
│   │   ├── Memberships/             #   Plan, PlanAccessRule, Membership
│   │   └── Schedules/               #   Class, Session, Booking, AttendanceLog
│   └── Enums/                       #   AccessMethod, AccessType, MembershipStatus
│
├── Gymly.Application/               # Application layer — use-case orchestration via MediatR
│   ├── Common/
│   │   └── Behaviours/              #   Validation, Logging, Performance, Transaction pipelines
│   ├── Features/                    #   ★ Vertical Slices — each feature is self-contained ★
│   │   ├── Auth/                    #   Login, CreateSystemUser, DeleteSystemUser, Profile
│   │   ├── Members/                 #   CRUD + Membership & Attendance queries
│   │   ├── Trainers/                #   CRUD + Lookup queries
│   │   ├── Classes/                 #   CRUD + Lookup queries
│   │   ├── Sessions/                #   Create + Dashboard list with sort/filter
│   │   ├── Plans/                   #   CRUD + Access Rules + Activate/Deactivate
│   │   ├── Memberships/             #   Assign Member to Plan + Form data
│   │   ├── Bookings/                #   Create, Cancel, List with pagination
│   │   ├── Attendance/              #   ProcessCheckIn + Recent check-ins
│   │   └── Home/                    #   Dashboard statistics
│   └── Interfaces/                  #   IApplicationDbContext, ICurrentUserService, IQrCodeService
│
├── Gymly.Infrastructure/            # Infrastructure layer — persistence, services, seeders
│   ├── Configurations/              #   EF Core entity type configurations
│   ├── Migrations/                  #   EF Core database migrations
│   ├── Seeders/                     #   DatabaseSeeder (roles + super admin user)
│   ├── Services/                    #   CurrentUserService, QrCodeService
│   └── DependencyInjection.cs       #   Infrastructure DI registrations
│
├── Gymly.Web/                       # Presentation layer — MVC controllers, views, static assets
│   ├── Controllers/                 #   Home, Auth, Members, Trainers, Classes, Sessions, Plans, Bookings, Attendance, Users
│   ├── Models/                      #   View models with validation attributes
│   ├── Views/                       #   Razor views (shared layout, CRUD pages, dashboard)
│   ├── Filters/                     #   FluentValidation exception filter
│   ├── wwwroot/                     #   CSS, JS, vendored libraries
│   ├── Program.cs                   #   App composition and HTTP pipeline
│   └── appsettings*.json            #   Configuration files (including SuperAdminSettings)
│
├── docker-compose.yml               # Production container definitions
├── docker-compose.override.yml      # Development overrides (ports, volumes)
├── Gymly.Web/Dockerfile             # Multi-stage Docker build for the web app
├── .env.example                     # Environment variable template
└── project-architecture-map.md      # Living architecture documentation
```

---

## 🚀 Getting Started

### Prerequisites

- **Docker Desktop** (v4.0+) with Docker Compose V2
- **Git** for cloning the repository

> No .NET SDK installation is required — the application builds and runs entirely inside Docker containers using a multi-stage Dockerfile.

### 1. Clone the Repository

```bash
git clone https://github.com/realAhmedAnwer/Gymly.git
cd Gymly
```

### 2. Configure Environment Variables

```bash
cp .env.example .env
```

Open `.env` in your editor and adjust values as needed. The defaults are configured for local development.

> **Important:** If you change `MSSQL_SA_PASSWORD`, update the `CONNECTION_STRING` variable to match. The SA password must be at least 8 characters with uppercase, lowercase, digits, and symbols.

### 3. Build & Launch

```bash
docker compose up --build
```

This command:
- Builds the `Gymly.Web` Docker image using the multi-stage Dockerfile (SDK restore → build → publish → runtime image).
- Pulls the `mcr.microsoft.com/mssql/server:2022-latest` image for the database.
- Pulls the `redis:8-alpine` image for distributed caching.
- Starts all containers on a shared `gymly-network` bridge.
- Runs EF Core migrations automatically on first startup (Development mode).
- Seeds the database with default roles and the Super Admin user.

### 4. Verify

```bash
docker ps
```

Expected output:

| CONTAINER       | IMAGE           | STATUS        | PORTS                        |
| --------------- | --------------- | ------------- | ---------------------------- |
| gymly-mvc-web   | gymly-web-app   | Up (healthy)  | 0.0.0.0:8080→8080/tcp        |
| gymly-sql-db    | mcr.microsoft.com/mssql/server:2022-latest | Up | 0.0.0.0:1433→1433/tcp |
| gymly-redis     | redis:8-alpine  | Up (healthy)  | 0.0.0.0:6379→6379/tcp        |

Open your browser and navigate to:

- **Application:** [http://localhost:8080](http://localhost:8080)
- **SQL Server:** `127.0.0.1,1433` (SA user, password from `.env`)

### 5. Tear Down

```bash
# Stop containers (preserves volumes/data)
docker compose down

# Stop containers AND delete database volumes (full reset)
docker compose down -v
```

---

## 🏗 Architecture & Design Patterns

This project combines **Clean Architecture** (horizontal layer separation) with **Vertical Slice Architecture** (feature-based organization within the Application layer), creating a hybrid approach that maximizes both structural clarity and feature cohesion.

### Clean Architecture (Layered Dependencies)

The solution enforces a strict inward dependency flow:

```
Domain ← Application ← Infrastructure ← Web
```

- **Domain** has zero external dependencies — pure entities and enums.
- **Application** defines abstractions (`IApplicationDbContext`, `ICurrentUserService`, `IQrCodeService`) and orchestrates use cases.
- **Infrastructure** implements abstractions and handles persistence, external services, and database migrations.
- **Web** composes everything at startup and serves the MVC presentation layer.

### Vertical Slice Architecture (Feature-Based Organization)

Within the Application layer, each business feature is a **self-contained vertical slice** that encapsulates everything it needs — commands, queries, handlers, validators, and DTOs — organized by use case rather than by technical concern:

```
Gymly.Application/Features/
├── Members/
│   ├── Commands/
│   │   ├── CreateMember/
│   │   │   ├── CreateMemberCommand.cs            # Input record + handler
│   │   │   └── CreateMemberCommandValidator.cs   # FluentValidation
│   │   ├── UpdateMember/
│   │   ├── DeactivateMember/
│   │   └── ActivateMember/
│   └── Queries/
│       ├── GetMembers/
│       │   ├── GetMembersQuery.cs                # Query + handler
│       │   └── MemberDto.cs                      # Projection DTO
│       ├── GetMemberById/
│       ├── GetMemberMembership/
│       └── GetMemberAttendanceLogs/
├── Sessions/
│   ├── Commands/CreateSession/
│   └── Queries/GetSessionsList/
└── ... (same pattern for every feature)
```

**Key benefits of this hybrid approach:**
- **Feature isolation** — Adding a new feature means adding a new folder under `Features/` with minimal changes elsewhere.
- **Layered enforceability** — The Clean Architecture dependency rules prevent domain or application logic from leaking into the presentation layer.
- **Scalability** — Slices can be developed, tested, and maintained independently as the codebase grows.

### CQRS (Command Query Responsibility Segregation)

Each vertical slice separates **Commands** (state mutation) from **Queries** (read-only access):

| Concern       | Convention                                                          |
| ------------- | ------------------------------------------------------------------- |
| **Commands**  | Mutate state, call `SaveChangesAsync`, wrapped in transactions      |
| **Queries**   | Read-only projections with `AsNoTracking`, return DTOs              |
| **Validators**| FluentValidation runs before handlers via the `ValidationBehavior` pipeline |

### MediatR Pipeline Behaviors

All MediatR requests flow through a centralized pipeline:

1. **ValidationBehavior** — Executes FluentValidation validators; throws on failure.
2. **LoggingBehavior** — Logs request name, duration, and payload; warns on slow requests (>500ms).
3. **PerformanceBehavior** — Monitors execution time with threshold warnings.
4. **TransactionBehavior** — Wraps command handlers in database transactions.

### Other Patterns

| Pattern                      | Usage                                                              |
| ---------------------------- | ------------------------------------------------------------------ |
| **Soft Delete**              | All `BaseEntity` descendants use `IsDeleted` flag; global query filters. |
| **Audit Trail**              | `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` via `SaveChangesAsync` interception. |
| **FluentValidation**         | Server-side validation pipeline; Data Annotations for client-side only. |
| **DTO Projection**           | Queries project to DTOs with `AsNoTracking` for read performance.  |
| **Filtered Unique Indexes**  | Booking uniqueness filtered by `IsCancelled` to allow re-booking.  |

---

## 🔐 Default Credentials

Seeded automatically on first startup (values from `appsettings.Development.json`):

| Role         | Username      | Email                    | Password          |
| ------------ | ------------- | ------------------------ | ----------------- |
| Super Admin  | `superadmin`  | `superadmin@gymly.com`   | `SuperAdmin@123`  |

> **⚠️ Security Note:** Change these credentials before deploying to any non-local environment. In production, override values via environment variables or a secrets manager.

---

## 🌐 Endpoints

| Route                          | Method | Description                        |
| ------------------------------ | ------ | ---------------------------------- |
| `/`                            | GET    | Dashboard with stats               |
| `/Auth/Login`                  | GET/POST | Login page                      |
| `/Auth/Logout`                 | POST   | Logout                             |
| `/Members`                     | GET    | Member list with pagination         |
| `/Members/Create`              | GET/POST | Register new member              |
| `/Members/Edit/{id}`           | GET/POST | Edit member details              |
| `/Members/Details/{id}`        | GET    | Member profile, QR, membership      |
| `/Trainers`                    | GET    | Trainer list                        |
| `/Trainers/Create`             | GET/POST | Add new trainer                   |
| `/Classes`                     | GET    | Class list                          |
| `/Classes/Create`              | GET/POST | Define new class                   |
| `/Sessions`                    | GET    | Session dashboard with sort/filter  |
| `/Sessions/Create`             | GET/POST | Schedule a session                |
| `/Plans`                       | GET    | Plan cards with sort/filter         |
| `/Plans/Create`                | GET/POST | Create membership plan            |
| `/Plans/Edit/{id}`             | GET/POST | Edit plan + access rules          |
| `/Bookings`                    | GET    | Booking list with pagination        |
| `/Bookings/Create`             | GET/POST | Create booking                    |
| `/Attendance`                  | GET    | Check-in dashboard                  |
| `/Users`                       | GET    | System user management (Admin+)     |

---

## 📄 License

This project is licensed under the [MIT License](LICENSE.txt).

---

<div align="center">

**Built with ❤️ by Ahmed Anwer**

</div>
