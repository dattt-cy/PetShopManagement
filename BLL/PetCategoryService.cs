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
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Tên loại không được để trống");

            if (GetAllCategories().Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Loại đã tồn tại");

            _repo.Add(category);
        }

        public void Update(PetCategory category)
        {
            if (category.PetCategoryId <= 0)
                throw new ArgumentException("ID không hợp lệ");

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Tên loại không được để trống");

            if (GetAllCategories().Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase) && c.PetCategoryId != category.PetCategoryId))
                throw new InvalidOperationException("Tên loại đã tồn tại");

            _repo.Update(category);
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID không hợp lệ");

            _repo.Delete(id);
        }
    }
}
