namespace KINOwpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subcriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        FilmId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Films", t => t.FilmId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.FilmId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Subscriptions", "FilmId", "dbo.Films");
            DropIndex("dbo.Subscriptions", new[] { "FilmId" });
            DropIndex("dbo.Subscriptions", new[] { "UserId" });
            DropTable("dbo.Subscriptions");
        }
    }
}
