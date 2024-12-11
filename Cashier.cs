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
    public partial class Cashier : Form
    {
        private OracleConnection con;
        public static Cashier instance;
        public Cashier()
        {
            InitializeComponent();
            instance = this;
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2; PASSWORD=123";
            con = new OracleConnection(conStr);
        }

        private void Cashier_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int orderId;
            if (!int.TryParse(textBox1.Text, out orderId))
            {
                MessageBox.Show("Please enter a valid Order ID.");
                return;
            }

            try
            {
                con.Open();

                // Query to fetch the order items along with product details
                string query = @"
                    SELECT P.NAME, OI.QUANTITY, OI.SUBTOTAL
                    FROM ORDERITEMS OI
                    JOIN PRODUCTS P ON OI.PRODUCTID = P.PRODUCTID
                    WHERE OI.ORDERID = :OrderID";

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(new OracleParameter("OrderID", orderId));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        Order_Receipt.Items.Clear();
                        decimal totalSubtotal = 0;

                        while (reader.Read())
                        {
                            string productName = reader.GetString(0);
                            int quantity = reader.GetInt32(1);
                            decimal subtotal = reader.GetDecimal(2);
                            totalSubtotal += subtotal;

                            // Display product details in ListBox
                            string itemDetails = $"{productName} (x{quantity}) - Subtotal: {subtotal:C2}";
                            Order_Receipt.Items.Add(itemDetails);
                        }

                        // Display total subtotal in a label
                        labelTotal.Text = $"Total: {totalSubtotal:C2}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching order details: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            label1.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void labelTotal_Paint(object sender, PaintEventArgs e)
        {
            labelTotal.BackColor = Color.Transparent;
            base.OnPaint(e);
        }
    }
}
