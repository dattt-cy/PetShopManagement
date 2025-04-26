using System;
using System.Windows.Forms;
using ShopPetManagement.BLL;
using ShopPetManagement.DAO;

namespace Pet_Shop_Management_System
{
    public partial class UserModule : Form
    {
        private readonly string title = "Pet Shop Management System";
        private readonly UserService _userService;
        private readonly UserForm _userForm;
        private UserAccount _editingUser;

        public UserModule(UserForm userForm)
        {
            InitializeComponent();
            _userForm = userForm;
            _userService = new UserService();

            // Thiết lập comboBox Role
            cbRole.Items.Clear();
            cbRole.Items.AddRange(new string[] { "Administrator", "Cashier", "Employee" });
            cbRole.SelectedIndex = 2; // mặc định Employee

            // Ban đầu chỉ enable nút Save
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                var user = new UserAccount
                {
                    Name = txtName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Role = cbRole.SelectedItem.ToString(),
                    DateOfBirth = dtDob.Value.Date,
                    Password = cbRole.SelectedItem.ToString() == "Employee" ? null : txtPass.Text
                };

                _userService.Add(user);
                MessageBox.Show("User successfully registered!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                _userForm.LoadUser();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_editingUser == null || !ValidateFields())
                return;

            try
            {
                _editingUser.Name = txtName.Text.Trim();
                _editingUser.Address = txtAddress.Text.Trim();
                _editingUser.Phone = txtPhone.Text.Trim();
                _editingUser.Role = cbRole.SelectedItem.ToString();
                _editingUser.DateOfBirth = dtDob.Value.Date;
                _editingUser.Password = cbRole.SelectedItem.ToString() == "Employee" ? null : txtPass.Text;

                _userService.Update(_editingUser);
                MessageBox.Show("User data successfully updated!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                _userForm.LoadUser();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isEmployee = cbRole.SelectedItem.ToString() == "Employee";
            lblPass.Visible = !isEmployee;
            txtPass.Visible = !isEmployee;
            this.Height = isEmployee ? 427 : 453;
        }

        #region Helpers
        private void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtPass.Clear();
            cbRole.SelectedIndex = 2;
            dtDob.Value = DateTime.Today;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            _editingUser = null;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Name and Address are required.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtDob.Value.Date > DateTime.Today.AddYears(-18))
            {
                MessageBox.Show("User must be at least 18 years old.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cbRole.SelectedItem.ToString() != "Employee" && string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Password is required for non-employee roles.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public void LoadUserForEdit(UserAccount user)
        {
            _editingUser = user;
            txtName.Text = user.Name;
            txtAddress.Text = user.Address;
            txtPhone.Text = user.Phone;
            cbRole.SelectedItem = user.Role;
            dtDob.Value = user.DateOfBirth;
            txtPass.Text = user.Password;

            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
        }
        #endregion
    }
}
