namespace InventoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class partStatus1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parts", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parts", "Status");
        }
    }
}
