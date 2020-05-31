namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EndUser", "Email", c => c.String(nullable: false));
            AddColumn("dbo.EndUser", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.EndUser", "Username", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EndUser", "Username", c => c.String());
            DropColumn("dbo.EndUser", "Password");
            DropColumn("dbo.EndUser", "Email");
        }
    }
}
