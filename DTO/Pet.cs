using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.DAO
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        [Required, StringLength(20)]
        public string PCode { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public int PetTypeId { get; set; }
        [ForeignKey(nameof(PetTypeId))]
        public PetType Type { get; set; }

        public int PetCategoryId { get; set; }
        [ForeignKey(nameof(PetCategoryId))]
        public PetCategory Category { get; set; }

        public int StockQty { get; set; }

        public decimal SalePrice { get; set; }
    }
}
