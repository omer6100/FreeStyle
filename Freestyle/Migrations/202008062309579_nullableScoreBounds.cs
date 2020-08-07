namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableScoreBounds : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Search", "ScoreLowerBound", c => c.Double());
            AlterColumn("dbo.Search", "ScoreUpperBound", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Search", "ScoreUpperBound", c => c.Double(nullable: false));
            AlterColumn("dbo.Search", "ScoreLowerBound", c => c.Double(nullable: false));
        }
    }
}
