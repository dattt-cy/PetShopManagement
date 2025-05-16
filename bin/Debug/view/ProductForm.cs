using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
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
            if (e.RowIndex < 0) return;

            var col = dgvProduct.Columns[e.ColumnIndex].Name;

          
            var vm = dgvProduct.Rows[e.RowIndex].DataBoundItem as PetViewModel;
            if (vm == null) return;

           
            if (col == "Edit")
            {
                using (var module = new ProductModule(this))
                {
                  
                    var pet = _service.GetAllPets()
                                      .FirstOrDefault(p => p.PetId == vm.PetId);
                    if (pet != null)
                    {
                        module.LoadForEdit(pet);
                        module.ShowDialog();
                    }
                }
            }
           
            else if (col == "Delete")
            {
                bool hasSales = _service.HasSales(vm.PetId);
               
                string msg = hasSales
                    ? "Thú cưng này đã từng có giao dịch bán hàng.\n" +
                      "Nếu bạn xóa, toàn bộ dữ liệu liên quan ở SaleDetails sẽ bị mất hoàn toàn.\n" +
                      "Bạn có chắc muốn tiếp tục?"
                    : "Bạn có chắc muốn xóa sản phẩm này?";
                MessageBoxIcon icon = hasSales ? MessageBoxIcon.Warning : MessageBoxIcon.Question;

                if (MessageBox.Show(msg, Title, MessageBoxButtons.YesNo, icon)
                    == DialogResult.Yes)
                {
                    try
                    {
                        _service.Delete(vm.PetId);
                        LoadProduct();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadProduct();
                    }
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
