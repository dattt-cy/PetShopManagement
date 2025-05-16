using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;

namespace ShopPetManagement.BLL
{
    public class UserService
    {
        private readonly UserAccountRepository _repo = new UserAccountRepository();

        public List<UserAccount> GetAllUser()
        {
            return _repo.GetAll();
        }

        public List<UserAccount> GetUsers(string searchKey)
        {
            var list = _repo.GetAll();
            if (string.IsNullOrWhiteSpace(searchKey))
                return list;

            searchKey = searchKey.Trim().ToLower();
            return list
                .Where(u =>
                    u.Name.ToLower().Contains(searchKey) ||
                    (u.Address ?? string.Empty).ToLower().Contains(searchKey) ||
                    (u.Phone ?? string.Empty).ToLower().Contains(searchKey) ||
                    u.Role.ToLower().Contains(searchKey) ||
                    u.DateOfBirth.ToString("yyyy-MM-dd").Contains(searchKey)
                )
                .ToList();
        }

        public void Add(UserAccount user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                throw new ArgumentException("Tên đăng nhập không được để trống");
            if (user.Name.Length < 3 || user.Name.Length > 50)
                throw new ArgumentException("Tên đăng nhập phải từ 3 đến 50 ký tự");
            if (_repo.GetAll()
                     .Any(u => u.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại");

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự");

            if (string.IsNullOrWhiteSpace(user.Name))
                throw new ArgumentException("Họ tên không được để trống");

            if (user.DateOfBirth > DateTime.Now)
                throw new ArgumentException("Ngày sinh không được lớn hơn ngày hiện tại");

            if (!string.IsNullOrWhiteSpace(user.Phone) &&
                !Regex.IsMatch(user.Phone, @"^\d{9,12}$"))
                throw new ArgumentException("Số điện thoại phải là 9–12 chữ số");

            if (string.IsNullOrWhiteSpace(user.Role))
                throw new ArgumentException("Role không được để trống");

            _repo.Add(user);
        }

        public void Update(UserAccount user)
        {
            if (user.UserAccountId <= 0)
                throw new ArgumentException("UserAccountId không hợp lệ");

            var existing = _repo.GetById(user.UserAccountId);
            if (existing == null)
                throw new InvalidOperationException("Người dùng không tồn tại");

            if (!existing.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (_repo.GetAll()
                         .Any(u => u.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException("Tên đăng nhập đã tồn tại");
            }

            if (string.IsNullOrWhiteSpace(user.Name))
                throw new ArgumentException("Họ tên không được để trống");

            if (user.DateOfBirth > DateTime.Now)
                throw new ArgumentException("Ngày sinh không được lớn hơn ngày hiện tại");

            if (!string.IsNullOrWhiteSpace(user.Phone) &&
                !Regex.IsMatch(user.Phone, @"^\d{9,12}$"))
                throw new ArgumentException("Số điện thoại phải là 9–12 chữ số");

            if (string.IsNullOrWhiteSpace(user.Role))
                throw new ArgumentException("Role không được để trống");

            _repo.Update(user);
        }

        public void Delete(int userAccountId)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("UserAccountId không hợp lệ");

            _repo.Delete(userAccountId);
        }

        public int GetUserIdByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username không được để trống");

            return _repo.GetUserIdByUsername(username);
        }

        public UserAccount GetById(int userAccountId)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("UserAccountId không hợp lệ");

            return _repo.GetById(userAccountId);
        }
        public bool HasSales(int cashierId)
        {
            return _repo.HasSales(cashierId);
        }
    }
}
