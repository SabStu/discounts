USE [HostAFan]
GO
/****** Object:  StoredProcedure [dbo].[Discounts_Insert]    Script Date: 6/23/2021 10:47:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author: <Andrew Garcia>
-- Create date: <4/19/21>
-- Description: <Discounts Insert Proc>
-- MODIFIED BY: author
-- MODIFIED DATE:
-- Note:
-- =============================================

ALTER proc [dbo].[Discounts_Insert]

		@ListingId INT,
		@CouponCode NVARCHAR(20),
		@Title NVARCHAR(20),
		@Description NVARCHAR(50),
		@Percentage DECIMAL(18,0),
		@ValidFrom DATETIME2(7),
		@ValidUntil DATETIME2(7),
		@IsRedeemedAllowed BIT,
		@CreatedBy INT,
		@ModifiedBy INT,
		@Id INT OUTPUT

/*

	DECLARE @ListingId INT = 103
			,@CouponCode NVARCHAR(20) = 'GUNPACKIIII'
			,@Title NVARCHAR(20) = 'COD-Warzone-DLC'
			,@Description NVARCHAR(50)= '2-New-Weapons'
			,@Percentage DECIMAL(18,0) = 20.5
			,@ValidFrom DATETIME2(7) = '2021-04-01'
			,@ValidUntil DATETIME2(7) = '2021-05-01'
			,@IsRedeemedAllowed BIT = 1
			,@CreatedBy INT = 3
			,@ModifiedBy INT = 3
			,@Id INT

	EXECUTE dbo.Discounts_Insert
			@ListingId
			,@CouponCode
			,@Title
			,@Description
			,@Percentage
			,@ValidFrom
			,@ValidUntil
			,@IsRedeemedAllowed
			,@CreatedBy
			,@Modifiedby
			,@Id OUTPUT

	SELECT [ListingId]
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
	WHERE Id = @Id
*/

AS

BEGIN
	INSERT INTO [dbo].[Discounts]
				([ListingId]
				,[CouponCode]
				,[Title]
				,[Description]
				,[Percentage]
				,[ValidFrom]
				,[ValidUntil]
				,[IsRedeemedAllowed]
				,[CreatedBy]
				,[ModifiedBy])
		VALUES 
				(@ListingId
				,@CouponCode
				,@Title
				,@Description
				,@Percentage
				,@ValidFrom
				,@ValidUntil
				,@IsRedeemedAllowed
				,@CreatedBy
				,@ModifiedBy)
	
	SET @Id = SCOPE_IDENTITY()
END