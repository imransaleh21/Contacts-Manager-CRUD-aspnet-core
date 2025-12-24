# Contacts Manager CRUD - ASP.NET Core

A comprehensive contact management system built with ASP.NET Core 8.0, implementing full CRUD operations with a clean multi-tier architecture. This application allows users to manage contact information with advanced features like searching, sorting, and exporting data in multiple formats.

## 🌟 Features

- **Complete CRUD Operations**: Create, Read, Update, and Delete contacts
- **Advanced Search & Filter**: Search contacts by name, email, date of birth, gender, country, or address
- **Flexible Sorting**: Sort contacts by any field in ascending or descending order
- **Multiple Export Formats**:
  - PDF export (landscape orientation)
  - Excel export (.xlsx)
  - CSV export
- **Country Management**: Manage countries associated with contacts
- **Data Validation**: Comprehensive input validation with custom validators
- **Database Integration**: SQL Server with Entity Framework Core
- **Stored Procedures**: Custom stored procedures for optimized database operations
- **Data Seeding**: Pre-populated sample data for countries and persons
- **Unit Testing**: Comprehensive xUnit tests for services

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0 (MVC)
- **Language**: C# with .NET 8.0
- **ORM**: Entity Framework Core 9.0.8
- **Database**: SQL Server (LocalDB)

### Frontend
- **View Engine**: Razor Views
- **UI**: HTML5, CSS3

### Libraries & Packages
- **Rotativa.AspNetCore** (1.4.0) - PDF generation
- **EPPlus** (8.2.1) - Excel file generation
- **CsvHelper** (33.1.0) - CSV file generation
- **Microsoft.EntityFrameworkCore.SqlServer** (9.0.8)
- **Microsoft.EntityFrameworkCore.Design** (9.0.8)
- **xUnit** - Unit testing framework

## 🏗️ Architecture

This project follows a **clean multi-tier architecture** with clear separation of concerns:

```
Contacts-Manager-CRUD/
├── Contacts-Manager-CRUD/    # Presentation Layer (MVC)
│   ├── Controllers/          # API/Web controllers
│   ├── Views/                # Razor views
│   ├── wwwroot/              # Static files (CSS, JS, images)
│   └── Program.cs            # Application entry point
│
├── Entities/                 # Domain Layer
│   ├── Person.cs             # Person entity
│   ├── Country.cs            # Country entity
│   ├── PersonsDbContext.cs   # EF Core DbContext
│   └── Migrations/           # Database migrations
│
├── ServiceContracts/         # Service Interfaces Layer
│   ├── IPersonsService.cs    # Persons service contract
│   ├── ICountriesService.cs  # Countries service contract
│   ├── DTO/                  # Data Transfer Objects
│   ├── Enums/                # Enumerations
│   └── Validations/          # Custom validators
│
├── Services/                 # Business Logic Layer
│   ├── PersonsService.cs     # Persons service implementation
│   ├── CountriesService.cs   # Countries service implementation
│   └── Helpers/              # Helper classes
│
└── xUnitTests/               # Testing Layer
    ├── PersonsServiceTest.cs
    └── CountriesServiceTest.cs
```

### Design Patterns
- **Dependency Injection**: Services registered in DI container
- **Repository Pattern**: Through EF Core DbContext
- **DTO Pattern**: Separation between entities and data transfer objects
- **Service Layer Pattern**: Business logic encapsulated in services

## 📋 Prerequisites

Before running this application, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server LocalDB (comes with Visual Studio)
- [Git](https://git-scm.com/) for version control

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core.git
cd Contacts-Manager-CRUD-aspnet-core
```

### 2. Configure Database Connection

Update the connection string in `Contacts-Manager-CRUD/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContactsManagerDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  }
}
```

For SQL Server, modify the connection string accordingly:

```json
"DefaultConnection": "Server=YOUR_SERVER;Database=ContactsManagerDB;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Apply Database Migrations

