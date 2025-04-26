using System;
using System.Collections.Generic;
using System.Linq;
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
                    (u.Address ?? "").ToLower().Contains(searchKey) ||
                    (u.Phone ?? "").ToLower().Contains(searchKey) ||
                    u.Role.ToLower().Contains(searchKey) ||
                    u.DateOfBirth.ToString("yyyy-MM-dd").Contains(searchKey)
                )
                .ToList();
        }

        public void Add(UserAccount user)
        {
            _repo.Add(user);
        }

    
        public void Update(UserAccount user)
        {
            _repo.Update(user);
        }

        public void Delete(int userAccountId)
        {
            _repo.Delete(userAccountId);
        }
    }
}
