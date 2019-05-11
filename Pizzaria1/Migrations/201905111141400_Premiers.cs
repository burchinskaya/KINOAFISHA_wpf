namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Premiers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FuturePremiers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        PosterByte = c.Binary(),
                        Trailer = c.String(),
                        Country = c.String(),
                        Slogan = c.String(),
                        PremierDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FilmsGenres", "FuturePremier_Id", c => c.Int());
            CreateIndex("dbo.FilmsGenres", "FuturePremier_Id");
            AddForeignKey("dbo.FilmsGenres", "FuturePremier_Id", "dbo.FuturePremiers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilmsGenres", "FuturePremier_Id", "dbo.FuturePremiers");
            DropIndex("dbo.FilmsGenres", new[] { "FuturePremier_Id" });
            DropColumn("dbo.FilmsGenres", "FuturePremier_Id");
            DropTable("dbo.FuturePremiers");
        }
    }
}
