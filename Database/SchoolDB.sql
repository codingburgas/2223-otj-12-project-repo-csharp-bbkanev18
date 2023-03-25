CREATE DATABASE SchoolDB
GO
USE [SchoolDB]

CREATE TABLE Roles (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL
);

CREATE TABLE Questions (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(1024) NOT NULL
);

CREATE TABLE Answers (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL
);

CREATE TABLE QuestionsAnswers (
    QuestionId VARCHAR(36) NOT NULL,
    AnswerId VARCHAR(36) NOT NULL,
    IsCorrect BIT NOT NULL,
    PRIMARY KEY (QuestionId, AnswerId),
    FOREIGN KEY (QuestionId) REFERENCES Questions (Id),
    FOREIGN KEY (AnswerId) REFERENCES Answers (Id)
);

-- Example how to insert a data into a QuestionsAnswers table
/*
INSERT INTO Questions([Name])
VALUES ('This is a test?')

INSERT INTO Answers([Name])
VALUES ('Yes')

INSERT INTO Answers([Name])
VALUES ('No')

INSERT INTO QuestionsAnswers ([QuestionId], [AnswerId], IsCorrect)
VALUES ((SELECT Id FROM Questions), (SELECT Id FROM Answers WHERE [Name] = 'Yes'), 1)

INSERT INTO QuestionsAnswers ([QuestionId], [AnswerId], IsCorrect)
VALUES ((SELECT Id FROM Questions), (SELECT Id FROM Answers WHERE [Name] = 'No'), 0)
*/

CREATE TABLE Courses (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL
);

CREATE TABLE Users (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    FirstName VARCHAR(255) NOT NULL,
    MiddleName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Age TINYINT NOT NULL,
    Email VARCHAR(255) NOT NULL,
    [Password] CHAR(64) NOT NULL,
    Phone VARCHAR(20) NULL,
    [Address] VARCHAR(255) NULL,
    DateOfCreation DATETIME2 DEFAULT GETDATE() NOT NULL,
    RoleId VARCHAR(36) NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

CREATE TABLE UsersCourses (
    CourseId VARCHAR(36) NOT NULL,
    UserId VARCHAR(36) NOT NULL,
    PRIMARY KEY (CourseId, UserId),
    FOREIGN KEY (CourseId) REFERENCES Courses (Id),
    FOREIGN KEY (UserId) REFERENCES Users (Id)
);

INSERT INTO Roles([Name]) 
VALUES ('Admin');

INSERT INTO Users
	(FirstName, MiddleName, LastName, Age, Email, [Password], RoleId)
VALUES
	('Admin', 'Admin', 'Admin', 69, 'Admin@abv.bg', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', (SELECT Id FROM Roles))
-- Password: 5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8 is password in sha256


-- Close all connection
USE master;
GO
ALTER DATABASE SchoolDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE SchoolDB