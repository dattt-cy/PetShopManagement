using System;
using System.Collections.Generic;
using System.Linq;
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;
namespace ShopPetManagement.BLL
{
    public class SaleService
    {
        private readonly SaleRepository _saleRepo = new SaleRepository();

        public int CreateSale(int customerId, int cashierId, List<(int petId, int quantity, decimal unitPrice)> products)
        {
            // 1) Chuẩn bị đối tượng Sale
            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                CustomerId = customerId,
                CashierId = cashierId
            };

            // 2) Chuyển sang list SaleDetail, không gán SaleId ở đây
            var details = products.Select(p => new SaleDetail
            {
                PetId = p.petId,
                Quantity = p.quantity,
                UnitPrice = p.unitPrice
            }).ToList();

            // 3) Gọi repo – EF sẽ xử lý insert sale trước, lấy SaleId rồi insert detail
            return _saleRepo.CreateSaleWithDetails(sale, details);
        }
    }
}

