namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlbumModelChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Album", "Artist_Id", "dbo.Artist");
            DropIndex("dbo.Album", new[] { "Artist_Id" });
            AddColumn("dbo.Album", "Artist", c => c.String());
            DropColumn("dbo.Album", "Artist_Id");
            DropTable("dbo.Artist");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Artist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Birthday = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Album", "Artist_Id", c => c.Int());
            DropColumn("dbo.Album", "Artist");
            CreateIndex("dbo.Album", "Artist_Id");
            AddForeignKey("dbo.Album", "Artist_Id", "dbo.Artist", "Id");
        }
    }
}
