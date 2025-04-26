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
    }
}
