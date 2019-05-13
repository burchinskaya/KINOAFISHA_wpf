namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SoldPlaces", "Student");
            DropColumn("dbo.SoldPlaces", "Retiree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoldPlaces", "Retiree", c => c.Boolean(nullable: false));
            AddColumn("dbo.SoldPlaces", "Student", c => c.Boolean(nullable: false));
        }
    }
}
