using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.DAO
{
    public class UserAccount
    {
        [Key]
        public int UserAccountId { get; set; }  

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required, StringLength(50)]
        public string Role { get; set; }   // Administrator, Cashier, Employee

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }
    }
}
