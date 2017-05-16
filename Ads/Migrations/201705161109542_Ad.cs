namespace Ads.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Priority = c.Int(nullable: false),
                        StatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ads");
        }
    }
}
