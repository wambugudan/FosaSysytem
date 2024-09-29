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
    public partial class Clear_Loan : Form
    {
        public Clear_Loan()
        {
            InitializeComponent();
        }

        //Database Connection code
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_users.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void Clear_Loan_Load(object sender, EventArgs e)
        {
            txtLId.Text = View_Loans.passingText;
            PopulateLoanID();
            Payable();
            lblPayable.Text = txtAmtPayable.Text;
        }
        public void Back()
        {
            this.Close();
            var View_Loans = new View_Loans();
            View_Loans.Show();
        }

        private void Payable()
        {

            //Getting the date difference
            DateTime db = dateTimePicker1.Value.Date;
            DateTime dd = dateTimePicker2.Value.Date;
            DateTime dp = dateTimePicker3.Value.Date;

            TimeSpan ts = dd - db;
            int days = ts.Days;

            TimeSpan ts1 = dp - dd;
            int edays = ts1.Days;

            TimeSpan ts2 = dp - db;
            int ndays = ts2.Days;

            //Calculating loan payable

            if (dp > dd && comboLType.SelectedItem.ToString() == "Emergency" && days != 0)
            {
                txtAmtPayable.Text = ((float.Parse(txtLAmt.Text)) + (float.Parse(txtLAmt.Text) * 0.1 * days) + edays * 500).ToString();
            }
            else if (dp <= dd && comboLType.SelectedItem.ToString() == "Emergency" && days != 0)
            {
                txtAmtPayable.Text = ((float.Parse(txtLAmt.Text)) + (float.Parse(txtLAmt.Text) * 0.1 * days)).ToString();
            }
            else if (comboLType.SelectedItem.ToString() == "Normal" && ndays > 14)
            {
                txtAmtPayable.Text = ((float.Parse(txtLAmt.Text) * 1.2) + ((ndays - 14) * 500)).ToString();
            }
            else if (comboLType.SelectedItem.ToString() == "Normal" && days <= 14)
            {
                txtAmtPayable.Text = (float.Parse(txtLAmt.Text) * 1.2).ToString();
            }

            //Populating Amount Payable label
            lblPayable.Text = txtAmtPayable.Text;
        }

        private void PopulateLoanID()
        {
            //Populating Loan details to textboxes for Checkoff
            try
            {
                con.Open();

                string get = "SELECT * FROM tbl_loans WHERE loan_id = '" + txtLId.Text + "'";
                cmd = new OleDbCommand(get, con);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txtmId.Text = reader["member_id"].ToString();
                        comboLType.Text = reader["loan_type"].ToString();
                        txtLAmt.Text = reader["loan_amount"].ToString();
                        dateTimePicker1.Text = reader["date_borrowed"].ToString();
                        dateTimePicker2.Text = reader["date_due"].ToString();
                        txtAmtPayable.Text = reader["amount_payable"].ToString();
                        txtCollateral.Text = reader["collateral"].ToString();
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            Payable();
        }

        private void buttonClose_Click(object sender, EventArgs e) => Back();

        private void label9_Click(object sender, EventArgs e) => Back();

        private void btnCheckOff_Click(object sender, EventArgs e)
        {
            // Save ClearedLoans to database 

            if (chckReturned.Checked == false)
            {
                MessageBox.Show("Please please ensure to return the Collateral", "Checkoff Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (MessageBox.Show("Are you sure you want to Checkoff this loan?", "Checkoff Loan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                
                try
                {
                    con.Open();

                    string save = "INSERT INTO tbl_clearedLoans (loan_id, member_id, loan_amount, date_borrowed, date_due, date_paid, amount_paid, collateral) VALUES ('" + txtLId.Text + "','" + txtmId.Text + "', '" + txtLAmt.Text + "', '" + dateTimePicker1.Value.Date + "', '" + dateTimePicker2.Value.Date + "', '" + dateTimePicker3.Value.Date + "', '" + txtAmtPayable.Text + "', '" + txtCollateral.Text + "')";
                    cmd = new OleDbCommand(save, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    con.Open();

                    string delete = "DELETE FROM tbl_loans WHERE loan_id = '" + txtLId.Text + "'";
                    cmd = new OleDbCommand(delete, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                                        
                    MessageBox.Show("Loan Checkedoff Successfully", "Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Return to loans Window
                    Back();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
