namespace Projet_Data_Base_Taha
{
    partial class StockForm
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.btnExportStock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(142, 155);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Reffresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvStock
            // 
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStock.Location = new System.Drawing.Point(369, 105);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersWidth = 51;
            this.dgvStock.RowTemplate.Height = 26;
            this.dgvStock.Size = new System.Drawing.Size(240, 150);
            this.dgvStock.TabIndex = 1;
            // 
            // btnExportStock
            // 
            this.btnExportStock.Location = new System.Drawing.Point(155, 223);
            this.btnExportStock.Name = "btnExportStock";
            this.btnExportStock.Size = new System.Drawing.Size(116, 50);
            this.btnExportStock.TabIndex = 2;
            this.btnExportStock.Text = "Print";
            this.btnExportStock.UseVisualStyleBackColor = true;
            this.btnExportStock.Click += new System.EventHandler(this.btnExportStock_Click);
            // 
            // StockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExportStock);
            this.Controls.Add(this.dgvStock);
            this.Controls.Add(this.btnRefresh);
            this.Name = "StockForm";
            this.Text = "StockForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StockForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.Button btnExportStock;
    }
}