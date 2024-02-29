DROP TABLE IF EXISTS [dbo].[Cart];
GO
CREATE TABLE [dbo].[Cart]
(
	[ID] INT NOT NULL, 
    [UpdatedTime] DATE NOT NULL, 
    CONSTRAINT [PK_Cart] PRIMARY KEY ([ID]) 
)
