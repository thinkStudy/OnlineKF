USE [OnlineKF]
GO
/****** Object:  StoredProcedure [dbo].[P_MessageData_insert]    Script Date: 05/25/2016 15:27:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		左都谷
-- Create date: 2016.5.21
-- Description:	累加聊天信息到数据库
-- =============================================
ALTER PROCEDURE [dbo].[P_MessageData_insert]
(
	@questionId varchar(500),
	@content nvarchar(4000),
	@maxLength int
)	
AS
BEGIN
	DECLARE @OddContent nvarchar(max);
	DECLARE @NewContent nvarchar(max);
	DECLARE @OnlyCount int;
	SELECT @OddContent = message,@OnlyCount = onlycount FROM dbo.MessageData WHERE questionid = @questionId
	
	IF SUBSTRING(@content,2,2) = 'qm'
	BEGIN
		SET @OnlyCount = @OnlyCount+1;
	END
	
	IF(LEN(@OddContent)> @maxLength)
	BEGIN
		set @NewContent = '{'+@content+'}'
		UPDATE dbo.MessageData SET  message = @NewContent, backmessage = @OddContent,onlycount = @OnlyCount WHERE questionid = @questionId
	END
	ELSE
	BEGIN
		set @NewContent = @OddContent+','+@content
		
		UPDATE dbo.MessageData SET  message = @NewContent,onlycount = @OnlyCount WHERE questionid = @questionId
	END
END
