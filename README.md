<a id="readme-top"></a>

<!-- BADGES -->
[![Build Status](https://img.shields.io/badge/Visual%20Studio-2022-purple?style=for-the-badge&logo=visualstudio)](#)
[![Database](https://img.shields.io/badge/SQL%20Server-MS%20SQL-lightgray?style=for-the-badge&logo=microsoftsqlserver)](#)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=for-the-badge)](#)

<br />
<p align="center">
  <img src="logo.png" alt="Agri-Energy Connect Logo" width="80" />
  <h1 align="center">Agri-Energy Connect Platform</h1>
  <p align="center">
    Bridging Farmers & Green Energy Innovation 🌱⚡
    <br />
    <strong>PROG7311 - Enterprise Application Development</strong>
    <br />
    <strong>Student: Alyssia Sookdeo | ST10266994</strong>
  </p>
</p>

---

## 📚 Table of Contents

- [📋 Project Overview](#-project-overview)
- [🌱 Features](#-features)
- [🔧 Architecture Overview](#-architecture-overview)
- [💻 Development Setup](#-development-setup)
  - [🧰 Prerequisites](#-prerequisites)
  - [⚙️ Installation & Build Steps](#️-installation--build-steps)
  - [🔗 Configure Connection String](#-configure-connection-string)
- [🚀 Running the Application](#-running-the-application)
- [🔐 User Roles & Authentication](#-user-roles--authentication)
- [🗃️ Database Schema](#️-database-schema)
- [🧪 Testing Guide](#-testing-guide)
- [⚠️ Known Issues](#️-known-issues)
- [🔮 Future Enhancements](#-future-enhancements)
- [👩‍💻 Author](#-author)

---

## 📋 Project Overview

**Agri-Energy Connect** is a web-based prototype that creates a collaborative space for farmers and renewable energy providers. The platform allows:

- Farmers to log and manage produce.
- Employees to manage farmer profiles and product inventories.
- Seamless interaction between agriculture and sustainability.

---

## 🌱 Features

### 👨‍🌾 Farmers
- Secure login & menu dropdown
- Add products (name, category, production date)
- View their own product listings

### 👩‍💼 Employees
- Secure login & menu dropdown
- Add new farmer profiles
- View and filter all products 
- Filter by date range, farmers, and product type

---

## 🔧 Architecture Overview

- **Frontend**: ASP.NET MVC (Razor Views)
- **Backend**: C# with Entity Framework Core
- **Database**: Microsoft SQL Server
- **Auth**: ASP.NET Core Identity

---

## 💻 Development Setup

### 🧰 Prerequisites

- ✅ Visual Studio 2022 or newer
- ✅ .NET 6 SDK
- ✅ SQL Server Management Studio 18+
- ✅ Git

---

### ⚙️ Installation & Build Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/st10266994/PROG7311_POE_PART_TWO.git
   cd AgriEnergyConnect
   Restore NuGet Packages

Right-click the solution > Restore NuGet Packages

Build the Solution

Menu: Build > Build Solution or Ctrl+Shift+B

🔗 Configure Connection String
Navigate to appsettings.json

Replace YOUR_SERVER_NAME with your SQL Server instance:

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=AgriEnergyConnectDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
Execute the script at:

pgsql
Copy
Edit
/Database/AgriEnergyConnectDB_Setup.sql
Confirm database is created successfully.

🚀 Running the Application
Set Web App as Startup Project

Right-click AgriEnergyConnect.Web > Set as Startup Project

Run the App

Click ▶️ Start or press F5

Opens in default browser

Login Credentials

Role	Username	Password
Farmer	farmer@gmail.com	Password@1
Employee	alyssiasookdeo@gmail.com	Password@1

🔐 User Roles & Authentication
🔑 Authentication: ASP.NET Core Identity

🔒 Roles: Role-based access control

Farmers: Personal product management

Employees: Farmer registration, product viewing & filtering

🗃️ Database Schema
Table	Description
Users	Stores login credentials
Roles	Role definitions: Farmer, Employee
UserRoles	Maps users to their roles
Farmers	Farmer details
Products	Products added by farmers

🧪 Testing Guide
Login Tests
Access login page

Use provided credentials

Redirected to correct dashboard

Farmer Workflow
Login → Add Product → View Listing

Employee Workflow
Login → Add Farmer → View/Filter Products

⚠️ Known Issues
⚠️ Minor responsiveness bugs on small screens

⚠️ DatePicker formatting may differ per browser

⚠️ Filtering may lag on large datasets

🔮 Future Enhancements
📈 API Integration for weather-based farming advice

🔔 Real-time product update notifications

📱 Mobile app version

📊 Advanced analytics dashboard

👩‍💻 Author
<table> <tr> <td align="center"> <a href="https://github.com/ST10266994"> <img src="https://avatars.githubusercontent.com/u/158015110?s=400&v=4" width="100px;" alt="Alyssia Sookdeo"/> <br /><sub><b>Alosa Grace</b></sub> </a> <br/> <a href="mailto:ST10266994@vcconnect.edu.za">ST10266994@vcconnect.edu.za</a> </td> </tr> </table>
<p align="right">(<a href="#readme-top">back to top</a>)</p> ```
