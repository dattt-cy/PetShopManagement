using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAO;

namespace ShopPetManagement.DAL
{
    public class UserAccountDAL
    {
        public UserAccount GetByUsernameAndPassword(string username, string password)
        {
            using (var ctx = new Model1())
            {
                return ctx.UserAccounts
                          .FirstOrDefault(u => u.Name == username
                                            && u.Password == password);
            }
        }
    }
}
