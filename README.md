# User Management API (Minimal API - ASP.NET Core)

This projest is a Test Project

This project is a simple User Management API built using **ASP.NET Core Minimal APIs** in a single `Program.cs` file. 
---

## 🚀 Features

- ✅ Create, read, update, and delete (CRUD) user records
- 🔒 Token-based authentication
- ⚠️ Centralized error handling (returns JSON error messages)
- 📝 Logging of HTTP methods, paths, and response status codes
- 📦 Input validation using data annotations

---

## 🧪 Endpoints

| Method | Endpoint        | Description             |
|--------|------------------|-------------------------|
| GET    | `/users`         | Get all users           |
| GET    | `/users/{id}`    | Get a user by ID        |
| POST   | `/users`         | Create a new user       |
| PUT    | `/users/{id}`    | Update an existing user |
| DELETE | `/users/{id}`    | Delete a user           |

---

## 🔐 Authentication

All endpoints require an Authorization header with a token:

Authorization: Bearer mysecrettoken123

If the token is missing or invalid, the API returns:

```json
{
  "error": "Unauthorized"
}

🧾 Example POST Request Body
{
  "name": "Alice",
  "email": "alice@example.com"
}

❌ Validation Errors
If user input is invalid (e.g., missing name or invalid email), the API returns:

[
  "Name is required.",
  "Invalid email format."
]

📂 File Structure
UserManagementAPI/
│
├── Program.cs        # All logic in a single file
└── README.md         # Project documentation


🧪 How to Run
dotnet run

👤 Author
Created by Max
