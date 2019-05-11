namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletepremiers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FilmsGenres", "FuturePremier_Id", "dbo.FuturePremiers");
            DropIndex("dbo.FilmsGenres", new[] { "FuturePremier_Id" });
            AddColumn("dbo.Films", "PremierDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.FilmsGenres", "FuturePremier_Id");
            DropTable("dbo.FuturePremiers");
        }
        
        public override void Down()
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
            DropColumn("dbo.Films", "PremierDate");
            CreateIndex("dbo.FilmsGenres", "FuturePremier_Id");
            AddForeignKey("dbo.FilmsGenres", "FuturePremier_Id", "dbo.FuturePremiers", "Id");
        }
    }
}
