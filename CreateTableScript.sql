USE [master]
GO

CREATE DATABASE [EmailLoggerExample]
GO

USE [EmailLoggerExample]
GO

CREATE TABLE [EmailLog] (
  [id] int identity(1, 1) not null primary key,
  [hostemail] varchar(255) null,
  [recipientemail] varchar(255) null,
  [emailsubject] varchar(255) null,
  [emailbody] varchar(2047) not null,
  [timesent] varchar(255)
)

GO