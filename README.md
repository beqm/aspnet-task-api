# **.NET Hexagonal Architecture Example**
This project aims to demonstrate the implementation of a .NET application using Hexagonal Architecture. The core idea is to illustrate a clear separation of concerns, leading to a more maintainable, testable, and technology-agnostic application design.

# 🔁 **Hexagonal Architecture Summary**
The architecture is designed around the idea of ports and adapters:

- The core domain (Domain + Application) defines the behavior.
- The outer layers (Infrastructure + API) adapt external inputs/outputs to fit the core.
- This separation makes the application testable, replaceable, and resilient to change.

# 📁 Project Structure Overview
This project is divided into logical layers that follow **Hexagonal Architecture** principles:

### `Domain`
- Contains the **business rules**, **entities**, and **value objects**.
- Defines contracts (e.g., `IRepository`) that the infrastructure layer implements.
- Does not depend on any other project.

### `Application`
- Holds **use cases**, **Dtos**, and **application logic**.
- Uses MediatR for commands and queries (CQRS pattern).
- Depends on `Domain` and `Infrastructure`.

### `Infrastructure`
- Contains the **implementations of interfaces** defined in `Application`.
- Handles persistence (e.g., Entity Framework Core), external APIs, file systems, etc.
- Depends on `Domain`.

### `Api`
- The outermost layer responsible for exposing **HTTP endpoints** (via ASP.NET Core).
- Receives requests, transforms them into application calls, and returns responses.
- Depends on `Application` and `Infrastructure`.

# 🏁 **Getting Started**
To get this project up and running, please follow these simple steps:

## ✅ **Prerequisites**
Ensure you have the .NET SDK 9+ installed on your machine. You can download the latest version [here](https://dotnet.microsoft.com/pt-br/download/dotnet/9.0).

## ▶️ **Running the Application Locally**

### **Create the Database Table**
Navigate to the project's root directory in your terminal and run these commands:

```bash
    dotnet ef migrations add Init --project Infrastructure --startup-project Api
```

and

```bash
    dotnet ef database update --project Infrastructure --startup-project Api
```

### **Start the API**
After ensuring the database table is created, navigate into the Api directory and run the application:

```bash
    dotnet run
```

This command will build and run the API.

### **Explore in your browser**
- [Scalar](http://localhost:5148/scalar/)
- [Swagger](http://localhost:5148/swagger/index.html)

Feel free to explore the project structure to understand how the various layers (e.g., Domain, Application, Infrastructure, API) interact within the principles of hexagonal architecture.