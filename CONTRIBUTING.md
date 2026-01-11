# Contributing to Contacts Manager

First off, thank you for considering contributing to Contacts Manager! It's people like you that make this project such a great learning resource.

## 📋 Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Setup](#development-setup)
- [Pull Request Process](#pull-request-process)
- [Coding Standards](#coding-standards)
- [Commit Message Guidelines](#commit-message-guidelines)

## 📜 Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to the project maintainers.

## 🤝 How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues to avoid duplicates. When you create a bug report, include as many details as possible:

- **Use a clear and descriptive title**
- **Describe the exact steps to reproduce the problem**
- **Provide specific examples**
- **Describe the behavior you observed and what you expected**
- **Include screenshots if applicable**
- **Note your environment** (.NET version, OS, SQL Server version)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, include:

- **Use a clear and descriptive title**
- **Provide a detailed description of the suggested enhancement**
- **Explain why this enhancement would be useful**
- **List any alternatives you've considered**

### Pull Requests

- Fill in the required template
- Follow the C# coding standards
- Include appropriate test cases
- Update documentation as needed
- Ensure all tests pass
- Follow the commit message guidelines

## 🛠️ Development Setup

1. **Fork and clone the repository**
   ```bash
   git clone https://github.com/your-username/Contacts-Manager-CRUD-aspnet-core.git
   cd Contacts-Manager-CRUD-aspnet-core
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Set up the database**
   ```bash
   dotnet ef database update --project src/ContactsManager.Infrastructure --startup-project src/ContractsManager.UI
   ```

4. **Run the application**
   ```bash
   cd src/ContractsManager.UI
   dotnet run
   ```

5. **Run tests**
   ```bash
   dotnet test
   ```

## 🔄 Pull Request Process

1. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes**
   - Write clean, maintainable code
   - Add or update tests as needed
   - Update documentation if required

3. **Commit your changes**
   ```bash
   git commit -m "Add feature: your feature description"
   ```

4. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

5. **Open a Pull Request**
   - Provide a clear title and description
   - Reference any related issues
   - Ensure all CI checks pass
   - Wait for review and address any feedback

## 💻 Coding Standards

### C# Style Guidelines

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Keep methods small and focused
- Add XML documentation comments for public APIs
- Use async/await for asynchronous operations
- Prefer LINQ over loops where appropriate

### Project Structure

- **Core Layer**: Domain entities and interfaces (no dependencies)
- **Infrastructure Layer**: Data access, repositories, DbContext
- **UI Layer**: Controllers, views, and presentation logic
- **Tests**: Unit tests and integration tests

### Testing

- Write unit tests for all business logic
- Use meaningful test names that describe the scenario
- Follow the Arrange-Act-Assert pattern
- Mock dependencies using Moq
- Aim for high code coverage

### Example Test Structure

```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    var input = new InputObject();
    
    // Act
    var result = methodUnderTest(input);
    
    // Assert
    result.Should().NotBeNull();
}
```

## 📝 Commit Message Guidelines

### Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation only changes
- **style**: Changes that don't affect code meaning (formatting, etc.)
- **refactor**: Code change that neither fixes a bug nor adds a feature
- **perf**: Performance improvement
- **test**: Adding or updating tests
- **chore**: Changes to build process or auxiliary tools

### Examples

```
feat(persons): add email validation to person entity

Add custom validation attribute to ensure email format is correct
before saving to database.

Closes #123
```

```
fix(export): resolve CSV export encoding issue

Fixed character encoding problem when exporting non-ASCII characters
to CSV format.

Fixes #456
```

## 🧪 Testing Guidelines

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test project
dotnet test test/xUnitTests/xUnitTests.csproj
```

### Writing Tests

- Test edge cases and boundary conditions
- Use descriptive test method names
- Keep tests independent and isolated
- Use AutoFixture for test data generation when appropriate
- Mock external dependencies

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)

## ❓ Questions?

Feel free to open an issue with the `question` label if you have any questions about contributing.

---

Thank you for contributing to Contacts Manager! 🎉
