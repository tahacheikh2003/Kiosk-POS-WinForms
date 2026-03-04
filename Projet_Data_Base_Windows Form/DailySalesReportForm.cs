using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class DailySalesReportForm : Form
    {
        private DataLayer d;
        private DataTable reportDt;

        // UI style elements
        private Panel card;
        private Label lblTitle;

        public DailySalesReportForm()
        {
            InitializeComponent();
            ApplyDailyReportStyle_NoRename(); // style after controls exist
        }

        private void DailySalesReportForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            // افتراضي: آخر 7 أيام
            dtTo.Value = DateTime.Today;
            dtFrom.Value = DateTime.Today.AddDays(-7);

            dgvReport.ReadOnly = true;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // =========================
        // ===== STYLE / LAYOUT =====
        // =========================
        private void ApplyDailyReportStyle_NoRename()
        {
            // Base form style
            this.Text = "Daily Sales Report";
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(980, 620);
            this.MinimumSize = new Size(980, 620);

            // Card
            card = new Panel
            {
                Size = new Size(900, 540),
                BackColor = Color.White
            };
            this.Controls.Add(card);

            // Title
            lblTitle = new Label
            {
                Text = "Rapport des ventes (par jour)",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(28, 22)
            };
            card.Controls.Add(lblTitle);

            // Move existing controls into card (no rename)
            MoveToCard(dtFrom);
            MoveToCard(dtTo);
            MoveToCard(btnLoad);
            MoveToCard(btnExportCsv);
            MoveToCard(dgvReport);

            // Labels for dates
            var lblFrom = new Label
            {
                Text = "Du",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            var lblTo = new Label
            {
                Text = "Au",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            card.Controls.Add(lblFrom);
            card.Controls.Add(lblTo);

            // Style controls
            StyleDatePickerBig(dtFrom);
            StyleDatePickerBig(dtTo);

            StylePrimaryButton(btnLoad);
            StyleSecondaryButton(btnExportCsv);

            StyleGridBig(dgvReport);

            // Layout
            int x = 28;
            int y = 80;

            // Date row
            lblFrom.SetBounds(x, y + 7, 40, 20);
            dtFrom.SetBounds(x + 50, y, 220, 34);

            lblTo.SetBounds(x + 290, y + 7, 40, 20);
            dtTo.SetBounds(x + 330, y, 220, 34);

            btnLoad.SetBounds(card.Width - 28 - 220, y, 220, 44);
            btnExportCsv.SetBounds(card.Width - 28 - 220, y + 54, 220, 44);

            // Grid
            dgvReport.SetBounds(28, y + 110, card.Width - 56, card.Height - (y + 140));
            dgvReport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Anchors for top controls
            dtFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dtTo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportCsv.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Keep buttons aligned on resize
            card.Resize += (s, e) =>
            {
                btnLoad.Left = card.Width - 28 - btnLoad.Width;
                btnExportCsv.Left = card.Width - 28 - btnExportCsv.Width;

                dgvReport.Width = card.Width - 56;
                dgvReport.Height = card.Height - (y + 140);
            };

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

        private void StyleDatePickerBig(DateTimePicker dtp)
        {
            if (dtp == null) return;
            dtp.Font = new Font("Segoe UI", 11f);
            dtp.CalendarFont = new Font("Segoe UI", 10f);
        }

        private void StyleGridBig(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.GridColor = Color.FromArgb(229, 231, 235);
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(17, 24, 39);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 45;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 11f);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dgv.RowTemplate.Height = 40;
        }

        private void StylePrimaryButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(37, 99, 235);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Height = 46;
            btn.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(29, 78, 216);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(37, 99, 235);
        }

        private void StyleSecondaryButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(17, 24, 39);
            btn.Cursor = Cursors.Hand;
            btn.Height = 46;
            btn.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(243, 244, 246);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
        }

        // =========================
        // ========= LOGIC ==========
        // =========================
        private void btnLoad_Click(object sender, EventArgs e)
        {
            DateTime from = dtFrom.Value.Date;
            DateTime to = dtTo.Value.Date.AddDays(1); // inclusive end day

            string fromSql = from.ToString("yyyy-MM-dd");
            string toSql = to.ToString("yyyy-MM-dd");

            string sql =
                "SELECT jour, nb_ventes, total_jour " +
                "FROM dbo.vw_DailySalesSummary " +
                $"WHERE jour >= '{fromSql}' AND jour < '{toSql}' " +
                "ORDER BY jour;";

            reportDt = d.GetData(sql, "DailySales");
            dgvReport.DataSource = reportDt;

            if (reportDt == null)
                MessageBox.Show("Erreur SQL.");
            else
                MessageBox.Show("Lignes chargées: " + reportDt.Rows.Count);
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (reportDt == null || reportDt.Rows.Count == 0)
            {
                MessageBox.Show("Aucune donnée à exporter. Cliquez sur Load d'abord.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (Excel)|*.csv";
            sfd.FileName = $"DailySales_{dtFrom.Value:yyyyMMdd}_{dtTo.Value:yyyyMMdd}.csv";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                ExportDataTableToCsv(reportDt, sfd.FileName);
                MessageBox.Show("Export terminé: " + sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur export: " + ex.Message);
            }
        }

        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dtTo_ValueChanged(object sender, EventArgs e) { }
        private void dtFrom_ValueChanged(object sender, EventArgs e) { }

        private void ExportDataTableToCsv(DataTable dt, string filePath)
        {
            char sep = ',';

            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sw.Write(sep);
                    sw.Write(EscapeCsv(dt.Columns[i].ColumnName));
                }
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i > 0) sw.Write(sep);
                        sw.Write(EscapeCsv(row[i]?.ToString() ?? ""));
                    }
                    sw.WriteLine();
                }
            }
        }

        private string EscapeCsv(string s)
        {
            if (s.Contains(";") || s.Contains("\"") || s.Contains("\n") || s.Contains("\r"))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
    }
}