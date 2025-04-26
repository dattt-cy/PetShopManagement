using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;

namespace ShopPetManagement.BLL
{
    public class PetService
    {
        private readonly PetRepository _repo = new PetRepository();

        public List<Pet> GetAllPets()
        {
            return _repo.GetAll();
        }

        public List<Pet> Search(string keyword)
        {
            var list = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(keyword))
                return list;

            keyword = keyword.Trim().ToLower();
            return list
                .Where(p =>
                    p.PCode.ToLower().Contains(keyword) ||
                    p.Name.ToLower().Contains(keyword) ||
                    p.Type.Name.ToLower().Contains(keyword) ||
                    p.Category.Name.ToLower().Contains(keyword)
                )
                .ToList();
        }

        public void Add(Pet pet)
        {
            _repo.Add(pet);
        }

        public void Update(Pet pet)
        {
            _repo.Update(pet);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
