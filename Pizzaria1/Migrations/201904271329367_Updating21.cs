namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updating21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReservationCodes", "FilmDateSeanceId", "dbo.FilmsDatesSeances");
            DropForeignKey("dbo.ReservationCodes", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReservationPlaces", "ReservationCode_Id", "dbo.ReservationCodes");
            DropIndex("dbo.ReservationCodes", new[] { "UserId" });
            DropIndex("dbo.ReservationCodes", new[] { "FilmDateSeanceId" });
            DropIndex("dbo.ReservationPlaces", new[] { "ReservationCode_Id" });

        
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReservationCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        UserId = c.Int(),
                        FilmDateSeanceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ReservationPlaces", "ReservationCode_Id");
            CreateIndex("dbo.ReservationCodes", "FilmDateSeanceId");
            CreateIndex("dbo.ReservationCodes", "UserId");
            AddForeignKey("dbo.ReservationPlaces", "ReservationCode_Id", "dbo.ReservationCodes", "Id");
            AddForeignKey("dbo.ReservationCodes", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.ReservationCodes", "FilmDateSeanceId", "dbo.FilmsDatesSeances", "Id");
        }
    }
}
