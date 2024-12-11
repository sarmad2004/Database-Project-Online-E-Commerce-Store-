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
    public partial class DeliveryMan : Form
    {
        public static DeliveryMan instance;
        private OracleConnection con;
        public DeliveryMan()
        {
            InitializeComponent();
            InitializeConnection();
            instance = this;
        }

        private void InitializeConnection()
        {
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2; PASSWORD=123";
            con = new OracleConnection(conStr);
        }

        private void DeliveryMan_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserId.Text) || string.IsNullOrWhiteSpace(txtOrderId.Text))
            {
                MessageBox.Show("Please enter both User ID and Order ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int userId = int.Parse(txtUserId.Text);
                int orderId = int.Parse(txtOrderId.Text);

                con.Open();

                // Validate if the Order ID belongs to the given User ID
                string validationQuery = @"
                    SELECT COUNT(*) 
                    FROM ORDERS 
                    WHERE ORDERID = :OrderId AND CUSTOMERID = :UserId AND ORDERSTATUS = 'Pending'";

                using (OracleCommand validateCmd = new OracleCommand(validationQuery, con))
                {
                    validateCmd.Parameters.Add(new OracleParameter("OrderId", orderId));
                    validateCmd.Parameters.Add(new OracleParameter("UserId", userId));

                    int count = Convert.ToInt32(validateCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("No matching Pending order found for the given User ID and Order ID.",
                            "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Update Order Status
                string updateQuery = @"
                    UPDATE ORDERS
                    SET ORDERSTATUS = 'Completed'
                    WHERE ORDERID = :OrderId AND CUSTOMERID = :UserId";

                using (OracleCommand updateCmd = new OracleCommand(updateQuery, con))
                {
                    updateCmd.Parameters.Add(new OracleParameter("OrderId", orderId));
                    updateCmd.Parameters.Add(new OracleParameter("UserId", userId));

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Order status updated to 'Completed' successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update order status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please ensure User ID and Order ID are numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
