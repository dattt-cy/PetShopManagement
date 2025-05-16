using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using PetShopApp.DTO;
using ShopPetManagement.BLL;
using ShopPetManagement.DAO;

namespace Pet_Shop_Management_System
{
    public partial class CustomerForm : Form
    {
        private readonly CustomerService _service = new CustomerService();

        public CustomerForm()
        {
            InitializeComponent();
            dgvCustomer.AutoGenerateColumns = false;
            dgvCustomer.DataBindingComplete += DgvCustomer_DataBindingComplete;
            LoadCustomers();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var module = new CustomerModule(this))
            {
                module.ShowDialog();
            }
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            Customer customer = dgvCustomer.Rows[e.RowIndex].DataBoundItem as Customer;
            if (customer == null) return;

            if (colName == "Edit")
            {
                using (var module = new CustomerModule(this))
                {
                    module.LoadForEdit(customer);
                    module.ShowDialog();
                }
            }
            else if (colName == "Delete")
            {
               
                bool hasSales = _service.hasSales(customer.CustomerId);

                DialogResult warningResult;
                if (hasSales)
                {
                    warningResult = MessageBox.Show(
                        "Khách hàng này đã có dữ liệu giao dịch (Sales).\n" +
                        "Nếu bạn xóa, tất cả dữ liệu Sales liên quan sẽ bị mất hoàn toàn.\n" +
                        "Bạn có chắc muốn tiếp tục?",
                        "Cảnh báo khi xóa khách hàng",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    warningResult = MessageBox.Show(
                        "Bạn có chắc muốn xóa khách hàng này?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                }

                
                if (warningResult == DialogResult.Yes)
                {
                    try
                    {
                        _service.Delete(customer.CustomerId);
                        LoadCustomers();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoadCustomers();
                    }
                }
            }
        }


        public void LoadCustomers()
        {
            var list = _service.Search(txtSearch.Text)
                               .OrderBy(c => c.CustomerId)
                               .ToList();

            dgvCustomer.DataSource = new BindingList<Customer>(list);
        }

        private void DgvCustomer_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvCustomer.Rows.Count; i++)
            {
                dgvCustomer.Rows[i].Cells["ColNo"].Value = i + 1;
            }
        }
    }
}