using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class ProductsForm : Form
    {
        private DataLayer d;
        private DataTable productsDt;

        private int? selectedProductId = null;
        System.Drawing.Printing.PrintDocument pd;

        // UI (style only)
        private Panel header;
        private Panel card;

        public ProductsForm()
        {
            InitializeComponent();
            ApplyProductsStyle_NoRename(); // style only, no logic changes
        }

        private void ApplyProductsStyle_NoRename()
        {
            // ===== Global form style
            this.Text = "Produits";
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Open maximized
            this.WindowState = FormWindowState.Maximized;

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
                Text = "Produits",
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

            // ===== Move existing controls into card (no renaming)
            var toMove = this.Controls.Cast<Control>()
                .Where(c => c != header && c != card)
                .ToList();

            foreach (var c in toMove)
            {
                this.Controls.Remove(c);
                card.Controls.Add(c);
            }

            // ===== Right panel for actions (buttons)
            var actionsPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 320,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // ===== Bottom panel for inputs (checkbox + combos + textboxes)
            var inputsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 290, // a bit taller because of labels
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Add panels in the correct order:
            card.Controls.Add(inputsPanel);
            card.Controls.Add(actionsPanel);

            // ===== Grid (fills remaining space)
            if (dgvProducts != null)
            {
                dgvProducts.Parent = card;
                dgvProducts.Dock = DockStyle.Fill;

                dgvProducts.BackgroundColor = Color.White;
                dgvProducts.BorderStyle = BorderStyle.FixedSingle;
                dgvProducts.GridColor = Color.FromArgb(229, 231, 235);
                dgvProducts.RowHeadersVisible = false;
                dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvProducts.EnableHeadersVisualStyles = false;

                dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
                dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(17, 24, 39);
                dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

                dgvProducts.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
                dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
                dgvProducts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);

                dgvProducts.BringToFront();
            }

            // ===== Style inputs
            StyleTextBox(txtNom);
            StyleTextBox(txtPrixVente);
            StyleTextBox(txtStockMin);

            if (cmbCategories != null)
            {
                cmbCategories.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbCategories.Font = new Font("Segoe UI", 10f);
            }
            if (cmbSuppliers != null)
            {
                cmbSuppliers.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbSuppliers.Font = new Font("Segoe UI", 10f);
            }
            if (chkActif != null)
            {
                chkActif.ForeColor = Color.FromArgb(55, 65, 81);
            }

            // ===== Labels for textboxes
            var lblNom = new Label
            {
                Text = "Nom",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            var lblPrix = new Label
            {
                Text = "Prix vente",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            var lblStockMin = new Label
            {
                Text = "Stock min",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };

            // ===== Move input controls into bottom inputsPanel
            MoveTo(inputsPanel, chkActif);
            MoveTo(inputsPanel, cmbCategories);
            MoveTo(inputsPanel, cmbSuppliers);

            MoveTo(inputsPanel, lblNom);
            MoveTo(inputsPanel, txtNom);

            MoveTo(inputsPanel, lblPrix);
            MoveTo(inputsPanel, txtPrixVente);

            MoveTo(inputsPanel, lblStockMin);
            MoveTo(inputsPanel, txtStockMin);

            // Layout inside inputsPanel (stack)
            int y = 15;
            int w = inputsPanel.ClientSize.Width - 40;

            chkActif.SetBounds(20, y, w, 24); y += 35;
            cmbCategories.SetBounds(20, y, w, 32); y += 40;
            cmbSuppliers.SetBounds(20, y, w, 32); y += 38;

            // Nom
            lblNom.SetBounds(20, y, w, 20); y += 22;
            txtNom.SetBounds(20, y, w, 32); y += 38;

            // Prix
            lblPrix.SetBounds(20, y, w, 20); y += 22;
            txtPrixVente.SetBounds(20, y, w, 32); y += 38;

            // Stock min
            lblStockMin.SetBounds(20, y, w, 20); y += 22;
            txtStockMin.SetBounds(20, y, w, 32);

            foreach (var c in new Control[]
            {
                chkActif, cmbCategories, cmbSuppliers,
                lblNom, txtNom,
                lblPrix, txtPrixVente,
                lblStockMin, txtStockMin
            })
            {
                if (c != null)
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }

            // ===== Style buttons
            StylePrimaryButton(btnRefresh);
            StylePrimaryButton(btnAdd);
            StylePrimaryButton(btnUpdate);
            StyleDangerButton(btnDelete);
            StyleSecondaryButton(btnExportExcel);
            StyleSecondaryButton(btnPrintPDF);

            // Move buttons into actions panel
            MoveTo(actionsPanel, btnRefresh);
            MoveTo(actionsPanel, btnAdd);
            MoveTo(actionsPanel, btnUpdate);
            MoveTo(actionsPanel, btnDelete);
            MoveTo(actionsPanel, btnExportExcel);
            MoveTo(actionsPanel, btnPrintPDF);

            int by = 20;
            int bw = actionsPanel.ClientSize.Width - 40;

            foreach (var b in new[] { btnRefresh, btnAdd, btnUpdate, btnDelete, btnExportExcel, btnPrintPDF })
            {
                if (b == null) continue;
                b.SetBounds(20, by, bw, 44);
                b.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                by += 58;
            }

            // Keep widths correct on resize
            inputsPanel.Resize += (s, e) =>
            {
                int ww = inputsPanel.ClientSize.Width - 40;

                if (chkActif != null) chkActif.Width = ww;
                if (cmbCategories != null) cmbCategories.Width = ww;
                if (cmbSuppliers != null) cmbSuppliers.Width = ww;

                if (lblNom != null) lblNom.Width = ww;
                if (txtNom != null) txtNom.Width = ww;

                if (lblPrix != null) lblPrix.Width = ww;
                if (txtPrixVente != null) txtPrixVente.Width = ww;

                if (lblStockMin != null) lblStockMin.Width = ww;
                if (txtStockMin != null) txtStockMin.Width = ww;
            };

            actionsPanel.Resize += (s, e) =>
            {
                int ww = actionsPanel.ClientSize.Width - 40;
                foreach (var b in new[] { btnRefresh, btnAdd, btnUpdate, btnDelete, btnExportExcel, btnPrintPDF })
                {
                    if (b != null) b.Width = ww;
                }
            };
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

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(243, 244, 246);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
        }

        private void MoveTo(Control parent, Control c)
        {
            if (parent == null || c == null) return;
            if (c.Parent != null) c.Parent.Controls.Remove(c);
            parent.Controls.Add(c);
        }

        private void StyleTextBox(TextBox t)
        {
            if (t == null) return;
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font = new Font("Segoe UI", 10f);
        }

        private void StylePrimaryButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(37, 99, 235);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Height = 44;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(29, 78, 216);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(37, 99, 235);
        }

        private void StyleDangerButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(220, 38, 38);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Height = 44;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(185, 28, 28);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(220, 38, 38);
        }

        private void ProductsForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            dgvProducts.ReadOnly = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadCategories();
            LoadSuppliers();
            LoadProducts();

            chkActif.Checked = true;
        }

        private void LoadCategories()
        {
            string sql = "SELECT id_categorie, libelle FROM dbo.Categories ORDER BY libelle;";
            var dt = d.GetData(sql, "cats");
            cmbCategories.DisplayMember = "libelle";
            cmbCategories.ValueMember = "id_categorie";
            cmbCategories.DataSource = dt;
        }

        private void LoadSuppliers()
        {
            string sql = "SELECT id_supplier, nom FROM dbo.Suppliers ORDER BY nom;";
            var dt = d.GetData(sql, "sups");
            cmbSuppliers.DisplayMember = "nom";
            cmbSuppliers.ValueMember = "id_supplier";
            cmbSuppliers.DataSource = dt;
        }

        private void LoadProducts()
        {
            string sql =
                "SELECT p.id_produit, p.nom, p.prix_vente, p.stock_minimum, p.actif, " +
                "       c.libelle AS categorie, s.nom AS supplier, " +
                "       i.quantite_actuelle " +
                "FROM dbo.Products p " +
                "INNER JOIN dbo.Categories c ON c.id_categorie = p.id_categorie " +
                "INNER JOIN dbo.Suppliers s  ON s.id_supplier  = p.id_supplier " +
                "LEFT JOIN dbo.Inventory i   ON i.id_produit   = p.id_produit " +
                "ORDER BY p.nom;";

            productsDt = d.GetData(sql, "products");
            dgvProducts.DataSource = productsDt;

            selectedProductId = null;
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtNom.Text = "";
            txtPrixVente.Text = "";
            txtStockMin.Text = "";
            chkActif.Checked = true;
        }

        private string SqlStr(string s)
        {
            if (s == null) return "NULL";
            return "N'" + s.Replace("'", "''") + "'";
        }

        private bool TryReadInputs(out string nom, out decimal prix, out int stockMin, out int actifBit, out int idCat, out int idSup)
        {
            nom = txtNom.Text.Trim();
            if (string.IsNullOrWhiteSpace(nom))
            {
                MessageBox.Show("Nom produit obligatoire.");
                prix = 0; stockMin = 0; actifBit = 1; idCat = 0; idSup = 0;
                return false;
            }

            if (!decimal.TryParse(txtPrixVente.Text.Trim().Replace(',', '.'),
                                  System.Globalization.NumberStyles.Any,
                                  System.Globalization.CultureInfo.InvariantCulture, out prix))
            {
                MessageBox.Show("Prix vente invalide.");
                stockMin = 0; actifBit = 1; idCat = 0; idSup = 0;
                return false;
            }

            if (!int.TryParse(txtStockMin.Text.Trim(), out stockMin))
            {
                MessageBox.Show("Stock minimum invalide.");
                actifBit = 1; idCat = 0; idSup = 0;
                return false;
            }

            if (cmbCategories.SelectedValue == null || cmbSuppliers.SelectedValue == null)
            {
                MessageBox.Show("Choisissez catégorie et supplier.");
                actifBit = 1; idCat = 0; idSup = 0;
                return false;
            }

            idCat = Convert.ToInt32(cmbCategories.SelectedValue);
            idSup = Convert.ToInt32(cmbSuppliers.SelectedValue);
            actifBit = chkActif.Checked ? 1 : 0;
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!TryReadInputs(out string nom, out decimal prix, out int stockMin, out int actifBit, out int idCat, out int idSup))
                return;

            string prixSql = prix.ToString("0.00").Replace(',', '.');

            string sql =
                "DECLARE @newId INT; " +
                $"EXEC dbo.usp_Products_Insert " +
                $"@nom = {SqlStr(nom)}, @prix_vente = {prixSql}, @stock_minimum = {stockMin}, @actif = {actifBit}, " +
                $"@id_categorie = {idCat}, @id_supplier = {idSup}, @new_id_produit = @newId OUTPUT; " +
                "SELECT @newId AS id_produit;";

            var dt = d.GetData(sql, "newprod");
            if (dt != null && dt.Rows.Count > 0)
            {
                MessageBox.Show("Produit ajouté. ID=" + dt.Rows[0]["id_produit"].ToString());
                LoadProducts();
            }
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvProducts.Rows[e.RowIndex];
            if (row == null) return;

            selectedProductId = Convert.ToInt32(row.Cells["id_produit"].Value);

            txtNom.Text = row.Cells["nom"].Value?.ToString() ?? "";
            txtPrixVente.Text = Convert.ToDecimal(row.Cells["prix_vente"].Value).ToString("0.00");
            txtStockMin.Text = row.Cells["stock_minimum"].Value?.ToString() ?? "";

            chkActif.Checked = Convert.ToBoolean(row.Cells["actif"].Value);

            var catName = row.Cells["categorie"].Value?.ToString();
            var supName = row.Cells["supplier"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(catName))
            {
                var catDt = cmbCategories.DataSource as DataTable;
                var catRow = catDt?.AsEnumerable().FirstOrDefault(r => r["libelle"].ToString() == catName);
                if (catRow != null) cmbCategories.SelectedValue = Convert.ToInt32(catRow["id_categorie"]);
            }

            if (!string.IsNullOrWhiteSpace(supName))
            {
                var supDt = cmbSuppliers.DataSource as DataTable;
                var supRow = supDt?.AsEnumerable().FirstOrDefault(r => r["nom"].ToString() == supName);
                if (supRow != null) cmbSuppliers.SelectedValue = Convert.ToInt32(supRow["id_supplier"]);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedProductId == null)
            {
                MessageBox.Show("Sélectionnez un produit à modifier.");
                return;
            }

            if (!TryReadInputs(out string nom, out decimal prix, out int stockMin, out int actifBit, out int idCat, out int idSup))
                return;

            string prixSql = prix.ToString("0.00").Replace(',', '.');

            string sql =
                $"EXEC dbo.usp_Products_Update " +
                $"@id_produit = {selectedProductId.Value}, " +
                $"@nom = {SqlStr(nom)}, @prix_vente = {prixSql}, @stock_minimum = {stockMin}, @actif = {actifBit}, " +
                $"@id_categorie = {idCat}, @id_supplier = {idSup};";

            d.ExecuteActionCommand(sql);
            MessageBox.Show("Produit modifié.");
            LoadProducts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProductId == null)
            {
                MessageBox.Show("Sélectionnez un produit à supprimer.");
                return;
            }

            var res = MessageBox.Show("Supprimer ce produit ?", "Confirmation",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;

            string sql = $"EXEC dbo.usp_Products_Delete @id_produit = {selectedProductId.Value};";
            d.ExecuteActionCommand(sql);

            MessageBox.Show("Produit supprimé.");
            LoadProducts();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (productsDt == null || productsDt.Rows.Count == 0)
            {
                MessageBox.Show("Aucune donnée à exporter.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel (TSV)|*.tsv|CSV (Excel)|*.csv";
            sfd.FileName = $"Products_{DateTime.Now:yyyyMMdd_HHmm}.csv";

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                if (sfd.FileName.EndsWith(".tsv", StringComparison.OrdinalIgnoreCase))
                    ExportDataTable(productsDt, sfd.FileName, '\t');
                else
                    ExportDataTable(productsDt, sfd.FileName, ',');

                MessageBox.Show("Export terminé: " + sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur export: " + ex.Message);
            }
        }

        private void ExportDataTable(DataTable dt, string filePath, char sep)
        {
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sw.Write(sep);
                    sw.Write(EscapeCell(dt.Columns[i].ColumnName, sep));
                }
                sw.WriteLine();

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
            if (s.Contains(sep) || s.Contains("\"") || s.Contains("\n") || s.Contains("\r"))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btnPrintPDF_Click(object sender, EventArgs e)
        {
            if (dgvProducts.Rows.Count == 0)
            {
                MessageBox.Show("Aucune donnée à imprimer.");
                return;
            }

            PrintDialog dlg = new PrintDialog();
            pd = new System.Drawing.Printing.PrintDocument();
            dlg.Document = pd;

            pd.PrintPage += Pd_PrintPage;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pd.PrinterSettings = dlg.PrinterSettings;
                pd.Print();
            }
        }

        private void Pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font headerFont = new Font("Arial", 10, FontStyle.Bold);
            Font cellFont = new Font("Arial", 10);

            int left = 50;
            int top = 50;
            int rowHeight = 25;

            g.DrawString("Liste des Produits", titleFont, Brushes.Black, left, top);
            top += 40;

            int colWidth = 150;
            string[] headers = { "Produit", "Prix", "Stock" };

            for (int i = 0; i < headers.Length; i++)
            {
                g.DrawRectangle(Pens.Black, left + i * colWidth, top, colWidth, rowHeight);
                g.DrawString(headers[i], headerFont, Brushes.Black,
                    new Rectangle(left + i * colWidth, top, colWidth, rowHeight));
            }

            top += rowHeight;

            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.IsNewRow) continue;

                g.DrawRectangle(Pens.Black, left, top, colWidth, rowHeight);
                g.DrawString(row.Cells["nom"].Value.ToString(), cellFont, Brushes.Black,
                    new Rectangle(left, top, colWidth, rowHeight));

                g.DrawRectangle(Pens.Black, left + colWidth, top, colWidth, rowHeight);
                g.DrawString(row.Cells["prix_vente"].Value.ToString(), cellFont, Brushes.Black,
                    new Rectangle(left + colWidth, top, colWidth, rowHeight));

                g.DrawRectangle(Pens.Black, left + 2 * colWidth, top, colWidth, rowHeight);
                g.DrawString(row.Cells["quantite_actuelle"].Value.ToString(), cellFont, Brushes.Black,
                    new Rectangle(left + 2 * colWidth, top, colWidth, rowHeight));

                top += rowHeight;
            }
        }
    }
}