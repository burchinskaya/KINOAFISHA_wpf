namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class str2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReservationPlaces", "Student", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReservationPlaces", "Retiree", c => c.Boolean(nullable: false));
            DropColumn("dbo.SoldPlaces", "Student");
            DropColumn("dbo.SoldPlaces", "Retiree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoldPlaces", "Retiree", c => c.Boolean(nullable: false));
            AddColumn("dbo.SoldPlaces", "Student", c => c.Boolean(nullable: false));
            DropColumn("dbo.ReservationPlaces", "Retiree");
            DropColumn("dbo.ReservationPlaces", "Student");
        }
    }
}
