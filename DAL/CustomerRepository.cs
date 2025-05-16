using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopApp.DTO;

namespace ShopPetManagement.DAL
{
    public class CustomerRepository: ICustomerRepository
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
                if (existing == null)
                    throw new InvalidOperationException("Không tìm thấy khách hàng để update.");

               
                existing.Name = customer.Name;
                existing.Address = customer.Address;
                existing.Phone = customer.Phone;

                ctx.SaveChanges();
            }
        }

        public void Delete(int customerId)
        {

            using (var ctx = new Model1())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                ctx.Configuration.LazyLoadingEnabled = false;
                var c = ctx.Customers.Find(customerId);
                if (c != null)
                {
                    ctx.Customers.Remove(c);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Khách hàng này đã bị xóa hoặc không tồn tại nữa.");
                }
            }
        }
        public Customer GetById(int customerId)
        {
            using (var ctx = new Model1())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                ctx.Configuration.LazyLoadingEnabled = false;

                return ctx.Customers.Find(customerId);
            }
        }
        public List<Customer> Search(string keyword)
        {
            using (var ctx = new Model1())
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return ctx.Customers.ToList();

                keyword = keyword.Trim().ToLower();

                return ctx.Customers
                          .Where(c =>
                              c.Name.ToLower().Contains(keyword) ||
                              (c.Address ?? "").ToLower().Contains(keyword) ||
                              (c.Phone ?? "").ToLower().Contains(keyword))
                          .ToList();
            }
        }
        public bool HasSales(int customerId)
        {
            using (var ctx = new Model1())
            {
                
                return ctx.Sales.Any(s => s.CustomerId == customerId);
            }
        }

    }
}
