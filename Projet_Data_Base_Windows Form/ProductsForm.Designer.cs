namespace Projet_Data_Base_Taha
{
    partial class ProductsForm
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
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.cmbSuppliers = new System.Windows.Forms.ComboBox();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.chkActif = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.txtPrixVente = new System.Windows.Forms.TextBox();
            this.txtStockMin = new System.Windows.Forms.TextBox();
            this.btnPrintPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProducts
            // 
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(0, 0);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowTemplate.Height = 26;
            this.dgvProducts.Size = new System.Drawing.Size(240, 150);
            this.dgvProducts.TabIndex = 0;
            this.dgvProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellContentClick);
            // 
            // cmbSuppliers
            // 
            this.cmbSuppliers.FormattingEnabled = true;
            this.cmbSuppliers.Location = new System.Drawing.Point(50, 266);
            this.cmbSuppliers.Name = "cmbSuppliers";
            this.cmbSuppliers.Size = new System.Drawing.Size(121, 24);
            this.cmbSuppliers.TabIndex = 1;
            // 
            // cmbCategories
            // 
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(50, 227);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(121, 24);
            this.cmbCategories.TabIndex = 2;
            // 
            // chkActif
            // 
            this.chkActif.AutoSize = true;
            this.chkActif.Location = new System.Drawing.Point(142, 170);
            this.chkActif.Name = "chkActif";
            this.chkActif.Size = new System.Drawing.Size(66, 21);
            this.chkActif.TabIndex = 3;
            this.chkActif.Text = "active";
            this.chkActif.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(597, 112);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(597, 240);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExportExcel.TabIndex = 5;
            this.btnExportExcel.Text = "ExportExcel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(597, 211);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(597, 182);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(597, 141);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtNom
            // 
            this.txtNom.Location = new System.Drawing.Point(368, 168);
            this.txtNom.Name = "txtNom";
            this.txtNom.Size = new System.Drawing.Size(100, 24);
            this.txtNom.TabIndex = 9;
            this.txtNom.Text = "Nom";
            // 
            // txtPrixVente
            // 
            this.txtPrixVente.Location = new System.Drawing.Point(368, 210);
            this.txtPrixVente.Name = "txtPrixVente";
            this.txtPrixVente.Size = new System.Drawing.Size(100, 24);
            this.txtPrixVente.TabIndex = 10;
            this.txtPrixVente.Text = "Prix vente";
            // 
            // txtStockMin
            // 
            this.txtStockMin.Location = new System.Drawing.Point(368, 257);
            this.txtStockMin.Name = "txtStockMin";
            this.txtStockMin.Size = new System.Drawing.Size(100, 24);
            this.txtStockMin.TabIndex = 11;
            this.txtStockMin.Text = "StockMin";
            // 
            // btnPrintPDF
            // 
            this.btnPrintPDF.Location = new System.Drawing.Point(597, 278);
            this.btnPrintPDF.Name = "btnPrintPDF";
            this.btnPrintPDF.Size = new System.Drawing.Size(75, 23);
            this.btnPrintPDF.TabIndex = 12;
            this.btnPrintPDF.Text = "PrintPDF";
            this.btnPrintPDF.UseVisualStyleBackColor = true;
            this.btnPrintPDF.Click += new System.EventHandler(this.btnPrintPDF_Click);
            // 
            // ProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnPrintPDF);
            this.Controls.Add(this.txtStockMin);
            this.Controls.Add(this.txtPrixVente);
            this.Controls.Add(this.txtNom);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.chkActif);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.cmbSuppliers);
            this.Controls.Add(this.dgvProducts);
            this.Name = "ProductsForm";
            this.Text = "ProductsForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ProductsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.ComboBox cmbSuppliers;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.CheckBox chkActif;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.TextBox txtPrixVente;
        private System.Windows.Forms.TextBox txtStockMin;
        private System.Windows.Forms.Button btnPrintPDF;
    }
}