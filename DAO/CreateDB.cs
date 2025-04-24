using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.DAO
{
    public class CreateDB:
        CreateDatabaseIfNotExists<Model1>
    {
        protected override void Seed(Model1 context)
        {

        }
    }
}
