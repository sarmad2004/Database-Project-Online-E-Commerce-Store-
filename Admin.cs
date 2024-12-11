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
    public partial class Admin : Form
    {
        OracleConnection con;
        public static Admin instance;
        public Admin()
        {
            InitializeComponent();
            instance = this;
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2; PASSWORD=123";
            con = new OracleConnection(conStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtProductID.Text.Trim(), out int productId))
            {
                MessageBox.Show("Invalid Product ID. Please enter a valid number.");
                return;
            }

            if (!int.TryParse(txtDiscountID.Text.Trim(), out int discountId))
            {
                MessageBox.Show("Invalid Discount ID. Please enter a valid number.");
                return;
            }

            try
            {
                con.Open();

                // Update the ProductID in the Discount table
                string updateDiscountQuery = "UPDATE DISCOUNT SET PRODUCTID = :ProductID WHERE DISCOUNTID = :DiscountID";
                using (OracleCommand updateCmd = new OracleCommand(updateDiscountQuery, con))
                {
                    updateCmd.Parameters.Add(new OracleParameter("ProductID", productId));
                    updateCmd.Parameters.Add(new OracleParameter("DiscountID", discountId));

                    int rowsUpdated = updateCmd.ExecuteNonQuery();
                    if (rowsUpdated == 0)
                    {
                        MessageBox.Show("No discount record found with the given Discount ID.");
                        return;
                    }
                }

                // Retrieve Product Name, Discount Percentage, and calculate new price
                string selectQuery = @"
                    SELECT P.NAME, P.PRICE, D.DISCOUNTPERCENTAGE
                    FROM PRODUCTS P
                    JOIN DISCOUNT D ON P.PRODUCTID = D.PRODUCTID
                    WHERE D.DISCOUNTID = :DiscountID";

                string productName = "";
                decimal originalPrice = 0, discountPercentage = 0, newPrice = 0;

                using (OracleCommand selectCmd = new OracleCommand(selectQuery, con))
                {
                    selectCmd.Parameters.Add(new OracleParameter("DiscountID", discountId));
                    using (OracleDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            productName = reader.GetString(0);
                            originalPrice = reader.GetDecimal(1);
                            discountPercentage = reader.GetDecimal(2);

                            // Calculate the new price after discount
                            newPrice = originalPrice - (originalPrice * discountPercentage / 100);
                        }
                        else
                        {
                            MessageBox.Show("No product associated with this Discount ID.");
                            return;
                        }
                    }
                }

                // Update the new price in the PRODUCTS table
                string updateProductQuery = "UPDATE PRODUCTS SET PRICE = :NewPrice WHERE PRODUCTID = :ProductID";
                using (OracleCommand updateProductCmd = new OracleCommand(updateProductQuery, con))
                {
                    updateProductCmd.Parameters.Add(new OracleParameter("NewPrice", newPrice));
                    updateProductCmd.Parameters.Add(new OracleParameter("ProductID", productId));
                    updateProductCmd.ExecuteNonQuery();
                }

                // Display the details in the ListBox
                listBoxDiscountDetails.Items.Add($"Product: {productName}");
                listBoxDiscountDetails.Items.Add($"Original Price: {originalPrice:C}");
                listBoxDiscountDetails.Items.Add($"Discount: {discountPercentage}%");
                listBoxDiscountDetails.Items.Add($"New Price: {newPrice:C}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void txtProductID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
