namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewModelChanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Review", "AuthorName");
            DropColumn("dbo.Review", "AlbumTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Review", "AlbumTitle", c => c.String());
            AddColumn("dbo.Review", "AuthorName", c => c.String());
        }
    }
}
