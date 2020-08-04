namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yos1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Search",
                c => new
                    {
                        type = c.String(nullable: false, maxLength: 128),
                        primaryName = c.String(),
                        secondaryName = c.String(),
                        score = c.Double(nullable: false),
                        genreCounry = c.String(),
                    })
                .PrimaryKey(t => t.type);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Search");
        }
    }
}
