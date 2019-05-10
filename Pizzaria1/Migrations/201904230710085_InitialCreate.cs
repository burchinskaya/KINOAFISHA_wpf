namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilmsDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilmId = c.Int(),
                        DateId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dates", t => t.DateId)
                .ForeignKey("dbo.Films", t => t.FilmId)
                .Index(t => t.FilmId)
                .Index(t => t.DateId);
            
            CreateTable(
                "dbo.Films",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Poster = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilmsDatesSeances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilmsDatesId = c.Int(),
                        SeanceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FilmsDates", t => t.FilmsDatesId)
                .ForeignKey("dbo.Seances", t => t.SeanceId)
                .Index(t => t.FilmsDatesId)
                .Index(t => t.SeanceId);
            
            CreateTable(
                "dbo.Seances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilmsDatesSeances", "SeanceId", "dbo.Seances");
            DropForeignKey("dbo.FilmsDatesSeances", "FilmsDatesId", "dbo.FilmsDates");
            DropForeignKey("dbo.FilmsDates", "FilmId", "dbo.Films");
            DropForeignKey("dbo.FilmsDates", "DateId", "dbo.Dates");
            DropIndex("dbo.FilmsDatesSeances", new[] { "SeanceId" });
            DropIndex("dbo.FilmsDatesSeances", new[] { "FilmsDatesId" });
            DropIndex("dbo.FilmsDates", new[] { "DateId" });
            DropIndex("dbo.FilmsDates", new[] { "FilmId" });
            DropTable("dbo.Users");
            DropTable("dbo.Seances");
            DropTable("dbo.FilmsDatesSeances");
            DropTable("dbo.Films");
            DropTable("dbo.FilmsDates");
            DropTable("dbo.Dates");
            DropTable("dbo.Admins");
        }
    }
}
