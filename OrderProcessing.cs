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
    public partial class OrderProcessing : Form
    {
        OracleConnection con;
        public static OrderProcessing instance;
        private int userId;
        private DataTable cartData;
        public OrderProcessing(int Uid, DataTable cartItems)
        {
            InitializeComponent();
            instance = this;
            this.userId = Uid;
            this.cartData = cartItems;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2; PASSWORD=123";
            con = new OracleConnection(conStr);

            // Populate the payment method dropdown
            comboBox1.Items.Add("Credit Card");
            comboBox1.Items.Add("Debit Card");
            comboBox1.Items.Add("Cash");
            comboBox1.Items.Add("Online");
            comboBox1.SelectedIndex = 0; // Default selection
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList; // Prevent manual input

            // Load order summary
            LoadOrderSummary();
        }


        
        private void LoadOrderSummary()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string query = @"
            SELECT 
                P.Name AS ProductName,
                SC.Quantity AS Quantity,
                (P.Price * SC.Quantity) AS SubTotal
            FROM SHOPPINGCART SC
            INNER JOIN PRODUCTS P ON SC.ProductID = P.ProductID
            WHERE SC.UserID = :UserID";

                OracleCommand cmd = new OracleCommand(query, con);
                cmd.Parameters.Add(new OracleParameter("UserID", userId)); // Replace with actual user ID

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable cartTable = new DataTable();
                adapter.Fill(cartTable);

                if (cartTable.Rows.Count == 0)
                {
                    textBox1.Text = "Your cart is empty!";
                    label3.Text = "Total Price: $0.00";
                    return;
                }

                decimal totalPrice = 0;
                textBox1.Clear();
                foreach (DataRow row in cartTable.Rows)
                {
                    string productName = row["ProductName"].ToString();
                    int quantity = Convert.ToInt32(row["Quantity"]);
                    decimal subTotal = Convert.ToDecimal(row["SubTotal"]);
                    totalPrice += subTotal;

                   // textBox1.AppendText($"Product: {productName}, Quantity: {quantity}, Subtotal: {subTotal:C2}\n");
                    textBox1.AppendText($"Product: {productName}, Quantity: {quantity}, Subtotal: {subTotal:C2}\n");

                }

                label3.Text = $"Total Price: {totalPrice:C2}";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order summary: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Calculate subtotal, final price, and fetch payment method
                decimal basePrice = GetTotalPrice(); // Subtotal
                decimal finalPrice = CalculateFinalPrice(basePrice); // Total including taxes
                string paymentMethod = comboBox1.SelectedItem.ToString();

                // Display the order summary in the ListBox
                DisplayOrderSummaryInListBox(basePrice, finalPrice, paymentMethod);

                // Insert the order into the Orders table
                string insertOrderQuery = @"
            INSERT INTO Orders (OrderID, CustomerID, TotalPrice, OrderStatus, OrderDate)
            VALUES (OrderSeq.NEXTVAL, :CustomerID, :TotalPrice, 'Pending', SYSDATE)";

                OracleCommand insertOrderCmd = new OracleCommand(insertOrderQuery, con);
                insertOrderCmd.Parameters.Add(new OracleParameter("CustomerID", userId)); // Replace with actual user ID
                insertOrderCmd.Parameters.Add(new OracleParameter("TotalPrice", finalPrice));
                insertOrderCmd.ExecuteNonQuery();

                // Retrieve the new OrderID
                string getOrderIdQuery = "SELECT OrderSeq.CURRVAL FROM DUAL";
                OracleCommand getOrderIdCmd = new OracleCommand(getOrderIdQuery, con);
                int orderId = Convert.ToInt32(getOrderIdCmd.ExecuteScalar());

                // Insert payment information
                string insertPaymentQuery = @"
            INSERT INTO Payment (PaymentID, OrderID, PaymentMethod, PaymentStatus)
            VALUES (PaymentSeq.NEXTVAL, :OrderID, :PaymentMethod, 'Paid')";

                OracleCommand insertPaymentCmd = new OracleCommand(insertPaymentQuery, con);
                insertPaymentCmd.Parameters.Add(new OracleParameter("OrderID", orderId));
                insertPaymentCmd.Parameters.Add(new OracleParameter("PaymentMethod", paymentMethod));
                insertPaymentCmd.ExecuteNonQuery();

                // Clear the shopping cart
                string clearCartQuery = "DELETE FROM SHOPPINGCART WHERE UserID = :UserID";
                OracleCommand clearCartCmd = new OracleCommand(clearCartQuery, con);
                clearCartCmd.Parameters.Add(new OracleParameter("UserID", userId));
                clearCartCmd.ExecuteNonQuery();

                MessageBox.Show("Order placed successfully!");

                // Redirect to the order tracking form
                OrderTracking customerForm = new OrderTracking(userId);
                customerForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private decimal CalculateFinalPrice(decimal basePrice)
        {
            const decimal taxRate = 0.10m; // 10% tax
            decimal taxAmount = basePrice * taxRate;
            return basePrice + taxAmount;
        }
        

        private decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (string line in textBox1.Lines)
            {
                if (line.Contains("Subtotal:"))
                {
                    try
                    {
                        // Extract the subtotal value using a safer method
                        string subTotalStr = line.Substring(line.LastIndexOf("Subtotal:") + 9).Trim();
                        subTotalStr = subTotalStr.Replace("$", ""); // Remove the dollar sign
                        totalPrice += Convert.ToDecimal(subTotalStr);
                    }
                    catch
                    {
                        MessageBox.Show($"Failed to parse line: {line}");
                    }
                }
            }

            return totalPrice;
        }

        private void DisplayOrderSummaryInListBox(decimal basePrice, decimal finalPrice, string paymentMethod)
        {
            // Clear the list box before populating
            OrderSummary.Items.Clear();

            // Add the order details to the ListBox
            foreach (string line in textBox1.Lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    OrderSummary.Items.Add(line); // Add each product line
                }
            }

            // Add payment method and amounts to the ListBox
            OrderSummary.Items.Add(""); // Blank line for spacing
            OrderSummary.Items.Add($"Payment Method: {paymentMethod}");
            OrderSummary.Items.Add($"Subtotal: {basePrice:C2}");
            OrderSummary.Items.Add($"Tax (10%): {(finalPrice - basePrice):C2}");
            OrderSummary.Items.Add($"Final Price: {finalPrice:C2}");
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
    }
}
