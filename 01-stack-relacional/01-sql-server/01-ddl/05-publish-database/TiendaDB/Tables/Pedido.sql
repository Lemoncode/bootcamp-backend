CREATE TABLE [dbo].[Pedido]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[ClienteId] INT NOT NULL,
    [Creado] DATETIME NOT NULL , 
    [CuponDescuentoId] INT NULL, 
    CONSTRAINT [FK_Pedido_Cliente] FOREIGN KEY ([ClienteId]) REFERENCES [Cliente]([Id]),	
)
