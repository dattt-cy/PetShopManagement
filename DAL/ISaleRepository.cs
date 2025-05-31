using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public interface ISaleRepository : ICrudRepository<Sale>
    {
      

      
        int CreateSaleWithDetails(int customerId, int cashierId, List<(int petId, int quantity, decimal unitPrice)> products);

     
        decimal GetRevenueByDate(DateTime date);

    
        decimal GetRevenueByDateAndCashier(DateTime date, int cashierId);
    }
}
