namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class str3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReservationCodes", "TotalPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReservationCodes", "TotalPrice");
        }
    }
}
