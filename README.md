# Client_Server
In this Computer Networking concept are use, like Server is running and some client want to use this application then GUI appear they fill their form and request send to Server, Server check client detail if match then return data according to their role otherwise not able to perfrom any task. Same the same like Add, Delete, and Edit in which first client send request to server, after response from server any update occues.


In this I Use Sql Server to Store database so code of the database is

Create Database LMS;

CREATE TABLE [dbo].[Student] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [UserName]   VARCHAR (50) NOT NULL,
    [Email]      VARCHAR (50) NOT NULL,
    [Session]    VARCHAR (50) NOT NULL,
    [RollNumber] VARCHAR (50) NOT NULL
);

CREATE TABLE [dbo].[User] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [UserName]    VARCHAR (50) NOT NULL,
    [Email]       VARCHAR (50) NOT NULL,
    [Password]    VARCHAR (50) NOT NULL,
    [PhoneNumber] VARCHAR (50) NOT NULL,
    [Role]        VARCHAR (50) NOT NULL
);
