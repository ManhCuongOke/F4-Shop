DROP TABLE IF EXISTS [dbo].[Order];
GO
CREATE TABLE [dbo].[Order] (
    [ID]         INT        NOT NULL,
    [UserID]     INT        NOT NULL,
    [Date]       DATE       NOT NULL,
    [TotalPrice] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Order_ToUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

