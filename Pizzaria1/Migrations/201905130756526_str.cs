namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class str : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoldPlaces", "Student", c => c.Boolean(nullable: false));
            AddColumn("dbo.SoldPlaces", "Retiree", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoldPlaces", "Retiree");
            DropColumn("dbo.SoldPlaces", "Student");
        }
    }
}
