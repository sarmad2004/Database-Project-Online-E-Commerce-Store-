using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace DB_pro
{
    public partial class ProDetails : Form
    {
        public static ProDetails instance;
        private int userId;
        public ProDetails(int Uid)
        {
            InitializeComponent();
            instance = this;
            userId = Uid;
        }
        private string connectionString = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2;PASSWORD=123";
        private void ProDetails_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductID, Name, Price, Description, StockQuantity FROM Products";
                    OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxStock_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxQuantity.Text) || int.Parse(textBoxQuantity.Text) <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 0) // Check if a row is selected
            {
                int productId = int.Parse(dataGridView1.SelectedRows[0].Cells["ProductID"].Value.ToString());
                int userId = 1; // Replace with the logged-in user's ID
                if (int.TryParse(textBoxQuantity.Text, out int quantity) && quantity > 0) // Validate quantity input
                {
                    // Proceed with adding to cart
                    using (OracleConnection conn = new OracleConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();

                            // Check stock availability
                            string stockQuery = "SELECT StockQuantity FROM Products WHERE ProductID = :ProductID";
                            OracleCommand stockCmd = new OracleCommand(stockQuery, conn);
                            stockCmd.Parameters.Add(new OracleParameter("ProductID", productId));
                            int stock = Convert.ToInt32(stockCmd.ExecuteScalar());

                            if (quantity > stock)
                            {
                                MessageBox.Show("Quantity exceeds available stock.");
                                return;
                            }

                            // Insert into ShoppingCart
                            //string insertQuery = "INSERT INTO ShoppingCart (CartID, UserID, Quantity) VALUES (SEQ_CARTID.NEXTVAL, :UserID, :Quantity)";
                            //OracleCommand insertCmd = new OracleCommand(insertQuery, conn);
                            //insertCmd.Parameters.Add(new OracleParameter("UserID", userId));
                            //insertCmd.Parameters.Add(new OracleParameter("Quantity", quantity));
                            //insertCmd.ExecuteNonQuery();

                            //// Update stock quantity in Products table
                            //string updateStockQuery = "UPDATE Products SET StockQuantity = StockQuantity - :Quantity WHERE ProductID = :ProductID";
                            //OracleCommand updateStockCmd = new OracleCommand(updateStockQuery, conn);
                            //updateStockCmd.Parameters.Add(new OracleParameter("Quantity", quantity));
                            //updateStockCmd.Parameters.Add(new OracleParameter("ProductID", productId));
                            //updateStockCmd.ExecuteNonQuery();

                            //MessageBox.Show("Product added to cart successfully.");
                            //// Refresh product list or perform other actions
                            ///
                            string query = "INSERT INTO ShoppingCart (CartID, UserID, ProductID, Quantity) " +
                       "VALUES (CartID_Seq.NEXTVAL, :UserID, :ProductID, :Quantity)";

                            // Create command and add parameters
                            OracleCommand cmd = new OracleCommand(query, conn);
                            cmd.Parameters.Add(new OracleParameter(":UserID", userId));
                            cmd.Parameters.Add(new OracleParameter(":ProductID", productId));
                            cmd.Parameters.Add(new OracleParameter(":Quantity", quantity));

                            // Execute the command to add the product to the cart
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Product added to the cart!");
                            this.Hide();
                            ShoppingCart form = new ShoppingCart(userId);
                            form.Show();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            }
            else
            {
                MessageBox.Show("Please select a product.");
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBoxName.Text = row.Cells["Name"].Value.ToString();
                textBoxPrice.Text = row.Cells["Price"].Value.ToString();
                textBoxDescription.Text = row.Cells["Description"].Value.ToString();
                textBoxStock.Text = row.Cells["StockQuantity"].Value.ToString();
            }
        }

        private void label5_Paint(object sender, PaintEventArgs e)
        {
            label5.BackColor = Color.Transparent;
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

        private void label4_Paint(object sender, PaintEventArgs e)
        {
            label4.BackColor = Color.Transparent;
            base.OnPaint(e);
        }
    }
}
