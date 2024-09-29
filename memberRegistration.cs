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
    public partial class memberRegistration : Form
    {
        public static string passingText;
        public memberRegistration()
        {
            InitializeComponent();
           
        }

        //Database Connection code
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_users.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        //Clear Fields
        void Clear()
        {
            {
                txtmId.Text = "";
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
            this.Close();
            var dash = new Dashboard();
            dash.Show();
        }

        //Grid populate function
        void Populate()
        {
            try
            {
                con.Open();

                string display = "Select * from tbl_members";
                cmd = new OleDbCommand(display, con);
                da = new OleDbDataAdapter(display, con);
                OleDbDataReader dr = cmd.ExecuteReader();

                var ds = new DataSet();
                da.Fill(ds);
                membersGV.DataSource = ds.Tables[0];

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        //Populate TextBox from Gridview selected record
        private void dGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmId.Text = membersGV.SelectedRows[0].Cells[0].Value.ToString();
            txtFname.Text = membersGV.SelectedRows[0].Cells[1].Value.ToString();
            txtLname.Text = membersGV.SelectedRows[0].Cells[2].Value.ToString();
            txtId.Text = membersGV.SelectedRows[0].Cells[3].Value.ToString();
            txtPhone.Text = membersGV.SelectedRows[0].Cells[4].Value.ToString();
            txtEmail.Text = membersGV.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Add a new memberto database 

            if (txtmId.Text == "" || txtFname.Text == "" || txtLname.Text == "" || txtId.Text == "" || txtPhone.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                try
                {
                    con.Open();

                    string save = "INSERT INTO tbl_members VALUES ('" + txtmId.Text + "', '" + txtFname.Text + "', '" + txtLname.Text + "', '" + txtId.Text + "', '" + txtPhone.Text + "', '" + txtEmail.Text + "')";
                    cmd = new OleDbCommand(save, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    //update grid view
                    Populate();

                    //Clear fields
                    Clear();

                    MessageBox.Show("New member Added Successfully", "Registration Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                                        
                }
                catch (Exception ex) {
                    MessageBox.Show("Error" + ex);
                }
                
            }
                       
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtmId.Text == "" || txtFname.Text == "" || txtLname.Text == "" || txtId.Text == "" || txtPhone.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill in all the details for the member to be deleted", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Delete this record", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        /*
                       cmd = con.CreateCommand();
                       cmd.CommandType = CommandType.Text;
                       cmd.CommandText = "DELETE FROM tbl_members WHERE member_ID = " + txtmId.Text + "";
                       cmd.ExecuteNonQuery();
                         */

                        string delete = "DELETE FROM tbl_members WHERE member_ID = " + txtmId.Text + "";
                        cmd = new OleDbCommand(delete, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                        //update grid view
                        Populate();

                        //Clear fields
                        Clear();

                        MessageBox.Show("Member Deleted Successfully", "Deletion Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Record Not Deleted", "Deletion Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    txtmId.Focus();
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void txtmId_TextChanged(object sender, EventArgs e)
        {

        }

        private void memberRegistration_Load(object sender, EventArgs e)
        {
            Populate();
        }

               

        private void txtmId_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for numbers
            char ch = e.KeyChar;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }

        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for numbers
            char ch = e.KeyChar;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for numbers

            char ch = e.KeyChar;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtFname_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for text
            char ch = e.KeyChar;

            if (!char.IsLetter(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtLname_KeyPress(object sender, KeyPressEventArgs e)
        {
            // input validation for text
            char ch = e.KeyChar;

            if (!char.IsLetter(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        
        private void button2_Click(object sender, EventArgs e)
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

                        string update = "UPDATE tbl_members SET f_name = '" + txtFname.Text + "', l_name = '" + txtLname.Text + "', ID_no = '" + txtId.Text + "', phone_no = '" + txtPhone.Text + "', email = '" + txtEmail.Text + "' WHERE member_ID = '" + txtmId.Text + "'";
                        cmd = new OleDbCommand(update, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                        //update grid view
                        Populate();

                        //Clear fields
                        Clear();

                        MessageBox.Show("Member Details Updated Successfully", "Update Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);


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

        private void Button4_Click(object sender, EventArgs e)
        {
            if (txtmId.Text == "")
            {
                MessageBox.Show("Please Select the member for whom a guarantor is to be added", "Guarantor Addition Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                //passing memberID to Guarantor's form
                passingText = txtmId.Text;
                GuarantorsReg pass = new GuarantorsReg();
                pass.Show();
                this.Hide();
            }
        }

        private void TxtFname_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtLname_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Back();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
