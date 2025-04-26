using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    internal class SaleRepository
    {
        public Sale Add(Sale sale)
        {
            using (var context = new Model1())
            {
                context.Sales.Add(sale);
                context.SaveChanges();
                return sale;
            }
        }

        public List<Sale> GetAll()
        {
            using (var context = new Model1())
            {
                return context.Sales.ToList();
            }
        }
    }
}
