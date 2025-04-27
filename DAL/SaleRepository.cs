using ShopPetManagement.DAO;
using System.Collections.Generic;
using System.Linq;

namespace ShopPetManagement.DAL
{
    public class SaleRepository
    {
        /// <summary>
        /// Tạo hoá đơn và chi tiết hoá đơn trong cùng 1 DbContext / transaction.
        /// EF sẽ tự động insert Sale trước, gán SaleId, rồi mới insert SaleDetail.
        /// </summary>
        public int CreateSaleWithDetails(Sale sale, IEnumerable<SaleDetail> details)
        {
            using (var ctx = new Model1())
            {
                // gán collection Details vào sale
                sale.Details = details.ToList();

                // thêm sale (kèm details)
                ctx.Sales.Add(sale);

                // một lần SaveChanges duy nhất
                ctx.SaveChanges();

                return sale.SaleId;
            }
        }

        // bạn có thể giữ lại Add / GetAll nếu cần
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
    }
}
