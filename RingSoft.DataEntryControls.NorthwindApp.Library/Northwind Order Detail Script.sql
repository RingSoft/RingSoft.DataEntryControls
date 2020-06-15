USE [RSDEC_Northwind]
GO
/****** Object:  Table [dbo].[Order Details]    Script Date: 6/12/2020 1:29:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @orderId INT
DECLARE @getOrderIdCursor CURSOR

DECLARE @getOrderDetailCursor CURSOR
DECLARE @productId INT
DECLARE @orderDetailId INT

SET @getOrderIdCursor = CURSOR FOR
SELECT DISTINCT [OrderID]
FROM [Order Details]
ORDER BY [OrderID] ASC

OPEN @getOrderIdCursor
FETCH NEXT
FROM @getOrderIdCursor INTO @orderId
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @orderDetailId = 1
    SET @getOrderDetailCursor = CURSOR FOR
	SELECT [Order Details].ProductID
	FROM [Order Details]
	WHERE [Order Details].[OrderID] = @orderId
	ORDER BY [ProductID] ASC

	OPEN @getOrderDetailCursor
	FETCH NEXT
	FROM @getOrderDetailCursor INTO @productId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE [Order Details]
		SET [OrderDetailID] = @orderDetailId
		WHERE [OrderID] = @orderId
			AND [ProductID] = @productId

		SET @orderDetailId = @orderDetailId + 1
		FETCH NEXT
		FROM @getOrderDetailCursor INTO @productId
	END

    FETCH NEXT
    FROM @getOrderIdCursor INTO @orderId
END

CLOSE @getOrderDetailCursor
DEALLOCATE @getOrderDetailCursor
CLOSE @getOrderIdCursor
DEALLOCATE @getOrderIdCursor