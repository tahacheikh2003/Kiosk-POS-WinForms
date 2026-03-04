using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class SuppliersForm : Form
    {
        private DataLayer d;
        private DataTable suppliersDt;
        private int? selectedSupplierId = null;

        // ===== Style elements =====
        private Panel header;
        private Panel card;

        public SuppliersForm()
        {
            InitializeComponent();
            ApplySuppliersStyle_NoRename();
        }

        // =================================================================
        // ======================  STYLE & LAYOUT  =========================
        // =================================================================
        private void ApplySuppliersStyle_NoRename()
        {
            // ===== Global form style
            this.Text = "Gestion Fournisseurs";
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Start maximized + fallback size
            this.WindowState = FormWindowState.Maximized;
            this.Size = new Size(1400, 900);

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
                Text = "Gestion des Fournisseurs",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(18, 18)
            };
            header.Controls.Add(lblTitle);

            // ===== Card (container)
            card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(24)
            };
            this.Controls.Add(card);
            card.BringToFront();

            // ===== Move designer controls into card (no renaming)
            var toMove = this.Controls.Cast<Control>()
                .Where(c => c != header && c != card)
                .ToList();

            foreach (var c in toMove)
            {
                this.Controls.Remove(c);
                card.Controls.Add(c);
            }

            // ===== Right actions panel
            var actionsPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 320,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            card.Controls.Add(actionsPanel);
            actionsPanel.BringToFront();

            // ===== Bottom inputs panel
            var inputsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 240,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            card.Controls.Add(inputsPanel);
            inputsPanel.BringToFront();

            // ===== Split container for 2 grids (left: suppliers, right: products of supplier)
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterWidth = 8,
                BackColor = Color.White
            };
            card.Controls.Add(split);
            split.SendToBack(); // keep it behind bottom/right panels

            // Titles above grids
            var supTitle = new Label
            {
                Text = "Liste des fournisseurs",
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Padding = new Padding(6, 6, 6, 0)
            };
            split.Panel1.Controls.Add(supTitle);
            supTitle.BringToFront();

            var prodTitle = new Label
            {
                Text = "Produits du fournisseur",
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Padding = new Padding(6, 6, 6, 0)
            };
            split.Panel2.Controls.Add(prodTitle);
            prodTitle.BringToFront();

            // ===== Attach + style grids
            if (dgvSuppliers != null)
            {
                dgvSuppliers.Parent = split.Panel1;
                dgvSuppliers.Dock = DockStyle.Fill;
                StyleGridBig(dgvSuppliers);
                dgvSuppliers.BringToFront();
            }

            // إذا عندك Grid تاني للمنتجات (اختياري حسب مشروعك)
            if (this.Controls.Find("dgvSupplierProducts", true).FirstOrDefault() is DataGridView dgvSupplierProducts)
            {
                dgvSupplierProducts.Parent = split.Panel2;
                dgvSupplierProducts.Dock = DockStyle.Fill;
                StyleGridBig(dgvSupplierProducts);
                dgvSupplierProducts.BringToFront();
            }

            // ===== Inputs styling
            StyleTextBoxBig(txtNom);
            StyleTextBoxBig(txtTel);
            StyleTextBoxBig(txtEmail);

            // ===== Labels (Nom / Tel / Email) قبل الـ TextBox
            var lblNom = new Label
            {
                Text = "Nom",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("Segoe UI", 10f, FontStyle.Regular)
            };
            var lblTel = new Label
            {
                Text = "Tel",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("Segoe UI", 10f, FontStyle.Regular)
            };
            var lblEmail = new Label
            {
                Text = "Email",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("Segoe UI", 10f, FontStyle.Regular)
            };

            // Move inputs into inputsPanel
            MoveTo(inputsPanel, lblNom);
            MoveTo(inputsPanel, txtNom);

            MoveTo(inputsPanel, lblTel);
            MoveTo(inputsPanel, txtTel);

            MoveTo(inputsPanel, lblEmail);
            MoveTo(inputsPanel, txtEmail);

            // Layout inputs (Label فوق الـ TextBox)
            int x = 20;
            int y = 18;
            int w = inputsPanel.ClientSize.Width - 40;

            // Nom
            lblNom.SetBounds(x, y, 200, 20); y += 24;
            txtNom.SetBounds(x, y, w, 34); y += 52;

            // Tel
            lblTel.SetBounds(x, y, 200, 20); y += 24;
            txtTel.SetBounds(x, y, w, 34); y += 52;

            // Email
            lblEmail.SetBounds(x, y, 200, 20); y += 24;
            txtEmail.SetBounds(x, y, w, 34);

            foreach (var c in new Control[] { lblNom, lblTel, lblEmail, txtNom, txtTel, txtEmail })
            {
                if (c != null)
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }

            // ===== Buttons styling
            StylePrimaryButton(btnAdd);
            StylePrimaryButton(btnUpdate);
            StyleDangerButton(btnDelete);

            // Move buttons into actionsPanel
            MoveTo(actionsPanel, btnAdd);
            MoveTo(actionsPanel, btnUpdate);
            MoveTo(actionsPanel, btnDelete);

            int by = 20;
            int bw = actionsPanel.ClientSize.Width - 40;

            btnAdd.SetBounds(20, by, bw, 46); by += 56;
            btnUpdate.SetBounds(20, by, bw, 46); by += 56;
            btnDelete.SetBounds(20, by, bw, 46);

            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ===== Keep widths correct on resize
            inputsPanel.Resize += (s, e) =>
            {
                int ww = inputsPanel.ClientSize.Width - 40;
                if (txtNom != null) txtNom.Width = ww;
                if (txtTel != null) txtTel.Width = ww;
                if (txtEmail != null) txtEmail.Width = ww;
            };

            actionsPanel.Resize += (s, e) =>
            {
                int ww = actionsPanel.ClientSize.Width - 40;
                if (btnAdd != null) btnAdd.Width = ww;
                if (btnUpdate != null) btnUpdate.Width = ww;
                if (btnDelete != null) btnDelete.Width = ww;
            };

            // ===== Split 50/50
            this.Shown += (s, e) =>
            {
                split.SplitterDistance = (split.Width / 2) - 10;
            };
            split.Resize += (s, e) =>
            {
                split.SplitterDistance = (split.Width / 2) - 10;
            };
        }

        private void MoveTo(Control parent, Control c)
        {
            if (parent == null || c == null) return;
            if (c.Parent != null) c.Parent.Controls.Remove(c);
            parent.Controls.Add(c);
        }

        private void StyleTextBoxBig(TextBox txt)
        {
            if (txt == null) return;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = Color.FromArgb(250, 250, 250);
            txt.Font = new Font("Segoe UI", 11f);
        }

        private void StyleGridBig(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.GridColor = Color.FromArgb(229, 231, 235);
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void StyleDangerButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(220, 38, 38);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Height = 46;
            btn.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(185, 28, 28);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(220, 38, 38);
        }

        // =================================================================
        // ======================  LOGIC (UNCHANGED)  ======================
        // =================================================================

        private void txtNom_TextChanged(object sender, EventArgs e) { }
        private void txtTel_TextChanged(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!TryReadSupplierInputs(out string nom, out string tel, out string email))
                return;

            string sql =
                "DECLARE @newId INT; " +
                $"EXEC dbo.usp_Suppliers_Insert @nom = {SqlStrOrNull(nom)}, @telephone = {SqlStrOrNull(tel)}, @email = {SqlStrOrNull(email)}, @new_id_supplier = @newId OUTPUT; " +
                "SELECT @newId AS id_supplier;";

            DataTable dt = d.GetData(sql, "newSup");
            if (dt != null && dt.Rows.Count > 0)
            {
                MessageBox.Show("Fournisseur ajouté. ID=" + dt.Rows[0]["id_supplier"].ToString());
                LoadSuppliers();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedSupplierId == null)
            {
                MessageBox.Show("Sélectionnez un fournisseur.");
                return;
            }

            if (!TryReadSupplierInputs(out string nom, out string tel, out string email))
                return;

            string sql =
                $"EXEC dbo.usp_Suppliers_Update @id_supplier = {selectedSupplierId.Value}, " +
                $"@nom = {SqlStrOrNull(nom)}, @telephone = {SqlStrOrNull(tel)}, @email = {SqlStrOrNull(email)};";

            d.ExecuteActionCommand(sql);
            MessageBox.Show("Fournisseur modifié.");
            LoadSuppliers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedSupplierId == null)
            {
                MessageBox.Show("Sélectionnez un fournisseur.");
                return;
            }

            string sqlCheck = $"SELECT COUNT(*) FROM dbo.Products WHERE id_supplier = {selectedSupplierId.Value};";
            object v = d.GetValue(sqlCheck);
            int cnt = (v == null) ? 0 : Convert.ToInt32(v);

            if (cnt > 0)
            {
                MessageBox.Show("Impossible de supprimer: ce fournisseur possède des produits.\nSupprimez/Modifiez les produits d'abord.");
                return;
            }

            var res = MessageBox.Show("Supprimer ce fournisseur ?", "Confirmation",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;

            string sql = $"EXEC dbo.usp_Suppliers_Delete @id_supplier = {selectedSupplierId.Value};";
            d.ExecuteActionCommand(sql);

            MessageBox.Show("Fournisseur supprimé.");
            LoadSuppliers();
        }

        private void dgvSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvSuppliers.Rows[e.RowIndex];
            selectedSupplierId = Convert.ToInt32(row.Cells["id_supplier"].Value);

            txtNom.Text = row.Cells["nom"].Value?.ToString() ?? "";
            txtTel.Text = row.Cells["telephone"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["email"].Value?.ToString() ?? "";

            LoadProductsOfSupplier(selectedSupplierId.Value);
        }

        private void LoadProductsOfSupplier(int idSupplier)
        {
            string sql =
                "SELECT p.id_produit, p.nom, p.prix_vente, p.stock_minimum, p.actif, c.libelle AS categorie " +
                "FROM dbo.Products p " +
                "INNER JOIN dbo.Categories c ON c.id_categorie = p.id_categorie " +
                $"WHERE p.id_supplier = {idSupplier} " +
                "ORDER BY p.nom;";

            DataTable dt = d.GetData(sql, "supplierProducts");

            // إذا عندك dgvSupplierProducts بالديزاينر:
            var grid = this.Controls.Find("dgvSupplierProducts", true).FirstOrDefault() as DataGridView;
            if (grid != null) grid.DataSource = dt;
        }

        private void dgvSupplierProducts_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnAddProductToSupplier_Click(object sender, EventArgs e)
        {
            if (selectedSupplierId == null)
            {
                MessageBox.Show("Choisissez un fournisseur d'abord.");
                return;
            }

            var f = new ProductAddForSupplierForm(selectedSupplierId.Value);
            f.ShowDialog();

            LoadProductsOfSupplier(selectedSupplierId.Value);
        }

        private void SuppliersForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            dgvSuppliers.ReadOnly = true;
            dgvSuppliers.AllowUserToAddRows = false;
            dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            string sql =
                "SELECT id_supplier, nom, telephone, email " +
                "FROM dbo.Suppliers " +
                "ORDER BY nom;";

            suppliersDt = d.GetData(sql, "suppliers");
            dgvSuppliers.DataSource = suppliersDt;

            selectedSupplierId = null;
            ClearSupplierInputs();
        }

        private void ClearSupplierInputs()
        {
            txtNom.Text = "";
            txtTel.Text = "";
            txtEmail.Text = "";
        }

        private string SqlStrOrNull(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "NULL";
            return "N'" + s.Trim().Replace("'", "''") + "'";
        }

        private bool TryReadSupplierInputs(out string nom, out string tel, out string email)
        {
            nom = txtNom.Text.Trim();
            tel = txtTel.Text.Trim();
            email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(nom))
            {
                MessageBox.Show("Nom fournisseur obligatoire.");
                return false;
            }
            return true;
        }
    }
}