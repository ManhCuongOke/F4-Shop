DROP TABLE IF EXISTS [dbo].[CartDetail];
GO
CREATE TABLE [dbo].[CartDetail] (
    [UserID]    INT NOT NULL,
    [ProductID] INT NULL,
    [CartID]    INT NULL,
    [Quantity]  INT NULL, 
    CONSTRAINT [FK_CartDetail_ToUser] FOREIGN KEY (UserID) REFERENCES [User](ID), 
    CONSTRAINT [FK_CartDetail_ToProduct] FOREIGN KEY ([ProductID]) REFERENCES [Products]([ID]), 
    CONSTRAINT [FK_CartDetail_ToCart] FOREIGN KEY ([CartID]) REFERENCES [Cart](ID)
);
