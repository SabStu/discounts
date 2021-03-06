USE [HostAFan]
GO
/****** Object:  StoredProcedure [dbo].[Discounts_SelectByCouponCode]    Script Date: 6/23/2021 10:48:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Andrew Garcia>
-- Create date: <4/19/21>
-- Description: <Discounts Insert Proc>
-- MODIFIED BY: author
-- MODIFIED DATE: <4/21/21>
-- Note:
-- =============================================


ALTER proc [dbo].[Discounts_SelectByCouponCode]
			@pageIndex INT
			,@pageSize INT
			,@CouponCode NVARCHAR(20)

/*
	DECLARE @pageIndex INT = 0
			,@pageSize INT = 1
			,@CouponCode NVARCHAR(20) = 'GUNPACKII'

	EXECUTE dbo.Discounts_SelectByCouponCode 
						@pageIndex
						,@pageSize
						,@CouponCode
*/

AS

BEGIN

	DECLARE @offset int = @PageIndex * @PageSize

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
			,[TotalCount] = COUNT(1) OVER()
	FROM [dbo].[Discounts]
	WHERE CouponCode = @CouponCode
	ORDER BY Id

	OFFSET @offSet Rows
	Fetch Next @pageSize Rows ONLY
END