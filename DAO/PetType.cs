using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.DAO
{
    public class PetType
    {
        public PetType()
        {
            Pets = new HashSet<Pet>();
        }
        [Key]
        public int PetTypeId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
