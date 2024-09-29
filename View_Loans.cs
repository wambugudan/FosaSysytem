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
    public partial class View_Loans : Form
    {
        public static string passingText;
        public View_Loans()
        {
            InitializeComponent();
        }

        //Database Connection code
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_users.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void View_Loans_Load(object sender, EventArgs e)
        {
            Populate();
        }


        //Populating Data Grid View
        void Populate()
        {
            try
            {
                con.Open();

                string display = "Select * from tbl_loans";
                cmd = new OleDbCommand(display, con);
                da = new OleDbDataAdapter(display, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                var ds = new DataSet();
                da.Fill(ds);
                LoansGV.DataSource = ds.Tables[0];

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        //back to dashboard
        public void Back()
        {
            new Dashboard().Show();
            this.Close();
        }

        //clear
        private void Clear()
        {
            txtmId.Text = "";
            txtLId.Text = "";
        }

        private void txtmId_TextChanged(object sender, EventArgs e)
        {
            // filter output from search box
            try
            {
                con.Open();

                string filter = "Select * from tbl_loans WHERE member_id like '" + txtmId.Text + "%'";
                cmd = new OleDbCommand(filter, con);
                da = new OleDbDataAdapter(filter, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                var ds = new DataSet();
                da.Fill(ds);
                LoansGV.DataSource = ds.Tables[0];

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtmId.Text == "")
            {
                MessageBox.Show("Please Select the member for whom a guarantor is to be added", "Guarantor Addition Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                //passing Loan_ID to Loan Edits's form
                passingText = txtLId.Text;
                Edit_Loan change = new Edit_Loan();
                change.Show();
                this.Hide();
            }
        }

        private void LoansGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmId.Text = LoansGV.SelectedRows[0].Cells[1].Value.ToString();
            txtLId.Text = LoansGV.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void btnCheckOff_Click(object sender, EventArgs e)
        {
            if (txtmId.Text == "" || txtLId.Text == "")
            {
                MessageBox.Show("Please Select the member and the loan to be checkedoff", "CheckOff Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                //passing Loan_ID to Loan Edits's form
                passingText = txtLId.Text;
                this.Hide();
                var Clear_Loan = new Clear_Loan();
                Clear_Loan.Show();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            // Delete Loans from database 

            if (txtLId.Text == "" || txtmId.Text == "")
            {
                MessageBox.Show("Please please ensure you have selected the loan to be deleted", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (MessageBox.Show("Are you sure you want to Delete this loan?", "Delete Loan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    con.Open();

                    string delete = "DELETE FROM tbl_loans WHERE loan_id = '" + txtLId.Text + "'";
                    cmd = new OleDbCommand(delete, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    Clear();
                                        
                    MessageBox.Show("Loan Deleted Successfully", "Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void btnLoansIssuance_Click(object sender, EventArgs e)
        {
            this.Close();
            new Loans().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new memberRegistration().Show();
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
    
}
