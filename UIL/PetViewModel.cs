using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.UIL
{
    public class PetViewModel
    {
        public int No { get; set; }
        public int PetId { get; set; }
        public string Pcode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
