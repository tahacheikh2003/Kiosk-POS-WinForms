namespace Projet_Data_Base_Taha
{
    partial class RestockForm
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
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.btnSaveRestock = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(166, 135);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(121, 24);
            this.cmbSupplier.TabIndex = 0;
            // 
            // cmbProduct
            // 
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(186, 214);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(121, 24);
            this.cmbProduct.TabIndex = 1;
            // 
            // numQty
            // 
            this.numQty.Location = new System.Drawing.Point(447, 213);
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(120, 24);
            this.numQty.TabIndex = 2;
            // 
            // btnSaveRestock
            // 
            this.btnSaveRestock.Location = new System.Drawing.Point(557, 122);
            this.btnSaveRestock.Name = "btnSaveRestock";
            this.btnSaveRestock.Size = new System.Drawing.Size(75, 23);
            this.btnSaveRestock.TabIndex = 3;
            this.btnSaveRestock.Text = "SaveRestock";
            this.btnSaveRestock.UseVisualStyleBackColor = true;
            this.btnSaveRestock.Click += new System.EventHandler(this.btnSaveRestock_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(86, 213);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(42, 17);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "label1";
            // 
            // RestockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnSaveRestock);
            this.Controls.Add(this.numQty);
            this.Controls.Add(this.cmbProduct);
            this.Controls.Add(this.cmbSupplier);
            this.Name = "RestockForm";
            this.Text = "RestockForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.RestockForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Button btnSaveRestock;
        private System.Windows.Forms.Label lblInfo;
    }
}