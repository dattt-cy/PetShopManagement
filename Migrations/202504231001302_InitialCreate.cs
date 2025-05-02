namespace ShopPetManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cashiers",
                c => new
                    {
                        CashierId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CashierId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        SaleDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        CashierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleId)
                .ForeignKey("dbo.Cashiers", t => t.CashierId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.CashierId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.SaleDetails",
                c => new
                    {
                        SaleDetailId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        PetId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SaleDetailId)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId)
                .Index(t => t.PetId);
            
            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        PetId = c.Int(nullable: false, identity: true),
                        PCode = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        PetTypeId = c.Int(nullable: false),
                        PetCategoryId = c.Int(nullable: false),
                        StockQty = c.Int(nullable: false),
                        SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PetId)
                .ForeignKey("dbo.PetCategories", t => t.PetCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.PetTypes", t => t.PetTypeId, cascadeDelete: true)
                .Index(t => t.PetTypeId)
                .Index(t => t.PetCategoryId);
            
            CreateTable(
                "dbo.PetCategories",
                c => new
                    {
                        PetCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.PetCategoryId);
            
            CreateTable(
                "dbo.PetTypes",
                c => new
                    {
                        PetTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.PetTypeId);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserAccountId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 20),
                        Role = c.String(nullable: false, maxLength: 50),
                        DateOfBirth = c.DateTime(nullable: false),
                        Password = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.UserAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleDetails", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.SaleDetails", "PetId", "dbo.Pets");
            DropForeignKey("dbo.Pets", "PetTypeId", "dbo.PetTypes");
            DropForeignKey("dbo.Pets", "PetCategoryId", "dbo.PetCategories");
            DropForeignKey("dbo.Sales", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Sales", "CashierId", "dbo.Cashiers");
            DropIndex("dbo.Pets", new[] { "PetCategoryId" });
            DropIndex("dbo.Pets", new[] { "PetTypeId" });
            DropIndex("dbo.SaleDetails", new[] { "PetId" });
            DropIndex("dbo.SaleDetails", new[] { "SaleId" });
            DropIndex("dbo.Sales", new[] { "CashierId" });
            DropIndex("dbo.Sales", new[] { "CustomerId" });
            DropTable("dbo.UserAccounts");
            DropTable("dbo.PetTypes");
            DropTable("dbo.PetCategories");
            DropTable("dbo.Pets");
            DropTable("dbo.SaleDetails");
            DropTable("dbo.Customers");
            DropTable("dbo.Sales");
            DropTable("dbo.Cashiers");
        }
    }
}
