namespace InventoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "PhNum", c => c.String());
            AlterColumn("dbo.Customers", "PostCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "PostCode", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "PhNum", c => c.Int(nullable: false));
        }
    }
}
