using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public interface IPetCategoryRepository : ICrudRepository<PetCategory>
    {
        List<PetCategory> GetByName(string name);
    }
}
