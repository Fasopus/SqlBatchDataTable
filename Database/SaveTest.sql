CREATE PROCEDURE [dbo].[SaveTest]
	@value INT,
	@id INT OUTPUT
AS
BEGIN
    DECLARE @tempIdent TABLE ([Ident] INT)
				
	INSERT INTO [dbo].[TestTable] ([Value])
		OUTPUT inserted.Id INTO @tempIdent
		VALUES (@value)

	-- Set the id variable to the identity output
	SET @id = (SELECT Ident FROM @tempIdent)
END
