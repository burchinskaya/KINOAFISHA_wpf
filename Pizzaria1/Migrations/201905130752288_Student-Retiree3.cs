namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentRetiree3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoldPlaces", "Student", c => c.Boolean(nullable: false));
            AddColumn("dbo.SoldPlaces", "Retiree", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReservationPlaces", "Retiree", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReservationPlaces", "Student", c => c.Boolean(nullable: false));
        }
    }
}
