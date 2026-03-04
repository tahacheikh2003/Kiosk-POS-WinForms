namespace Projet_Data_Base_Taha
{
    partial class PosForm
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
            this.cmbProducts = new System.Windows.Forms.ComboBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSaveAndPrint = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbProducts
            // 
            this.cmbProducts.FormattingEnabled = true;
            this.cmbProducts.Location = new System.Drawing.Point(362, 81);
            this.cmbProducts.Name = "cmbProducts";
            this.cmbProducts.Size = new System.Drawing.Size(254, 24);
            this.cmbProducts.TabIndex = 0;
            this.cmbProducts.SelectedIndexChanged += new System.EventHandler(this.cmbProducts_SelectedIndexChanged);
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(362, 121);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.ReadOnly = true;
            this.txtPrice.Size = new System.Drawing.Size(256, 24);
            this.txtPrice.TabIndex = 1;
            // 
            // numQty
            // 
            this.numQty.Location = new System.Drawing.Point(362, 171);
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(243, 24);
            this.numQty.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(32, 462);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(203, 29);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "ADD";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvCart
            // 
            this.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCart.Location = new System.Drawing.Point(158, 243);
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.RowHeadersWidth = 51;
            this.dgvCart.RowTemplate.Height = 26;
            this.dgvCart.Size = new System.Drawing.Size(709, 200);
            this.dgvCart.TabIndex = 4;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(417, 494);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(42, 17);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "label1";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(653, 462);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(200, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Pour afficher les produits :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Prix du produit sélectionné";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Quantité";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Panier";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // btnSaveAndPrint
            // 
            this.btnSaveAndPrint.Location = new System.Drawing.Point(665, 496);
            this.btnSaveAndPrint.Name = "btnSaveAndPrint";
            this.btnSaveAndPrint.Size = new System.Drawing.Size(187, 29);
            this.btnSaveAndPrint.TabIndex = 12;
            this.btnSaveAndPrint.Text = "SaveAndPrint";
            this.btnSaveAndPrint.UseVisualStyleBackColor = true;
            this.btnSaveAndPrint.Click += new System.EventHandler(this.btnSaveAndPrint_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(35, 504);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(199, 28);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "Remove Item";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // PosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 537);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnSaveAndPrint);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dgvCart);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.numQty);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.cmbProducts);
            this.Name = "PosForm";
            this.Text = "PosForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PosForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbProducts;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSaveAndPrint;
        private System.Windows.Forms.Button btnRemove;
    }
}