namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yos1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Search", "DateLowerBound", c => c.DateTime());
            AlterColumn("dbo.Search", "DateUpperBound", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Search", "DateUpperBound", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Search", "DateLowerBound", c => c.DateTime(nullable: false));
        }
    }
}
