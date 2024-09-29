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
    public partial class GuarantorsReg : Form
    {
        public GuarantorsReg()
        {
            InitializeComponent();
        }

        //Database Connection code
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_users.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        void Clear()
        {
            {
                txtGID.Text = "";
                txtFname.Text = "";
                txtLname.Text = "";
                txtId.Text = "";
                txtPhone.Text = "";
                txtEmail.Text = "";
                txtmId.Focus();
            }
        }

        void Back()
        {
            //Back to members form
            this.Close();
            new Dashboard().Show();
        }

        //Populating Data Grid View
        void Populate()
        {
            try
            {
                con.Open();

                string display = "Select * from tbl_guarantor where member_id = '" + txtmId.Text + "'";
                cmd = new OleDbCommand(display, con);
                da = new OleDbDataAdapter(display, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                var ds = new DataSet();
                da.Fill(ds);
                GuarantorGV.DataSource = ds.Tables[0];

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Back();            
        }

        private void GuarantorsReg_Load(object sender, EventArgs e)
        {
            txtmId.Text = memberRegistration.passingText;
            Populate();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Add a Guarantor database 

            if (txtGID.Text == "" || txtFname.Text == "" || txtLname.Text == "" || txtId.Text == "" || txtPhone.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();

                    string save = "INSERT INTO tbl_guarantor VALUES ('" + txtGID.Text + "', '" + txtmId.Text + "', '" + txtFname.Text + "', '" + txtLname.Text + "', '" + txtId.Text + "', '" + txtPhone.Text + "', '" + txtEmail.Text + "')";
                    cmd = new OleDbCommand(save, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    //update grid view
                    Populate();

                    //Clear fields
                    Clear();

                    MessageBox.Show("New Guarantor Added Successfully", "Registration Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex);
                }

            }

        }

        private void TxtGID_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtGID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for numbers

            char ch = e.KeyChar;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void TxtmId_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void TxtFname_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for text
            char ch = e.KeyChar;

            if (!char.IsLetter(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void TxtLname_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for text
            char ch = e.KeyChar;

            if (!char.IsLetter(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void TxtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for numbers

            char ch = e.KeyChar;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void TxtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for numbers

            char ch = e.KeyChar;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        
        private void dGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtGID.Text = GuarantorGV.SelectedRows[0].Cells[0].Value.ToString();
            txtmId.Text = GuarantorGV.SelectedRows[0].Cells[1].Value.ToString();
            txtFname.Text = GuarantorGV.SelectedRows[0].Cells[2].Value.ToString();
            txtLname.Text = GuarantorGV.SelectedRows[0].Cells[3].Value.ToString();
            txtId.Text = GuarantorGV.SelectedRows[0].Cells[4].Value.ToString();
            txtPhone.Text = GuarantorGV.SelectedRows[0].Cells[5].Value.ToString();
            txtEmail.Text = GuarantorGV.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (txtmId.Text == "" || txtFname.Text == "" || txtLname.Text == "" || txtId.Text == "" || txtPhone.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill in all the details for the member to be updated", "Updation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (MessageBox.Show("Are you sure you want to update this record", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();

                        string update = "UPDATE tbl_guarantor SET guarantor_id = '" + txtGID.Text + "', member_ID = '" + txtmId.Text + "', f_name = '" + txtFname.Text + "', l_name = '" + txtLname.Text + "', phone_no = '" + txtPhone.Text + "', email = '" + txtEmail.Text + "' WHERE ID_no = " + txtId.Text + "";
                        cmd = new OleDbCommand(update, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                        //update grid view
                        Populate();

                        //Clear fields
                        Clear();

                        MessageBox.Show("Guarantor Details Updated Successfully", "Update Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Record Not Updated", "Update Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    txtmId.Focus();
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (txtmId.Text == "" || txtFname.Text == "" || txtLname.Text == "" || txtId.Text == "" || txtPhone.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill in all the details for the Guarantor to be deleted", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Delete guarantor record", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();

                        string delete = "DELETE FROM tbl_guarantor WHERE ID_no = " + txtId.Text + "";
                        cmd = new OleDbCommand(delete, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                        //update grid view
                        Populate();

                        //Clear fields
                        Clear();

                        MessageBox.Show("Guarantor Deleted Successfully", "Deletion Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Guarantor Not Deleted", "Deletion Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    txtmId.Focus();
                }
            }
        }

        private void txtmId_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            new memberRegistration().Show();
            this.Close();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
