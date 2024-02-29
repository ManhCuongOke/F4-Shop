DROP TABLE IF EXISTS [dbo].[Products];
GO
CREATE TABLE [dbo].[Products] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [Quantity]    INT            NOT NULL,
    [CategotyID]  INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [ImageUrl]    VARCHAR (MAX)  NOT NULL,
    [Price]       INT            NOT NULL,
    [IsVisible]   BIT            DEFAULT ((1)) NOT NULL,
	[CreatedTime] DATE NOT NULL, 
    [UpdatedTime] DATE NOT NULL, 
    [DeletedTime] DATE NULL, 
    CONSTRAINT [FK_Products_Category] FOREIGN KEY ([CategotyID]) REFERENCES [dbo].[Categories] ([ID]), 
    CONSTRAINT [PK_Products] PRIMARY KEY ([ID])
);


