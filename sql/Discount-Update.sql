USE [HostAFan]
GO
/****** Object:  StoredProcedure [dbo].[Discounts_Update]    Script Date: 6/23/2021 10:48:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Andrew Garcia>
-- Create date: <4/19/21>
-- Description: <Discounts Update Proc>
-- MODIFIED BY: author
-- MODIFIED DATE:
-- Note:
-- =============================================

ALTER proc [dbo].[Discounts_Update]
		@Id INT
		,@ListingId INT
		,@CouponCode NVARCHAR(20)
		,@Title NVARCHAR(50)
		,@Description NVARCHAR(100)
		,@Percentage DECIMAL(18,0)
		,@ValidFrom DATETIME2(7)
		,@ValidUntil DATETIME2(7)
		,@IsRedeemedAllowed BIT
		,@ModifiedBy INT
/*
	DECLARE @Id INT = 1
			,@ListingId INT = 102
			,@CouponCode NVARCHAR(20) = 'GUNPACKII'
			,@Title NVARCHAR(50) = 'WARZONE-DLC'
			,@Description NVARCHAR(100) = '5-NEW-SKINS'
			,@Percentage DECIMAL(18,0) = 11.23234
			,@ValidFrom DATETIME2(7) = '2021-04-01'
			,@ValidUntil DATETIME2(7) = '2021-04-14'
			,@IsRedeemedAllowed BIT = 0
			,@ModifiedBy INT = 1
			

	EXECUTE dbo.Discounts_Update
				@Id
				,@ListingId
				,@CouponCode
				,@Title
				,@Description
				,@Percentage
				,@ValidFrom
				,@ValidUntil
				,@IsRedeemedAllowed
				,@ModifiedBy
	SELECT *
	FROM dbo.Discounts
*/

AS

BEGIN
	DECLARE @DateNow DATETIME2 = GETUTCDATE()
	UPDATE [dbo].[Discounts]
	SET [ListingId] = @ListingId
		,[CouponCode] = @CouponCode
		,[Title] = @Title
		,[Description] = @Description
		,[Percentage] = @Percentage
		,[ValidFrom] = @ValidFrom
		,[ValidUntil] = @ValidUntil
		,[IsRedeemedAllowed] = @IsRedeemedAllowed
		,[DateModified] = @DateNow
		,[ModifiedBy] = @ModifiedBy
	WHERE Id = @Id

END