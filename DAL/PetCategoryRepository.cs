using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public class PetCategoryRepository
    {
        public List<PetCategory> GetAll()
        {
            using (var ctx = new Model1())
            {
                return ctx.PetCategories
                          .OrderBy(c => c.PetCategoryId)
                          .ToList();
            }
        }

        public void Add(PetCategory category)
        {
            using (var ctx = new Model1())
            {
                ctx.PetCategories.Add(category);
                ctx.SaveChanges();
            }
        }

        public void Update(PetCategory category)
        {
            using (var ctx = new Model1())
            {
                var existing = ctx.PetCategories.Find(category.PetCategoryId);
                if (existing != null)
                {
                    ctx.Entry(existing).CurrentValues.SetValues(category);
                    ctx.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new Model1())
            {
                var c = ctx.PetCategories.Find(id);
                if (c != null)
                {
                    ctx.PetCategories.Remove(c);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Category này đã bị xóa hoặc không tồn tại nữa.");
                }
            }
        }
    }
}
