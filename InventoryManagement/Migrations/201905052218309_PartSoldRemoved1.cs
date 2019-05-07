namespace InventoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartSoldRemoved1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Parts", "Sold");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Parts", "Sold", c => c.Boolean(nullable: false));
        }
    }
}
