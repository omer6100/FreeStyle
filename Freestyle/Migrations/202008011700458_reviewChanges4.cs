namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewChanges4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Review", "Username", c => c.String());
            AddColumn("dbo.Review", "AlbumTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Review", "AlbumTitle");
            DropColumn("dbo.Review", "Username");
        }
    }
}
