namespace DB_pro
{
    partial class Admin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Admin));
            this.label1 = new System.Windows.Forms.Label();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtDiscountID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxDiscountDetails = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(783, 120);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Product ID";
            // 
            // txtProductID
            // 
            this.txtProductID.Location = new System.Drawing.Point(930, 115);
            this.txtProductID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.Size = new System.Drawing.Size(148, 26);
            this.txtProductID.TabIndex = 1;
            this.txtProductID.TextChanged += new System.EventHandler(this.txtProductID_TextChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Menu;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(930, 500);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 63);
            this.button1.TabIndex = 2;
            this.button1.Text = "Apply Discount";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtDiscountID
            // 
            this.txtDiscountID.Location = new System.Drawing.Point(930, 200);
            this.txtDiscountID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDiscountID.Name = "txtDiscountID";
            this.txtDiscountID.Size = new System.Drawing.Size(148, 26);
            this.txtDiscountID.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(783, 211);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Discount ID";
            // 
            // listBoxDiscountDetails
            // 
            this.listBoxDiscountDetails.FormattingEnabled = true;
            this.listBoxDiscountDetails.ItemHeight = 20;
            this.listBoxDiscountDetails.Location = new System.Drawing.Point(114, 89);
            this.listBoxDiscountDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxDiscountDetails.Name = "listBoxDiscountDetails";
            this.listBoxDiscountDetails.Size = new System.Drawing.Size(486, 304);
            this.listBoxDiscountDetails.TabIndex = 5;
            // 
            // Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.listBoxDiscountDetails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDiscountID);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtProductID);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Admin";
            this.Text = "q";
            this.Load += new System.EventHandler(this.Admin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtDiscountID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxDiscountDetails;
    }
}