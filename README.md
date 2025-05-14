# Agri-Energy Connect Platform

A web application that connects farmers and energy markets, allowing farmers to manage their products and employees to manage farmer profiles.

## Project Overview

The Agri-Energy Connect Platform is designed to bridge the gap between agricultural producers and energy markets. The platform enables farmers to showcase their products and connect with potential buyers in the energy sector.

### Key Features

1. **User Role Management**
   - Farmer role: Can add and manage their own products
   - Employee role: Can add farmer profiles and view/filter all products

2. **Farmer Management**
   - Add new farmer profiles with essential details
   - View and edit farmer information
   - Track farmer registration dates

3. **Product Management**
   - Add new products with details (name, category, production date, etc.)
   - Filter products by various criteria (date range, category, etc.)
   - View product details and history

4. **Dashboard**
   - Role-specific dashboards showing relevant information
   - Quick access to recent activities and statistics

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 8.0 SDK
- SQL Server (LocalDB is sufficient for development)

### Setup Instructions

1. **Clone the Repository**
   ```
   git clone <repository-url>
   cd ST10122161_Agri_Energy_Connect_Platform
   ```

2. **Open the Solution**
   - Open `ST10122161_Agri_Energy_Connect_Platform.sln` in Visual Studio

3. **Update the Database**
   - Open the Package Manager Console in Visual Studio
   - Run the following commands:
     ```
     Update-Database
     ```
   - This will create the database and apply all migrations

4. **Run the Application**
   - Press F5 or click the "Start" button in Visual Studio
   - The application will launch in your default browser

### Sample Login Credentials

The application is seeded with the following sample users:

1. **Employee Account**
   - Email: employee@agrienergyconnect.com
   - Password: Employee1!

2. **Farmer Accounts**
   - Email: farmer1@agrienergyconnect.com
   - Password: Farmer1!
   
   - Email: farmer2@agrienergyconnect.com
   - Password: Farmer2!

## System Architecture

### Database Schema

The application uses Entity Framework Core with the following main entities:

1. **ApplicationUser** (extends IdentityUser)
   - Custom user properties including FirstName, LastName
   - Relationship to Farmer entity

2. **Farmer**
   - Farm details (name, address, contact information)
   - Registration date
   - Collection of products

3. **Product**
   - Product details (name, category, production date)
   - Quantity and unit information
   - Relationship to Farmer entity

### Authentication and Authorization

- ASP.NET Core Identity for user authentication
- Role-based authorization (Farmer and Employee roles)
- Secure password policies

### User Interface

- Responsive design using Bootstrap 5
- Role-specific navigation and views
- Filter and search capabilities

## Development Notes

### Project Structure

- **Controllers**: Handle HTTP requests and responses
- **Models**: Define the data structure
- **Views**: Render the user interface
- **Data**: Contains database context and migrations
- **ViewModels**: Specialized models for view-specific data

### Data Validation

- Server-side validation using Data Annotations
- Client-side validation using jQuery Validation

### Error Handling

- Custom error pages
- Proper exception handling throughout the application

## Future Enhancements

1. **Advanced Reporting**
   - Generate reports on product trends
   - Export data to various formats

2. **Marketplace Features**
   - Enable direct communication between farmers and energy buyers
   - Implement a bidding system

3. **Mobile Application**
   - Develop a companion mobile app for on-the-go access

4. **Integration with External Systems**
   - Weather data for production planning
   - Market price information

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- ASP.NET Core team for the excellent web framework
- Bootstrap team for the responsive UI components
- All contributors to the project
