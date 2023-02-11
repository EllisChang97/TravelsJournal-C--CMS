namespace TravelsJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingID = c.Int(nullable: false, identity: true),
                        RatingDescription = c.String(),
                    })
                .PrimaryKey(t => t.RatingID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ratings");
        }
    }
}
