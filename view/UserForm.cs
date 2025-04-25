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
using ShopPetManagement.BLL;
using ShopPetManagement.DAO;

namespace Pet_Shop_Management_System
{
    public partial class UserForm : Form
    {
        private readonly UserService _userService = new UserService();
        //SqlConnection cn = new SqlConnection();
        //SqlCommand cm = new SqlCommand();
        //DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public UserForm()
        {
            InitializeComponent();
            //cn = new SqlConnection(dbcon.connection());
            LoadUser();
            dgvUser.DataBindingComplete += DgvUser_DataBindingComplete;
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
        private void DgvUser_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!dgvUser.Columns.Contains("No"))
            {
                dgvUser.Columns.Insert(0, new DataGridViewTextBoxColumn
                {
                    Name = "No",
                    HeaderText = "No",
                    ReadOnly = true
                });
            }
            for (int i = 0; i < dgvUser.Rows.Count; i++)
                dgvUser.Rows[i].Cells["No"].Value = i + 1;
        }




        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var colName = dgvUser.Columns[e.ColumnIndex].Name;
            // 1) Lấy đối tượng UserAccount của dòng này
            var account = dgvUser.Rows[e.RowIndex].DataBoundItem as UserAccount;
            if (account == null) return;

            //if (colName == "Edit")
            //{
            //    // Chuyển sang Form sửa, truyền thẳng object
            //    using (var module = new UserModule(this))
            //    {
            //        module.LoadUser(account);   // bạn tự viết helper trong UserModule
            //        module.ShowDialog();
            //    }
            //}
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi này?",
                                    "Xác nhận",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question)
                                    == DialogResult.Yes)
                {
                    // Gọi BLL xóa
                    _userService.DeleteAccount(account.UserAccountId);
                    MessageBox.Show("Xóa thành công!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUser();  // refresh grid
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
