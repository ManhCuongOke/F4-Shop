DROP TABLE IF EXISTS [dbo].[OrderDetail];
GO
CREATE TABLE [dbo].[OrderDetail]
(
	[OrderID] INT NOT NULL, 
    [ProductID] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [UnitPrice] INT NOT NULL, 
    CONSTRAINT [FK_OrderDetail_ToOrder] FOREIGN KEY ([OrderID]) REFERENCES [Order](ID), 
    CONSTRAINT [FK_OrderDetail_ToProducts] FOREIGN KEY ([ProductID]) REFERENCES [Products](ID)
)
