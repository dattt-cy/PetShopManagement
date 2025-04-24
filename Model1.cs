using System;
using System.Data.Entity;
using System.Linq;
using PetShopApp.DTO;
using ShopPetManagement.DAO;

namespace ShopPetManagement
{
    public class Model1 : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ShopPetManagement.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public Model1()
            : base("name=Model1")
        {
            Database.SetInitializer(new CreateDB());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<PetCategory> PetCategories { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

}