using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace DB_pro
{
    public partial class ShoppingCart : Form
    {
        OracleConnection con;
        public static ShoppingCart instance;
        private int userId;
        public ShoppingCart(int Uid)
        {
            InitializeComponent();
            instance = this;
            userId = Uid;
        }

        private void ShoppingCart_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2;PASSWORD=123";
            con = new OracleConnection(conStr);
            LoadCartItems();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PRODUCTID"].Value);
                int currentQuantity = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Quantity"].Value);
                UpdateQuantity(productId, currentQuantity + 1);
            }
            else
            {
                MessageBox.Show("Please select a product to increase the quantity.");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PRODUCTID"].Value);
                int currentQuantity = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Quantity"].Value);

                if (currentQuantity > 1) // Prevent quantity from going below 1
                {
                    UpdateQuantity(productId, currentQuantity - 1);
                }
                else
                {
                    MessageBox.Show("Quantity cannot be less than 1.");
                }
            }
            else
            {
                MessageBox.Show("Please select a product to decrease the quantity.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PRODUCTID"].Value);
                RemoveProduct(productId);
            }
            else
            {
                MessageBox.Show("Please select a product to remove.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProceedToCheckout();
            //this.Hide();
            //OrderProcessing form = new OrderProcessing(userId);
            //form.Show();
        }
        private void UpdateQuantity(int productId, int newQuantity)
        {
            //string connectionString = "Your Oracle Connection String Here";

            //using (OracleConnection conn = new OracleConnection(connectionString))
            //{
                try
                {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }



                string query = "UPDATE SHOPPINGCART SET QUANTITY = :Quantity WHERE PRODUCTID = :ProductID AND USERID = :UserID";


                OracleCommand cmd = new OracleCommand(query, con);
                    cmd.Parameters.Add(new OracleParameter("Quantity", newQuantity));
                    cmd.Parameters.Add(new OracleParameter("ProductID", productId));
                    cmd.Parameters.Add(new OracleParameter("UserID", userId)); // Replace with actual user ID

                    cmd.ExecuteNonQuery();
                    LoadCartItems(); // Refresh the cart
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating quantity: " + ex.Message);
                }
                finally
                {
                   con.Close(); // Ensure connection is closed even if an error occurs
                }
            //}
        }

        private void RemoveProduct(int productId)
        {
            
                try
                {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }


                string query = "DELETE FROM SHOPPINGCART WHERE PRODUCTID = :ProductID AND USERID = :UserID";


                OracleCommand cmd = new OracleCommand(query, con);
                    cmd.Parameters.Add(new OracleParameter("ProductID", productId));
                    cmd.Parameters.Add(new OracleParameter("UserID", userId)); // Replace with actual user ID

                    cmd.ExecuteNonQuery();
                    LoadCartItems(); // Refresh the cart
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing product: " + ex.Message);
                }
                finally
                {
                    con.Close(); // Ensure connection is closed even if an error occurs
                }
            
        }

        //private void ProceedToCheckout()
        //{

        //        try
        //        {
        //        if (con.State != ConnectionState.Open)
        //        {
        //            con.Open();
        //        }


        //        // Create a new order in the Orders table
        //        string orderQuery = "INSERT INTO Orders (OrderID, CUSTOMERID, OrderDate) VALUES (OrderSeq.NEXTVAL, :CUSTOMERID, SYSDATE)";
        //            OracleCommand orderCmd = new OracleCommand(orderQuery, con);
        //            orderCmd.Parameters.Add(new OracleParameter("UserID", userId)); // Replace with actual user ID
        //            orderCmd.ExecuteNonQuery();

        //            // Retrieve the new OrderID
        //            string getOrderIdQuery = "SELECT OrderSeq.CURRVAL FROM DUAL";
        //            OracleCommand getOrderIdCmd = new OracleCommand(getOrderIdQuery, con);
        //            int orderId = Convert.ToInt32(getOrderIdCmd.ExecuteScalar());

        //            // Insert all cart items into OrderItems table
        //            foreach (DataGridViewRow row in dataGridView1.Rows)
        //            {
        //                int productId = Convert.ToInt32(row.Cells["PRODUCTID"].Value);
        //                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
        //                decimal subtotal = Convert.ToDecimal(row.Cells["SubTotal"].Value);

        //                string insertOrderItemQuery = @"
        //            INSERT INTO OrderItems (OrderItemID, OrderID, ProductID, Quantity, SubTotal)
        //            VALUES (OrderItemSeq.NEXTVAL, :OrderID, :ProductID, :Quantity, :SubTotal)";

        //                OracleCommand insertCmd = new OracleCommand(insertOrderItemQuery, con);
        //                insertCmd.Parameters.Add(new OracleParameter("OrderID", orderId));
        //                insertCmd.Parameters.Add(new OracleParameter("ProductID", productId));
        //                insertCmd.Parameters.Add(new OracleParameter("Quantity", quantity));
        //                insertCmd.Parameters.Add(new OracleParameter("SubTotal", subtotal));
        //                insertCmd.ExecuteNonQuery();

        //            }

        //            // Clear the shopping cart
        //            string clearCartQuery = "DELETE FROM SHOPPINGCART WHERE USERID = :UserID";
        //            OracleCommand clearCartCmd = new OracleCommand(clearCartQuery, con);
        //            clearCartCmd.Parameters.Add(new OracleParameter("UserID", userId)); // Replace with actual user ID
        //            clearCartCmd.ExecuteNonQuery();

        //            MessageBox.Show("Checkout successful!");
        //            LoadCartItems(); // Refresh the cart
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error during checkout: " + ex.Message);
        //        }
        //        finally
        //        {
        //            con.Close(); // Ensure connection is closed even if an error occurs
        //        }

        //}

        private void ProceedToCheckout()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Create a DataTable to hold cart data
                string query = @"
            SELECT 
                SC.ProductID, 
                P.Name AS ProductName, 
                SC.Quantity, 
                (P.Price * SC.Quantity) AS SubTotal
            FROM SHOPPINGCART SC
            INNER JOIN PRODUCTS P ON SC.ProductID = P.ProductID
            WHERE SC.UserID = :UserID";

                OracleCommand cmd = new OracleCommand(query, con);
                cmd.Parameters.Add(new OracleParameter("UserID", userId));
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable cartTable = new DataTable();
                adapter.Fill(cartTable);

                // Pass the DataTable to the OrderProcessing form
                OrderProcessing orderForm = new OrderProcessing(userId, cartTable);
                orderForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error proceeding to checkout: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


        private void LoadCartItems()
        {
            
                try
                {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }


                string query = @"SELECT SC.CARTID, P.PRODUCTID,P.NAME AS ProductName,P.PRICE AS Price,SC.QUANTITY AS Quantity,
                               (P.PRICE * SC.QUANTITY) AS SubTotal
                               FROM SHOPPINGCART SC
                               INNER JOIN PRODUCTS P ON SC.PRODUCTID = P.PRODUCTID
                               WHERE SC.USERID = :UserID";


                OracleCommand cmd = new OracleCommand(query, con);
                    cmd.Parameters.Add(new OracleParameter("UserID", userId)); // Replace with the logged-in user's ID

                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataTable cartTable = new DataTable();
                    adapter.Fill(cartTable);

                    // Set the DataGridView's data source
                    dataGridView1.DataSource = cartTable;

                    // Optional: Adjust column headers for better readability
                    dataGridView1.Columns["ProductName"].HeaderText = "Product Name";
                    dataGridView1.Columns["Price"].HeaderText = "Price";
                    dataGridView1.Columns["Quantity"].HeaderText = "Quantity";
                    dataGridView1.Columns["SubTotal"].HeaderText = "Subtotal";

                    // Calculate total cost and display it in a label
                    decimal totalCost = 0;
                    foreach (DataRow row in cartTable.Rows)
                    {
                        totalCost += Convert.ToDecimal(row["SubTotal"]);
                    }

                    lblTotalCost.Text = "Total: $" + totalCost.ToString("F2");
                
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading cart items: " + ex.Message);
                }
            finally
            {
                con.Close(); // Ensure connection is closed even if an error occurs
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTotalCost_Paint(object sender, PaintEventArgs e)
        {
            lblTotalCost.BackColor = Color.Transparent;
            base.OnPaint(e);
        }
    }
}
