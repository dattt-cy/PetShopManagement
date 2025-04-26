using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public class PetTypeRepository
    {
        public List<PetType> GetAll()
        {
            using (var ctx = new Model1())
            {
                return ctx.PetTypes
                          .OrderBy(t => t.PetTypeId)
                          .ToList();
            }
        }

        public void Add(PetType type)
        {
            using (var ctx = new Model1())
            {
                ctx.PetTypes.Add(type);
                ctx.SaveChanges();
            }
        }

        public void Update(PetType type)
        {
            using (var ctx = new Model1())
            {
                var existing = ctx.PetTypes.Find(type.PetTypeId);
                if (existing != null)
                {
                    ctx.Entry(existing).CurrentValues.SetValues(type);
                    ctx.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new Model1())
            {
                var t = ctx.PetTypes.Find(id);
                if (t != null)
                {
                    ctx.PetTypes.Remove(t);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
