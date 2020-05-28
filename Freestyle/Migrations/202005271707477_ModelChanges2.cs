namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "PageVisits", c => c.Int(nullable: false));
            AddColumn("dbo.Review", "ReviewDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Review", "ReviewDate");
            DropColumn("dbo.Album", "PageVisits");
        }
    }
}
