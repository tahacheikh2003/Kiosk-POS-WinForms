using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class ShiftForm : Form
    {
        private DataLayer d;
        private int? currentShiftId = null;

        // UI
        private Panel card;
        private Label lblTitle;

        public ShiftForm()
        {
            InitializeComponent();          // Must be first
            ApplyShiftStyle_NoRename();     // Apply style after controls exist
        }

        private void ShiftForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                this.Close();
                return;
            }

            LoadOpenShift();
        }

        private void ApplyShiftStyle_NoRename()
        {
            // ===== Form base style
            this.Text = "Shift";
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterParent;

            // ===== Create card
            card = new Panel
            {
                Size = new Size(760, 380),
                BackColor = Color.White
            };
            this.Controls.Add(card);

            // ===== Title
            lblTitle = new Label
            {
                Text = "Gestion du Shift",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(28, 22)
            };
            card.Controls.Add(lblTitle);

            // ===== Hide any existing designer label that says "Montant reel/réel"
            foreach (var lab in this.Controls.OfType<Label>().ToList())
            {
                string t = (lab.Text ?? "").Trim().ToLower();
                if (t == "montant reel" || t == "montant réel")
                    lab.Visible = false;
            }

            // ===== Move existing controls into card (no renaming)
            MoveToCard(lblStatus);
            MoveToCard(btnOpenShift);
            MoveToCard(btnCloseShift);
            MoveToCard(txtMontantReel);
            MoveToCard(lblTheorique);
            MoveToCard(lblEcart);

            // ===== Styles
            lblStatus.ForeColor = Color.FromArgb(31, 41, 55);
            lblTheorique.ForeColor = Color.FromArgb(55, 65, 81);
            lblEcart.ForeColor = Color.FromArgb(55, 65, 81);

            txtMontantReel.BorderStyle = BorderStyle.FixedSingle;
            txtMontantReel.Font = new Font("Segoe UI", 10f);

            StylePrimaryButton(btnOpenShift);
            StylePrimaryButton(btnCloseShift);

            // ===== Layout
            lblStatus.SetBounds(28, 70, 700, 24);

            // Buttons (left)
            btnOpenShift.SetBounds(28, 120, 280, 44);
            btnCloseShift.SetBounds(28, 175, 280, 44);

            // Montant réel (NEW label guaranteed visible) + textbox same line
            var lblMontant = new Label
            {
                Text = "Montant réel",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            card.Controls.Add(lblMontant);

            int yMontant = 135; // adjust if you want it lower/higher
            lblMontant.SetBounds(360, yMontant + 7, 120, 20);
            txtMontantReel.SetBounds(490, yMontant, 220, 34);
            txtMontantReel.BringToFront();

            // Results (bottom)
            lblTheorique.SetBounds(28, 255, 700, 24);
            lblEcart.SetBounds(28, 285, 700, 24);

            // Center card
            CenterCard();
            this.Resize += (s, e) => CenterCard();
        }

        private void MoveToCard(Control c)
        {
            if (c == null) return;
            if (c.Parent != null) c.Parent.Controls.Remove(c);
            card.Controls.Add(c);
        }

        private void CenterCard()
        {
            if (card == null) return;
            card.Left = (this.ClientSize.Width - card.Width) / 2;
            card.Top = (this.ClientSize.Height - card.Height) / 2;
        }

        private void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(37, 99, 235);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(29, 78, 216);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(37, 99, 235);
        }

        private void LoadOpenShift()
        {
            string sql = $"EXEC dbo.usp_Shift_GetOpenByUser @id_user = {Session.CurrentUserId}";
            DataTable dt = d.GetData(sql, "openShift");

            if (dt != null && dt.Rows.Count > 0)
            {
                currentShiftId = Convert.ToInt32(dt.Rows[0]["id_shift"]);
                DateTime opened = Convert.ToDateTime(dt.Rows[0]["date_ouverture"]);

                lblStatus.Text = $"Shift: OPEN - {Session.CurrentUserName} (Depuis {opened:dd/MM/yyyy HH:mm})";

                btnOpenShift.Enabled = false;
                btnCloseShift.Enabled = true;
                txtMontantReel.Enabled = true;
            }
            else
            {
                currentShiftId = null;
                lblStatus.Text = "Shift: CLOSED";

                btnOpenShift.Enabled = true;
                btnCloseShift.Enabled = false;

                txtMontantReel.Clear();
                txtMontantReel.Enabled = false;
            }

            lblTheorique.Text = "";
            lblEcart.Text = "";
        }

        private void btnOpenShift_Click(object sender, EventArgs e)
        {
            string sql =
                "DECLARE @newId INT; " +
                $"EXEC dbo.usp_Shift_Open @id_user = {Session.CurrentUserId}, @new_id_shift = @newId OUTPUT; " +
                "SELECT @newId AS id_shift;";

            DataTable dt = d.GetData(sql, "newShift");
            if (dt != null && dt.Rows.Count > 0)
            {
                currentShiftId = Convert.ToInt32(dt.Rows[0]["id_shift"]);
                MessageBox.Show("Shift opened.");
            }

            LoadOpenShift();
        }

        private void btnCloseShift_Click(object sender, EventArgs e)
        {
            if (currentShiftId == null)
            {
                MessageBox.Show("Aucun shift ouvert.");
                return;
            }

            if (!decimal.TryParse(txtMontantReel.Text.Trim(), out decimal montantReel))
            {
                MessageBox.Show("Montant réel invalide.");
                return;
            }

            string montantSql = montantReel.ToString(CultureInfo.InvariantCulture);

            string sql = $"EXEC dbo.usp_Shift_Close @id_shift = {currentShiftId.Value}, @montant_reel = {montantSql}";
            d.ExecuteActionCommand(sql);

            string sqlGet = $"EXEC dbo.usp_Shift_SelectOne @id_shift = {currentShiftId.Value}";
            DataTable dt = d.GetData(sqlGet, "shiftOne");

            if (dt != null && dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                lblTheorique.Text = "Théorique: " + r["montant_theorique"];
                lblEcart.Text = "Écart: " + r["ecart"];
            }

            MessageBox.Show("Shift closed.");
            LoadOpenShift();
        }
    }
}
