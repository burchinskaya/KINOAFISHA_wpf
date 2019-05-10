namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updating3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReservationCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        UserId = c.Int(),
                        FilmDateSeanceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FilmsDatesSeances", t => t.FilmDateSeanceId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.FilmDateSeanceId);
            
            CreateTable(
                "dbo.ReservationPlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Range = c.Int(nullable: false),
                        Place = c.Int(nullable: false),
                        CodeId = c.Int(),
                        ReservationCode_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReservationCodes", t => t.ReservationCode_Id)
                .Index(t => t.ReservationCode_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReservationPlaces", "ReservationCode_Id", "dbo.ReservationCodes");
            DropForeignKey("dbo.ReservationCodes", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReservationCodes", "FilmDateSeanceId", "dbo.FilmsDatesSeances");
            DropIndex("dbo.ReservationPlaces", new[] { "ReservationCode_Id" });
            DropIndex("dbo.ReservationCodes", new[] { "FilmDateSeanceId" });
            DropIndex("dbo.ReservationCodes", new[] { "UserId" });
            DropTable("dbo.ReservationPlaces");
            DropTable("dbo.ReservationCodes");
        }
    }
}
