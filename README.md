# Agri-Energy Connect Platform



## 📋 Project Overview

This prototype web application bridges the gap between the agricultural sector and green energy technology providers, creating a digital ecosystem where farmers, green energy experts, and enthusiasts can collaborate, share resources, and innovate in sustainable agriculture and renewable energy.

### Repository Information
- **Project Owner:** Alyssia Sookdeo
- **Student Number:** ST10266994
- **Module:** PROG7311
- **Year:** 2025

---

## 🌱 Features

### For Farmers
- Secure login and profile management
- Add and manage agricultural products with details (name, category, production date)
- View personal product listings

### For Employees
- Secure login with authenticated access
- Add new farmer profiles to the system
- View and filter all products from specific farmers
- Search products using criteria (date range, product type)

---

## 🔧 Technical Architecture

This application implements a multi-tier architecture:

- **Presentation Layer**: ASP.NET MVC with responsive design
- **Business Logic Layer**: C# service classes with business rules
- **Data Access Layer**: Entity Framework Core with SQL Server
- **Database**: Microsoft SQL Server


---

## 💻 Development Environment Setup

### Prerequisites

- Visual Studio 2022 (or later)
- SQL Server Management Studio 18.0 (or later)
- .NET 6.0 SDK (or later)
- Windows 10/11 operating system

### Installation Steps

1. **Clone the repository**
   ```
   git clone https://github.com/st10266994/PROG7311_POE_PART_TWO.git
   cd AgriEnergyConnect
   ```

2. **Database Setup**
   - Open SQL Server Management Studio
   - Connect to your local SQL Server instance
   - Execute the database creation script:
     ```
     File location: /Database/AgriEnergyConnectDB_Setup.sql
     ```
   - Verify the database was created successfully

3. **Configure Database Connection**
   - Open the solution in Visual Studio
   - Locate the `appsettings.json` file in the project root
   - Update the connection string to match your SQL Server configuration:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=AgriEnergyConnectDB;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
     ```

4. **Restore NuGet Packages**
   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"
   - Wait for the process to complete

5. **Build the Solution**
   - Press `Ctrl+Shift+B` or select "Build > Build Solution" from the menu
   - Ensure there are no build errors

---

## 🚀 Running the Application

1. **Set the Web Project as Startup Project**
   - Right-click on "AgriEnergyConnect.Web" in Solution Explorer
   - Select "Set as Startup Project"

2. **Run the Application**
   - Press `F5` or click the "Start" button in Visual Studio
   - The application will launch in your default browser

3. **Login Credentials**
   
   **Farmer Account:**
   - Username: `farmer@gmail.com`
   - Password: `Password@1`
   
   **Employee Account:**
   - Username: `alyssiasookdeo@gmail.com`
   - Password: `Password@1`

---

## 🔑 System Authentication

The system uses ASP.NET Core Identity for authentication and authorization:

1. **Authentication Flow**
   - Users navigate to the login page
   - Credentials are validated against the database
   - Successful login redirects to role-specific dashboard

2. **Role-Based Access Control**
   - Farmers have access to their product management features
   - Employees have access to farmer management and product viewing features

---

## 📊 Database Schema

The database consists of the following primary tables:

- **Users** - Stores authentication details
- **Roles** - Defines user roles (Farmer, Employee)
- **UserRoles** - Maps users to their roles
- **Farmers** - Stores farmer profile information
- **Products** - Contains product details with farmer associations


---

## 🧪 Testing the Application

1. **Login Testing**
   - Navigate to the login page
   - Enter the provided credentials for either a farmer or employee
   - Verify you are redirected to the appropriate dashboard

2. **Farmer Features Testing**
   - Login as a farmer
   - Navigate to "My Products"
   - Try adding a new product with all required fields
   - Verify the product appears in your listing

3. **Employee Features Testing**
   - Login as an employee
   - Navigate to "Manage Farmers"
   - Try adding a new farmer profile
   - Navigate to "View Products" to see products by farmer
   - Test the filtering functionality by date range and product type

---

## 📝 Known Issues

- Filter functionality may be slow with large datasets
- Some responsive design elements need improvement on very small screens
- Date picker may show inconsistent formatting in some browsers

---

## 🔄 Future Enhancements

- Integration with weather data API for farming insights
- Implementation of notification system for product updates
- Enhanced reporting capabilities for employees
- Mobile application version

---



