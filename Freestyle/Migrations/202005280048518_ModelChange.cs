namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Album", "PageVisits");
            DropColumn("dbo.Album", "Genre");
            DropColumn("dbo.Artist", "PageViews");
            DropColumn("dbo.Review", "ReviewDate");
            DropColumn("dbo.Review", "PageViews");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Review", "PageViews", c => c.Int(nullable: false));
            AddColumn("dbo.Review", "ReviewDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Artist", "PageViews", c => c.Int(nullable: false));
            AddColumn("dbo.Album", "Genre", c => c.String());
            AddColumn("dbo.Album", "PageVisits", c => c.Int(nullable: false));
        }
    }
}
