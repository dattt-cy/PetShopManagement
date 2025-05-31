using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public interface IPetTypeRepository : ICrudRepository<PetType>
    {
      
        List<PetType> GetByName(string name);
    }
}
