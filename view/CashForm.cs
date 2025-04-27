using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ShopPetManagement.BLL;
using ShopPetManagement.UIL;   // chứa CartItemViewModel

namespace Pet_Shop_Management_System
{
    public partial class CashForm : Form
    {
        // Services
        private readonly SaleService _saleService = new SaleService();
        private readonly CustomerService _customerSvc = new CustomerService();
        private readonly UserService _userService = new UserService();

        // Giỏ hàng tạm
        private readonly List<CartItemViewModel> _cartItems = new List<CartItemViewModel>();

        private readonly MainForm _mainForm;
        private int _cashierId;
        private int _customerId;
        private string _customerName;
        private bool _customerChosen;

        public CashForm(MainForm form)
        {
            InitializeComponent();
            _mainForm = form;
            _cashierId = _userService.GetUserIdByUsername(_mainForm.lblUsername.Text);
            _customerId = 0;
            _customerChosen = false;
            _customerName = "";

            // ánh xạ column của DataGridView với CartItemViewModel
            dgvCash.AutoGenerateColumns = false;
            dgvCash.Columns["No"].DataPropertyName = nameof(CartItemViewModel.No);
            dgvCash.Columns["Pcode"].DataPropertyName = nameof(CartItemViewModel.PCode);
            dgvCash.Columns["Name2"].DataPropertyName = nameof(CartItemViewModel.Name);
            dgvCash.Columns["Qty"].DataPropertyName = nameof(CartItemViewModel.Qty);
            dgvCash.Columns["Price"].DataPropertyName = nameof(CartItemViewModel.Price);
            dgvCash.Columns["Total"].DataPropertyName = nameof(CartItemViewModel.Total);
            dgvCash.Columns["CustomerName"].DataPropertyName = nameof(CartItemViewModel.CustomerName);
            dgvCash.Columns["CashierName"].DataPropertyName = nameof(CartItemViewModel.CashierName);

            // Hook sự kiện chỉnh sửa trực tiếp ô Qty/Price
            dgvCash.CellEndEdit += dgvCash_CellEndEdit;

            GenerateTransNo();
            RefreshCartGrid();
        }

        // Nút "+ Product"
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new CashProduct(this))
                frm.ShowDialog();
        }

        // Nút "Cash" (2 bước)
        private void btnCash_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm trước khi thanh toán.",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_customerChosen)
            {
                // Bước 1: chọn khách
                using (var frm = new CashCustomer(this))
                    frm.ShowDialog();

                if (_customerChosen)
                {
                    btnCash.Text = "Xác nhận thanh toán";
                    RefreshCartGrid();  // để hiện tên khách trong grid
                }
                return;
            }

            // Bước 2: đã chọn khách, thực sự ghi hoá đơn
            DoCompleteSale();
        }

        // Được gọi từ CashCustomer để đẩy thông tin khách về
        public void SetCustomer(int customerId, string customerName)
        {
            _customerId = customerId;
            _customerName = customerName;
            _customerChosen = true;
        }

        private void DoCompleteSale()
        {
            try
            {
                var products = _cartItems
                    .Select(c => (petId: c.PetId, quantity: c.Qty, unitPrice: c.Price))
                    .ToList();

                int saleId = _saleService.CreateSale(_customerId, _cashierId, products);

                MessageBox.Show($"Thanh toán thành công! Mã hoá đơn: {saleId}",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // reset form...
            }
            catch (Exception ex)
            {
                // In ra chi tiết sâu nhất của exception
                var baseEx = ex.GetBaseException();
                MessageBox.Show(
                    $"Lỗi khi ghi hoá đơn:\n{baseEx.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddCartItem(CartItemViewModel item)
        {
            var exist = _cartItems.FirstOrDefault(x => x.PetId == item.PetId);
            if (exist != null) exist.Qty += item.Qty;
            else _cartItems.Add(item);
            RefreshCartGrid();
        }

       
        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = dgvCash.Columns[e.ColumnIndex].Name;
            var vm = dgvCash.Rows[e.RowIndex].DataBoundItem as CartItemViewModel;
            if (vm == null) return;

            var item = _cartItems.FirstOrDefault(x => x.PetId == vm.PetId);
            if (item == null) return;

            switch (col)
            {
                case "Up": item.Qty++; break;
                case "Down": if (item.Qty > 1) item.Qty--; break;
                case "Delete": _cartItems.Remove(item); break;
                default: return;
            }
            RefreshCartGrid();
        }


        private void dgvCash_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = dgvCash.Columns[e.ColumnIndex].Name;
            if (colName != "Qty" && colName != "Price") return;

            var vm = dgvCash.Rows[e.RowIndex].DataBoundItem as CartItemViewModel;
            var item = _cartItems.FirstOrDefault(x => x.PetId == vm?.PetId);
            if (vm == null || item == null) return;

            if (colName == "Qty"
                && int.TryParse(dgvCash.Rows[e.RowIndex].Cells["Qty"].Value?.ToString(), out var q))
            {
                item.Qty = q;
            }
            else if (colName == "Price"
                && decimal.TryParse(dgvCash.Rows[e.RowIndex].Cells["Price"].Value?.ToString(), out var p))
            {
                item.Price = p;
            }

            RefreshCartGrid();
        }

       
        private void RefreshCartGrid()
        {
            var list = _cartItems
                .Select((c, i) => new CartItemViewModel
                {
                    No = i + 1,
                    PetId = c.PetId,
                    PCode = c.PCode,
                    Name = c.Name,
                    Qty = c.Qty,
                    Price = c.Price,
                    CustomerName = _customerChosen
                                      ? _customerName
                                      : "",
                    CashierName = _userService
                                      .GetById(_cashierId)?
                                      .Name ?? ""
                })
                .ToList();

            dgvCash.DataSource = new BindingList<CartItemViewModel>(list);

            lblTotal.Text = list.Sum(x => x.Total)
                                .ToString("#,##0.00");
        }

     
        private void GenerateTransNo()
        {
            lblTransno.Text = DateTime.Now.ToString("yyyyMMdd")
                            + new Random().Next(1000, 9999).ToString();
        }
    }
}
