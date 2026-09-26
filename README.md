📚 Library Management System API

A RESTful Web API built with ASP.NET Core and Entity Framework Core following Clean Architecture principles. This system enables librarians to efficiently manage books, authors, categories, members, and borrowing workflows.

🎯 Project Overview & Purpose

The main objective of this project is to provide a robust back-end system for managing library operations. It automates day-to-day administrative tasks such as tracking inventory, keeping records of members and authors, and handling book borrowing and returns reliably.

🏗️ Architecture & Technology Stack

Framework: ASP.NET Core Web API

Database Access: Entity Framework Core

Database: SQL Server

Architecture Pattern: Clean Architecture

Documentation: Swagger / OpenAPI

✨ Core Features

📖 1. Book Management

Add, update, and delete books.

Retrieve all books or get detailed information for a specific book.

Search books by Title or ISBN.

Filter books by Category or Author.

Check real-time book availability for borrowing.

✍️ 2. Author Management

Create, update, and delete authors.

View author profiles and list all books published by a specific author.

🏷️ 3. Category Management

Manage book categories (Create, Read, Update, Delete).

Retrieve all books belonging to a specific category.

👤 4. Member Management

Register, update, and remove library members.

View member profiles and track member borrowing history.

🔄 5. Borrowing & Return Operations

Borrow Books: Issue books to active members.

Return Books: Process book returns and update inventory state.

Validation: Automatic checks to prevent borrowing already unavailable books.

Due Date Calculation: Automatically tracks and calculates borrowing deadlines and history.

🔐 Bonus Features

Authentication: User login functionality using JWT (JSON Web Tokens).

Authorization: Role-based access control with Admin and Librarian roles.

Interactive API Docs: Integrated Swagger UI for testing endpoints directly.