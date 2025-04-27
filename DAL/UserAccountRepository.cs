using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopApp.DTO;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public class UserAccountRepository
    {
        public List<UserAccount> GetAll()
        {
            using (var ctx = new Model1())
            {
                return ctx.UserAccounts
                          .OrderBy(u => u.UserAccountId)
                          .ToList();
            }
        }
        public void Delete(int userAccountId)
        {
            using (var ctx = new Model1())
            {
                var u = ctx.UserAccounts.Find(userAccountId);
                if (u != null)
                {
                    ctx.UserAccounts.Remove(u);
                    ctx.SaveChanges();
                }
            }
        }
        public void Add(UserAccount user)
        {
            using (var ctx = new Model1())
            {
                ctx.UserAccounts.Add(user);
                ctx.SaveChanges();
            }
        }

        public void Update(UserAccount user)
        {
            using (var ctx = new Model1())
            {
                var existing = ctx.UserAccounts.Find(user.UserAccountId);
                if (existing != null)
                {
                    ctx.Entry(existing).CurrentValues.SetValues(user);
                    ctx.SaveChanges();
                }
            }
        }
        public int GetUserIdByUsername(string username)
        {
            using (var ctx = new Model1())
            {
                var user = ctx.UserAccounts
                              .FirstOrDefault(u => u.Name == username);
                return user?.UserAccountId ?? 0;
            }
        }
        public UserAccount GetById(int userAccountId)
        {
            using (var ctx = new Model1())
            {
                return ctx.UserAccounts.Find(userAccountId);
            }
        }

    }
}
