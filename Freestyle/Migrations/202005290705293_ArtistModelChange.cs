namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtistModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artist", "OriginCountry", c => c.String(nullable: false));
            AlterColumn("dbo.Artist", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artist", "Name", c => c.String());
            DropColumn("dbo.Artist", "OriginCountry");
        }
    }
}
