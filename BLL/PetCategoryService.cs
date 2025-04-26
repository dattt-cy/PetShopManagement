using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;

namespace ShopPetManagement.BLL
{
    public class PetCategoryService
    {
        private readonly PetCategoryRepository _repo = new PetCategoryRepository();

        public List<PetCategory> GetAllCategories()
        {
            return _repo.GetAll();
        }

        public void Add(PetCategory category)
        {
            _repo.Add(category);
        }

        public void Update(PetCategory category)
        {
            _repo.Update(category);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
