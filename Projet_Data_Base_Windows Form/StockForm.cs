using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class StockForm : Form
    {
        private DataTable stockDt;
        private DataLayer d;

        // UI (style only)
        private Panel header;
        private Panel card;

        public StockForm()
        {
            InitializeComponent();
            ApplyStockStyle_NoRename(); // style only
        }

        private void ApplyStockStyle_NoRename()
        {
            // ===== Form base style
            this.Text = "Stock";
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterParent;

            // ===== Header
            header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = Color.White
            };
            this.Controls.Add(header);
            header.BringToFront();

            var lblTitle = new Label
            {
                Text = "Stock",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(18, 18)
            };
            header.Controls.Add(lblTitle);

            // ===== Card (content area)
            card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            this.Controls.Add(card);
            card.BringToFront();

            // ===== Move existing controls into the card (no rename)
            var toMove = this.Controls.Cast<Control>()
                .Where(c => c != header && c != card)
                .ToList();

            foreach (var c in toMove)
            {
                this.Controls.Remove(c);
                card.Controls.Add(c);
            }

            // ===== Style buttons (keep your existing names)
            StylePrimaryButton(btnRefresh);
            StyleSecondaryButton(btnExportStock);

            // ===== Layout buttons nicely (top-left)
            btnRefresh.SetBounds(20, 20, 160, 40);
            btnExportStock.SetBounds(190, 20, 180, 40);

            // ===== Style DataGridView
            if (dgvStock != null)
            {
                dgvStock.SetBounds(20, 80, card.ClientSize.Width - 40, card.ClientSize.Height - 100);
                dgvStock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                dgvStock.BackgroundColor = Color.White;
                dgvStock.BorderStyle = BorderStyle.FixedSingle;
                dgvStock.GridColor = Color.FromArgb(229, 231, 235);
                dgvStock.RowHeadersVisible = false;
                dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvStock.EnableHeadersVisualStyles = false;

                dgvStock.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
                dgvStock.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(17, 24, 39);
                dgvStock.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

                dgvStock.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
                dgvStock.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
                dgvStock.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            }
        }

        private void StylePrimaryButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(37, 99, 235);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Height = 40;

            // Hover effect
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
            btn.Height = 40;

            // Hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(243, 244, 246);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
        }
        private void btnExportStock_Click(object sender, EventArgs e)
        {
            if (stockDt == null || stockDt.Rows.Count == 0)
            {
                MessageBox.Show("Aucune donnée à exporter. Cliquez sur Refresh d'abord.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel (TSV)|*.tsv|CSV (Excel)|*.csv",
                FileName = $"Stock_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                if (sfd.FileName.EndsWith(".tsv", StringComparison.OrdinalIgnoreCase))
                    ExportDataTable(stockDt, sfd.FileName, '\t');
                else
                    ExportDataTable(stockDt, sfd.FileName, ','); // Excel FR often uses ';' depending on settings

                MessageBox.Show("Export terminé: " + sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur export: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStock();
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            dgvStock.ReadOnly = true;
            dgvStock.AllowUserToAddRows = false;
            dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadStock();
        }

        private void LoadStock()
        {
            string sql =
                "SELECT id_produit, produit, categorie, supplier, prix_vente, " +
                "quantite_actuelle, stock_minimum, stock_status, date_maj " +
                "FROM dbo.vw_CurrentStock " +
                "ORDER BY stock_status, produit;";

            stockDt = d.GetData(sql, "stock");
            dgvStock.DataSource = stockDt;
        }

        private void ExportDataTable(DataTable dt, string filePath, char sep)
        {
            using (var sw = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Header
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sw.Write(sep);
                    sw.Write(EscapeCell(dt.Columns[i].ColumnName, sep));
                }
                sw.WriteLine();

                // Rows
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i > 0) sw.Write(sep);
                        sw.Write(EscapeCell(row[i]?.ToString() ?? "", sep));
                    }
                    sw.WriteLine();
                }
            }
        }

        private string EscapeCell(string s, char sep)
        {
            // If the cell contains separator, quotes, or newlines, wrap with quotes
            if (s.Contains(sep) || s.Contains("\"") || s.Contains("\n") || s.Contains("\r"))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
    }
}
