namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Review", "ReviewCreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Search", "DateLowerBound", c => c.DateTime(nullable: false));
            AddColumn("dbo.Search", "DateUpperBound", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Search", "DateUpperBound");
            DropColumn("dbo.Search", "DateLowerBound");
            DropColumn("dbo.Review", "ReviewCreationTime");
        }
    }
}
