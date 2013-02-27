


DECLARE @myCounter INT = 0
WHILE @myCounter < 10
BEGIN
INSERT INTO HighScores (Level, Name, Score) VALUES (1, 'AAA', 0)
SET @myCounter = 1 + @myCounter 
END