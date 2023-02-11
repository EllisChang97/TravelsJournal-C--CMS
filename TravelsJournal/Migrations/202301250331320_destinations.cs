namespace TravelsJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class destinations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        DestinationID = c.Int(nullable: false, identity: true),
                        DestinationName = c.String(),
                        DestinationSummary = c.String(),
                    })
                .PrimaryKey(t => t.DestinationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Destinations");
        }
    }
}
