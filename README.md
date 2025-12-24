# Contacts Manager - ASP.NET Core MVC CRUD Application

A comprehensive contact management system built with ASP.NET Core 8.0 MVC, demonstrating modern web development practices, clean architecture, and advanced Entity Framework Core features.

## 📋 Overview

This application provides a full-featured contact management system with Create, Read, Update, and Delete (CRUD) operations. It showcases professional-grade development practices including repository pattern, dependency injection, comprehensive testing, and multiple data export formats.

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
- **Custom Validations**: Age validation (minimum 18 years) and other business rules

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

### Core Technologies
- **.NET 8.0**: Latest LTS version of .NET
- **ASP.NET Core MVC**: Model-View-Controller web framework
- **C# 12**: Modern C# with nullable reference types enabled
- **Entity Framework Core 9.0.8**: ORM for data access
- **SQL Server**: Relational database (LocalDB for development)

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

#### Expression Trees for Filtering
Repository uses expression trees for flexible, composable queries:
```csharp
Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);
```

#### Custom Validation Attributes
Custom `DateCheckerAttribute` ensures users are at least 18 years old:
```csharp
[DateChecker] // Validates minimum age requirement
public DateTime? DateOfBirth { get; set; }
```

#### Validation Helper
Centralized model validation using `ValidationHelper` class for consistent error handling across services.

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

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server (LocalDB is included with Visual Studio)
- Visual Studio 2022 or VS Code (optional)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/<your-username>/Contacts-Manager-CRUD-aspnet-core.git
   cd Contacts-Manager-CRUD-aspnet-core
   ```
   **Note**: Replace `<your-username>` with the actual repository owner's username.

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

## 🧪 Code Highlights

### Nullable Reference Types
Enabled throughout the project for better null safety:
```xml
<Nullable>enable</Nullable>
```

### InternalsVisibleTo
Test project has access to internal members:
```xml
<InternalsVisibleTo Include="xUnitTests" />
```

### Partial Program Class
Enables integration testing:
```csharp
public partial class Program { }
```

### Data Seeding
Countries and persons seeded from JSON files in `OnModelCreating`.

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

## 📝 License

This project is for educational purposes.

## 🤝 Contributing

This is a learning project. Feel free to fork and experiment with your own enhancements.

---

Built with ASP.NET Core 8.0 MVC
