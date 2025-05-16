using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{

    public class PetRepository
    {
        public List<Pet> GetAll()
        {
            using (var ctx = new Model1())
            {
                return ctx.Pets
                           .Include("Type")
                           .Include("Category")
                           .OrderBy(p => p.PetId)
                           .ToList();
            }
        }
        public Pet GetById(int id)
        {
            using (var ctx = new Model1())
            {
                return ctx.Pets
                          .Include("Type")
                          .Include("Category")
                          .FirstOrDefault(p => p.PetId == id);
            }
        }

        public void Add(Pet pet)
        {
            using (var ctx = new Model1())
            {
                ctx.Pets.Add(pet);
                ctx.SaveChanges();
            }
        }

        public void Update(Pet pet)
        {
            using (var ctx = new Model1())
            {
                var existing = ctx.Pets.Find(pet.PetId);
                if (existing != null)
                {
                    ctx.Entry(existing).CurrentValues.SetValues(pet);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Mặt hàng này đã bị xóa hoặc không tồn tại nữa.");
                }
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new Model1())
            {
                var p = ctx.Pets.Find(id);
                if (p != null)
                {
                    ctx.Pets.Remove(p);
                    ctx.SaveChanges();
                }
            }
        }
        public List<Pet> Search(string keyword)
        {
            using (var ctx = new Model1())
            {
                var query = ctx.Pets
                               .Include("Type")
                               .Include("Category")
                               .AsQueryable();
                    
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(p =>
                        p.PCode.ToLower().Contains(keyword) ||
                        p.Name.ToLower().Contains(keyword) ||
                        p.Type.Name.ToLower().Contains(keyword) ||
                        p.Category.Name.ToLower().Contains(keyword)
                    );
                }
                return query.ToList();
            }
        }

        public bool HasSales(int petId)
        {
            using (var ctx = new Model1())
            {
                
                return ctx.SaleDetails.Any(sd => sd.PetId == petId);
            }
        }
    }
}
