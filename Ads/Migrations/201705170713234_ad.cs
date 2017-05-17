namespace Ads.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ads",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                        Priority = c.Int(nullable: false),
                        StatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stats", t => t.StatId, cascadeDelete: true)
                .Index(t => t.StatId);
            
            CreateTable(
                "dbo.Stats",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ads", "StatId", "dbo.Stats");
            DropIndex("dbo.Ads", new[] { "StatId" });
            DropTable("dbo.Stats");
            DropTable("dbo.Ads");
        }
    }
}
