namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Album",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false),
                    Artist = c.String(nullable: false),
                    ArtistId = c.Int(nullable: false),
                    ReleaseDate = c.DateTime(nullable: false),
                    Genre = c.String(nullable: false),
                    AvgScore = c.Double(nullable: false),
                    PageViews = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Artist",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    OriginCountry = c.String(nullable: false),
                    AvgScore = c.Double(nullable: false),
                    PageViews = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Review",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AuthorName = c.String(),
                    UserId = c.Int(nullable: false),
                    AlbumTitle = c.String(),
                    AlbumId = c.Int(nullable: false),
                    Text = c.String(),
                    Score = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EndUser",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Email = c.String(nullable: false),
                    Password = c.String(nullable: false),
                    Username = c.String(),
                    Bio = c.String(),
                    FavAlbum_Id = c.Int(),
                    FavArtist_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Album", t => t.FavAlbum_Id)
                .ForeignKey("dbo.Artist", t => t.FavArtist_Id)
                .Index(t => t.FavAlbum_Id)
                .Index(t => t.FavArtist_Id);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EndUser", "FavArtist_Id", "dbo.Artist");
            DropForeignKey("dbo.EndUser", "FavAlbum_Id", "dbo.Album");
            DropIndex("dbo.EndUser", new[] { "FavArtist_Id" });
            DropIndex("dbo.EndUser", new[] { "FavAlbum_Id" });
            DropTable("dbo.EndUser");
            DropTable("dbo.Review");
            DropTable("dbo.Artist");
            DropTable("dbo.Album");
        }
    }
}
