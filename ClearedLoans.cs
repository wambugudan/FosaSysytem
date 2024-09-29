using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Login_and_Register
{
    public partial class ClearedLoans : Form
    {
        public ClearedLoans()
        {
            InitializeComponent();
        }

        //Database Connection code
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_users.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void ClearedLoans_Load(object sender, EventArgs e)
        {
            Populate();
        }

        //populate Data Grid
        void Populate()
        {
            try
            {
                con.Open();

                string display = "Select * from tbl_clearedLoans";
                cmd = new OleDbCommand(display, con);
                da = new OleDbDataAdapter(display, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                var ds = new DataSet();
                da.Fill(ds);
                ClearedLoansGV.DataSource = ds.Tables[0];

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void Delete()
        {
            // Delete Cleared Loans from database 

            if(txtLId.Text == "")
            {
                MessageBox.Show("Please please ensure you have selected the loan to be deleted", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (MessageBox.Show("Are you sure you want to Delete this loan?", "Delete Loan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    con.Open();

                    string delete = "DELETE FROM tbl_clearedLoans WHERE loan_id = '" + txtLId.Text + "'";
                    cmd = new OleDbCommand(delete, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    Populate();

                    MessageBox.Show("Loan Deleted Successfully", "Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

            }

        }

        private void btnBackToDash_Click(object sender, EventArgs e)
        {
            this.Close();
            new Dashboard().Show();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void ClearedLoansGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtLId.Text = ClearedLoansGV.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Close();
            new Dashboard().Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
