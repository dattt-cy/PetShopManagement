namespace ShopPetManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class deleteCashier : DbMigration
    {
        public override void Up()
        {
            // 1) Xóa foreign-key từ Sales.CashierId trước
            DropForeignKey("dbo.Sales", "CashierId", "dbo.Cashiers");
            // 2) Xóa index (nếu có) trên cột CashierId
            DropIndex("dbo.Sales", new[] { "CashierId" });
            // 3) Bây giờ mới drop table Cashiers
            DropTable("dbo.Cashiers");
        }

        public override void Down()
        {
            // Nếu rollback, phải tạo lại table, index và FK
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
