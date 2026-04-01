# Hospital Management API

ASP.NET Core Web API for managing doctors, patients, medical services, and appointments with JWT authentication and PostgreSQL using Entity Framework Core.

## Technologies Used

- `ASP.NET Core Web API` - Framework for building REST APIs and middleware pipeline.
- `Entity Framework Core` - ORM for data access, relationships, LINQ querying, and migrations.
- `Npgsql.EntityFrameworkCore.PostgreSQL` - PostgreSQL provider for EF Core.
- `JWT Bearer Authentication` - Stateless token-based authentication for secured endpoints.
- `BCrypt.Net-Next` - Password hashing for secure credential storage.
- `Swashbuckle.AspNetCore (Swagger)` - OpenAPI generation and Swagger UI endpoint documentation.

## Project Features

- One-to-one relationship: `Doctor` -> `DoctorProfile`
- One-to-many relationship: `Doctor` -> `MedicalServices`
- Many-to-many relationship: `Patients` <-> `MedicalServices` via `Appointments`
- Service layer with dependency injection
- DTO-based request/response models with data annotation validation
- Role-based authorization (`Admin`, `Doctor`, `Patient`)
- LINQ `Select()` projection and `AsNoTracking()` on read queries
- EF Core migrations for schema versioning

## Prerequisites

- .NET SDK 10
- PostgreSQL server

## Configuration

Update connection and JWT settings in `appsettings.Development.json` (or `appsettings.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=hospitaldb;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "8f3c9a1d5b7e4f2a9c6d1e8b3f7a2c4e9d6b1a5f3c8e2d7b4a9f6c1e5d8b2a7",
    "Issuer": "HospitalManagementAPI",
    "Audience": "HospitalManagementAPIClient",
    "ExpiryHours": "24"
  }
}
```

## How To Run

1. Restore dependencies:
   - `dotnet restore`
2. Apply migrations:
   - `dotnet ef database update`
3. Run the API:
   - `dotnet run`

## API Documentation

- Swagger UI (HTTPS): `https://localhost:7253/swagger`
- Swagger UI (HTTP): `http://localhost:5142/swagger`
- OpenAPI JSON: `https://localhost:7253/swagger/v1/swagger.json`

## End-to-End API Testing Flow

Use this sequence to test all APIs without foreign key issues.

### 1) Start the project

1. `dotnet restore`
2. `dotnet ef database update`
3. `dotnet run`
4. Open Swagger: `https://localhost:7253/swagger`

### 2) Create users and get JWT tokens

1. `POST /api/auth/register` (Admin)

```json
{
  "username": "admin1",
  "email": "admin1@example.com",
  "password": "Admin123!",
  "role": "Admin"
}
```

2. `POST /api/auth/register` (Patient)

```json
{
  "username": "patient1",
  "email": "patient1@example.com",
  "password": "Student123!",
  "role": "Patient"
}
```

3. `POST /api/auth/login` for each user and copy `token`.
4. In Swagger click **Authorize** and paste:
   - `Bearer <admin-token>` for admin endpoints.
   - switch to `Bearer <patient-token>` when testing patient-only scenarios.

### 3) Doctors flow (Admin token)

1. `POST /api/doctors` to create doctor.
2. `GET /api/doctors` and `GET /api/doctors/{id}`.
3. `POST /api/doctors/profile` to create profile for that doctor.
4. `GET /api/doctors/{doctorId}/profile`.
5. `PUT /api/doctors/{id}` and `PUT /api/doctors/{doctorId}/profile` to test updates.

### 4) Patients flow (Admin token)

1. `POST /api/patients` to create a patient.
2. `GET /api/patients` and `GET /api/patients/{id}`.
3. `PUT /api/patients/{id}` to test update.

### 5) Medical Services flow (Admin or Doctor token)

1. `POST /api/medicalservices` using a valid `doctorId` from step 3.
2. `GET /api/medicalservices`.
3. `GET /api/medicalservices/{id}`.
4. `GET /api/medicalservices/doctor/{doctorId}`.
5. `PUT /api/medicalservices/{id}` to test update.

### 6) Appointments flow (Admin or Patient token)

1. `POST /api/appointments` using valid `patientId` and `medicalServiceId`.
2. `GET /api/appointments`.
3. `GET /api/appointments/{id}`.
4. `GET /api/appointments/patient/{patientId}`.
5. `GET /api/appointments/medicalservice/{medicalServiceId}`.
6. `PUT /api/appointments/{id}` to update `status`.

### 7) Authorization checks (important for assignment)

1. Call an Admin-only endpoint (for example `DELETE /api/doctors/{id}`) with Patient token.
2. Confirm it returns `403 Forbidden`.
3. Call protected endpoints without token.
4. Confirm it returns `401 Unauthorized`.

### 8) Optional cleanup

1. Delete appointments first.
2. Delete medical services.
3. Delete patients and doctors.

## Authentication Header Format

- `Authorization: Bearer <token>`

## Why HTTP-only Cookies Are Common for Authentication Security

HTTP-only cookies are widely used because JavaScript cannot read them directly, which reduces token theft risk from XSS attacks. They can also be configured with `Secure` and `SameSite` attributes to reduce transport and CSRF risks. In many production systems, this provides stronger browser-side protection than storing tokens in local/session storage, especially for web clients.

## Submission Checklist

- Source code included
- `Migrations` folder included
- README included with run steps and technologies
- README includes HTTP-only cookies explanation
- API documented with Swagger
- Swagger/Postman screenshots of working endpoints included
