﻿namespace Freestyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class albumModelChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "Genre", c => c.String(nullable: false));
            AddColumn("dbo.Artist", "PageViews", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artist", "PageViews");
            DropColumn("dbo.Album", "Genre");
        }
    }
}
