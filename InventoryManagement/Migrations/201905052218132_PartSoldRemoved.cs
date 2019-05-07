namespace InventoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartSoldRemoved : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Parts", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Parts", "Status", c => c.Int(nullable: false));
        }
    }
}
