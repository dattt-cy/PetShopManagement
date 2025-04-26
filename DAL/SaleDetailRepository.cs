using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public class SaleDetailRepository
    {
        public void Add(SaleDetail detail)
        {
            using (var context = new Model1())
            {
                context.SaleDetails.Add(detail);
                context.SaveChanges();
            }
        }

        public List<SaleDetail> GetBySaleId(int saleId)
        {
            using (var context = new Model1())
            {
                return context.SaleDetails
                              .Where(d => d.SaleId == saleId)
                              .ToList();
            }
        }
    }
}
