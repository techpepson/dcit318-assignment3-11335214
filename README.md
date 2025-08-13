# DCIT 318 - Programming 2 - Assignment 3

## Student Information
- **Name**: Dickson Daniel Peprah
- **Student ID**: 11335214
- **Course**: DCIT 318 - Programming 2
- **Assignment**: 3 - Advanced C# Concepts

## Project Overview
This project demonstrates advanced C# programming concepts including:
- Records and immutability
- Interfaces and generic types
- Exception handling
- File I/O operations
- LINQ and collections

## Systems Implemented

### 1. Finance Management System
- **Features**:
  - Transaction processing with different payment methods
  - Account management with balance tracking
  - Transaction history and reporting
- **Key Concepts**:
  - Records for immutable data
  - Interface implementation
  - Sealed classes
  - Virtual/override methods

### 2. Healthcare System
- **Features**:
  - Patient management
  - Prescription tracking
  - Patient-prescription relationship mapping
- **Key Concepts**:
  - Generic Repository pattern
  - LINQ for data querying
  - Collection management

### 3. Warehouse Inventory Management
- **Features**:
  - Product inventory tracking
  - Electronic and grocery item management
  - Stock level management
- **Key Concepts**:
  - Custom exceptions
  - Generic constraints
  - Collection operations

### 4. Student Grading System
- **Features**:
  - Student score processing
  - Grade calculation
  - Report generation
- **Key Concepts**:
  - File I/O operations
  - Data validation
  - Custom exception handling
  - Report generation

### 5. Inventory Management System
- **Features**:
  - Immutable inventory items
  - JSON serialization/deserialization
  - Data persistence
- **Key Concepts**:
  - C# records
  - Generic types with constraints
  - JSON processing
  - File operations

## How to Run
1. Ensure you have .NET 8.0 SDK installed
2. Clone the repository
3. Navigate to the project directory
4. Run `dotnet run`

## Project Structure
- `Program.cs` - Contains all the implementation code
- `inventory.json` - Data file for the inventory system
- `students.txt` - Sample student data file
- `grade_report.txt` - Generated grade report

## Dependencies
- .NET 8.0
- System.Text.Json (included in .NET 8.0)

## Notes
- All systems are implemented in a single file for demonstration purposes
- Sample data is generated at runtime if not found
- Error handling is implemented throughout the application

## Author
Dickson Daniel Peprah - 11335214
