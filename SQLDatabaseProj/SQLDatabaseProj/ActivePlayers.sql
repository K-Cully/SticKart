CREATE TABLE [dbo].[ActivePlayers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ip] NCHAR(39) NOT NULL,
    [Port] NCHAR(4) NOT NULL,
    [State] NCHAR(1) NOT NULL, 
    [Player] INT NOT NULL, 
    CONSTRAINT [FK_ActivePlayers_Statistics] FOREIGN KEY ([Player]) REFERENCES [Statistics]([Id])
)
