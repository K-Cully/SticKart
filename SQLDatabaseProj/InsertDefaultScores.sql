

DECLARE @myCounter INT = 0
WHILE @myCounter < 10
BEGIN
INSERT INTO HighScores_000 (Name, Score) VALUES ('AAA', 0)
SET @myCounter = 1 + @myCounter 
END