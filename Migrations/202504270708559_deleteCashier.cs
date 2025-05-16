namespace ShopPetManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class deleteCashier : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sales", "CashierId", "dbo.Cashiers");
         
            DropIndex("dbo.Sales", new[] { "CashierId" });
          
            DropTable("dbo.Cashiers");
        }

        public override void Down()
        {
           
            CreateTable(
                "dbo.Cashiers",
                c => new
                {
                    CashierId = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                })
                .PrimaryKey(t => t.CashierId);

            CreateIndex("dbo.Sales", "CashierId");
            AddForeignKey("dbo.Sales", "CashierId", "dbo.Cashiers", "CashierId");
        }
    }

}
