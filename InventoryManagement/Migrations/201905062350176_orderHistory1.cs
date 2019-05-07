namespace InventoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderHistory1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "OrderHistory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OrderHistory", c => c.String());
        }
    }
}
