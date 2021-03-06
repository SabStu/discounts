USE [HostAFan]
GO
/****** Object:  StoredProcedure [dbo].[Discounts_DeleteById]    Script Date: 6/23/2021 10:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Andrew Garcia>
-- Create date: <4/19/21>
-- Description: <Discounts DeleteById Proc>
-- MODIFIED BY: author
-- MODIFIED DATE:
-- Note:
-- =============================================

ALTER proc [dbo].[Discounts_DeleteById]
			@Id INT

/*
	Select		[Id]
				,[ListingId]
				,[CouponCode]
				,[Title]
				,[Description]
				,[Percentage]
				,[ValidFrom]
				,[ValidUntil]
				,[IsRedeemedAllowed]
				,[CreatedBy]
				,[ModifiedBy]
	From dbo.Discounts

	DECLARE @Id INT = 2;

	EXECUTE dbo.Discounts_DeleteById @Id

	SELECT	[Id]
				,[ListingId]
				,[CouponCode]
				,[Title]
				,[Description]
				,[Percentage]
				,[ValidFrom]
				,[ValidUntil]
				,[IsRedeemedAllowed]
				,[CreatedBy]
				,[ModifiedBy]
	FROM dbo.Discounts
*/
AS

BEGIN
	DELETE FROM [dbo].[Discounts]
	WHERE Id = @Id
END