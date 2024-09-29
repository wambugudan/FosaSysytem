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
    public partial class Loans : Form
    {
        public Loans()
        {
            InitializeComponent();
        }

        //Database Connection code
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_users.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        void Back()
        {
            this.Close();
            new Dashboard().Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void Loans_Load(object sender, EventArgs e)
        {
            //Loading current date
            dateTimePicker1.Value = DateTime.Now;

            PopulateCombo();

            PopulateLoanID();
        }

        private void PopulateLoanID()
        {
            //Populating LoanID with data from database
            try
            {
                con.Open();

                string get = "SELECT MAX(loan_id) FROM tbl_loans";
                cmd = new OleDbCommand(get, con);
                OleDbDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    if (reader[0] != System.DBNull.Value)
                    {
                        int a = Convert.ToInt32(reader[0].ToString());
                        txtLID.Text = (a + 1).ToString();
                    }
                    else
                    {
                        txtLID.Text = "1";
                    }

                }
                
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void PopulateCombo()
        {
            //Populating Combobox with data from database
            try
            {
                con.Open();

                string get = "SELECT * FROM tbl_members";
                cmd = new OleDbCommand(get, con);
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    combo_Id.Items.Add(reader["member_ID"].ToString());
                }
                con.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void Clear()
        {
            comboLType.SelectedItem = null;
            txtLAmt.Text = "";
            combo_Id.SelectedItem = null;
            txtLAmt.Text = "";
            txtCollateral.Text = "";
            txtAmtPayable.Text = "";
            txtFname.Text = "";
            txtLname.Text = "";
            txtId.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void combo_Id_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Populating all fields with data from database
            try
            {
                con.Open();

                string get = "SELECT * FROM tbl_members WHERE member_ID ='" + combo_Id.Text + "'";
                cmd = new OleDbCommand(get, con);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txtFname.Text = reader["f_name"].ToString();
                        txtLname.Text = reader["l_name"].ToString();
                        txtId.Text = reader["ID_no"].ToString();
                    }
                }
                con.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save Loans to database 

            if (comboLType.SelectedItem == null || txtLAmt.Text == "" || combo_Id.SelectedItem == null || txtLAmt.Text == "" || txtCollateral.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();

                    string save = "INSERT INTO tbl_loans (loan_id, member_id, fname, lname, loan_type, loan_amount, date_borrowed, date_due, amount_payable, Collateral ) VALUES ('" + txtLID.Text + "','" + combo_Id.SelectedItem + "', '" + txtFname.Text + "', '" + txtLname.Text + "', '" + comboLType.SelectedItem + "', '" + txtLAmt.Text + "', '" + dateTimePicker1.Value.Date + "', '" + dateTimePicker2.Value.Date + "', '" + txtAmtPayable.Text + "', '" + txtCollateral.Text + "')";
                    cmd = new OleDbCommand(save, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    //Clear fields
                    Clear();

                    //update LoanID
                    PopulateLoanID();

                    //clear User Details Textboxes
                    

                    MessageBox.Show("Loan Details Saved Successfully", "Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
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
                    txtAmtPayable.Text = (float.Parse(txtLAmt.Text)*1.2).ToString();                
                }
                else if(comboLType.SelectedItem.ToString() == "Normal" && days > 14)
                {
                    txtAmtPayable.Text = ((float.Parse(txtLAmt.Text) * 1.2) + ((days - 14)*500)).ToString();
                }
                else if(comboLType.SelectedItem.ToString() == "Emergency" && days >= 0)
                {
                    txtAmtPayable.Text = ((float.Parse(txtLAmt.Text)) + (float.Parse(txtLAmt.Text) * 0.1 * days)).ToString();
                }
                else if (comboLType.SelectedItem.ToString() == "Emergency" && days <= 0)
                {
                    txtAmtPayable.Text = ((float.Parse(txtLAmt.Text)) + (float.Parse(txtLAmt.Text) * 0.1 * 1)).ToString();
                }
            }
                        
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.Close();
            new View_Loans().Show();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
