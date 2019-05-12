namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class soldplaces : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoldPlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Range = c.Int(nullable: false),
                        Place = c.Int(nullable: false),
                        FilmDateSeanceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FilmsDatesSeances", t => t.FilmDateSeanceId)
                .Index(t => t.FilmDateSeanceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoldPlaces", "FilmDateSeanceId", "dbo.FilmsDatesSeances");
            DropIndex("dbo.SoldPlaces", new[] { "FilmDateSeanceId" });
            DropTable("dbo.SoldPlaces");
        }
    }
}
