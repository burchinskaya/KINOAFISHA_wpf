namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceForPlace : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReservationPlaces", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReservationPlaces", "Price");
        }
    }
}
