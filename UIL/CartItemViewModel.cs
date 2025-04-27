using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.UIL
{
    public class CartItemViewModel
    {
        public int No { get; set; }
        public int PetId { get; set; }  
        public string PCode { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Qty * Price;

        public string  CustomerName { get; set; }
        public string  CashierName  { get; set; }

    }
}
