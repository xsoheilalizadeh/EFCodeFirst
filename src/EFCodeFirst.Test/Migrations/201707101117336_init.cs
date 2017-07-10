namespace EFCodeFirst.Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Description);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.People", new[] { "Description" });
            DropTable("dbo.People");
        }
    }
}
