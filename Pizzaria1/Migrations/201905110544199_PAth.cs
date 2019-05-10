namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PAth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PathForTickets", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PathForTickets");
        }
    }
}
