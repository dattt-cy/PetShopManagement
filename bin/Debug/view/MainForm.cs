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
using ShopPetManagement.DAL;
using ShopPetManagement.DAO;

namespace Pet_Shop_Management_System
{
    public partial class MainForm : Form
    {
        private readonly SaleService _saleService;
        private readonly UserAccount _currentUser;
        public MainForm(UserAccount account)
        {
            InitializeComponent();
            _saleService = new SaleService();
            _currentUser = account;
            btnDashboard.PerformClick();
            loadDailySale();
     
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
        }  

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
          
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
            
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
            
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            openChildForm(new CashForm(this));
            
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                 LoginForm login = new LoginForm();
                 this.Dispose();
                 login.ShowDialog();
            }
           
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            progress.Invoke((MethodInvoker)delegate
            {
                progress.Text = DateTime.Now.ToString("hh:mm:ss");
                progress.Value = Convert.ToInt32(DateTime.Now.Second);
            });
        }
        #region Method
        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public void loadDailySale()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");

            try
            {

                DateTime yesterday = DateTime.Today.AddDays(-1);

             
                int cashierId = _currentUser.UserAccountId;  

                decimal revenue = _saleService.GetDailyRevenueByCashier(yesterday, cashierId);

                lblDailySale.Text = revenue.ToString("#,##0") + " đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải doanh thu: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #endregion Method

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
