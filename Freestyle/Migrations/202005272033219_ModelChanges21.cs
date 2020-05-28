namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "Genre", c => c.String());
            AddColumn("dbo.Artist", "PageViews", c => c.Int(nullable: false));
            AddColumn("dbo.Review", "PageViews", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Review", "PageViews");
            DropColumn("dbo.Artist", "PageViews");
            DropColumn("dbo.Album", "Genre");
        }
    }
}
