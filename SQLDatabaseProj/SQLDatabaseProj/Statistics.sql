CREATE TABLE [dbo].[Statistics]
(
    [Id] INT NOT NULL IDENTITY,
    [Name] NCHAR(32) NOT NULL,
    [Password] NCHAR(4) NOT NULL,
    [GamesPlayed] INT NOT NULL DEFAULT 0,
    [GamesWon] INT NOT NULL DEFAULT 0,
    [GamesLost] INT NOT NULL DEFAULT 0,
    PRIMARY KEY ([Id]) 
)
