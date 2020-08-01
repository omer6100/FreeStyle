namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EndUser", "FavAlbum_Id", "dbo.Album");
            DropForeignKey("dbo.EndUser", "FavArtist_Id", "dbo.Artist");
            DropIndex("dbo.EndUser", new[] { "FavAlbum_Id" });
            DropIndex("dbo.EndUser", new[] { "FavArtist_Id" });
            DropColumn("dbo.EndUser", "Bio");
            DropColumn("dbo.EndUser", "FavAlbum_Id");
            DropColumn("dbo.EndUser", "FavArtist_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EndUser", "FavArtist_Id", c => c.Int());
            AddColumn("dbo.EndUser", "FavAlbum_Id", c => c.Int());
            AddColumn("dbo.EndUser", "Bio", c => c.String());
            CreateIndex("dbo.EndUser", "FavArtist_Id");
            CreateIndex("dbo.EndUser", "FavAlbum_Id");
            AddForeignKey("dbo.EndUser", "FavArtist_Id", "dbo.Artist", "Id");
            AddForeignKey("dbo.EndUser", "FavAlbum_Id", "dbo.Album", "Id");
        }
    }
}
