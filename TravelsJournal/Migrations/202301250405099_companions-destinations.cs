namespace TravelsJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companionsdestinations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companions",
                c => new
                    {
                        CompanionID = c.Int(nullable: false, identity: true),
                        CompanionFirstName = c.String(),
                        CompanionLastName = c.String(),
                    })
                .PrimaryKey(t => t.CompanionID);
            
            CreateTable(
                "dbo.CompanionDestinations",
                c => new
                    {
                        Companion_CompanionID = c.Int(nullable: false),
                        Destination_DestinationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Companion_CompanionID, t.Destination_DestinationID })
                .ForeignKey("dbo.Companions", t => t.Companion_CompanionID, cascadeDelete: true)
                .ForeignKey("dbo.Destinations", t => t.Destination_DestinationID, cascadeDelete: true)
                .Index(t => t.Companion_CompanionID)
                .Index(t => t.Destination_DestinationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanionDestinations", "Destination_DestinationID", "dbo.Destinations");
            DropForeignKey("dbo.CompanionDestinations", "Companion_CompanionID", "dbo.Companions");
            DropIndex("dbo.CompanionDestinations", new[] { "Destination_DestinationID" });
            DropIndex("dbo.CompanionDestinations", new[] { "Companion_CompanionID" });
            DropTable("dbo.CompanionDestinations");
            DropTable("dbo.Companions");
        }
    }
}
