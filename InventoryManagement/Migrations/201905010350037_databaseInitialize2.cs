namespace InventoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseInitialize2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Make = c.String(maxLength: 50),
                        Model = c.String(maxLength: 50),
                        Cost = c.Decimal(nullable: false, storeType: "money"),
                        Colour = c.String(maxLength: 50),
                        Chassis = c.String(maxLength: 50),
                        Year = c.Int(nullable: false),
                        SupplierID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID)
                .Index(t => t.SupplierID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        PhNum = c.Int(nullable: false),
                        StreetNum = c.String(maxLength: 10),
                        StreetName = c.String(maxLength: 50),
                        Region = c.String(maxLength: 50),
                        PostCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        PartID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Condition = c.String(maxLength: 50),
                        Area = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Sold = c.Boolean(nullable: false),
                        CarID = c.Int(),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PartID)
                .ForeignKey("dbo.Cars", t => t.CarID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.CarID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parts", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Parts", "CarID", "dbo.Cars");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Cars", "SupplierID", "dbo.Suppliers");
            DropIndex("dbo.Parts", new[] { "OrderID" });
            DropIndex("dbo.Parts", new[] { "CarID" });
            DropIndex("dbo.Orders", new[] { "CustomerID" });
            DropIndex("dbo.Cars", new[] { "SupplierID" });
            DropTable("dbo.Parts");
            DropTable("dbo.Orders");
            DropTable("dbo.Customers");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Cars");
        }
    }
}
