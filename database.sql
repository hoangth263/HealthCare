USE master
GO

CREATE DATABASE HealthCare
GO

USE HealthCare
GO

CREATE TABLE [Customer]
(
	[ID] int Identity(1,1) PRIMARY KEY,
	[Email] NVARCHAR(50) NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[PhoneNumber] CHAR(13) NOT NULL,
	[Address] NVARCHAR(200) NOT NULL
)

CREATE TABLE [Agent]
(
	[ID] int Identity(1,1) PRIMARY KEY,
	[Email] NVARCHAR(50) NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[PhoneNumber] CHAR(13) NOT NULL,
	[Address] NVARCHAR(200) NOT NULL,
	[Password] NVARCHAR(200) NOT NULL
)
GO
CREATE TABLE [Asign]
(
	[ID] int Identity(1,1) PRIMARY KEY,
	[AgentID] int NOT NULL,
	[CustomerID] int NOT NULL,
	FOREIGN KEY([AgentID]) REFERENCES  [Agent]([ID]),
	FOREIGN KEY([CustomerID]) REFERENCES  [Customer]([ID])
)
GO
CREATE TABLE [Note]
(
	[ID] int Identity(1,1) PRIMARY KEY,
	[AsignID] int NOT NULL,
	[Title] NVARCHAR(200) NOT NULL,
	[Content] TEXT NOT NULL,
	[Type] CHAR(1) NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY([AsignID]) REFERENCES  [Asign]([ID])
)
GO

-- Insert data into Customer table
INSERT INTO [Customer] ([Email], [FirstName], [LastName], [PhoneNumber], [Address])
VALUES
('customer1@example.com', 'John', 'Doe', '1234567890', '123 Main St'),
('customer2@example.com', 'Jane', 'Smith', '0987654321', '456 Elm St');

-- Insert data into Agent table
INSERT INTO [Agent] ([Email], [FirstName], [LastName], [PhoneNumber], [Address], [Password])
VALUES
('agent1@example.com', 'Alice', 'Johnson', '5551234567', '789 Oak St', 'password1'),
('agent2@example.com', 'Bob', 'Williams', '5559876543', '321 Maple St', 'password2');

-- Insert data into Asign table
INSERT INTO [Asign] ([AgentID], [CustomerID])
VALUES
(1, 1),
(2, 2);

-- Insert data into Note table
INSERT INTO [Note] ([AsignID], [Title], [Content], [Type])
VALUES
(1, 'First Note', 'This is the content of the first note.', 'F'),
(2, 'Important Note', 'This is an important note for the second customer.', 'I');


-- Insert more data into Customer table
INSERT INTO [Customer] ([Email], [FirstName], [LastName], [PhoneNumber], [Address])
VALUES
('customer3@example.com', 'Michael', 'Johnson', '1112223333', '555 Pine St'),
('customer4@example.com', 'Emily', 'Davis', '4445556666', '777 Cedar St');

-- Insert more data into Agent table
INSERT INTO [Agent] ([Email], [FirstName], [LastName], [PhoneNumber], [Address], [Password])
VALUES
('agent3@example.com', 'Sophia', 'Martinez', '5551112233', '888 Walnut St', 'password3'),
('agent4@example.com', 'William', 'Brown', '5554445566', '999 Birch St', 'password4');

-- Insert more data into Asign table
INSERT INTO [Asign] ([AgentID], [CustomerID])
VALUES
(1, 3),
(2, 4);

-- Insert more data into Note table
INSERT INTO [Note] ([AsignID], [Title], [Content], [Type])
VALUES
(3, 'Follow-Up Note', 'This is a follow-up note for the third customer.', 'F'),
(4, 'Reminder Note', 'This is a reminder note for the fourth customer.', 'R');
