CREATE TABLE [dbo].[AlbumTable]
(
    [AlbumId] INT NOT NULL Primary Key, 
    [Artist] NVARCHAR(100) NOT NULL, 
    [ReleaseDate] DATE NULL, 
    [AvgScore] FLOAT NOT NULL, 
    [Title] NVARCHAR(100) NOT NULL
)
