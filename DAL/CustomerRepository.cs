using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopApp.DTO;

namespace ShopPetManagement.DAL
{
    public class CustomerRepository
    {
        public List<Customer> GetAll()
        {
            using (var ctx = new Model1())
            {
                return ctx.Customers
                          .OrderBy(c => c.CustomerId)
                          .ToList();
            }
        }

        public void Add(Customer customer)
        {
            using (var ctx = new Model1())
            {
                ctx.Customers.Add(customer);
                ctx.SaveChanges();
            }
        }

        public void Update(Customer customer)
        {
            using (var ctx = new Model1())
            {
                var existing = ctx.Customers.Find(customer.CustomerId);
                if (existing != null)
                {
                    ctx.Entry(existing).CurrentValues.SetValues(customer);
                    ctx.SaveChanges();
                }
            }
        }

        public void Delete(int customerId)
        {
            using (var ctx = new Model1())
            {
                var c = ctx.Customers.Find(customerId);
                if (c != null)
                {
                    ctx.Customers.Remove(c);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
