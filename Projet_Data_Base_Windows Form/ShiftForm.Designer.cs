namespace Projet_Data_Base_Taha
{
    partial class ShiftForm
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
            this.btnOpenShift = new System.Windows.Forms.Button();
            this.btnCloseShift = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtMontantReel = new System.Windows.Forms.TextBox();
            this.lblTheorique = new System.Windows.Forms.Label();
            this.lblEcart = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOpenShift
            // 
            this.btnOpenShift.Location = new System.Drawing.Point(99, 90);
            this.btnOpenShift.Name = "btnOpenShift";
            this.btnOpenShift.Size = new System.Drawing.Size(95, 25);
            this.btnOpenShift.TabIndex = 0;
            this.btnOpenShift.Text = "OpenShift";
            this.btnOpenShift.UseVisualStyleBackColor = true;
            this.btnOpenShift.Click += new System.EventHandler(this.btnOpenShift_Click);
            // 
            // btnCloseShift
            // 
            this.btnCloseShift.Location = new System.Drawing.Point(99, 143);
            this.btnCloseShift.Name = "btnCloseShift";
            this.btnCloseShift.Size = new System.Drawing.Size(93, 23);
            this.btnCloseShift.TabIndex = 1;
            this.btnCloseShift.Text = "CloseShift";
            this.btnCloseShift.UseVisualStyleBackColor = true;
            this.btnCloseShift.Click += new System.EventHandler(this.btnCloseShift_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(116, 278);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 17);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "label1";
            // 
            // txtMontantReel
            // 
            this.txtMontantReel.Location = new System.Drawing.Point(523, 91);
            this.txtMontantReel.Name = "txtMontantReel";
            this.txtMontantReel.Size = new System.Drawing.Size(147, 24);
            this.txtMontantReel.TabIndex = 3;
            // 
            // lblTheorique
            // 
            this.lblTheorique.AutoSize = true;
            this.lblTheorique.Location = new System.Drawing.Point(116, 355);
            this.lblTheorique.Name = "lblTheorique";
            this.lblTheorique.Size = new System.Drawing.Size(42, 17);
            this.lblTheorique.TabIndex = 4;
            this.lblTheorique.Text = "label2";
            // 
            // lblEcart
            // 
            this.lblEcart.AutoSize = true;
            this.lblEcart.Location = new System.Drawing.Point(116, 388);
            this.lblEcart.Name = "lblEcart";
            this.lblEcart.Size = new System.Drawing.Size(42, 17);
            this.lblEcart.TabIndex = 5;
            this.lblEcart.Text = "label3";
            // 
            // ShiftForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblEcart);
            this.Controls.Add(this.lblTheorique);
            this.Controls.Add(this.txtMontantReel);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCloseShift);
            this.Controls.Add(this.btnOpenShift);
            this.Name = "ShiftForm";
            this.Text = "ShiftForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ShiftForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenShift;
        private System.Windows.Forms.Button btnCloseShift;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtMontantReel;
        private System.Windows.Forms.Label lblTheorique;
        private System.Windows.Forms.Label lblEcart;
    }
}