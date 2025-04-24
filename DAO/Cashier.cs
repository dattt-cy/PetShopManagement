using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.DAO
{
    public class Cashier
    {
        public Cashier()
        {
            SalesProcessed = new HashSet<Sale>();
        }
        [Key]
        public int CashierId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Sale> SalesProcessed { get; set; }
    }
}
