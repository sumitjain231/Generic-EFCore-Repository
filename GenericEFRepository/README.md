# GenericEFRepository

## Overview

GenericEFRepository is a .NET 6 solution that provides a generic repository pattern implementation using Entity Framework Core. The solution is structured into multiple projects, each serving a specific purpose within the architecture.

## Projects

### 1. GenericEFRepositoryBLL
This project contains the business logic layer of the application. It references the domain model, data access layer, and utility projects.

### 2. GenericEFRepositoryDAL
This project contains the data access layer of the application. It references the domain data and framework projects.

### 3. GenericEFRepositorySampleAPI
This project is a sample API that demonstrates how to use the generic repository pattern. It references the business logic layer project and includes Swagger for API documentation.

### 4. EFDomainModel
This project contains the domain models used by Entity Framework Core.

### 5. EFDomainData
This project contains the Entity Framework Core migrations and context configuration.

### 6. Utility
This project contains utility classes and methods used across the solution.

## Getting Started

### Prerequisites
- .NET 6 SDK
- Visual Studio 2022

### Setup

1. Clone the repository:
    git clone https://github.com/your-repo/GenericEFRepository.git

2. Open the solution in Visual Studio 2022.

3. Restore the NuGet packages:
    dotnet restore

4. Apply the migrations to the database:
    dotnet ef database update --project EFDomainData

5. Run the sample API project:
    dotnet run --project GenericEFRepositorySampleAPI

### Usage

- The sample API project includes Swagger for API documentation. You can access it by navigating to `http://localhost:5000/swagger` in your browser.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
