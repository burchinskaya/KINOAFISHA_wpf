namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PosterByte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "PosterByte", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Films", "PosterByte");
        }
    }
}
