namespace Projet_Data_Base_Taha
{
    partial class DailySalesReportForm
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
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.btnExportCsv = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(114, 53);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(267, 24);
            this.dtFrom.TabIndex = 0;
            this.dtFrom.ValueChanged += new System.EventHandler(this.dtFrom_ValueChanged);
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(99, 102);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(281, 24);
            this.dtTo.TabIndex = 1;
            this.dtTo.ValueChanged += new System.EventHandler(this.dtTo_ValueChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(108, 148);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(251, 32);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Done";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dgvReport
            // 
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Location = new System.Drawing.Point(105, 220);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.RowHeadersWidth = 51;
            this.dgvReport.RowTemplate.Height = 26;
            this.dgvReport.Size = new System.Drawing.Size(386, 146);
            this.dgvReport.TabIndex = 3;
            this.dgvReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReport_CellContentClick);
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Location = new System.Drawing.Point(141, 410);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(288, 24);
            this.btnExportCsv.TabIndex = 4;
            this.btnExportCsv.Text = "Print";
            this.btnExportCsv.UseVisualStyleBackColor = true;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // DailySalesReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExportCsv);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Name = "DailySalesReportForm";
            this.Text = "DailySalesReportForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DailySalesReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Button btnExportCsv;
    }
}