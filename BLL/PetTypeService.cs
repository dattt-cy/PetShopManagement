using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;

namespace ShopPetManagement.BLL
{
    public class PetTypeService
    {
        private readonly PetTypeRepository _repo = new PetTypeRepository();

        public List<PetType> GetAllTypes()
        {
            return _repo.GetAll();
        }

        public void Add(PetType type)
        {
            _repo.Add(type);
        }

        public void Update(PetType type)
        {
            _repo.Update(type);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
