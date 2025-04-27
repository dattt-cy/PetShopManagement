using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ShopPetManagement.BLL;
using ShopPetManagement.UIL;  // chứa PetViewModel

namespace Pet_Shop_Management_System
{
    public partial class CashProduct : Form
    {
        private readonly PetService _petService = new PetService();
        private readonly CashForm _parent;

        public CashProduct(CashForm parent)
        {
            InitializeComponent();
            _parent = parent;
            LoadProduct();    // gọi ngay khi form khởi tạo
        }

        private void LoadProduct()
        {
            var pets = _petService
                .GetAllPets()              
                .OrderBy(p => p.PetId)
                .Select((p, i) => new PetViewModel
                {
                    No = i + 1,
                    PetId = p.PetId,
                    Pcode = p.PCode,
                    Name = p.Name,
                    Type = p.Type.Name,
                    Category = p.Category.Name,
                    Price = p.SalePrice
                })
                .ToList();

            dgvProduct.AutoGenerateColumns = false;
            dgvProduct.DataSource = new BindingList<PetViewModel>(pets);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // nếu muốn search theo tên:
            var keyword = txtSearch.Text.Trim();
            var pets = string.IsNullOrEmpty(keyword)
                ? _petService.GetAllPets()
                : _petService.Search(keyword);

            // Re-bind tương tự như trên
            var vms = pets
                .OrderBy(p => p.PetId)
                .Select((p, i) => new PetViewModel
                {
                    No = i + 1,
                    PetId = p.PetId,
                    Pcode = p.PCode,
                    Name = p.Name,
                    Type = p.Type.Name,
                    Category = p.Category.Name,
                    Price = p.SalePrice
                })
                .ToList();

            dgvProduct.DataSource = new BindingList<PetViewModel>(vms);
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
          
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["Select"].Value);
                if (!isChecked) continue;

                var vm = row.DataBoundItem as PetViewModel;
                if (vm == null) continue;

                var item = new CartItemViewModel
                {
                    PetId = vm.PetId,
                    PCode = vm.Pcode,
                    Name = vm.Name,
                    Qty = 1,            
                    Price = vm.Price
                };
                _parent.AddCartItem(item);
            }
            this.Close();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
