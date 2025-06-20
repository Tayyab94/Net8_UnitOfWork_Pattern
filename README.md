# UnitOfWork Pattern Example with .NET 8

This repository demonstrates the implementation of the **Unit of Work** pattern in a .NET 8 Web API project. The Unit of Work pattern is used to group one or more operations (usually database operations) into a single transaction, ensuring data consistency and simplifying management of changes.

## Features

- **.NET 8 Web API**: Built with the latest .NET platform.
- **Unit of Work Pattern**: Centralizes repository operations to manage transactions efficiently.
- **Repository Pattern**: Separates data access logic, making the codebase more maintainable and testable.
- **Sample Controllers and Models**: Demonstrates how to interact with the Unit of Work and repositories.

## Project Structure

- `Controllers/` – API controllers for handling HTTP requests.
- `Models/` – Domain models/entities.
- `Repos/` – Repository and Unit of Work implementations.
- `Program.cs` – Application entry point and configuration.
- `appsettings.json` – App configuration settings.

## How to Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Tayyab94/Net8_UnitOfWork_Pattern.git
   ```

2. **Open in Visual Studio 2022+ or VS Code**

3. **Restore dependencies and run the project:**
   ```bash
   dotnet restore
   dotnet run --project UnitOfWorkPractice/UnitOfWorkPractice.csproj
   ```

4. **Test the API:**  
   Use tools like Postman or Swagger (if enabled) to send requests to the endpoints.

## Why Use the Unit of Work Pattern?

- Groups multiple database operations into a single transaction.
- Reduces code duplication.
- Provides a single entry point for saving changes to the database.
- Makes your application easier to maintain and test.

## License

This project is licensed under the MIT License.

---

> **Note:** This is a basic implementation for learning purposes. You can expand upon this by adding authentication, validation, or advanced data access scenarios.

---

For more details, review the source code in the [UnitOfWorkPractice](https://github.com/Tayyab94/Net8_UnitOfWork_Pattern/tree/main/UnitOfWorkPractice) directory.
