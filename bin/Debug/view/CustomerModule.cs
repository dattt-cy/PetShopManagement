using System;
using System.Windows.Forms;
using PetShopApp.DTO;
using ShopPetManagement.BLL;
using ShopPetManagement.DAO;

namespace Pet_Shop_Management_System
{
    public partial class CustomerModule : Form
    {
        private readonly CustomerService _service;
        private readonly CustomerForm _parent;
        private Customer _editingCustomer;

        public CustomerModule(CustomerForm parent)
        {
            InitializeComponent();
            _service = new CustomerService();
            _parent = parent;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            var customer = new Customer
            {
                Name = txtName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Phone = txtPhone.Text.Trim()
            };

            try
            {
                _service.Add(customer);
                MessageBox.Show("Đăng ký khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parent.LoadCustomers();
                Clear();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_editingCustomer == null)
                return;

            if (!ValidateFields())
                return;

            _editingCustomer.Name = txtName.Text.Trim();
            _editingCustomer.Address = txtAddress.Text.Trim();
            _editingCustomer.Phone = txtPhone.Text.Trim();

            try
            {
                _service.Update(_editingCustomer);
                MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parent.LoadCustomers();
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Tên, địa chỉ và số điện thoại không được để trống.", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            _editingCustomer = null;
        }

   
        public void LoadForEdit(Customer customer)
        {
            _editingCustomer = customer;
            txtName.Text = customer.Name;
            txtAddress.Text = customer.Address;
            txtPhone.Text = customer.Phone;

            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
        }
    }
}
