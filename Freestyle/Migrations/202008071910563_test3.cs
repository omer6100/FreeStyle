namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Album", "PageViews", c => c.Int(nullable: false));
            AlterColumn("dbo.Artist", "PageViews", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artist", "PageViews", c => c.Int());
            AlterColumn("dbo.Album", "PageViews", c => c.Int());
        }
    }
}
