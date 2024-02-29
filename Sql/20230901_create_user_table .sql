DROP TABLE IF EXISTS [dbo].[User];
GO
CREATE TABLE [dbo].[User] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [Address]   NVARCHAR (500) NOT NULL,
    [Email]     NVARCHAR (50)  NOT NULL,
    [RoleID]    INT            NOT NULL,
    [DateBirth] DATE           NOT NULL,
    [Phone]     CHAR (20)      NOT NULL,
    [CreatedTime] DATE NOT NULL, 
    [UpdatedTime] DATE NOT NULL, 
    [DeletedTime] DATE NULL, 
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([ID]), 
    CONSTRAINT [PK_User] PRIMARY KEY ([ID])
);

