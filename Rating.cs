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
    
    public partial class Rating : Form
    {
        private int selectedRating = 0; // To store the selected star rating
        private OracleConnection con;
        public static Rating instance;
        private int userId;
        public Rating(int Uid)
        {
            InitializeComponent();
            InitializeConnection();
            InitializeStars();
            instance = this;
            this.userId = Uid;
        }

        private void InitializeConnection()
        {
            // Connection string (update with your DB credentials)
            string conStr = @"DATA SOURCE = localhost:1521/XE; USER ID=DB_A#2; PASSWORD=123";
            con = new OracleConnection(conStr);
        }

        private void InitializeStars()
        {
            // Add all PictureBox controls for the stars
            var stars = new[] { pbStar1, pbStar2, pbStar3, pbStar4, pbStar5 };

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].Tag = (i + 1).ToString(); // Assign a rating value (1-5) to each star
                stars[i].MouseHover += pbStar5_MouseHover;
                stars[i].MouseLeave += pbStar1_MouseLeave;
                stars[i].Click += pbStar1_Click;
            }
        }
        private void FeedbackForm_Load(object sender, EventArgs e)
        {
            // Reset stars to empty by default
            ResetStars();

            // Optional: Prepopulate Product IDs in a ComboBox or similar control
            // comboBoxProductID.DataSource = ...; // Fetch Product IDs from database
        }

        // Reset all stars to empty
        private void ResetStars()
        {
            pbStar1.Image = Properties.Resources.empty_star;
            pbStar2.Image = Properties.Resources.empty_star;
            pbStar3.Image = Properties.Resources.empty_star;
            pbStar4.Image = Properties.Resources.empty_star;
            pbStar5.Image = Properties.Resources.empty_star;
        }

        private void HighlightStars(int rating)
        {
            ResetStars();
            if (rating >= 1) pbStar1.Image = Properties.Resources.Star_full;
            if (rating >= 2) pbStar2.Image = Properties.Resources.Star_full;
            if (rating >= 3) pbStar3.Image = Properties.Resources.Star_full;
            if (rating >= 4) pbStar4.Image = Properties.Resources.Star_full;
            if (rating >= 5) pbStar5.Image = Properties.Resources.Star_full;
        }

        


        private void Rating_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate UserID
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                OracleCommand validateUserCmd = new OracleCommand(
                    "SELECT COUNT(*) FROM USERS WHERE USERID = :UserID", con);
                validateUserCmd.Parameters.Add(new OracleParameter("UserID", userId));
                int userExists = Convert.ToInt32(validateUserCmd.ExecuteScalar());

                if (userExists == 0)
                {
                    MessageBox.Show("Invalid User ID. Please ensure the user exists before submitting feedback.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error validating User ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Proceed with feedback submission logic
            if (selectedRating == 0)
            {
                MessageBox.Show("Please select a star rating before submitting feedback.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProductID.Text))
            {
                MessageBox.Show("Please enter a valid Product ID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtComments.Text))
            {
                MessageBox.Show("Please enter your feedback in the comments.");
                return;
            }

            try
            {
                string insertQuery = @"
            INSERT INTO FEEDBACK (FEEDBACKID, USERID, PRODUCTID, RATING, COMMENTS)
            VALUES (FeedbackSeq.NEXTVAL, :USERID, :ProductId, :Rating, :Comments)";

                OracleCommand cmd = new OracleCommand(insertQuery, con);
                cmd.Parameters.Add(new OracleParameter("USERID", userId));
                cmd.Parameters.Add(new OracleParameter("ProductId", int.Parse(txtProductID.Text)));
                cmd.Parameters.Add(new OracleParameter("Rating", selectedRating));
                cmd.Parameters.Add(new OracleParameter("Comments", txtComments.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Feedback submitted successfully!");

                // Reset the form after submission
                ResetStars();
                txtProductID.Clear();
                txtComments.Clear();
                lblSelectedRating.Text = "You rated: 0 stars";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting feedback: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


        private void Rating_Click(object sender, EventArgs e)
        {

        }

        private void pbStar4_Click(object sender, EventArgs e)
        {

        }

        private void pbStar3_Click(object sender, EventArgs e)
        {

        }

        private void pbStar2_Click(object sender, EventArgs e)
        {

        }

        private void pbStar5_Click(object sender, EventArgs e)
        {

        }

        private void pbStar1_Click(object sender, EventArgs e)
        {
            PictureBox star = sender as PictureBox;

            if (star != null && int.TryParse(star.Tag?.ToString(), out int rating))
            {
                selectedRating = rating;
                HighlightStars(selectedRating);

                // Update label with the selected rating
                lblSelectedRating.Text = $"You rated: {selectedRating} stars";
            }
        }

        private void pbStar5_MouseHover(object sender, EventArgs e)
        {
            PictureBox star = sender as PictureBox;

            if (star != null && int.TryParse(star.Tag?.ToString(), out int rating))
            {
                HighlightStars(rating);
            }
        }

        private void pbStar1_MouseLeave(object sender, EventArgs e)
        {
            HighlightStars(selectedRating);
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
