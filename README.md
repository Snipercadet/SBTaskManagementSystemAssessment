# Task Management System API

## Introduction

This is the README for the Task Management System API Assessment. This API was built using .NET 6 and uses PostgreSQL for the database. Additionally, a background service project is included to handle notifications for due dates, task completions, and new task assignments.

## Prerequisites

Before you can set up and run this API, make sure you have the following installed on your development machine:

- .NET 6 SDK
- PostgreSQL (with pgAdmin 4 for database management)
- Visual Studio or Visual Studio Code (for running the API and background service)

## Setup

Follow these steps to set up and run the Task Management System API:

1. **No extra setup needed**:

   Just build the project to install all dependencies.

2. **Run Migrations**:

   - Open a package manager console in the API project directory.
   - Make sure the default project is "SBTaskManagement.Infrastructure."

   - Run the following command to apply the database migrations:
   
     ```
     add-migration nameOfMigration
     ```

     (Replace "nameOfMigration" with the name you want to call the migration. This command will add a new migration.)

   - Run the following command to update the database:

     ```
     update-database
     ```

3. **Background Service**:

   - Open a new instance of Visual Studio or Visual Studio Code.
   - Open the background service project.
   - Run the background service project. This will handle notifications.

4. **Run the API**:

   - Open another instance of Visual Studio or Visual Studio Code.
   - Open the API project.
   - Run the API project.

## Usage

Once the API is up and running, you can interact with it using HTTP requests. Here are some example endpoints:

- `GET /api/tasks`: Get a list of tasks.
- `POST /api/tasks`: Create a new task.

Please refer to the API swagger documentation for a complete list of endpoints and their usage.

## Background Service

The background service project handles notifications for due date reminders, task completions, and new task assignments. Make sure it's running alongside the API for these notifications to work.

## Design Decisions and Assumptions

Here are some design decisions and assumptions made during the development of this API:

- .NET 6 was chosen as the development framework for its modern features and performance improvements.
- PostgreSQL was selected as the database system for its robustness and support for complex data structures.
- A background service was implemented to handle notifications, improving the user experience.

## Conclusion

You should now have the Task Management System API up and running on your machine. Feel free to explore the API endpoints and. If you encounter any issues or have questions, please refer to the documentation or reach out to me for assistance and further clarifications.

Happy coding!
