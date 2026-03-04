namespace Projet_Data_Base_Taha
{
    partial class MainForm
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
            this.btnPOS = new System.Windows.Forms.Button();
            this.btnShift = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.btnSuppliers = new System.Windows.Forms.Button();
            this.btnStock = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnDailyReport = new System.Windows.Forms.Button();
            this.Restock = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPOS
            // 
            this.btnPOS.Location = new System.Drawing.Point(111, 45);
            this.btnPOS.Name = "btnPOS";
            this.btnPOS.Size = new System.Drawing.Size(122, 32);
            this.btnPOS.TabIndex = 0;
            this.btnPOS.Text = "Nouvelle Vente";
            this.btnPOS.UseVisualStyleBackColor = true;
            this.btnPOS.Click += new System.EventHandler(this.btnPOS_Click);
            // 
            // btnShift
            // 
            this.btnShift.Location = new System.Drawing.Point(111, 83);
            this.btnShift.Name = "btnShift";
            this.btnShift.Size = new System.Drawing.Size(122, 38);
            this.btnShift.TabIndex = 1;
            this.btnShift.Text = "Open/Close Shift";
            this.btnShift.UseVisualStyleBackColor = true;
            this.btnShift.Click += new System.EventHandler(this.btnShift_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.Location = new System.Drawing.Point(112, 127);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(121, 36);
            this.btnProducts.TabIndex = 2;
            this.btnProducts.Text = "Produits";
            this.btnProducts.UseVisualStyleBackColor = true;
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // btnSuppliers
            // 
            this.btnSuppliers.Location = new System.Drawing.Point(112, 169);
            this.btnSuppliers.Name = "btnSuppliers";
            this.btnSuppliers.Size = new System.Drawing.Size(122, 41);
            this.btnSuppliers.TabIndex = 3;
            this.btnSuppliers.Text = "Fournisseurs";
            this.btnSuppliers.UseVisualStyleBackColor = true;
            this.btnSuppliers.Click += new System.EventHandler(this.btnSuppliers_Click);
            // 
            // btnStock
            // 
            this.btnStock.Location = new System.Drawing.Point(112, 216);
            this.btnStock.Name = "btnStock";
            this.btnStock.Size = new System.Drawing.Size(122, 38);
            this.btnStock.TabIndex = 4;
            this.btnStock.Text = "Stock";
            this.btnStock.UseVisualStyleBackColor = true;
            this.btnStock.Click += new System.EventHandler(this.btnStock_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(112, 260);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(121, 44);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnDailyReport
            // 
            this.btnDailyReport.Location = new System.Drawing.Point(114, 335);
            this.btnDailyReport.Name = "btnDailyReport";
            this.btnDailyReport.Size = new System.Drawing.Size(119, 38);
            this.btnDailyReport.TabIndex = 6;
            this.btnDailyReport.Text = "DailySalesReport";
            this.btnDailyReport.UseVisualStyleBackColor = true;
            this.btnDailyReport.Click += new System.EventHandler(this.btnDailyReport_Click);
            // 
            // Restock
            // 
            this.Restock.Location = new System.Drawing.Point(132, 405);
            this.Restock.Name = "Restock";
            this.Restock.Size = new System.Drawing.Size(75, 23);
            this.Restock.TabIndex = 7;
            this.Restock.Text = "Restock";
            this.Restock.UseVisualStyleBackColor = true;
            this.Restock.Click += new System.EventHandler(this.Restock_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 473);
            this.Controls.Add(this.Restock);
            this.Controls.Add(this.btnDailyReport);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnStock);
            this.Controls.Add(this.btnSuppliers);
            this.Controls.Add(this.btnProducts);
            this.Controls.Add(this.btnShift);
            this.Controls.Add(this.btnPOS);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPOS;
        private System.Windows.Forms.Button btnShift;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnSuppliers;
        private System.Windows.Forms.Button btnStock;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnDailyReport;
        private System.Windows.Forms.Button Restock;
    }
}