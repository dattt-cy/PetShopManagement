using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ShopPetManagement.BLL;
using ShopPetManagement.UIL;

namespace Pet_Shop_Management_System
{
    public partial class CashCustomer : Form
    {
        private readonly CustomerService _customerService = new CustomerService();
        private readonly CashForm _parent;

        public CashCustomer(CashForm parent)
        {
            InitializeComponent();
            _parent = parent;
            LoadCustomer();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void LoadCustomer()
        {
            var list = _customerService
                .Search(txtSearch.Text)
                .Select((c, i) => new CustomerViewModel
                {
                    No = i + 1,
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Phone = c.Phone
                })
                .ToList();

            dgvCustomer.AutoGenerateColumns = false;
            dgvCustomer.DataSource = new BindingList<CustomerViewModel>(list);
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCustomer.Columns[e.ColumnIndex].Name != "Choice") return;

            var vm = dgvCustomer.Rows[e.RowIndex].DataBoundItem as CustomerViewModel;
            if (vm == null) return;


            _parent.SetCustomer(vm.CustomerId, vm.Name);
            this.Close();
        }
    }
}
