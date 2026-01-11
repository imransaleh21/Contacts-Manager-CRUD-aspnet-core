<div align="center">

# 📇 Contacts Manager - ASP.NET Core MVC

### Professional Contact Management System with Clean Architecture

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://docs.microsoft.com/en-us/aspnet/core/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=for-the-badge&logo=nuget&logoColor=white)](https://docs.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)

[![Build Status](https://img.shields.io/github/actions/workflow/status/imransaleh21/Contacts-Manager-CRUD-aspnet-core/dotnet-ci.yml?branch=main&style=for-the-badge&logo=github)](https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=for-the-badge)](CONTRIBUTING.md)
[![Code Quality](https://img.shields.io/badge/code%20quality-A+-brightgreen?style=for-the-badge)](README.md)
[![GitHub Stars](https://img.shields.io/github/stars/imransaleh21/Contacts-Manager-CRUD-aspnet-core?style=for-the-badge&logo=github)](https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/imransaleh21/Contacts-Manager-CRUD-aspnet-core?style=for-the-badge&logo=github)](https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core/network/members)

---

### A comprehensive contact management system built with ASP.NET Core 8.0 MVC, demonstrating modern web development practices, clean architecture, and advanced Entity Framework Core features.

[Features](#-key-features) • [Architecture](#-architecture) • [Tech Stack](#-technology-stack) • [Getting Started](#-getting-started) • [Contributing](#-contributing)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Architecture](#-architecture)
- [Technology Stack](#-technology-stack)
- [Code Quality Practices](#-code-quality-practices)
- [Database](#-database)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure-details)
- [Views](#-views)
- [Configuration](#-configuration)
- [Learning Outcomes](#-learning-outcomes)
- [Contributing](#-contributing)
- [Security](#-security)
- [License](#-license)
- [Contact](#-contact)

---

## 🌟 Overview

This application provides a full-featured contact management system with Create, Read, Update, and Delete (CRUD) operations. It showcases professional-grade development practices including repository pattern, dependency injection, comprehensive testing, and multiple data export formats.

### 🎯 What Makes This Project Special?

- ✅ **Production-Ready Code**: Follows industry best practices and clean architecture principles
- 🧪 **Comprehensive Testing**: Unit tests, integration tests, and mocking strategies
- 📦 **Repository Pattern**: Clean separation of concerns with abstracted data access
- 🎨 **Modern UI**: Responsive design with Bootstrap and custom styling
- 📊 **Multiple Export Formats**: PDF, Excel, and CSV export capabilities
- 🔐 **Data Validation**: Custom validators and model validation throughout

---

## 🎬 Demo & Screenshots

<div align="center">

### 🖼️ Application Preview

<!-- Add your screenshots here -->
*Screenshots coming soon! The application features:*
- 📋 Clean, intuitive contact listing interface
- ➕ Easy-to-use contact creation forms
- 🔍 Advanced search and filtering options
- 📊 Multiple data export formats (PDF, Excel, CSV)
- 📱 Responsive design that works on all devices

</div>

---

## ✨ Key Features

### Core Functionality
- **Complete CRUD Operations**: Create, read, update, and delete person records
- **Advanced Search**: Filter persons by name, email, date of birth, gender, country, or address
- **Dynamic Sorting**: Sort by any field in ascending or descending order using reflection
- **Country Management**: Manage and associate countries with persons
- **Data Validation**: Custom validation attributes and model validation

### Export Capabilities
- **PDF Export**: Generate PDF reports using Rotativa
- **Excel Export**: Create Excel files using EPPlus
- **CSV Export**: Export data to CSV format using CsvHelper

### Technical Features
- **Repository Pattern**: Clean separation of data access logic
- **Dependency Injection**: Loosely coupled, testable architecture
- **Entity Framework Core**: Code-first approach with migrations
- **Stored Procedures**: Custom SQL Server stored procedures for complex operations
- **Fluent API**: Advanced entity configuration with constraints and relationships
- **Custom Validations**: Add validation and other business rules

## 🏗️ Architecture

### Clean Architecture Layers

The application follows a multi-layered architecture for separation of concerns:

```
Contacts-Manager-CRUD/          # Web Layer (MVC)
├── Controllers/                # HTTP request handlers
├── Views/                      # Razor views
└── wwwroot/                    # Static files

Entities/                       # Domain Layer
├── Person.cs                   # Person entity
├── Country.cs                  # Country entity
├── PersonsDbContext.cs         # Database context
└── Migrations/                 # EF Core migrations

ServiceContracts/               # Application Layer Contracts
├── IPersonsService.cs          # Person service interface
├── ICountriesService.cs        # Country service interface
├── DTO/                        # Data Transfer Objects
├── Enums/                      # Application enumerations
└── Validations/                # Custom validation attributes

Services/                       # Application Layer Implementation
├── PersonsService.cs           # Business logic for persons
├── CountriesService.cs         # Business logic for countries
└── Helpers/                    # Helper utilities

RepositoryContracts/            # Data Access Contracts
├── IPersonsRepository.cs       # Person repository interface
└── ICountriesRepository.cs     # Country repository interface

Repository/                     # Data Access Implementation
├── PersonsRepository.cs        # Data access for persons
└── CountriesRepository.cs      # Data access for countries

xUnitTests/                     # Test Layer
├── PersonsServiceTest.cs       # Unit tests
├── CountriesServiceTest.cs     # Unit tests
├── PersonsControllerTest.cs    # Controller tests
└── PersonsControllerIntegrationTest.cs  # Integration tests
```

## 🛠️ Technology Stack

<div align="center">

### Core Technologies

![.NET](https://img.shields.io/badge/.NET%208.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core%20MVC-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C# 12](https://img.shields.io/badge/C%23%2012-239120?style=flat-square&logo=c-sharp&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-512BD4?style=flat-square&logo=nuget&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white)

### Frontend

![HTML5](https://img.shields.io/badge/HTML5-E34F26?style=flat-square&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?style=flat-square&logo=css3&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=flat-square&logo=bootstrap&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black)

### Testing

![xUnit](https://img.shields.io/badge/xUnit-5E5E5E?style=flat-square)
![Moq](https://img.shields.io/badge/Moq-5E5E5E?style=flat-square)
![FluentAssertions](https://img.shields.io/badge/FluentAssertions-5E5E5E?style=flat-square)

</div>

### NuGet Packages

#### Web Application
- `Microsoft.EntityFrameworkCore.Design 9.0.8` - EF Core design-time tools
- `Microsoft.EntityFrameworkCore.SqlServer 9.0.8` - SQL Server provider
- `Rotativa.AspNetCore 1.4.0` - PDF generation from HTML

#### Entities Layer
- `Microsoft.EntityFrameworkCore.SqlServer 9.0.8` - Database provider
- `Microsoft.EntityFrameworkCore.Tools 9.0.8` - Migration tools

#### Services Layer
- `CsvHelper 33.1.0` - CSV file handling
- `EPPlus 8.2.1` - Excel file generation

#### Testing
- `xUnit 2.5.3` - Testing framework
- `Moq 4.20.72` - Mocking framework
- `AutoFixture 4.18.1` - Test data generation
- `FluentAssertions 8.8.0` - Fluent assertion library
- `EntityFrameworkCoreMock.Moq 2.4.0` - EF Core mocking
- `Microsoft.EntityFrameworkCore.InMemory 9.0.8` - In-memory database for testing
- `Microsoft.AspNetCore.Mvc.Testing 8.0.22` - Integration testing
- `Fizzler.Systems.HtmlAgilityPack 1.2.1` - HTML parsing for view testing

## 🎯 Code Quality Practices

### Design Patterns
- **Repository Pattern**: Abstracts data access logic
- **Dependency Injection**: Constructor-based DI throughout
- **DTO Pattern**: Separation of domain models and data transfer objects
- **Service Layer Pattern**: Business logic isolated from controllers

### Entity Framework Core Features
- **Code-First Migrations**: Version-controlled database schema
- **Fluent API**: Advanced entity configuration
  - Check constraints (e.g., PIN length validation)
  - Custom column types
  - Relationships and foreign keys
- **Stored Procedures**: Custom SQL procedures for complex queries
- **Include/ThenInclude**: Eager loading of related entities
- **Expression Trees**: Dynamic LINQ queries in repositories
- **Data Seeding**: Initial data from JSON files

### Advanced Techniques
#### Reflection-Based Sorting
Instead of lengthy switch statements, the application uses reflection for dynamic property sorting:
```csharp
// Dynamic sorting using reflection
var sortByProperty = typeof(PersonResponse).GetProperty(sortBy, 
    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
```
**Benefits**: More maintainable, works with new properties without code changes

#### Custom Validation Attributes
Custom `DateCheckerAttribute` ensures users are at least 18 years old:
```csharp
[DateChecker] // Validates minimum age requirement
public DateTime? DateOfBirth { get; set; }
```
#### Validation Helper
Centralized model validation using the `ValidationHelper` class for consistent error handling across services.

### Testing Strategy

#### Unit Tests
- **Service Layer Tests**: Mock repositories with Moq
- **AutoFixture**: Automatic test data generation
- **FluentAssertions**: Readable test assertions
- **Isolation**: Each test is independent with mocked dependencies

#### Integration Tests
- **WebApplicationFactory**: Full application testing
- **In-Memory Database**: Fast test database
- **Custom Factory**: `CustomWebApplicationFactory` for test configuration
- **HTML Parsing**: Fizzler for view testing

#### Test Coverage
- PersonsService tests
- CountriesService tests
- PersonsController tests
- End-to-end integration tests

## 🗄️ Database

### Entity Model

#### Person Entity
- PersonId (Guid, Primary Key)
- PersonName (String, max 45 chars)
- Email (String, max 30 chars)
- DateOfBirth (DateTime, nullable)
- Gender (String, max 10 chars)
- CountryId (Guid, Foreign Key, nullable)
- Address (String, max 65 chars)
- ReceiveNewsLettter (Boolean, nullable)
- PIN (String, varchar(6) with check constraint)

#### Country Entity
- CountryID (Guid, Primary Key)
- CountryName (String)

### Migrations
The application includes multiple migrations demonstrating database evolution:
- Initial schema creation
- Adding stored procedures (GetAllPersons, InsertPerson)
- Adding PIN column
- Updating stored procedures for new columns
- Adding check constraints

### Stored Procedures
- `GetAllPersons`: Retrieves all persons with country information
- `InsertPerson`: Inserts a new person record

## 🚀 Getting Started

### 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or full version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension
- (Optional) [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) for database management

### 🔧 Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core.git
   cd Contacts-Manager-CRUD-aspnet-core
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update the connection string** (if needed)
   
   Edit `Contacts-Manager-CRUD/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactsManagerDB;..."
   }
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update --project Entities --startup-project Contacts-Manager-CRUD
   ```

5. **Run the application**
   ```bash
   cd Contacts-Manager-CRUD
   dotnet run
   ```

6. **Access the application**
   
   Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

### Running Tests

Run all tests:
```bash
dotnet test
```

Run specific test project:
```bash
dotnet test xUnitTests/xUnitTests.csproj
```

Run with detailed output:
```bash
dotnet test --verbosity detailed
```

## 📁 Project Structure Details

### Controllers
- **PersonsController**: Handles person CRUD operations, search, sort, and export
- **CountriesController**: Manages country data and Excel upload

### Services
- **PersonsService**: Business logic for person management
- **CountriesService**: Business logic for country management
- **ValidationHelper**: Centralized model validation

### DTOs (Data Transfer Objects)
- **PersonAddRequest**: DTO for creating a person
- **PersonUpdateRequest**: DTO for updating a person
- **PersonResponse**: DTO for returning person data
- **CountryAddRequest**: DTO for adding a country
- **CountryResponse**: DTO for returning country data

### Enums
- **GenderOptions**: Enumeration for gender values
- **SortOrderOptions**: ASC/DESC sorting options

### Validations
- **DateCheckerAttribute**: Custom validation for minimum age (18 years)

## 🎨 Views

The application uses Razor views with:
- **_Layout.cshtml**: Master layout template
- **Index.cshtml**: Person list with search and sort
- **Create.cshtml**: Form for adding new persons
- **PersonsPDF.cshtml**: PDF report template
- **_GridColumnHeader.cshtml**: Reusable column header partial

## 🔧 Configuration

### EPPlus License
Non-commercial license configured in `Program.cs`:
```csharp
ExcelPackage.License.SetNonCommercialPersonal("<YourName>");
```
**Note**: Replace `<YourName>` with your actual name/identifier for non-commercial use.

### Rotativa Configuration
PDF generation configured with wkhtmltopdf:
```csharp
RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
```

### Dependency Injection
Services registered in `Program.cs`:
- DbContext with SQL Server provider
- Repository implementations
- Service implementations
- 
## 📚 Learning Outcomes

This project demonstrates:
- Clean architecture principles
- Repository pattern implementation
- Comprehensive unit and integration testing
- Entity Framework Core advanced features
- Custom validation attributes
- Dependency injection best practices
- Reflection for dynamic behavior
- Expression trees for flexible queries
- Multiple export format generation
- Stored procedure integration
- Database migrations and seeding

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**!

### How to Contribute

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

### 💡 Ideas for Contribution

- Add authentication and authorization
- Implement pagination for large datasets
- Add more export formats (JSON, XML)
- Create a RESTful API layer
- Add dark mode theme
- Implement real-time updates with SignalR
- Add Docker support
- Create mobile-responsive improvements

---

## 📚 Learning Outcomes

This project demonstrates:
- ✅ Clean architecture principles
- ✅ Repository pattern implementation
- ✅ Comprehensive unit and integration testing
- ✅ Entity Framework Core advanced features
- ✅ Custom validation attributes
- ✅ Dependency injection best practices
- ✅ Reflection for dynamic behavior
- ✅ Expression trees for flexible queries
- ✅ Multiple export format generation
- ✅ Stored procedure integration
- ✅ Database migrations and seeding

---

## 🔒 Security

We take security seriously. If you discover a security vulnerability, please follow our [Security Policy](SECURITY.md) for responsible disclosure.

Key security features:
- ✅ Input validation through model validation
- ✅ SQL injection prevention via Entity Framework Core
- ✅ XSS protection through Razor encoding
- ✅ CSRF protection via ASP.NET Core anti-forgery tokens

For more details, see [SECURITY.md](SECURITY.md).

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 📧 Contact

**Imran Saleh** - [@imransaleh21](https://github.com/imransaleh21)

Project Link: [https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core](https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core)

---

## ⭐ Show Your Support

If you found this project helpful or interesting, please consider giving it a ⭐ star on GitHub! It helps others discover this project.

---

<div align="center">

### 🚀 Built with passion using ASP.NET Core 8.0 MVC

**[⬆ back to top](#-contacts-manager---aspnet-core-mvc)**

</div>
