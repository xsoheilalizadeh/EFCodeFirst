namespace EFCodeFirst.Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modified_Id1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.People", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.People");
            AlterColumn("dbo.People", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.People", "Id");
        }
    }
}
