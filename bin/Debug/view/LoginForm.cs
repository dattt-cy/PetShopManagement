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

    public partial class LoginForm : Form
    {
        private readonly UserAccountService _userService = new UserAccountService();

        //SqlConnection cn = new SqlConnection();
        //SqlCommand cm = new SqlCommand();
        //DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Pet Shop Management System";
        public LoginForm()
        {
            InitializeComponent();
            //cn = new SqlConnection(dbcon.connection());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Exit Application?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtname.Text.Trim();
                string password = txtpass.Text;
                UserAccount user = _userService.Authenticate(username, password);

                if (user != null)
                {
                   
                    MessageBox.Show($"Welcome {user.Name}",
                                    "ACCESS GRANTED",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    
                    var main = new MainForm(user);
                    main.lblUsername.Text = user.Name;
                    main.lblRole.Text = user.Role;
                    main.btnUser.Enabled = (user.Role == "Administrator");

                    this.Hide();
                    main.ShowDialog();
                    this.Show();  
                }
                else
                {
                    MessageBox.Show("Invalid username or password!",
                                    "ACCESS DENIED",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    txtpass.Clear();
                    txtname.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

        }

        private void btnForget_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your BOSS!", "FORGET PASSWORD", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
