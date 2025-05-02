using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopApp.DTO;

namespace ShopPetManagement.DAO
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        public DateTime SaleDate { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        public int CashierId { get; set; }
        [ForeignKey(nameof(CashierId))]
        public virtual UserAccount Cashier { get; set; }

        public virtual ICollection<SaleDetail> Details { get; set; }

        public Sale()
        {
            Details = new HashSet<SaleDetail>();
        }
    }
}
