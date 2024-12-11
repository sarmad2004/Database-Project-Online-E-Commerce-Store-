//using Oracle.ManagedDataAccess.Client;
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
using System.Security.Cryptography;

namespace DB_pro
{
    public partial class Customer : Form
    {
        public static Customer instance;
        //OracleConnection con;
        private int userId;
        public Customer(int Uid)
        {
            InitializeComponent( );
            instance = this;
            userId = Uid;
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            // Connection string
            string connectionString = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2;PASSWORD=123";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Base query
                    string query = "SELECT * FROM Products WHERE 1=1";

                    // Add filters based on user input
                    if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        query += " AND LOWER(Name) LIKE :Name";
                    }
                    if (!string.IsNullOrEmpty(textBox2.Text))
                    {
                        query += " AND LOWER(Category) LIKE :Category";
                    }
                    if (!string.IsNullOrEmpty(textBox3.Text))
                    {
                        query += " AND Price = :Price";
                    }

                    OracleCommand cmd = new OracleCommand(query, conn);

                    // Add parameters
                    if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        cmd.Parameters.Add(new OracleParameter("Name", "%" + textBox1.Text.ToLower() + "%"));
                    }
                    if (!string.IsNullOrEmpty(textBox2.Text))
                    {
                        cmd.Parameters.Add(new OracleParameter("Category", "%" + textBox2.Text.ToLower() + "%"));
                    }
                    if (!string.IsNullOrEmpty(textBox3.Text))
                    {
                        cmd.Parameters.Add(new OracleParameter("Price", decimal.Parse(textBox3.Text)));
                    }

                    // Execute the query and get results
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Clear ListBox before adding new items
                    listBoxProducts.Items.Clear();

                    // Add products to ListBox
                    foreach (DataRow row in dt.Rows)
                    {
                        string productInfo = $"ID: {row["ProductID"]}, Name: {row["Name"]}, " +
                                             $"Category: {row["Category"]}, Price: {row["Price"]}";
                        listBoxProducts.Items.Add(productInfo);
                    }

                    // Show a message if no results found
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No products found with the given criteria.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProDetails form= new ProDetails(userId);
            form.Show();
        }
    }
}
