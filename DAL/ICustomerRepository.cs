using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopApp.DTO;

namespace ShopPetManagement.DAL
{
    public interface ICustomerRepository : ICrudRepository<Customer>
    {
        List<Customer> Search(string keyword);
    }
}
