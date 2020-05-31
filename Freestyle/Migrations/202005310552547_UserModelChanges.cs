namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Artist", "OriginCountry", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artist", "OriginCountry", c => c.String());
        }
    }
}
