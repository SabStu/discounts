USE [HostAFan]
GO
/****** Object:  StoredProcedure [dbo].[Discounts_SelectById]    Script Date: 6/23/2021 10:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Andrew Garcia>
-- Create date: <4/19/21>
-- Description: <Discounts Select By Id Proc>
-- MODIFIED BY: author
-- MODIFIED DATE:
-- Note:
-- =============================================

ALTER proc [dbo].[Discounts_SelectById]
			@Id INT
/*
	DECLARE @Id INT = 4;

	EXECUTE dbo.Discounts_SelectbyId @Id

*/

AS

BEGIN
	SELECT [Id]
			,[ListingId]
			,[CouponCode]
			,[Title]
			,[Description]
			,[Percentage]
			,[ValidFrom]
			,[ValidUntil]
			,[IsRedeemedAllowed]
			,[DateCreated]
			,[DateModified]
			,[CreatedBy]
			,[ModifiedBy]
	FROM [dbo].[Discounts]
	WHERE Id = @Id
END