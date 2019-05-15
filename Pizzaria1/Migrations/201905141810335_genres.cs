namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genres : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilmsGenres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilmId = c.Int(),
                        GenreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Films", t => t.FilmId)
                .ForeignKey("dbo.Genres", t => t.GenreId)
                .Index(t => t.FilmId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilmsGenres", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.FilmsGenres", "FilmId", "dbo.Films");
            DropIndex("dbo.FilmsGenres", new[] { "GenreId" });
            DropIndex("dbo.FilmsGenres", new[] { "FilmId" });
            DropTable("dbo.Genres");
            DropTable("dbo.FilmsGenres");
        }
    }
}
