


DECLARE @value INT = 110
DECLARE @name CHAR(10) = 'AAA'

DECLARE @maxRows INT = 10
DECLARE @rowCount INT = 0
DECLARE @lowest INT = 0
SELECT @rowCount = COUNT(*) FROM HighScores_000
SELECT TOP 1 @lowest = Score FROM HighScores_000 ORDER BY Score ASC

IF @rowCount < @maxRows
BEGIN
INSERT INTO HighScores_000 (Name, Score) VALUES (@name, @value)
END

ELSE IF @value > @lowest
BEGIN
INSERT INTO HighScores_000 (Name, Score) VALUES (@name, @value)
DELETE TOP(1) FROM HighScores_000 WHERE Score < @value
END