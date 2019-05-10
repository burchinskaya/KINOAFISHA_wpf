namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrailerMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "Trailer", c => c.String());
            AlterColumn("dbo.Films", "Poster", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Films", "Poster", c => c.Binary());
            DropColumn("dbo.Films", "Trailer");
        }
    }
}
