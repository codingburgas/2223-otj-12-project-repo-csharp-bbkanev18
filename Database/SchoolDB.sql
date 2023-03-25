CREATE DATABASE SchoolDB
GO
USE [SchoolDB]

CREATE TABLE Files (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Filename] VARCHAR(255) NOT NULL,
    FileData VARBINARY(MAX) NOT NULL
);

CREATE TABLE Roles (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL,
	DateOfCreated DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Questions (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(1024) NOT NULL,
	Points INT NOT NULL,
	DateOfCreated DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Answers (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL
);

CREATE TABLE Tests (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL,
    TimeLimit INT NOT NULL,
	Deadline DATETIME2 NULL,
    DateOfCreated DATETIME2 NOT NULL DEFAULT GETDATE()
);


CREATE TABLE Courses (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL
);

-- TODO: Ask is okey to make FileId NULL?
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
	FileId VARCHAR(36) NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id),
	FOREIGN KEY (FileId) REFERENCES Files(Id),
);

-- Score is store in percentage
CREATE TABLE UsersTests (
    TestId VARCHAR(36) NOT NULL,
    UserId VARCHAR(36) NOT NULL,
    Score INT NOT NULL,
    PRIMARY KEY (TestId, UserId),
    FOREIGN KEY (TestId) REFERENCES Tests (Id),
    FOREIGN KEY (UserId) REFERENCES Users (Id)
);

CREATE TABLE QuestionsTests (
    QuestionId VARCHAR(36) NOT NULL,
    TestId VARCHAR(36) NOT NULL,
    PRIMARY KEY (QuestionId, TestId),
    FOREIGN KEY (QuestionId) REFERENCES Questions (Id),
    FOREIGN KEY (TestId) REFERENCES Tests (Id)
);

CREATE TABLE QuestionsAnswers (
    QuestionId VARCHAR(36) NOT NULL,
    AnswerId VARCHAR(36) NOT NULL,
    IsCorrect BIT NOT NULL,
    PRIMARY KEY (QuestionId, AnswerId),
    FOREIGN KEY (QuestionId) REFERENCES Questions (Id),
    FOREIGN KEY (AnswerId) REFERENCES Answers (Id)
);

CREATE TABLE UsersCourses (
    CourseId VARCHAR(36) NOT NULL,
    UserId VARCHAR(36) NOT NULL,
    PRIMARY KEY (CourseId, UserId),
    FOREIGN KEY (CourseId) REFERENCES Courses (Id),
    FOREIGN KEY (UserId) REFERENCES Users (Id)
);

CREATE TABLE CoursesSections (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] VARCHAR(255) NOT NULL,
    CourseId VARCHAR(36) NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Courses (Id)
);

CREATE TABLE CoursesSectionsTests (
    CourseSectionId VARCHAR(36) NOT NULL,
    TestId VARCHAR(36) NOT NULL,
    PRIMARY KEY (CourseSectionId, TestId),
    FOREIGN KEY (CourseSectionId) REFERENCES CoursesSections (Id),
    FOREIGN KEY (TestId) REFERENCES Tests (Id)
);

CREATE TABLE CoursesSectionsFiles (
    CourseSectionId VARCHAR(36) NOT NULL,
    FileId VARCHAR(36) NOT NULL,
    PRIMARY KEY (CourseSectionId, FileId),
    FOREIGN KEY (CourseSectionId) REFERENCES CoursesSections (Id),
    FOREIGN KEY (FileId) REFERENCES Files (Id)
);

-- Example how to store a file
INSERT INTO Files ([Filename], FileData)
VALUES ('example.txt', (SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Users\user\Downloads\example.txt', SINGLE_BLOB) AS FileData));


INSERT INTO Roles([Name]) 
VALUES ('Admin');

INSERT INTO Users
	(FirstName, MiddleName, LastName, Age, Email, [Password], RoleId, FileId)
VALUES
	('Admin', 'Admin', 'Admin', 69, 'Admin@abv.bg', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', (SELECT Id FROM Roles), (SELECT Id FROM Files))
-- Password: 5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8 is password in sha256


-- Example how to insert a data into a QuestionsAnswers table
INSERT INTO Questions([Name], [Points])
VALUES ('This is a test?', 10)

INSERT INTO Answers([Name])
VALUES ('Yes')

INSERT INTO Answers([Name])
VALUES ('No')

INSERT INTO QuestionsAnswers ([QuestionId], [AnswerId], IsCorrect)
VALUES ((SELECT Id FROM Questions), (SELECT Id FROM Answers WHERE [Name] = 'Yes'), 1)

INSERT INTO QuestionsAnswers ([QuestionId], [AnswerId], IsCorrect)
VALUES ((SELECT Id FROM Questions), (SELECT Id FROM Answers WHERE [Name] = 'No'), 0)


-- Close all connection and delete db
USE master;
GO
ALTER DATABASE SchoolDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE SchoolDB