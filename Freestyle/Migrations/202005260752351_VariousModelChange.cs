namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VariousModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artist", "AvgScore", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artist", "AvgScore");
        }
    }
}
