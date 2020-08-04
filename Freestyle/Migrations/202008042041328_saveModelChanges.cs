namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saveModelChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Search", "ScoreLowerBound", c => c.Double(nullable: false));
            AddColumn("dbo.Search", "ScoreUpperBound", c => c.Double(nullable: false));
            AddColumn("dbo.Search", "GenreCountry", c => c.String());
            DropColumn("dbo.Search", "score");
            DropColumn("dbo.Search", "genreCounry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Search", "genreCounry", c => c.String());
            AddColumn("dbo.Search", "score", c => c.Double(nullable: false));
            DropColumn("dbo.Search", "GenreCountry");
            DropColumn("dbo.Search", "ScoreUpperBound");
            DropColumn("dbo.Search", "ScoreLowerBound");
        }
    }
}
