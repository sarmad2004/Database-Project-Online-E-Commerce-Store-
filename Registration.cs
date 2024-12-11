using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace DB_pro
{
    public partial class Registration : Form
    {
        OracleConnection con;
        public static Registration instance;    
        public Registration()
        {
            InitializeComponent();
            instance = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2;PASSWORD=123";
            con = new OracleConnection(conStr);
          //   updateGrid();
        }

        //private void updateGrid()
        //{
        //    con.Open();
        //    OracleCommand getEmps = con.CreateCommand();
        //    getEmps.CommandText = "SELECT * FROM Users";
        //    getEmps.CommandType = CommandType.Text;
        //    OracleDataReader empDR = getEmps.ExecuteReader();
        //    DataTable empDT = new DataTable();
        //    empDT.Load(empDR);
        //    dataGridView1.DataSource = empDT;
        //    con.Close();
        //}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            
                try
                {
                    con.Open();

                    // Trim inputs and validate Role
                    string roleInput = textBox3.Text.Trim();
                    if (roleInput != "Customer" && roleInput != "Manager")
                    {
                        MessageBox.Show("Invalid Role. Allowed values are 'Customer' or 'Manager'.");
                        return; // Exit the method if the Role is invalid
                    }

                    // Prepare the insert command with parameters
                    OracleCommand insertEmp = con.CreateCommand();
                    insertEmp.CommandText = "INSERT INTO USERS (UserID, Email, Password, Role) VALUES (:UserID, :Email, :Password, :Role)";

                    // Add parameters
                    insertEmp.Parameters.Add(new OracleParameter("UserID", OracleDbType.Int32) { Value = Convert.ToInt32(maskedTextBox1.Text.Trim()) });
                    insertEmp.Parameters.Add(new OracleParameter("Email", OracleDbType.Varchar2) { Value = textBox1.Text.Trim() });
                    insertEmp.Parameters.Add(new OracleParameter("Password", OracleDbType.Varchar2) { Value = textBox2.Text.Trim() });
                    insertEmp.Parameters.Add(new OracleParameter("Role", OracleDbType.Varchar2) { Value = roleInput });

                    // Execute the query
                    int rows = insertEmp.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Registration Successful");
                        this.Hide();
                        login form = new login();
                        form.Show();
                    }
                    else
                    {
                        MessageBox.Show("Registration Failed");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
           

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            login form = new login();
            form.Show();
        }

        private void label4_Paint(object sender, PaintEventArgs e)
        {
            label4.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            label1.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void label2_Paint(object sender, PaintEventArgs e)
        {
            label2.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void label3_Paint(object sender, PaintEventArgs e)
        {
            label3.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
