using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;

namespace ShopPetManagement.BLL
{
    public class PetService
    {
        private readonly PetRepository _repo = new PetRepository();

        public Pet GetById(int petId)
        {
            if (petId <= 0)
                throw new ArgumentException("PetId không hợp lệ");
            var pet = _repo.GetById(petId);
            if (pet == null)
                throw new InvalidOperationException("Thú cưng không tồn tại");
            return pet;
        }

        public List<Pet> GetAllPets()
        {
            return _repo.GetAll();
        }

        public List<Pet> Search(string keyword)
        {
            return _repo.Search(keyword);
        }

        public void Add(Pet pet)
        {
            if (string.IsNullOrWhiteSpace(pet.Name))    
                throw new ArgumentException("Tên thú cưng không được để trống");
            if (pet.Name.Length > 100)
                throw new ArgumentException("Tên thú cưng không được vượt quá 100 ký tự");
            if (string.IsNullOrWhiteSpace(pet.PCode))
                throw new ArgumentException("Mã sản phẩm không được để trống");
      
            if (pet.SalePrice <= 0)
                throw new ArgumentException("Giá bán phải lớn hơn 0");
            if (pet.StockQty < 0)
                throw new ArgumentException("Số lượng tồn kho không được âm");
           

            _repo.Add(pet);
        }

        public void Update(Pet pet)
        {
            if (pet.PetId <= 0)
                throw new ArgumentException("PetId không hợp lệ");
            var existing = _repo.GetById(pet.PetId) ?? throw new InvalidOperationException("Thú cưng không tồn tại");
            if (string.IsNullOrWhiteSpace(pet.Name))
                throw new ArgumentException("Tên thú cưng không được để trống");
            if (pet.Name.Length > 100)
                throw new ArgumentException("Tên thú cưng không được vượt quá 100 ký tự");
            if (pet.SalePrice <= 0)
                throw new ArgumentException("Giá bán phải lớn hơn 0");
            if (pet.StockQty < 0)
                throw new ArgumentException("Số lượng tồn kho không được âm");
            if (!pet.PCode.Equals(existing.PCode, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(pet.PCode))
                    throw new ArgumentException("Mã sản phẩm không được để trống");
              
            }

            _repo.Update(pet);
        }

        public void Delete(int petId)
        {
            if (petId <= 0)
                throw new ArgumentException("PetId không hợp lệ");
            var pet = _repo.GetById(petId) ?? throw new InvalidOperationException("Thú cưng không tồn tại");
           

            _repo.Delete(petId);
        }

        public bool HasSales(int petId)
        {
           return _repo.HasSales(petId);
        }
    }
}
