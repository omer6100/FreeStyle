SET IDENTITY_INSERT [dbo].[Review] ON
INSERT INTO [dbo].[Review] ([Id], [UserId], [AlbumId], [Text], [Score], [Username], [AlbumTitle]) VALUES (1, 2, 3, N'Strokes RULE', 9, N'Julian', N'The New Abnormal')
INSERT INTO [dbo].[Review] ([Id], [UserId], [AlbumId], [Text], [Score], [Username], [AlbumTitle]) VALUES (2, 3, 5, N'The Best Album of all time', 10, N'FrenchHouseLover', N'Discovery')
INSERT INTO [dbo].[Review] ([Id], [UserId], [AlbumId], [Text], [Score], [Username], [AlbumTitle]) VALUES (5, 4, 43, N'good album,  but it stays in the radio-friendly pop landscape', 7, N'omerh', N'Future Nostalgia')
INSERT INTO [dbo].[Review] ([Id], [UserId], [AlbumId], [Text], [Score], [Username], [AlbumTitle]) VALUES (6, 6, 36, N'one of the top pop albums of the 2010''s.  ', 9, N'example', N'Charli')
INSERT INTO [dbo].[Review] ([Id], [UserId], [AlbumId], [Text], [Score], [Username], [AlbumTitle]) VALUES (7, 4, 36, N'truly "Next Level"', 10, N'omerh', N'Charli')
INSERT INTO [dbo].[Review] ([Id], [UserId], [AlbumId], [Text], [Score], [Username], [AlbumTitle]) VALUES (8, 4, 41, N'odd turn for kanye in the musical sense, even though his earlier music did have some connection to god.  ', 5, N'omerh', N'Jesus is King')
INSERT INTO [dbo].[Review] ([Id], [UserId], [AlbumId], [Text], [Score], [Username], [AlbumTitle]) VALUES (1004, 4, 2, N'emotional and honest rap from kanye, as he talks about his mental health struggles and love for his family', 8, N'omerh', N'Ye')
SET IDENTITY_INSERT [dbo].[Review] OFF
