using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PetShopApp.DTO;
using ShopPetManagement.BLL;
using ShopPetManagement.DAO;

namespace Pet_Shop_Management_System
{
    public partial class UserForm : Form
    {
        private readonly UserService _userService = new UserService();
        string title = "Pet Shop Management System";
        public UserForm()
        {
            InitializeComponent();
            dgvUser.DataBindingComplete += DgvUser_DataBindingComplete;
            LoadUser();
        }
        private void DgvUser_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
          
            for (int i = 0; i < dgvUser.Rows.Count; i++)
            {
                dgvUser.Rows[i].Cells["Column1"].Value = (i + 1).ToString();
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();
        }



        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var colName = dgvUser.Columns[e.ColumnIndex].Name;
            var account = dgvUser.Rows[e.RowIndex].DataBoundItem as UserAccount;
            if (account == null) return;

            if (colName == "Edit")
            {
             
                using (var module = new UserModule(this))
                {
                    module.LoadUserForEdit(account);   
                    module.ShowDialog();
                }
            }
            else if (colName == "Delete")
            {

                bool hasSales = _userService.HasSales(account.UserAccountId);

               
                string msg = hasSales
                    ? "Nhân viên này đã thực hiện giao dịch bán hàng.\n" +
                      "Nếu bạn xóa, toàn bộ dữ liệu Sales liên quan sẽ bị mất hoàn toàn.\n" +
                      "Bạn có chắc muốn tiếp tục?"
                    : "Bạn có chắc muốn xóa bản ghi này?";
                var icon = hasSales ? MessageBoxIcon.Warning : MessageBoxIcon.Question;

              
                if (MessageBox.Show(msg, "Xác nhận xóa người dùng",
                                    MessageBoxButtons.YesNo, icon)
                    == DialogResult.Yes)
                {
                    try
                    {
                        _userService.Delete(account.UserAccountId);
                        MessageBox.Show("Xóa thành công!", "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUser();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadUser();
                    }
                }
            }
        }

        #region Method
        public void LoadUser()
        {
            var list = _userService.GetUsers(txtSearch.Text);

            dgvUser.AutoGenerateColumns = false;



            dgvUser.DataSource = new BindingList<UserAccount>(list);
        }

        #endregion Method
    }
}
