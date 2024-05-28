# Alumini Management System (AMS)

This project implements a basic client-server architecture for a Learning Management System (LMS) using SQL Server for database management. The system allows clients to interact with the server to perform operations such as adding, deleting, and editing data, contingent on their roles and authentication.


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

- User Authentication: Ensures that only registered users can access the system.
- Role-Based Authorization: Different roles have different levels of access and permissions.
- CRUD Operations: Users can perform Create, Read, Update, and Delete operations on the student and user database.
- Secure Data Management: User passwords are stored securely.


## Database Schema

The database is implemented in SQL Server and includes two main tables: Student and User.

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

