namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialData1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.User", newName: "EndUser");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.EndUser", newName: "User");
        }
    }
}
