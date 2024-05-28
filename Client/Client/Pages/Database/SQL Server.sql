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