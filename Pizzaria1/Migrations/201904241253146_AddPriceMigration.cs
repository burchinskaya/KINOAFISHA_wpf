namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dishes", "Price", c => c.Single(nullable: false));
            AddColumn("dbo.Dishes", "Photo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dishes", "Photo");
            DropColumn("dbo.Dishes", "Price");
        }
    }
}
