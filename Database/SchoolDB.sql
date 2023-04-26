CREATE DATABASE SchoolDB
COLLATE Cyrillic_General_100_CI_AS_SC_UTF8;
GO
USE [SchoolDB]

CREATE TABLE Files (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Filename] NVARCHAR(255) NOT NULL,
    FileData VARBINARY(MAX) NOT NULL
);

CREATE TABLE Roles (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(255) NOT NULL,
	DateOfCreated DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Questions (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(1024) NOT NULL,
	Points INT NOT NULL,
	DateOfCreated DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Answers (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(255) NOT NULL
);

CREATE TABLE Tests (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(255) NOT NULL,
    TimeLimit INT NOT NULL,
	Deadline DATETIME2 NULL,
    DateOfCreated DATETIME2 NOT NULL DEFAULT GETDATE()
);


CREATE TABLE Courses (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(255) NOT NULL
);

-- TODO: Ask is okey to make FileId NULL?
CREATE TABLE Users (
    Id VARCHAR(36) PRIMARY KEY DEFAULT NEWID(),
    FirstName NVARCHAR(255) NOT NULL,
    MiddleName NVARCHAR(255) NOT NULL,
    LastName NVARCHAR(255) NOT NULL,
    Age TINYINT NOT NULL,
    Email VARCHAR(255) NOT NULL,
    [Password] CHAR(64) NOT NULL,
    Phone VARCHAR(20) NULL,
    [Address] NVARCHAR(255) NULL,
    DateOfCreation DATETIME2 DEFAULT GETDATE() NOT NULL,
    RoleId VARCHAR(36) NOT NULL,
	FileId VARCHAR(36) NULL,
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
/*
INSERT INTO Files ([Filename], FileData)
VALUES ('example.txt', (SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Users\user\Downloads\example.txt', SINGLE_BLOB) AS FileData));
*/

-- Add default roles which is guest, user, teacher, admin
INSERT INTO Roles([Name]) 
VALUES ('guest');

INSERT INTO Roles([Name]) 
VALUES ('user');

INSERT INTO Roles([Name]) 
VALUES ('teacher');

INSERT INTO Roles([Name]) 
VALUES ('admin');

INSERT INTO Users
	(FirstName, MiddleName, LastName, Age, Email, [Password], RoleId)
VALUES
	('Admin', 'Admin', 'Admin', 69, 'Admin@abv.bg', 'ec9c81957e5bbfb455d9bd41091c56399291bfdcbd00b1ec4b9e8e1a09c841e2', (SELECT Id FROM Roles WHERE [Name] = 'admin'))
-- Password: ec9c81957e5bbfb455d9bd41091c56399291bfdcbd00b1ec4b9e8e1a09c841e2 is 'Test!1234' in sha256


-- Add 100 users to database
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Krissie', 'Poaceae', 'Ronca', 56, 'kronca0@comsenz.com', '3b4d852894252dbe0c28c405dc6b1b1d3def2e91f5754257a051369e95c889cf', '9 Ilene Parkway', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Alon', 'Asteraceae', 'Slyvester', 44, 'aslyvester1@google.co.uk', 'ef797a8d530337aa173dd7281e1c59130e941fb67bcb84df0fc8f0f563e40fdc', '701 Lien Trail', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Pamelina', 'Seligeriaceae', 'Trudgeon', 18, 'ptrudgeon2@ucsd.edu', '99656292696b20278f9103e8bd275be9dc9b303a0f2fe1ce0c9655237a39af3a', '9492 Springview Parkway', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Kerrill', 'Stereocaulaceae', 'Swindall', 56, 'kswindall3@slideshare.net', 'c15aa127742ba1b3cb0aebebce4af79bfc36d69cc5e439477a326b0f696cfd14', '232 Dapin Lane', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Janeva', 'Polygonaceae', 'Corhard', 43, 'jcorhard4@ocn.ne.jp', 'e80e4c3c4fe161abed870913596488176d5fb6950884945f24b133e945e5a872', '2 Messerschmidt Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Melinda', 'Loganiaceae', 'Langtree', 23, 'mlangtree5@topsy.com', '60fe4f55b2919d568f88b39118b1a9231950c13a32541d10ed6c3c9e856a1a24', '7907 Atwood Parkway', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Jasmin', 'Onagraceae', 'Castanone', 58, 'jcastanone6@eventbrite.com', '8f5280ccd67a59a3cdef38b31f3061a62f7740894647dc20264da011f75ee70f', '53 Clyde Gallagher Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Alonso', 'Ophioglossaceae', 'Baldrick', 46, 'abaldrick7@artisteer.com', '3e8b0c590db064a177f38abef7f0d7533724a56dc01f2937cbc4ab6ea1070e30', '0 Bay Point', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Yoko', 'Poaceae', 'Soden', 40, 'ysoden8@cmu.edu', 'c3d86e3a008ccd24d07b7c7943479bd32a0319d4d0cb5fbc6e2dca4ef7dd1fe1', '6323 Badeau Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Robinetta', 'Crassulaceae', 'Sydney', 24, 'rsydney9@mail.ru', '196daa1b02b1c7169575dbc7fef40d0e839ba10c8ebcc7ac90abf0194b8ef9c0', '2 Old Gate Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Brittani', 'Campanulaceae', 'Whettleton', 47, 'bwhettletona@blinklist.com', '7831e767a88f491519fe65a85800604fa26cb864471ecb319bf447e315666156', '8 Memorial Center', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Cherilyn', 'Cyperaceae', 'Balogh', 44, 'cbaloghb@tumblr.com', '9312156eb4b503a6f20c07a8d90dca605aab99a7b22619d6e85b4f6260d41f4d', '3 Oak Valley Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Waldo', 'Arthopyreniaceae', 'Pickett', 19, 'wpickettc@salon.com', '12b49788190c012b84390997aee49a475987116dbc8e9b4a425905e3c5dec558', '84 Meadow Vale Drive', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Agustin', 'Brassicaceae', 'Worling', 28, 'aworlingd@devhub.com', '2d1b41d269b69abbc158e4c65bd0910557f76ae5ed7610de04ff268b46813048', '29709 Buhler Park', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Beaufort', 'Saxifragaceae', 'Barthelme', 59, 'bbarthelmee@acquirethisname.com', '520a3ab2d38b149e2c89ceaf05125c21857600b9d7cadf5c4cddb7f72600d29a', '965 Trailsway Court', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Burgess', 'Myrsinaceae', 'Dayer', 38, 'bdayerf@taobao.com', 'b9e422add878d2c384f24258e1c2136e8ae41e8e1ee4eebf9a4556ae00475ae5', '8760 Packers Pass', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Hermina', 'Brassicaceae', 'Fletcher', 33, 'hfletcherg@ask.com', 'd6681a53dc3ef65d1aacc30b0d1f6e2cf3aac959df057b8d265c009c8109b669', '768 Corben Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Kaile', 'Empetraceae', 'Iannello', 58, 'kiannelloh@slashdot.org', '3632583da282754b2eb644b44758d4be9834d2f31a7df233b2f2f65776e4f82c', '832 Brown Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Ernst', 'Scrophulariaceae', 'Huntress', 51, 'ehuntressi@nba.com', '3bdb26908d26fa15ab39e3cfbe5303266bc4017d2e666da3f524f18116569e77', '4641 Bayside Way', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Ronnie', 'Asteraceae', 'Connechie', 56, 'rconnechiej@ustream.tv', '226689e77147ad391077cdffcc1333af1bf4c0c6a376e59797aab858595f5cff', '861 Westridge Hill', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Kippie', 'Anomodontaceae', 'O''Dea', 54, 'kodeak@uiuc.edu', '0b1160a8aad6321fda1f0142ca54c3537f0129b26061d28abb196efebb3b0d7c', '13 Melvin Park', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Sidney', 'Asteraceae', 'Sebborn', 27, 'ssebbornl@huffingtonpost.com', 'd5c5c8145c8b6b85c402c5a36bacef5809c91ce0ef8417c4818ed5fa87ff74fe', '82 Eliot Drive', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Kesley', 'Saxifragaceae', 'Stackbridge', 50, 'kstackbridgem@cdc.gov', '60a99e0a3a130c07eca70ad4ee4ae5917f0f0db1dce35c48adca75dfcbe80fc2', '89 Blackbird Parkway', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Kimbell', 'Polygonaceae', 'Stidworthy', 43, 'kstidworthyn@mozilla.org', 'ec28c3a96b0063f9f5c94b6ea4440cf259c13c08d7b8d510e3728916112c6454', '3080 Parkside Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Rriocard', 'Apiaceae', 'Doxey', 48, 'rdoxeyo@163.com', 'c3fa5d4afa1d0d9ac45044461bc552c0e828a478f60b36271eb2f76795de898a', '97825 Eagan Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Netti', 'Polygonaceae', 'Steven', 52, 'nstevenp@chronoengine.com', '51c63267c3ddea7498631a9351d126fd17abed5a7776d9076b256b2d510d5866', '2 Calypso Lane', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Esme', 'Dryopteridaceae', 'Kubik', 30, 'ekubikq@bluehost.com', 'c4252899f302c8c37f518e649592a82b448bceea92f4503e95e9a06929224ce7', '27172 Vahlen Lane', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Gaby', 'Lamiaceae', 'Mandrey', 37, 'gmandreyr@phpbb.com', '5dca0a30323d8bef216d83c4c80e73f7336648595aea94e67626a27e2c97d5be', '9443 Bayside Park', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Kerri', 'Cyperaceae', 'Eisig', 35, 'keisigs@bbb.org', '903846ed036a8e63511251d855881fcfe1f1110d31a634aaf06c58a6667c104e', '3883 Fulton Crossing', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Nicky', 'Parmeliaceae', 'Hamstead', 31, 'nhamsteadt@parallels.com', '7e50ac8ecf0e9b95e7dc53c8be0d3c771f298ebef960046d5d5aa295ae4367ef', '1 Waywood Parkway', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Merl', 'Combretaceae', 'Karmel', 55, 'mkarmelu@lulu.com', '7a3350bb98b8c8963745162b7c698544f4e91dc26ac673ff78b68d38c68129e8', '6129 Ronald Regan Park', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Kylynn', 'Asteraceae', 'Greensall', 31, 'kgreensallv@ezinearticles.com', 'aa696a98d1c11026928a2421f15b9a7a61bf0c9e64b1b1777a754b2039acf187', '3 Pearson Trail', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Minerva', 'Ericaceae', 'Arnaudon', 26, 'marnaudonw@stanford.edu', '119a8c30ba72b205460cde266dfca0f05f98e91fce1e58f1e89bbdf6d44490b2', '720 Del Sol Center', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Payton', 'Violaceae', 'Carles', 48, 'pcarlesx@huffingtonpost.com', 'ab1e4fa108a5be959f9f43d580bbc5d967c6a4c15a4697bc51a3cc6b1163c121', '798 Fisk Lane', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Wilone', 'Cistaceae', 'Ivers', 21, 'wiversy@mlb.com', '005295a8f3e986290c80eb56e697e3ad4327ca262ab8d4269055e368b5fff7ae', '67534 Sugar Parkway', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Wilek', 'Verrucariaceae', 'Blaine', 25, 'wblainez@aboutads.info', 'd86779362932e29bd8139624af32a71aff218f4973b683512c31a7543e4ccadc', '984 Blue Bill Park Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Donetta', 'Aponogetonaceae', 'Crewe', 26, 'dcrewe10@geocities.jp', 'a5f6d91f0f8e35baa7c0e9583b530ae904acd81d8d8dbc5afdd96d91fcbc0373', '4 Paget Place', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Merralee', 'Hedwigiaceae', 'Jeggo', 35, 'mjeggo11@webnode.com', 'ab509fd8c0bc7d091c8bce495b21cc1a3316b8de56490773067ae76a09abb534', '92 Hovde Drive', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Cornall', 'Poaceae', 'Dinnis', 57, 'cdinnis12@ibm.com', '4ad58f1c8f5b9ed20086f9d99550ad4be580aad2dea29d93341b3e722f6c0076', '4 Helena Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Gabey', 'Solanaceae', 'Zapater', 38, 'gzapater13@skype.com', 'e9c76c2bd884f77d5a513a74ea5f1bd6ba379c344753ebd9b33ade307f19fe69', '5 Armistice Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Taddeo', 'Thelypteridaceae', 'Alejandri', 18, 'talejandri14@usa.gov', 'edb74355c89aa5f43c116f5e5ca80f9df126d14b8b8b0d40eb918db4a383f824', '6140 La Follette Pass', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Adel', 'Fabaceae', 'MacRirie', 31, 'amacririe15@reference.com', 'fcd13234fe3be96cdf69baec1aa9c72631d2e30c4dfff6a9fbecc53da7209859', '8 Troy Point', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Edna', 'Lycopodiaceae', 'Stepto', 38, 'estepto16@ibm.com', 'd6743397ccfb05bb1c718ead6ddd1cb1802050ca1bb389c5d0c15cdcaf5105aa', '11875 Sundown Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Boris', 'Aizoaceae', 'Tyers', 56, 'btyers17@thetimes.co.uk', '9424343a1e4a69a3460a615945bc99acea9aa6893240247df5c35fce2e6a30df', '0066 Moulton Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Karole', 'Campanulaceae', 'Brownell', 22, 'kbrownell18@photobucket.com', 'fe19e69822ae97ccd0bb749aa0478f0d00810d50b84ede1e7702f0c3b6f7dadd', '4 Doe Crossing Junction', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Laverne', 'Lamiaceae', 'Tench', 56, 'ltench19@wufoo.com', 'd6ab6cfc4690de5bd2f95123ff6634e625189a764233872d3968f0451b0daac7', '7 Bluestem Point', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Arlinda', 'Fabaceae', 'Rottcher', 28, 'arottcher1a@mayoclinic.com', '5520e4e83fe72e9beadd0db81820c6bf9efefd85c4748384ce3fb116a38af45c', '5 Macpherson Way', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Ashlan', 'Cupressaceae', 'Fries', 16, 'afries1b@shutterfly.com', '4255e7d36c4ac33456020e980dc11d02d908fb5501031d3ba0b11684ebdefc97', '74 Stone Corner Drive', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Marion', 'Haloragaceae', 'Di Carli', 58, 'mdicarli1c@soup.io', '35dd3225c604faa18d1f39c46664c62dc0255e156aa63c97a5f486ca8443b6f1', '674 Dorton Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Bridgette', 'Cucurbitaceae', 'Soutter', 45, 'bsoutter1d@myspace.com', 'ff970ce697fbd869c5abf7c32449ae9017c5d35468a14bd402dc32a061fa1297', '0 Blaine Park', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Monte', 'Campanulaceae', 'Brugsma', 56, 'mbrugsma1e@webmd.com', 'd41facfc960af917bd951bb06246c5836a4bca6fa4236ad2aa149b440baf2823', '0185 Barby Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Pia', 'Lamiaceae', 'Cussons', 24, 'pcussons1f@zdnet.com', '7e81b167782fc9501970535a4cb87b397a13e790a7ad948acdaa3ce9852edf00', '548 Tony Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Bettine', 'Fabaceae', 'Pounsett', 43, 'bpounsett1g@51.la', 'fd377287589b857b13669e031c4ee16956457793fc346e7c709f3fe9c58b5679', '7 Bonner Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Amery', 'Portulacaceae', 'Bugbee', 55, 'abugbee1h@deviantart.com', 'f4f0304d9353d63822fa53540c76736ac2f0e3b0cbdabca11decfcb070d40325', '7840 Blaine Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Terrell', 'Portulacaceae', 'Lathee', 51, 'tlathee1i@squarespace.com', '09772a94c64bbe4ef8fa3b3199ff55be27c57b62792730ded5d1b63430c45b4a', '5 Russell Place', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Briano', 'Stereocaulaceae', 'Vowells', 42, 'bvowells1j@joomla.org', '0172157089d25fb3f480ed5d7c4c9e2ee94571b4d0967f2ac043f9b0db72c041', '5726 Green Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Edwin', 'Caryophyllaceae', 'Risley', 32, 'erisley1k@ovh.net', '1b3babe491a8d0f2de00a1646c1c7d369a5c512235ad1c3f95524e1843dc4aff', '6711 Harbort Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Lula', 'Asteraceae', 'Hanks', 21, 'lhanks1l@mayoclinic.com', 'b953fac791bcc20bf0a1096a726b3e80f1261e8ac77d5419b72513d646d0c2c9', '43 Carberry Drive', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Izaak', 'Lamiaceae', 'Ibert', 40, 'iibert1m@shareasale.com', '96e0be12660b67e22ad9d3456007f81cac1746a9fa980b654e44d0bf9b6bc526', '4246 Tomscot Point', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Johnnie', 'Asteraceae', 'O''Brian', 22, 'jobrian1n@boston.com', 'f120ea548190561368c2794ea9663488ded72862095e7c2db838ecb0561155ae', '47240 Ryan Trail', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Nevins', 'Poaceae', 'Blumson', 41, 'nblumson1o@cargocollective.com', '0112c6664a0f1becd286d087aa08bf602152862a6d92803f57a7e61a2d577295', '942 Meadow Vale Point', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Rodd', 'Arecaceae', 'Pavett', 57, 'rpavett1p@ehow.com', '4617d318a6d619b4af090d4df2eaab87acec4ce176c50bbfa69fcf5d4a72e422', '7 Linden Pass', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Nanci', 'Lamiaceae', 'Glanvill', 26, 'nglanvill1q@live.com', '8f7363922e3911a8721aa53f4dd95478bb9fc3bf3ae9287289d0a31dc65cfd52', '7 Shopko Hill', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Clemens', 'Rosaceae', 'Colleton', 31, 'ccolleton1r@reuters.com', '22f45875517386358cf3d32d9b410f56789637c243fe59f8368f0813bdc0c488', '3323 Namekagon Lane', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Marlowe', 'Ranunculaceae', 'Arthy', 30, 'marthy1s@nbcnews.com', 'ea2f957db2b5fafed8e5f6a771b5250818d02526fc358ec39b109b3cd8669445', '64 Ronald Regan Road', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Aleta', 'Polemoniaceae', 'Skains', 42, 'askains1t@merriam-webster.com', '02d770c351d80a21c66c4c8cfbe70b77192732e190daf1bf74f23b5612f5b014', '9 David Place', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Cammy', 'Campanulaceae', 'Giannazzo', 20, 'cgiannazzo1u@spiegel.de', '9d8f09ae02dfe9677189b5b587fa2466c300ea874bb7ccbb30d8dad8fe0e4405', '4 Blaine Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Arabela', 'Asteraceae', 'Bednell', 24, 'abednell1v@alibaba.com', 'ebedfb0e44930f7adf3a4bd3f733bed57509622214b807202475a5272b3f1d67', '63682 Mifflin Crossing', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Laurel', 'Lecanoraceae', 'Boyett', 22, 'lboyett1w@redcross.org', '6d3ac24c6a13537f9f266bf0ad15092a4d76e8e55eded23309adcb995ac10808', '86 Victoria Lane', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Fidel', 'Cladoniaceae', 'Iacovini', 50, 'fiacovini1x@ovh.net', 'a413a11e61ae226ae5485d6b650c929c5d952de8ade6bebdacaad45c86dddc74', '6136 Badeau Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Letizia', 'Brassicaceae', 'Giorgini', 42, 'lgiorgini1y@tuttocitta.it', '5ce81ee43df3795b3a1caedb426a059cb970812ff7a73ee6f2e7d476417fd035', '8302 School Hill', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Cherice', 'Fabaceae', 'Beauchop', 41, 'cbeauchop1z@blog.com', 'a8333ad22c195fe3bb00e9c4b817195975ad413efc002bf4b4566a95d3a9b680', '6 Lyons Hill', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Carleton', 'Hypnaceae', 'Pieterick', 58, 'cpieterick20@last.fm', 'c3b10587229f4ae2ae16a6c78701010624d710391b8aaaae1cebae4c6947d2a4', '2756 Monument Lane', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Cahra', 'Asteraceae', 'Bilam', 37, 'cbilam21@mysql.com', 'a101e4acf59ca300e85b602856ad307937891cb6a0e98d71fc03969d9f408928', '4787 Service Center', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Tatiania', 'Geraniaceae', 'Davidovici', 48, 'tdavidovici22@amazon.co.jp', 'b65062fbb820934d873024521c118bdb4fe563b2da65426c208e32af45ac1fdc', '25 Fairview Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Alexander', 'Asteraceae', 'Pruckner', 45, 'apruckner23@squarespace.com', 'af535a3eb5c4253af24fc6744c91e0e0077facb3c7c59975bcbe71fccb14bb0c', '1393 Pennsylvania Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Hernando', 'Polemoniaceae', 'Bayman', 56, 'hbayman24@about.com', '578c58814bfc5046bb70b3bde79a5c869a3562bebdb1422d1a943e575ed0aa06', '2510 Carpenter Center', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Yoshi', 'Crassulaceae', 'Kaas', 38, 'ykaas25@de.vu', '050c7d7dcab1c79a9fa8cffa7f66a38f4c39bbd4faba876233eea39e008534fd', '33 Rusk Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Faustine', 'Liliaceae', 'Grannell', 54, 'fgrannell26@macromedia.com', 'af17db8bd2d7382a4ee3128b654790c4aa63600ef181dae2f1bda452445fcd73', '31 Mariners Cove Parkway', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Celie', 'Caprifoliaceae', 'Kempson', 26, 'ckempson27@wordpress.com', '548d4a81f38d02bd3e223d7bf51c08d8426ec405ed3ebae6cd0fe2c467ed55da', '9 Crescent Oaks Road', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Inness', 'Anacardiaceae', 'Borit', 36, 'iborit28@google.cn', 'f2324c4fdb71180ebfe4db18fcb3ccf62d5258b5a6c6d899e85ad646a6271006', '401 Logan Park', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Sarena', 'Mniaceae', 'Cheers', 60, 'scheers29@engadget.com', '3b686913ec8a803848e5edaf451cfb39f9bd7b3c47a5e0d0bcacad36e95da7c7', '950 Grasskamp Junction', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Dewain', 'Rosaceae', 'Baile', 51, 'dbaile2a@smugmug.com', 'e9b239aa62426d28526fd07d371e2776f8962a6345a7d92ca5c79c90dd3c0596', '452 Fordem Road', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Livvyy', 'Brassicaceae', 'Bett', 60, 'lbett2b@dedecms.com', 'd19095392fe63e302b330493e8b87dc3c5b0ed1c71dfcfe56b0d7c4af36829e0', '7357 Memorial Drive', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Andreas', 'Cyperaceae', 'Baugham', 41, 'abaugham2c@rakuten.co.jp', 'b753aa4e86124d83a6ed3d01978da71f257e41ad9ae0d03e1da16808b5f727dc', '1 Petterle Junction', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Shena', 'Poaceae', 'Brea', 59, 'sbrea2d@imageshack.us', '7fd15cd103e08f0c4076143b6556761d314c4b7b5aec606ebce99c08dbd4661b', '6959 Vahlen Plaza', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Paige', 'Asteraceae', 'Bumphrey', 52, 'pbumphrey2e@bloglines.com', '0919ae9eb90ef8c0c5c3927b93fcdddad91b283bf2e3c6fac781f794edae3381', '6 Moulton Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Amelie', 'Asteraceae', 'Eschalette', 30, 'aeschalette2f@psu.edu', '0ad6cd23d75c72594f5c958d6dcd0dd80f7b1df25b05a4ffe637b5d7473df85e', '51 Rusk Circle', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Laurent', 'Lamiaceae', 'Edlin', 20, 'ledlin2g@stumbleupon.com', '54eda4882d8647d8f7036770ff323059b5870fad6d35f6b6070403edd944d3f4', '50 Lake View Place', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Innis', 'Physciaceae', 'Keough', 56, 'ikeough2h@cnbc.com', '75a5a543dffd23710d3d3fb65187e6de0f3f4afc9033384085f34e589612c5de', '720 Browning Center', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Angelo', 'Lauraceae', 'Ruske', 58, 'aruske2i@mozilla.com', '76d71662ed91eb6119686d6a2b45b50c2e60db945b20b2e967311dc14b9203d8', '12 Myrtle Terrace', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Lotte', 'Asteraceae', 'Roche', 26, 'lroche2j@yale.edu', '7c6108f57068f1f5dae282a62e51afaf94bb95cf5d4367a07bcc52234bd59179', '8442 Michigan Center', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Ad', 'Orchidaceae', 'Bakesef', 38, 'abakesef2k@engadget.com', '5ca5e447dbc0b3d12ad7591da0bc21bffc192fb2e660bc4881c387d6876bf30a', '2291 Dovetail Crossing', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Romona', 'Onagraceae', 'Spalls', 20, 'rspalls2l@gnu.org', '8725503b7448e53ba5ee578a135fe895b49e4b54b23fff0e486acee5b2647824', '473 Jay Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Christen', 'Liliaceae', 'Fowles', 54, 'cfowles2m@prlog.org', '9c2580a7fdcb69ab5f42e3f69b1f5734792a93f62124ea90c72c0d803d865d6f', '5040 Mockingbird Crossing', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Faustina', 'Rubiaceae', 'Hanway', 20, 'fhanway2n@unicef.org', '081a829cd39f98dd569fc4a41c18e8c192267a845ae351bca9f74c8ee25e7248', '2046 Monica Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Collette', 'Poaceae', 'Broschek', 26, 'cbroschek2o@constantcontact.com', 'd36b106ed115d4f9e555a159ff450f570ad238c394aa8d2e76ee73c3db289ae1', '0 Barby Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Filide', 'Ranunculaceae', 'Drews', 35, 'fdrews2p@miitbeian.gov.cn', 'c07002d5f981291cc33166d28213425337f3d78d4982a63eae28f7112d62b549', '3027 Graedel Court', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Mikol', 'Bignoniaceae', 'Jenkison', 23, 'mjenkison2q@npr.org', 'c97aef444841774cfc0adb45a88a31aff0a95cef4ac6cbfb6c66e1c1e5dbde58', '5752 Bay Street', (SELECT Id FROM Roles WHERE [Name] = 'guest'));
insert into Users (FirstName, MiddleName, LastName, Age, Email, [Password], [Address], RoleId) values ('Xever', 'Cactaceae', 'Grigson', 29, 'xgrigson2r@illinois.edu', '548118fdf069a44b8296b9ad1cd50a52cea2ce028d72ef71099375e5a5e014ee', '3813 Fisk Alley', (SELECT Id FROM Roles WHERE [Name] = 'guest'));


-- Example how to insert a data into a QuestionsAnswers table
/*
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
*/

-- Close all connection and delete db
/*
USE master;
GO
ALTER DATABASE SchoolDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE SchoolDB
*/