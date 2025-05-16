using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pet_Shop_Management_System;
using System.Data.Entity.Migrations;                      
using ShopPetManagement.Migrations;
using System.Data.Entity;
namespace ShopPetManagement
{
    internal static class Program
    {
   
        [STAThread]
          static void Main()
    {

            Database.SetInitializer(
                  new MigrateDatabaseToLatestVersion<Model1, Configuration>());


            using (var ctx = new Model1())
        {
            ctx.Database.Initialize(false);
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new LoginForm());
    }
    }
}
