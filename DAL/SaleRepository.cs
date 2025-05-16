using ShopPetManagement.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ShopPetManagement.DAL
{
    public class SaleRepository
    {
   
        public int CreateSaleWithDetails(Sale sale, IEnumerable<SaleDetail> details)
        {
            using (var ctx = new Model1())
            {
             
                sale.Details = details.ToList();

               
                ctx.Sales.Add(sale);

               
                ctx.SaveChanges();

                return sale.SaleId;
            }
        }

       
        public Sale Add(Sale sale)
        {
            using (var ctx = new Model1())
            {
                ctx.Sales.Add(sale);
                ctx.SaveChanges();
                return sale;
            }
        }

        public List<Sale> GetAll()
        {
            using (var ctx = new Model1())
            {
                return ctx.Sales.ToList();
            }
        }
        public int CreateSaleWithDetails(int customerId, int cashierId, List<(int petId, int quantity, decimal unitPrice)> products)
        {
            using (var ctx = new Model1())
            using (var tran = ctx.Database.BeginTransaction())
            {
                var sale = new Sale
                {
                    CustomerId = customerId,
                    CashierId = cashierId,
                    SaleDate = DateTime.Now
                };
                ctx.Sales.Add(sale);
                ctx.SaveChanges(); 

                foreach (var p in products)
                {
                    var detail = new SaleDetail
                    {
                        SaleId = sale.SaleId,
                        PetId = p.petId,
                        Quantity = p.quantity,
                        UnitPrice = p.unitPrice
                    };
                    ctx.SaleDetails.Add(detail);

                    var pet = ctx.Pets.SingleOrDefault(x => x.PetId == p.petId);
                    if (pet == null)
                        throw new InvalidOperationException($"Không tìm thấy pet với ID = {p.petId}");
                    pet.StockQty = Math.Max(0, pet.StockQty - p.quantity);
                    ctx.Entry(pet).State = EntityState.Modified;
                }

                ctx.SaveChanges();
                tran.Commit();
                return sale.SaleId;
            }
        }
        public decimal GetRevenueByDate(DateTime date)
        {
            using (var ctx = new Model1())
            {
              
                return ctx.Sales
                          .Where(s => DbFunctions.TruncateTime(s.SaleDate) == date.Date)
                          .SelectMany(s => s.Details)
                          .Sum(d => d.Quantity * d.UnitPrice);
            }
        }
        public decimal GetRevenueByDateAndCashier(DateTime date, int cashierId)
        {
            using (var ctx = new Model1())
            {
                return ctx.Sales
                          .Where(s =>
                              DbFunctions.TruncateTime(s.SaleDate) == date.Date
                              && s.CashierId == cashierId
                          )
                          .SelectMany(s => s.Details)
                          .Sum(d => d.Quantity * d.UnitPrice);
            }
        }

    }
}
