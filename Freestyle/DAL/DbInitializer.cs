using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web;
using Freestyle.Contexts;
using Freestyle.Models;

namespace Freestyle.DAL
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<MusicContext>
    {
        
        protected override void Seed(MusicContext context)
        {
            var albums = new List<Album>
                {
                    new Album
                    {
                        Id = 1,
                        Artist = "Dua Lipa",
                        Title = "Future Nostalgia",
                        AvgScore = 8.0,
                        ReleaseDate = new DateTime(2020, 3, 27),
                        ArtistId = 5
                    },
                    new Album
                    {
                        Id = 2,
                        Artist = "Kanye West",
                        Title = "Ye",
                        AvgScore = 8.57,
                        ReleaseDate = new DateTime(2018, 7, 1),
                        ArtistId = 1
                    },
                    new Album
                    {
                        Id = 3,
                        Artist = "The Strokes",
                        Title = "Is This It",
                        AvgScore = 9,
                        ReleaseDate = new DateTime(2001, 7, 2001),
                        ArtistId = 3
                    },
                    new Album
                    {
                        Id = 4,
                        Artist = "Yes",
                        Title = "Fragile",
                        AvgScore = 9.3,
                        ReleaseDate = new DateTime(1971, 11, 12),
                        ArtistId = 2
                    },
                    new Album
                    {
                        Id = 5,
                        Artist = "Daft Punk",
                        Title = "Discovery",
                        AvgScore = 10,
                        ReleaseDate = new DateTime(2001, 1, 2001),
                        ArtistId = 4
                    }
            };
            
            albums.ForEach(a =>context.Albums.Add(a));
            context.SaveChanges();

            var artists = new List<Artist>
            {
                new Artist
                {
                    Id = 1, Name = "Kanye West"
                },
                new Artist
                {
                    Id = 2, Name = "Yes"
                },
                new Artist
                {
                    Id =3, Name = "The Strokes"
                },
                new Artist
                {
                    Id =4, Name = "Daft Punk"
                },
                new Artist
                {
                    Id=5, Name = "Dua Lipa"
                }
            };

            artists.ForEach(a => context.Artists.Add(a));
            context.SaveChanges();
        }

    }
}