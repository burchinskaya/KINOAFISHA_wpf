namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnsMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "Country", c => c.String());
            AddColumn("dbo.Films", "RatingIMDb", c => c.Single(nullable: false));
            AddColumn("dbo.Films", "RatingKinopoisk", c => c.Single(nullable: false));
            AddColumn("dbo.Films", "Slogan", c => c.String());
            AddColumn("dbo.Films", "Genre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Films", "Genre");
            DropColumn("dbo.Films", "Slogan");
            DropColumn("dbo.Films", "RatingKinopoisk");
            DropColumn("dbo.Films", "RatingIMDb");
            DropColumn("dbo.Films", "Country");
        }
    }
}
