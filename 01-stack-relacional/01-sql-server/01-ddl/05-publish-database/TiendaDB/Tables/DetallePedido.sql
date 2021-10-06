CREATE TABLE [dbo].[DetallePedido]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[PedidoId] INT NOT NULL,
	[ArticuloId] INT NOT NULL, 
	[Precio] DECIMAL(12, 2) NOT NULL,
    CONSTRAINT [FK_DetallePedido_Pedido] FOREIGN KEY ([PedidoId]) REFERENCES [Pedido]([Id]), 
    CONSTRAINT [FK_DetallePedido_Articulo] FOREIGN KEY ([ArticuloId]) REFERENCES [Articulo]([Id]),
)
