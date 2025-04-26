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

namespace Pet_Shop_Management_System
{
    public partial class CashCustomer : Form
    {

        string title = "Pet Shop Management System";
        CashForm cash;
        public CashCustomer(CashForm form)
        {
            InitializeComponent();
            cash = form;
            LoadCustomer();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

    

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            //if(colName == "Choice")
            //{
            //    dbcon.executeQuery("UPDATE tbCash SET cid=" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + " WHERE transno=" + cash.lblTransno.Text + "");
            //    cash.loadCash();
            //    this.Dispose();
            //}

        }

        #region method
        public void LoadCustomer()
        {
            
            //try
            //{
            //    int i = 0;
            //    dgvCustomer.Rows.Clear();
            //    cm = new SqlCommand("SELECT id,name,phone FROM tbCustomer WHERE name LIKE '%" + txtSearch.Text + "%'", cn);
            //    cn.Open();
            //    dr = cm.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        i++;
            //        dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            //    }
            //    dr.Close();
            //    cn.Close();
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message, title);
            //}
        }
        #endregion method
    }
}
