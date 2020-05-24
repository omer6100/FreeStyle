namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Artist_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artist", t => t.Artist_Id)
                .Index(t => t.Artist_Id);
            
            CreateTable(
                "dbo.Artist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Birthday = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Album", "Artist_Id", "dbo.Artist");
            DropIndex("dbo.Album", new[] { "Artist_Id" });
            DropTable("dbo.Artist");
            DropTable("dbo.Album");
        }
    }
}
