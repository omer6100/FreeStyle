namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class artistModelChange2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Artist", "OriginCountry", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artist", "OriginCountry", c => c.String(nullable: false));
        }
    }
}
