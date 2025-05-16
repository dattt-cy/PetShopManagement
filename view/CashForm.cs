using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using ShopPetManagement.BLL;
using ShopPetManagement.UIL; 

namespace Pet_Shop_Management_System
{
    public partial class CashForm : Form
    {
        private int _currentSaleId;
        private readonly SaleService _saleService = new SaleService();
        private readonly CustomerService _customerSvc = new CustomerService();
        private readonly UserService _userService = new UserService();

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
            _customerName = "";
            _customerChosen = false;

 
            dgvCash.AutoGenerateColumns = false;
            dgvCash.Columns["No"].DataPropertyName = nameof(CartItemViewModel.No);
            dgvCash.Columns["Pcode"].DataPropertyName = nameof(CartItemViewModel.PCode);
            dgvCash.Columns["Name2"].DataPropertyName = nameof(CartItemViewModel.Name);
            dgvCash.Columns["Qty"].DataPropertyName = nameof(CartItemViewModel.Qty);
            dgvCash.Columns["Price"].DataPropertyName = nameof(CartItemViewModel.Price);
            dgvCash.Columns["Total"].DataPropertyName = nameof(CartItemViewModel.Total);
            dgvCash.Columns["CustomerName"].DataPropertyName = nameof(CartItemViewModel.CustomerName);
            dgvCash.Columns["CashierName"].DataPropertyName = nameof(CartItemViewModel.CashierName);

            dgvCash.CellEndEdit += dgvCash_CellEndEdit;

            GenerateTransNo();
            RefreshCartGrid();

            printDocumentInvoice.PrintPage += PrintDocumentInvoice_PrintPage;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new CashProduct(this))
                frm.ShowDialog();
        }

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
                using (var frm = new CashCustomer(this))
                    frm.ShowDialog();

                if (_customerChosen)
                {
                    btnCash.Text = "Thanh toán";
                    RefreshCartGrid(); 
                }
                return;
            }


            DoCompleteSale();
        }

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

          
                _currentSaleId = _saleService.CreateSale(_customerId, _cashierId, products);

             
                printDocumentInvoice.PrintController = new StandardPrintController();
                printDocumentInvoice.Print();

                MessageBox.Show($"Thanh toán & in hóa đơn thành công! Mã: {_currentSaleId}",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

             
                _cartItems.Clear();
                _customerId = 0;
                _customerChosen = false;
                btnCash.Text = "Cash";
                GenerateTransNo();
                RefreshCartGrid();
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                MessageBox.Show($"Lỗi khi ghi hoá đơn:\n{baseEx.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocumentInvoice_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            int x = 50, y = 50, lineHeight = 25;
            var fontTitle = new Font("Arial", 16, FontStyle.Bold);
            var fontHeader = new Font("Arial", 12, FontStyle.Bold);
            var fontBody = new Font("Arial", 10);

       
            g.DrawString("HÓA ĐƠN BÁN HÀNG", fontTitle, Brushes.Black, x, y);
            y += lineHeight * 2;

            g.DrawString($"Mã hóa đơn: {_currentSaleId}", fontBody, Brushes.Black, x, y);
            y += lineHeight;
            g.DrawString($"Ngày: {DateTime.Now:dd/MM/yyyy HH:mm}", fontBody, Brushes.Black, x, y);
            y += lineHeight;
            g.DrawString($"Khách hàng: {(_customerChosen ? _customerName : "Khách lẻ")}",
                         fontBody, Brushes.Black, x, y);
            y += lineHeight * 2;

          
            g.DrawString("Sản phẩm", fontHeader, Brushes.Black, x, y);
            g.DrawString("SL", fontHeader, Brushes.Black, x + 300, y);
            g.DrawString("Đơn giá", fontHeader, Brushes.Black, x + 350, y);
            g.DrawString("Thành tiền", fontHeader, Brushes.Black, x + 450, y);
            y += lineHeight;
            g.DrawLine(Pens.Black, x, y, x + 550, y);
            y += 5;

          
            decimal grandTotal = 0;
            foreach (var item in _cartItems)
            {
                var subTotal = item.Qty * item.Price;
                g.DrawString(item.Name, fontBody, Brushes.Black, x, y);
                g.DrawString(item.Qty.ToString(), fontBody, Brushes.Black, x + 300, y);
                g.DrawString(item.Price.ToString("N0"), fontBody, Brushes.Black, x + 350, y);
                g.DrawString(subTotal.ToString("N0"), fontBody, Brushes.Black, x + 450, y);
                y += lineHeight;
                grandTotal += subTotal;
            }

            y += lineHeight;
            g.DrawLine(Pens.Black, x, y, x + 550, y);
            y += lineHeight;
            g.DrawString($"Tổng cộng: {grandTotal:N0} đ", fontHeader, Brushes.Black, x, y);
            y += lineHeight * 2;
            using (var italicFont = new Font(fontBody.FontFamily, fontBody.Size, FontStyle.Italic))
            {
                g.DrawString(
                    "Cảm ơn quý khách!",   
                    italicFont,            
                    Brushes.Black,         
                    x, y                 
                );
            }

            e.HasMorePages = false;
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
                case "Up":
              
                    var pet = _petService.GetById(item.PetId);
                    if (pet == null)
                    {
                        MessageBox.Show("Không tìm thấy pet.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (item.Qty < pet.StockQty)
                    {
                        item.Qty++;
                    }
                    else
                    {
                        MessageBox.Show($"Chỉ còn {pet.StockQty} con trong kho. Không thể tăng thêm.",
                                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                case "Down":
                    if (item.Qty > 1)
                    {
                        item.Qty--;
                    }
                    else
                    {
                       
                        MessageBox.Show("Số lượng không thể nhỏ hơn 1.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                case "Delete":
                    _cartItems.Remove(item);
                    break;

                default:
                    return;
            }

            RefreshCartGrid();
        }


      
        private readonly PetService _petService = new PetService();


        private void dgvCash_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = dgvCash.Columns[e.ColumnIndex].Name;
            if (colName != "Qty") 
                return;

            var vm = dgvCash.Rows[e.RowIndex].DataBoundItem as CartItemViewModel;
            var item = _cartItems.FirstOrDefault(x => x.PetId == vm?.PetId);
            if (vm == null || item == null) return;

           
            if (int.TryParse(dgvCash.Rows[e.RowIndex].Cells["Qty"].Value?.ToString(), out var q))
            {
           
                var pet = _petService.GetById(item.PetId);
                if (q > pet.StockQty)
                {
                    MessageBox.Show($"Chỉ còn {pet.StockQty} con trong kho.",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    q = pet.StockQty;
                }

                item.Qty = q;
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
                    CustomerName = _customerChosen ? _customerName : "",
                    CashierName = _userService.GetById(_cashierId)?.Name ?? ""
                })
                .ToList();

            dgvCash.DataSource = new BindingList<CartItemViewModel>(list);
            lblTotal.Text = list.Sum(x => x.Total).ToString("#,##0.00");
        }

        private void GenerateTransNo()
        {
            lblTransno.Text = DateTime.Now.ToString("yyyyMMdd")
                            + new Random().Next(1000, 9999);
        }
    }
}
