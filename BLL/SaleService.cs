using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            return _saleRepo.CreateSaleWithDetails(customerId, cashierId, products);
        }
        public decimal GetDailyRevenue(DateTime date)
        {
            return _saleRepo.GetRevenueByDate(date);
        }
        public decimal GetDailyRevenueByCashier(DateTime date, int cashierId)
        {
            return _saleRepo.GetRevenueByDateAndCashier(date, cashierId);
        }

    }
}

