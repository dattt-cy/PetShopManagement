using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            return _repo.Search(keyword);
        }

        public void Add(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("Tên khách hàng không được để trống.");

          
            if (customer.Name.Length > 100)
                throw new ArgumentException("Tên khách hàng không dài quá 100 ký tự.");

           
            if (string.IsNullOrWhiteSpace(customer.Address) || customer.Address.Length < 5)
                throw new ArgumentException("Địa chỉ phải có ít nhất 5 ký tự.");

            if (string.IsNullOrWhiteSpace(customer.Phone) ||
                !Regex.IsMatch(customer.Phone, @"^\d{9,12}$"))
                throw new ArgumentException("Số điện thoại phải là 9–12 chữ số.");

            var exists = _repo.GetAll()
                              .Any(c => c.Phone == customer.Phone);
            if (exists)
                throw new InvalidOperationException("Số điện thoại này đã tồn tại.");

            _repo.Add(customer);
        }

        public void Update(Customer customer)
        {
            if (customer.CustomerId <= 0)
                throw new ArgumentException("CustomerId không hợp lệ.");

            var existing = _repo.GetById(customer.CustomerId);
            if (existing == null)
                throw new InvalidOperationException("Khách hàng không tồn tại.");

           
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("Tên khách hàng không được để trống.");
            if (customer.Name.Length > 100)
                throw new ArgumentException("Tên khách hàng không dài quá 100 ký tự.");
            if (string.IsNullOrWhiteSpace(customer.Address) || customer.Address.Length < 5)
                throw new ArgumentException("Địa chỉ phải có ít nhất 5 ký tự.");
            if (string.IsNullOrWhiteSpace(customer.Phone) ||
                !Regex.IsMatch(customer.Phone, @"^\d{9,12}$"))
                throw new ArgumentException("Số điện thoại phải là 9–12 chữ số.");

            
            if (customer.Phone != existing.Phone &&
                _repo.GetAll().Any(c => c.Phone == customer.Phone))
            {
                throw new InvalidOperationException("Số điện thoại này đã tồn tại.");
            }
            _repo.Update(customer);

        }

        public void Delete(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("CustomerId không hợp lệ.");

            
            var hasSales = _repo.GetById(customerId)?.Sales.Any() ?? false;
            if (hasSales)
                throw new InvalidOperationException("Không thể xóa khách đã từng mua hàng.");

            _repo.Delete(customerId);
        }
        public Customer GetById(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("CustomerId không hợp lệ.");
            return _repo.GetById(customerId);
        }

        public bool hasSales(int customerId)
        {
            return _repo.HasSales(customerId);
        }
    }
}
