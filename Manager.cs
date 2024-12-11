using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB_pro
{
    public partial class Manager : Form
    {
        private OracleDataAdapter adapter;
        private DataTable productTable;
        private OracleConnection con;
        public static Manager instance;
        public Manager()
        {
            InitializeComponent();
            instance = this;
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2; PASSWORD=123";
            con = new OracleConnection(conStr);

            LoadInventory();
        }

        private void LoadInventory()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string query = "SELECT * FROM Products";
                adapter = new OracleDataAdapter(query, con);

                // Create a CommandBuilder to enable saving changes back to the database
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);

                productTable = new DataTable();
                adapter.Fill(productTable);

                dataGridView1.DataSource = productTable;

                // Auto-adjust column widths
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string query = @"
                    INSERT INTO Products (ProductID, Name, Description, Category, Price, StockQuantity)
                    VALUES (ProductSeq.NEXTVAL, :Name, :Description, :Category, :Price, :StockQuantity)";

                OracleCommand cmd = new OracleCommand(query, con);
                cmd.Parameters.Add(new OracleParameter("Name", textBoxName.Text));
                cmd.Parameters.Add(new OracleParameter("Description", textBoxDescription.Text));
                cmd.Parameters.Add(new OracleParameter("Category", textBox1.Text));
                cmd.Parameters.Add(new OracleParameter("Price", Convert.ToDecimal(textBoxPrice.Text)));
                cmd.Parameters.Add(new OracleParameter("StockQuantity", Convert.ToInt32(textBoxStock.Text)));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Product added successfully!");

                // Refresh the inventory
                LoadInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {


            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to edit.");
                return;
            }

            // Populate the input fields with the selected row's data
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            textBoxName.Text = selectedRow.Cells["Name"].Value.ToString();
            textBoxDescription.Text = selectedRow.Cells["Description"].Value.ToString();
            textBox1.Text = selectedRow.Cells["Category"].Value.ToString();
            textBoxPrice.Text = selectedRow.Cells["Price"].Value.ToString();
            textBoxStock.Text = selectedRow.Cells["StockQuantity"].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

                string query = "DELETE FROM Products WHERE ProductID = :ProductID";

                OracleCommand cmd = new OracleCommand(query, con);
                cmd.Parameters.Add(new OracleParameter("ProductID", productId));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Product deleted successfully!");

                // Refresh the inventory
                LoadInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                adapter.Update(productTable);
                MessageBox.Show("Changes saved successfully!");
                LoadInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
