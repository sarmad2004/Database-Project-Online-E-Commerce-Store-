using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace DB_pro
{
    
    public partial class OrderTracking : Form
    {
        public static OrderTracking instance;
        private int parsedUserId = -1;

        private int userId;
        private string connectionString = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2; PASSWORD=123";
        public OrderTracking(int Uid)
        {
            InitializeComponent();
            instance=this;
            this.userId = Uid;
        }
       

        private void OrderTracking_Load(object sender, EventArgs e)
        {
            
        }

       

        private void button1_Click(object sender, EventArgs e)
        {

            if (!int.TryParse(txtUserId.Text.Trim(), out parsedUserId))
            {
                MessageBox.Show("Please enter a valid numeric User ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userId = txtUserId.Text.Trim(); // Assuming txtUserId is the TextBox for UserID input
            string orderId = txtOrderId.Text.Trim(); // Assuming txtOrderId is the TextBox for OrderID input

            // Clear the ListBox before displaying new results
            lstOrders.Items.Clear();

            // Build the SQL query
            string query = "SELECT OrderID, CustomerID, TotalPrice, OrderStatus FROM Orders WHERE 1=1";

            // Add conditions based on input
            if (!string.IsNullOrEmpty(userId))
            {
                query += " AND CustomerID = :UserID";
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                query += " AND OrderID = :OrderID";
            }

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Create Oracle command
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        if (!string.IsNullOrEmpty(userId))
                        {
                            command.Parameters.Add(new OracleParameter(":UserID", OracleDbType.Varchar2) { Value = userId });
                        }
                        if (!string.IsNullOrEmpty(orderId))
                        {
                            command.Parameters.Add(new OracleParameter(":OrderID", OracleDbType.Varchar2) { Value = orderId });
                        }

                        // Execute query and load results into a DataTable
                        using (OracleDataAdapter dataAdapter = new OracleDataAdapter(command))
                        {
                            DataTable dtOrders = new DataTable();
                            dataAdapter.Fill(dtOrders);

                            // Display each order as a string in the ListBox
                            foreach (DataRow row in dtOrders.Rows)
                            {
                                string orderInfo = $"Order ID: {row["OrderID"]}, Customer ID: {row["CustomerID"]}, " +
                                                   $"Total Price: {row["TotalPrice"]}, Status: {row["OrderStatus"]}";
                                lstOrders.Items.Add(orderInfo); // Add the order info to the ListBox
                            }

                            // Show a message if no orders are found
                            if (dtOrders.Rows.Count == 0)
                            {
                                MessageBox.Show("No orders found for the given criteria.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rating customerForm = new Rating(userId);
            customerForm.Show();
            this.Hide();
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
    }
}
