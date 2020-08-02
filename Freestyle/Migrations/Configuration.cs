namespace Freestyle.Migrations
{
    using Freestyle.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexts.MusicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Contexts.MusicContext context)
        {
            var albums = new List<Album>
                {
                    new Album
                    {
                        Id = 43,
                        Artist = "Dua Lipa",
                        Title = "Future Nostalgia",
                        AvgScore = 8.0,
                        ReleaseDate = new DateTime(2020, 3, 27),
                        ArtistId = 5,
                        Genre="Pop"
                    },
                    new Album
                    {
                        Id = 2,
                        Artist = "Kanye West",
                        Title = "Ye",
                        AvgScore = 8.57,
                        ReleaseDate = new DateTime(2018, 7, 1),
                        ArtistId = 1,
                        Genre="Rap"
                    },
                    new Album
                    {
                        Id = 3,
                        Artist = "The Strokes",
                        Title = "Is This It",
                        AvgScore = 9,
                        ReleaseDate = new DateTime(2001, 7, 30),
                        ArtistId = 3,
                        Genre="Alternative"
                    },
                    new Album
                    {
                        Id = 4,
                        Artist = "Yes",
                        Title = "Fragile",
                        AvgScore = 9.3,
                        ReleaseDate = new DateTime(1971, 11, 12),
                        ArtistId = 2,
                        Genre="Rock"
                    },
                    new Album
                    {
                        Id = 5,
                        Artist = "Daft Punk",
                        Title = "Discovery",
                        AvgScore = 10,
                        ReleaseDate = new DateTime(2001, 1, 22),
                        ArtistId = 4,
                        Genre="Electronic"
                    }
            };

            albums.ForEach(a => context.Albums.AddOrUpdate(e=>e.Id, a));
            context.SaveChanges();

            var artists = new List<Artist>
            {
                new Artist
                {
                    Id = 1, Name = "Kanye West", AvgScore = 9.5, OriginCountry = "United States"
                },
                new Artist
                {
                    Id = 2, Name = "Yes", AvgScore = 7.22, OriginCountry = "England"
                },
                new Artist
                {
                    Id =3, Name = "The Strokes", AvgScore = 8, OriginCountry = "United States"
                },
                new Artist
                {
                    Id =4, Name = "Daft Punk", AvgScore = 9.19, OriginCountry = "France"
                },
                new Artist
                {
                    Id=5, Name = "Dua Lipa", AvgScore = 5.97, OriginCountry = "England"
                }
            };

            artists.ForEach(a => context.Artists.AddOrUpdate(e=>e.Id, a));
            context.SaveChanges();

            var reviews = new List<Review>
            {
                new Review()
                {
                    AlbumId = 3, Id = 1, Score = 9, AlbumTitle = "The New Abnormal", 
                    Text = "Strokes RULE", UserId = 1, Username = "Julian"
                },
                new Review()
                {
                    AlbumId = 5, Id = 2, Score = 10, AlbumTitle = "Discovery", 
                    Text = "The Best Album of all time", UserId = 2, Username = "FrenchHouseLover"
                }
            };
           
            reviews.ForEach(r=>context.Reviews.AddOrUpdate(e=>e.Id, r));
            context.SaveChanges();

            var users = new List<EndUser>()
            {
                new EndUser()
                {
                    Id = 0, Username = "admin", Email = "admin@freestyle.com", Password = "adm1nPassword"
                },
                new EndUser()
                {
                    Id = 1, Username = "Julian", Email = "JulianCaz@Strokes.com", Password = "Julian1988"
                },
                new EndUser()
                {
                    Id = 2, Username = "FrenchHouseLover", Email = "Guy@Manuel.com"
                },
                new EndUser()
                {
                    Id = 3, Username = "omerh", Email = "omer@freestyle.com", Password = "omer1234"
                },
                new EndUser()
                {
                    Id =4 , Username = "shmuelyo", Email = "joseph@freestyle.com", Password="joseph5678"
                }
            };

            users.ForEach(u=>context.Users.AddOrUpdate(e=>e.Username, u));
            context.SaveChanges();
        }
    }
}
