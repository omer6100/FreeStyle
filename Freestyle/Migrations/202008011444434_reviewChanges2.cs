namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewChanges2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Review", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.Review", "Score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Review", "Score", c => c.Double(nullable: false));
            AlterColumn("dbo.Review", "Text", c => c.String());
        }
    }
}
