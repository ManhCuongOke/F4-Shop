DROP TABLE IF EXISTS [dbo].[Review];
GO
CREATE TABLE [dbo].[Review]
(
	[ID] INT NOT NULL, 
    [Stars] INT NOT NULL, 
    [UserID] INT NOT NULL, 
    [ProductID] INT NOT NULL, 
    [Comment] NVARCHAR(MAX) NULL, 
    [CreatedTime] DATE NOT NULL, 
    [UpdatedTime] DATE NOT NULL, 
    [DeletedTime] DATE NULL, 
    CONSTRAINT [PK_Review] PRIMARY KEY ([ID]), 
    CONSTRAINT [CK_Review_Star] CHECK ([Stars] >= 1 and [Stars] <=5), 
    CONSTRAINT [FK_Review_ToUser] FOREIGN KEY ([UserID]) REFERENCES [User](ID), 
    CONSTRAINT [FK_Review_ToProduct] FOREIGN KEY ([ProductID]) REFERENCES [Products]([ID]) 
)
