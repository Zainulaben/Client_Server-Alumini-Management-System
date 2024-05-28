# Alumini Management System (AMS)

This project implements a basic client-server architecture for a Alumini Management System (AMS) using SQL Server for database management. The system allows clients to interact with the server to perform operations such as adding, deleting, and editing data, contingent on their roles and authentication.


## Table of Contents

- Overview
- Features
- Database Schema
- Setup Instructions
- Usage
- Contributing

## Overview

The AMS system is designed to manage student information and user roles effectively. The server handles requests from clients, checks their credentials, and responds according to their roles. The following operations are supported:

- Authentication
- Authorization based on roles
- CRUD (Create, Read, Update, Delete) operations on student data

## Features

- **User Authentication:** Ensures that only registered users can access the system.
- **Role-Based Authorization:** Different roles have different levels of access and permissions.
- **CRUD Operations:** Users can perform Create, Read, Update, and Delete operations on the student and user database.
- **Secure Data Management:** User passwords are stored securely.


## Database Schema

The database is implemented in SQL Server and includes two main tables: **Student** and **User**.

### Student Table
```bash
  CREATE TABLE [dbo].[Student] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [UserName]   VARCHAR (50) NOT NULL,
    [Email]      VARCHAR (50) NOT NULL,
    [Session]    VARCHAR (50) NOT NULL,
    [RollNumber] VARCHAR (50) NOT NULL,
    PRIMARY KEY (Id)
);
```
### User Table
```bash
CREATE TABLE [dbo].[User] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [UserName]    VARCHAR (50) NOT NULL,
    [Email]       VARCHAR (50) NOT NULL,
    [Password]    VARCHAR (50) NOT NULL,
    [PhoneNumber] VARCHAR (50) NOT NULL,
    [Role]        VARCHAR (50) NOT NULL,
    PRIMARY KEY (Id)
);
```


## Setup Instructions

To set up the LMS on your local machine, follow these steps:

- **Install SQL Server**: Ensure SQL Server is installed and running on your machine.

- **Create Database**: Run the following SQL commands to create the database and tables:
   ```bash
   CREATE DATABASE LMS;
      
   USE LMS;
   
   CREATE TABLE [dbo].[Student] (
      [Id]  INT      IDENTITY (1, 1) NOT NULL,
      [UserName]     VARCHAR (50) NOT NULL,
      [Email]        VARCHAR (50) NOT NULL,
      [Session]      VARCHAR (50) NOT NULL,
      [RollNumber]   VARCHAR (50) NOT NULL,
      PRIMARY KEY (Id)
    );
    
    CREATE TABLE [dbo].[User] (
      [Id] INT   IDENTITY (1, 1) NOT NULL,
      [UserName]    VARCHAR (50) NOT NULL,
      [Email]       VARCHAR (50) NOT NULL,
      [Password]    VARCHAR (50) NOT NULL,
      [PhoneNumber] VARCHAR (50) NOT NULL,
      [Role]        VARCHAR (50) NOT NULL,
      PRIMARY KEY (Id)
   );
   ```
- **Server Setup**: Configure the server application to connect to the SQL Server instance and handle client requests.

- **Client Setup**: Develop or configure client applications to interact with the server. This could be a web-based application, desktop application, or mobile app.


## Usage
- **Launch the Server:** Start the server application to listen for client requests. Server response every requests of client.

- **Client Interaction:** Clients can log in using their credentials. Depending on their role, they can perform various operations such as adding a new student, editing existing student information, or deleting a student record, adding a new user, editing existing user information, or deleting a user record.

## Example Operations
- **Login Request:**
   - Client sends a login request with email and password.
   - Server verifies credentials and responds with an authentication token or error message.

- **Add Student:**
   - Client sends a request with student details.
   - Server checks permissions and adds the student to the database if authorized.

- **Delete Student:**
   - Client sends a request with student details.
   - Server checks permissions and deletes the student to the database if authorized.

- **Edit Student:**
   - Client sends a request with student details.
   - Server checks permissions and edits the student to the database if authorized.

- **Add User:**
   - Client sends a request with user details.
   - Server checks permissions and adds the user to the database if authorized.

- **Delete User:**
   - Client sends a request with user details.
   - Server checks permissions and deletes the user to the database if authorized.

- **Edit User:**
   - Client sends a request with user details.
   - Server checks permissions and edits the user to the database if authorized.

## Screenshots

![Dashboard](https://github.com/Zainulaben/Client_Server-Alumini-Management-System/blob/master/OutputImages/AddStudent.png)
