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

namespace Pet_Shop_Management_System
{
    public partial class Dashboard : Form
    {
            
        string title = "Pet Shop Management System";
        private readonly PetService _petSvc = new PetService();
        public Dashboard()
        {
            InitializeComponent();
        
        }


        #region method
        public int extractData(string str)
        {
            int data = 0;
            //try
            //{
            //    cn.Open();
            //    cm = new SqlCommand("SELECT ISNULL(SUM(pqty),0) AS qty FROM tbProduct WHERE pcategory='"+ str +"'", cn);
            //    data = int.Parse(cm.ExecuteScalar().ToString());
            //    cn.Close();
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message, title);
            //}
            return data;
        }

        #endregion method

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblAnimal.Text = extractData("Dog").ToString();
            lblFood.Text = extractData("Cat").ToString();
            lblMedicine.Text = extractData("Bird").ToString();
            lblItem.Text = extractData("Fish").ToString();
            lblAnimal.Text = _petSvc.CountByCategories("Thú kiểng", "Thú cảnh").ToString();
            lblFood.Text = _petSvc.CountByCategories("Thức ăn").ToString();
            lblItem.Text = _petSvc.CountByCategories("Phụ kiện").ToString();
            lblMedicine.Text = _petSvc.CountByCategories("Thuốc").ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
