using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_pro
{
    public partial class SignUp : Form
    {
        public static SignUp instance;
        public SignUp()
        {
            InitializeComponent();
            instance = this;
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registration form = new Registration();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            login form = new login();
            form.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            label1.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            button1.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            button1.BackColor = Color.Transparent;
            base.OnPaint(e);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