Navigate to the main project directory and run:

```bash
cd Contacts-Manager-CRUD
dotnet ef database update
```

This will:
- Create the ContactsManagerDB database
- Apply all migrations
- Seed initial data for countries and persons
- Create stored procedures

### 5. Run the Application

```bash
dotnet run --project Contacts-Manager-CRUD
```

Or use Visual Studio:
- Open `Contacts-Manager-CRUD.sln`
- Press `F5` or click the Run button

The application will start at `https://localhost:5001` or `http://localhost:5000`

## 📖 Usage

### Managing Contacts

1. **View All Contacts**: Navigate to the home page to see all contacts
2. **Search Contacts**: Use the search dropdown to select a field and enter search criteria
3. **Sort Contacts**: Click on column headers to sort by that field
4. **Create Contact**: Click "Create Person" button and fill in the form
5. **Export Data**:
   - Click "PersonsListPDF" to download contacts as PDF
   - Click "PersonsListCSV" to download contacts as CSV
   - Click "PersonsListExcel" to download contacts as Excel

### Person Fields
- **Person Name**: Contact's full name (max 45 characters)
- **Email**: Contact's email address (max 30 characters)
- **Date of Birth**: Contact's birth date
- **Gender**: Male, Female, or Other
- **Country**: Selected from pre-populated countries
- **Address**: Contact's address (max 65 characters)
- **Receive Newsletter**: Boolean flag
- **PIN**: Personal Identification Number (4 digits with check constraint)

## 🧪 Running Tests

Execute the unit tests using:

```bash
dotnet test
```

Or run specific test projects:

```bash
dotnet test xUnitTests/xUnitTests.csproj
```

The test suite includes:
- PersonsService tests (CRUD operations, filtering, sorting)
- CountriesService tests (country management)

## 🗂️ Database Schema

### Persons Table
- `PersonId` (GUID, Primary Key)
- `PersonName` (VARCHAR(45))
- `Email` (VARCHAR(30))
- `DateOfBirth` (DATETIME)
- `Gender` (VARCHAR(10))
- `CountryId` (GUID, Foreign Key)
- `Address` (VARCHAR(65))
- `ReceiveNewsLettter` (BIT)
- `PIN` (VARCHAR(6) with CHECK constraint: length = 4)

### Countries Table
- `CountryID` (GUID, Primary Key)
- `CountryName` (VARCHAR)

### Stored Procedures
- `GetAllPersons`: Retrieves all persons with country information
- `InsertPerson`: Inserts a new person record

## 🔧 Configuration

### EPPlus License
The application uses EPPlus for Excel generation. The license is set to non-commercial personal use in `Program.cs`:

```csharp
ExcelPackage.License.SetNonCommercialPersonal("Imran88");
```

For commercial use, update the license configuration accordingly.

### Rotativa PDF Configuration
Rotativa is configured for PDF generation. The wkhtmltopdf binaries should be in the `wwwroot/Rotativa` folder.

## 📦 Project Dependencies

### Main Application
- Microsoft.EntityFrameworkCore.Design (9.0.8)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.8)
- Rotativa.AspNetCore (1.4.0)

### Services Layer
- CsvHelper (33.1.0)
- EPPlus (8.2.1)

### Entities Layer
- Microsoft.EntityFrameworkCore.SqlServer (9.0.8)
- Microsoft.EntityFrameworkCore.Tools (9.0.8)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is available for educational and personal use.

## 👤 Author

**Imran Saleh**
- GitHub: [@imransaleh21](https://github.com/imransaleh21)

## 🙏 Acknowledgments

- ASP.NET Core documentation
- Entity Framework Core documentation
- Rotativa library for PDF generation
- EPPlus library for Excel generation
- CsvHelper library for CSV operations

## 📞 Support

For issues, questions, or suggestions, please open an issue in the GitHub repository.

---

**Happy Coding! 🚀**
