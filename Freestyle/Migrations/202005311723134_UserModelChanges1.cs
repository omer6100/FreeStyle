namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelChanges1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EndUser", "Username", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EndUser", "Username", c => c.String(nullable: false));
        }
    }
}
