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

      
        string title = "Pet Shop Management System";
        public LoginForm()
        {
            InitializeComponent();
   
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
                   
                    MessageBox.Show($"Chao Mung {user.Name}",
                                    "Ket noi duoc chap nhan",
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
                    MessageBox.Show("Mat khau hoac username khong ton tai!",
                                    "Tu choi truy cap",
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
            MessageBox.Show("Hay lien he cho admin!", "Quen Mat Khau", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
