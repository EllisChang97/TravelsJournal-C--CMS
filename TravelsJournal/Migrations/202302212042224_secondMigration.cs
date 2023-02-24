namespace TravelsJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CompanionDestinations", newName: "DestinationCompanions");
            DropPrimaryKey("dbo.DestinationCompanions");
            AddPrimaryKey("dbo.DestinationCompanions", new[] { "Destination_DestinationID", "Companion_CompanionID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DestinationCompanions");
            AddPrimaryKey("dbo.DestinationCompanions", new[] { "Companion_CompanionID", "Destination_DestinationID" });
            RenameTable(name: "dbo.DestinationCompanions", newName: "CompanionDestinations");
        }
    }
}
