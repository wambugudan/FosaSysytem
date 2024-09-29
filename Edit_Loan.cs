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
    public partial class Edit_Loan : Form
    {
        public Edit_Loan()
        {
            InitializeComponent();
        }

        //Database Connection code
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_users.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void Edit_Loan_Load(object sender, EventArgs e)
        {
            txtLID.Text = View_Loans.passingText;
            PopulateLoanID();
        }

        private void PopulateLoanID()
        {
            //Populating Loan details to textboxes for editing
            try
            {
                con.Open();

                string get = "SELECT * FROM tbl_loans WHERE loan_id = '" + txtLID.Text + "'";
                cmd = new OleDbCommand(get, con);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txtmID.Text = reader["member_id"].ToString();
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
       
        public void Back()
        {
            this.Close();
            var View_Loans = new View_Loans();
            View_Loans.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboLType.SelectedItem == null || txtLAmt.Text == "" || txtAmtPayable.Text == "" || txtCollateral.Text == "")
            {
                MessageBox.Show("Please fill in all the load details", "Updation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (MessageBox.Show("Are you sure you want to update this record", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();

                        string update = "UPDATE tbl_loans SET member_id = '" + txtmID.Text + "', loan_type = '" + comboLType.SelectedItem + "', loan_amount = '" + txtLAmt.Text + "', date_borrowed = '" + dateTimePicker1.Value.Date + "', date_due = '" + dateTimePicker2.Value.Date + "', amount_payable = '" + txtAmtPayable.Text + "', collateral = '" + txtCollateral.Text + "' WHERE loan_id = '" + txtLID.Text + "'";
                        cmd = new OleDbCommand(update, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                        //update grid view
                       // Populate();

                        //Clear fields
                        //Clear();

                        MessageBox.Show("Loan Details Updated Successfully", "Update Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Loan Not Updated", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Clear();
                    //txtmId.Focus();
                }
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            //Getting the date difference
            DateTime db = dateTimePicker1.Value.Date;
            DateTime dd = dateTimePicker2.Value.Date;

            TimeSpan ts = dd - db;
            int days = ts.Days;

            //Calculating loan payable

            if (comboLType.SelectedItem == null || txtLAmt.Text == "")
            {
                MessageBox.Show("Please ensure Loan type and Loan Amount are filled in", "Calculation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (comboLType.SelectedItem.ToString() == "Normal" && days <= 14)
                {
                    txtAmtPayable.Text = (float.Parse(txtLAmt.Text) * 1.2).ToString();
                }
                else if (comboLType.SelectedItem.ToString() == "Normal" && days > 14)
                {
                    txtAmtPayable.Text = ((float.Parse(txtLAmt.Text) * 1.2) + ((days - 14) * 500)).ToString();
                }
                else if (comboLType.SelectedItem.ToString() == "Emergency" && days != 0)
                {
                    txtAmtPayable.Text = ((float.Parse(txtLAmt.Text)) + (float.Parse(txtLAmt.Text) * 0.1 * days)).ToString();
                }
                else if (comboLType.SelectedItem.ToString() == "Emergency" && days == 0)
                {
                    txtAmtPayable.Text = ((float.Parse(txtLAmt.Text)) + (float.Parse(txtLAmt.Text) * 0.1 * 1)).ToString();
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
