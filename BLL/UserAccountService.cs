using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;

namespace ShopPetManagement.BLL
{
    public class UserAccountService
    {
        private readonly UserAccountDAL _dal = new UserAccountDAL();

        public UserAccount Authenticate(string username, string password)
        {
            return _dal.GetByUsernameAndPassword(username, password);
        }
    }
}
