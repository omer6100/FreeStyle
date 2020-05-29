namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class artistModelChange21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artist", "PageViews", c => c.Int(nullable: false));
            DropColumn("dbo.Artist", "PageVisits");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Artist", "PageVisits", c => c.Int(nullable: false));
            DropColumn("dbo.Artist", "PageViews");
        }
    }
}
