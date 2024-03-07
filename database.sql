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
	[Address] NVARCHAR(200) NOT NULL,
	[Country] NVARCHAR(100),
	[City] NVARCHAR(100),
	[CompanyName] NVARCHAR(100),
	[Region] NVARCHAR(100),
	[PostalCode] NVARCHAR(20),
	[State] NVARCHAR(100),
	[County] NVARCHAR(100),
	[HomePhone] char(13),
	[HomePage] NVARCHAR(100),
	[Longitude] DECIMAL(10, 6),
	[Latitude] DECIMAL(10, 6),
	[JobTitle] NVARCHAR(100),
	[ContactGender] NVARCHAR(20),
	[EmployeeSize] int,
	[CapitalSize] DECIMAL(18, 2),
	[ClassifyCode] NVARCHAR(20),
	[BusinessType] NVARCHAR(100),
	[Nationality] NVARCHAR(100),
	[IsMarried] bit Default 0,
	[HaveChildren] bit Default 0,
	[HomeOwner] bit Default 0,
	[YearInBusiness] int,
	[IsSelfEmployed] bit Default 0, 
	[DynamicInfo] NVARCHAR(MAX),
	[CreatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[IsDeleted] bit NOT NULL DEFAULT 0
)

CREATE TABLE [Agent]
(
	[ID] int Identity(1,1) PRIMARY KEY,
	[Email] NVARCHAR(50) NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[PhoneNumber] CHAR(13) NOT NULL,
	[Address] NVARCHAR(200) NOT NULL,
	[Password] NVARCHAR(200) NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[IsDeleted] bit NOT NULL DEFAULT 0,
	[Role] NVARCHAR(5) NOT NULL DEFAULT 'User'
)
GO
CREATE TABLE [Asign]
(
	[ID] int Identity(1,1) PRIMARY KEY,
	[AgentID] int NOT NULL,
	[CustomerID] int NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[IsDeleted] bit NOT NULL DEFAULT 0,
	FOREIGN KEY([AgentID]) REFERENCES  [Agent]([ID]),
	FOREIGN KEY([CustomerID]) REFERENCES  [Customer]([ID])
)
GO
CREATE TABLE [Note]
(
	[ID] int Identity(1,1) PRIMARY KEY,
	[CustomerID] int NOT NULL,
	[AgentID] int NOT NULL,
	[Title] NVARCHAR(200) NOT NULL,
	[Content] TEXT NOT NULL,
	[Type] CHAR(1) NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	[UpdatedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY([CustomerID]) REFERENCES  [Customer]([ID]),
	[IsDeleted] BIT NOT NULL DEFAULT 0
)
GO

-- Insert data into Customer table
INSERT INTO [Customer] ([Email], [FirstName], [LastName], [PhoneNumber], [Address])
VALUES
('customer1@example.com', 'John', 'Doe', '123-456-7890', '123 Main St'),
('customer2@example.com', 'Jane', 'Smith', '987-654-3210', '456 Elm St'),
('customer3@example.com', 'Michael', 'Johnson', '111-222-3333', '789 Oak St'),
('customer4@example.com', 'Emily', 'Davis', '444-555-6666', '999 Pine St');

-- Insert data into Agent table
INSERT INTO [Agent] ([Email], [FirstName], [LastName], [PhoneNumber], [Address], [Password], [Role])
VALUES
('agent1@example.com', 'Alice', 'Johnson', '555-123-4567', '123 Oak St', '$2a$10$4xJAJJT61iHdqTeC2WKaUu19SUgenxiujNgIn56illQ94IObuHaly', 'Admin'),
('agent2@example.com', 'Bob', 'Williams', '555-987-6543', '321 Maple St', 'password2', 'User'),
('agent3@example.com', 'Sophia', 'Martinez', '555-111-2233', '456 Cedar St', 'password3', 'User'),
('agent4@example.com', 'William', 'Brown', '555-444-5566', '789 Walnut St', 'password4', 'User');

-- Insert data into Asign table
INSERT INTO [Asign] ([AgentID], [CustomerID])
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4);

-- Insert data into Note table
INSERT INTO [Note] ([CustomerID], [AgentID], [Title], [Content], [Type])
VALUES
(1, 1, 'Meeting Note', 'This is a note about the meeting.', 'M'),
(2, 2, 'Reminder', 'This is a reminder note.', 'R'),
(3, 3, 'Follow-Up Note', 'This is a follow-up note for customer.', 'F'),
(4, 4, 'Important Note', 'This is an important note.', 'I');

-- Insert more data into Asign table
INSERT INTO [Asign] ([AgentID], [CustomerID])
VALUES
(1, 2), -- Agent 1 assigned to Customer 2
(2, 3), -- Agent 2 assigned to Customer 3
(3, 4), -- Agent 3 assigned to Customer 4
(4, 1); -- Agent 4 assigned to Customer 1

-- Insert more data into Note table
INSERT INTO [Note] ([CustomerID], [AgentID], [Title], [Content], [Type])
VALUES
(2, 1, 'Follow-Up Call', 'This is a follow-up call note.', 'F'),
(3, 2, 'Meeting Note', 'This is a note about the meeting.', 'M'),
(4, 3, 'Reminder', 'This is a reminder note.', 'R'),
(1, 4, 'Follow-Up Note', 'This is a follow-up note for customer.', 'F'),
(2, 3, 'Appointment Confirmation', 'This is a confirmation note for the appointment.', 'A'),
(3, 4, 'Important Note', 'This is an important note for the customer.', 'I'),
(4, 1, 'Feedback Note', 'This is a feedback note from the customer.', 'F'),
(1, 2, 'Emergency Note', 'This is an emergency note.', 'E');
GO