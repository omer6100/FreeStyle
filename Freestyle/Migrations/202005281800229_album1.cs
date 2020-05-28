namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class album1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "PageViews", c => c.Int(nullable: false));
            AlterColumn("dbo.Album", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Album", "Artist", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Album", "Artist", c => c.String());
            AlterColumn("dbo.Album", "Title", c => c.String());
            DropColumn("dbo.Album", "PageViews");
        }
    }
}
