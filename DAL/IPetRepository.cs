using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public interface IPetRepository : ICrudRepository<Pet>
    {

        List<Pet> Search(string keyword);

     
        bool HasSales(int petId);

        int CountByCategories(params string[] categoryNames);
    }
}
