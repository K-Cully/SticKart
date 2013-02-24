
DECLARE @name CHAR(10) = 'AAA'
DECLARE @score INT = 110

SELECT COUNT(*) FROM HighScores_000 WHERE Name=@name AND Score=@score