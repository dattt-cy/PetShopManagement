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
            if (string.IsNullOrWhiteSpace(type.Name))
                throw new ArgumentException("Tên loại không được để trống");

            if (GetAllTypes().Any(t => t.Name.Equals(type.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Loại đã tồn tại");

            _repo.Add(type);
        }

        public void Update(PetType type)
        {
            if (type.PetTypeId <= 0)
                throw new ArgumentException("ID không hợp lệ");

            if (string.IsNullOrWhiteSpace(type.Name))
                throw new ArgumentException("Tên loại không được để trống");

            if (GetAllTypes().Any(t => t.Name.Equals(type.Name, StringComparison.OrdinalIgnoreCase) && t.PetTypeId != type.PetTypeId))
                throw new InvalidOperationException("Tên loại đã tồn tại");

            _repo.Update(type);
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID không hợp lệ");

            _repo.Delete(id);
        }
    }
}
