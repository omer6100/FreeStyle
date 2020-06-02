namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelChanges2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EndUser", "Bio", c => c.String());
            AddColumn("dbo.EndUser", "FavAlbum_Id", c => c.Int());
            AddColumn("dbo.EndUser", "FavArtist_Id", c => c.Int());
            CreateIndex("dbo.EndUser", "FavAlbum_Id");
            CreateIndex("dbo.EndUser", "FavArtist_Id");
            AddForeignKey("dbo.EndUser", "FavAlbum_Id", "dbo.Album", "Id");
            AddForeignKey("dbo.EndUser", "FavArtist_Id", "dbo.Artist", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EndUser", "FavArtist_Id", "dbo.Artist");
            DropForeignKey("dbo.EndUser", "FavAlbum_Id", "dbo.Album");
            DropIndex("dbo.EndUser", new[] { "FavArtist_Id" });
            DropIndex("dbo.EndUser", new[] { "FavAlbum_Id" });
            DropColumn("dbo.EndUser", "FavArtist_Id");
            DropColumn("dbo.EndUser", "FavAlbum_Id");
            DropColumn("dbo.EndUser", "Bio");
        }
    }
}
