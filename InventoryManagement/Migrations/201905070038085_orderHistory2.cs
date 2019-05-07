namespace InventoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderHistory2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderHistory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderHistory");
        }
    }
}
