using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopApp.DTO;
using ShopPetManagement.DAL;

namespace ShopPetManagement.BLL
{
    public class CustomerService
    {
        private readonly CustomerRepository _repo = new CustomerRepository();

        public List<Customer> GetAllCustomers()
        {
            return _repo.GetAll();
        }

        public List<Customer> Search(string keyword)
        {
            var list = _repo.GetAll();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return list;
            }

            keyword = keyword.Trim().ToLower();

            return list
                .Where(c =>
                    c.Name.ToLower().Contains(keyword) ||
                    (c.Address ?? "").ToLower().Contains(keyword) ||
                    (c.Phone ?? "").ToLower().Contains(keyword))
                .ToList();
        }

        public void Add(Customer customer)
        {
            _repo.Add(customer);
        }

        public void Update(Customer customer)
        {
            _repo.Update(customer);
        }

        public void Delete(int customerId)
        {
            _repo.Delete(customerId);
        }
    }
}
