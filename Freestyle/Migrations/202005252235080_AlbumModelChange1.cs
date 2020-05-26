namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlbumModelChange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "ArtistId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Album", "ArtistId");
        }
    }
}
