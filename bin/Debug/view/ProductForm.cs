using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ShopPetManagement.BLL;
using ShopPetManagement.DAO;
using ShopPetManagement.UIL;

namespace Pet_Shop_Management_System
{
    public partial class ProductForm : Form
    {
        private readonly PetService _service = new PetService();
        private const string Title = "Pet Shop Management System";

        public ProductForm()
        {
            InitializeComponent();
            dgvProduct.AutoGenerateColumns = false;
            dgvProduct.DataBindingComplete += DgvProduct_DataBindingComplete;
            LoadProduct();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var module = new ProductModule(this))
            {
                module.ShowDialog();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Không xử lý header
            if (e.RowIndex < 0) return;

            // Xác định cột vừa click
            var col = dgvProduct.Columns[e.ColumnIndex].Name;

            // Lấy ViewModel
            var vm = dgvProduct.Rows[e.RowIndex].DataBoundItem as PetViewModel;
            if (vm == null) return;

            // Trường hợp Edit
            if (col == "Edit")
            {
                using (var module = new ProductModule(this))
                {
                    // Lấy Pet thật từ BLL
                    var pet = _service.GetAllPets()
                                      .FirstOrDefault(p => p.PetId == vm.PetId);
                    if (pet != null)
                    {
                        module.LoadForEdit(pet);
                        module.ShowDialog();
                    }
                }
            }
            // Trường hợp Delete
            else if (col == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?",
                                    Title,
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    _service.Delete(vm.PetId);
                    LoadProduct();
                }
            }
        }


        public void LoadProduct()
        {
            var vmList = _service.Search(txtSearch.Text)
     .OrderBy(p => p.PetId)
     .Select((p, i) => new PetViewModel
     {
         No = i + 1,
         PetId = p.PetId,
         Pcode = p.PCode,
         Name = p.Name,
         Type = p.Type.Name,
         Category = p.Category.Name,
         Qty = p.StockQty,
         Price = p.SalePrice
     })
     .ToList();

            dgvProduct.AutoGenerateColumns = false;
            dgvProduct.DataSource = new BindingList<PetViewModel>(vmList);
        }

        private void DgvProduct_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                dgvProduct.Rows[i].Cells["Column1"].Value = i + 1;
            }
        }
    }
}
