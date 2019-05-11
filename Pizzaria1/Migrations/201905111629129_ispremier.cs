namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ispremier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "IsPremiere", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Films", "IsPremiere");
        }
    }
}
