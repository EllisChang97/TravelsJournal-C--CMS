namespace TravelsJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class destinationrating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Destinations", "RatingID", c => c.Int(nullable: false));
            CreateIndex("dbo.Destinations", "RatingID");
            AddForeignKey("dbo.Destinations", "RatingID", "dbo.Ratings", "RatingID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Destinations", "RatingID", "dbo.Ratings");
            DropIndex("dbo.Destinations", new[] { "RatingID" });
            DropColumn("dbo.Destinations", "RatingID");
        }
    }
}
