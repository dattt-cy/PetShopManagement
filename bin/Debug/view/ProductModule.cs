using System;
using System.Linq;
using System.Windows.Forms;
using ShopPetManagement.BLL;
using ShopPetManagement.DAO;

namespace Pet_Shop_Management_System
{
    public partial class ProductModule : Form
    {
        private readonly PetService _service;
        private readonly PetTypeService _typeService;
        private readonly PetCategoryService _categoryService;
        private readonly ProductForm _parent;
        private Pet _editingPet;
        private const string TitleText = "Pet Shop Management System";

        public ProductModule(ProductForm parent)
        {
            InitializeComponent();
            _service = new PetService();
            _typeService = new PetTypeService();
            _categoryService = new PetCategoryService();
            _parent = parent;

            var categories = _categoryService.GetAllCategories();
            cbCategory.DataSource = categories;
            cbCategory.DisplayMember = "Name";
            cbCategory.ValueMember = "PetCategoryId";

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            var allPets = _service.GetAllPets();
            int nextNo = allPets.Count + 1;

            // 2) Sinh PCode
            string newPCode = $"P{nextNo:D3}";   // => P001, P002, ...

            // 3) Chuẩn bị Pet mới
            string typeName = txttype.Text.Trim();
            var type = _typeService.GetAllTypes()
                                   .FirstOrDefault(t =>
                                       t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
            if (type == null)
            {
                type = new PetType { Name = typeName };
                _typeService.Add(type);
            }

            var pet = new Pet
            {
                PCode = newPCode,
                Name = txtName.Text.Trim(),
                PetTypeId = type.PetTypeId,
                PetCategoryId = (int)cbCategory.SelectedValue,
                StockQty = ParseInt(txtQty.Text),
                SalePrice = ParseDecimal(txtPrice.Text)
            };

            // 4) Thêm vào DB
            _service.Add(pet);

            // 5) Làm mới lưới và đóng form
            _parent.LoadProduct();
            MessageBox.Show("Product has been successfully registered!", TitleText,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_editingPet == null || !ValidateFields())
                return;


            string typeName = txttype.Text.Trim();
            var type = _typeService.GetAllTypes()
                                   .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
            if (type == null)
            {
                type = new PetType { Name = typeName };
                _typeService.Add(type);
            }

            // Apply updates
            _editingPet.Name = txtName.Text.Trim();
            _editingPet.PetTypeId = type.PetTypeId;
            _editingPet.PetCategoryId = (int)cbCategory.SelectedValue;
            _editingPet.StockQty = ParseInt(txtQty.Text);
            _editingPet.SalePrice = ParseDecimal(txtPrice.Text);

            _service.Update(_editingPet);
            MessageBox.Show("Product has been successfully updated!", TitleText, MessageBoxButtons.OK, MessageBoxIcon.Information);
            _parent.LoadProduct();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
         //   Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txttype.Text) ||
                string.IsNullOrWhiteSpace(txtQty.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("All fields are required.", TitleText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Quantity must be a positive integer.", TitleText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Price must be a positive number.", TitleText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private int ParseInt(string text)
        {
            return int.TryParse(text, out int value) ? value : 0;
        }

        private decimal ParseDecimal(string text)
        {
            return decimal.TryParse(text, out decimal value) ? value : 0m;
        }

        private string GeneratePCode()
        {
            // Example: generate based on timestamp
            return "P" + DateTime.Now.Ticks;
        }

        public void LoadForEdit(Pet pet)
        {
            _editingPet = pet;
            lblPcode.Text = pet.PCode;
            txtName.Text = pet.Name;
            txttype.Text = _typeService.GetAllTypes()
                                               .FirstOrDefault(t => t.PetTypeId == pet.PetTypeId)?.Name;
            cbCategory.SelectedValue = pet.PetCategoryId;
            txtQty.Text = pet.StockQty.ToString();
            txtPrice.Text = pet.SalePrice.ToString();

            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
        }
    }
}
