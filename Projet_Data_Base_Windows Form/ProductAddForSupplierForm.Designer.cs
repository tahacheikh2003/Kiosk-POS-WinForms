namespace Projet_Data_Base_Taha
{
    partial class ProductAddForSupplierForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.chkActif = new System.Windows.Forms.CheckBox();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.txtPrix = new System.Windows.Forms.TextBox();
            this.txtStockMin = new System.Windows.Forms.TextBox();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(468, 156);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 94);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // chkActif
            // 
            this.chkActif.AutoSize = true;
            this.chkActif.Location = new System.Drawing.Point(45, 229);
            this.chkActif.Name = "chkActif";
            this.chkActif.Size = new System.Drawing.Size(67, 21);
            this.chkActif.TabIndex = 1;
            this.chkActif.Text = "Active";
            this.chkActif.UseVisualStyleBackColor = true;
            // 
            // txtNom
            // 
            this.txtNom.Location = new System.Drawing.Point(123, 65);
            this.txtNom.Name = "txtNom";
            this.txtNom.Size = new System.Drawing.Size(169, 24);
            this.txtNom.TabIndex = 2;
            // 
            // txtPrix
            // 
            this.txtPrix.Location = new System.Drawing.Point(137, 118);
            this.txtPrix.Name = "txtPrix";
            this.txtPrix.Size = new System.Drawing.Size(154, 24);
            this.txtPrix.TabIndex = 3;
            // 
            // txtStockMin
            // 
            this.txtStockMin.Location = new System.Drawing.Point(138, 164);
            this.txtStockMin.Name = "txtStockMin";
            this.txtStockMin.Size = new System.Drawing.Size(163, 24);
            this.txtStockMin.TabIndex = 4;
            // 
            // cmbCategories
            // 
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(105, 299);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(164, 24);
            this.cmbCategories.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Nom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Prix";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Stock MIn";
            // 
            // ProductAddForSupplierForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.txtStockMin);
            this.Controls.Add(this.txtPrix);
            this.Controls.Add(this.txtNom);
            this.Controls.Add(this.chkActif);
            this.Controls.Add(this.btnSave);
            this.Name = "ProductAddForSupplierForm";
            this.Text = "ProductAddForSupplierForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ProductAddForSupplierForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkActif;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.TextBox txtPrix;
        private System.Windows.Forms.TextBox txtStockMin;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}