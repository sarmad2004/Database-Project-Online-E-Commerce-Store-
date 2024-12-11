using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DB_pro
{
    public partial class login : Form
    {
        OracleConnection con;
        public static login instance;
        public login()
        {
            InitializeComponent();
            instance = this;
        }

        private void login_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2;PASSWORD=123";
            con = new OracleConnection(conStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                // SQL Query to verify login credentials and get the UserID
                string query = "SELECT UserID, Role FROM Users WHERE Email = :Email AND Password = :Password";

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(new OracleParameter("Email", OracleDbType.Varchar2) { Value = textBox1.Text.Trim() });
                    cmd.Parameters.Add(new OracleParameter("Password", OracleDbType.Varchar2) { Value = textBox2.Text.Trim() });

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(0); // Get UserID
                            string role = reader.GetString(1); // Get Role

                            MessageBox.Show($"Login successful! Welcome, {role}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (role == "Manager")
                            {
                                Manager adminForm = new Manager();
                                adminForm.Show();
                            }
                            else if (role == "Customer")
                            {
                                
                                Customer customerForm = new Customer(userId);
                                customerForm.Show();
                            }
                            else if (role == "Cashier")
                            {

                                Cashier customerForm = new Cashier();
                                customerForm.Show();
                            }
                            else if (role == "Delivery Man")
                            {

                                DeliveryMan customerForm = new DeliveryMan();
                                customerForm.Show();
                            }
                            else if (role == "Admin")
                            {

                                Admin customerForm = new Admin();
                                customerForm.Show();
                            }
                            this.Hide(); // Hide the login form
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
