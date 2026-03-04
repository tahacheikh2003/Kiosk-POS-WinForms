using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class RestockForm : Form
    {
        private DataLayer d;

        // UI style elements
        private Panel card;
        private Label lblTitle;

        public RestockForm()
        {
            InitializeComponent();
            ApplyRestockStyle_NoRename(); // style after controls exist
        }

        private void RestockForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            // UI defaults
            numQty.Minimum = 1;
            numQty.Value = 1;

            if (lblInfo != null) lblInfo.Text = "";

            // Load suppliers once (this will also load products for first supplier)
            LoadSuppliers();
        }

        // =========================
        // ===== STYLE / LAYOUT =====
        // =========================
        private void ApplyRestockStyle_NoRename()
        {
            // Base form style (same vibe as ShiftForm)
            this.Text = "Restock";
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(860, 460);
            this.MinimumSize = new Size(860, 460);

            // Card
            card = new Panel
            {
                Size = new Size(760, 360),
                BackColor = Color.White
            };
            this.Controls.Add(card);

            // Title
            lblTitle = new Label
            {
                Text = "Restock (Réapprovisionnement)",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(28, 22)
            };
            card.Controls.Add(lblTitle);

            // Move existing controls into card (no rename)
            MoveToCard(cmbSupplier);
            MoveToCard(cmbProduct);
            MoveToCard(numQty);
            MoveToCard(btnSaveRestock);
            MoveToCard(lblInfo);

            // Labels (before fields)
            var lblSupplier = new Label
            {
                Text = "Fournisseur",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            var lblProduct = new Label
            {
                Text = "Produit",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            var lblQty = new Label
            {
                Text = "Quantité",
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            card.Controls.Add(lblSupplier);
            card.Controls.Add(lblProduct);
            card.Controls.Add(lblQty);

            // Style controls
            StyleComboBig(cmbSupplier);
            StyleComboBig(cmbProduct);
            StyleNumericBig(numQty);
            StylePrimaryButton(btnSaveRestock);

            if (lblInfo != null)
            {
                lblInfo.ForeColor = Color.FromArgb(107, 114, 128);
                lblInfo.Font = new Font("Segoe UI", 10f);
            }

            // Layout
            int xL = 28;
            int xF = 210;
            int y = 85;
            int fieldW = 520;
            int fieldH = 34;
            int rowGap = 18;

            // Supplier row
            lblSupplier.SetBounds(xL, y + 7, 160, 20);
            cmbSupplier.SetBounds(xF, y, fieldW, fieldH);
            y += fieldH + rowGap;

            // Product row
            lblProduct.SetBounds(xL, y + 7, 160, 20);
            cmbProduct.SetBounds(xF, y, fieldW, fieldH);
            y += fieldH + rowGap;

            // Qty row
            lblQty.SetBounds(xL, y + 7, 160, 20);
            numQty.SetBounds(xF, y, 160, fieldH);

            // Save button (right aligned)
            btnSaveRestock.SetBounds(xF + fieldW - 220, y + 70, 220, 44);

            // Info label bottom
            if (lblInfo != null)
                lblInfo.SetBounds(28, card.Height - 48, card.Width - 56, 24);

            // Anchors (keep nice on resize)
            cmbSupplier.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbProduct.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnSaveRestock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            if (lblInfo != null) lblInfo.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

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

        private void StyleComboBig(ComboBox cmb)
        {
            if (cmb == null) return;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Font = new Font("Segoe UI", 11f);
            cmb.BackColor = Color.FromArgb(250, 250, 250);
        }

        private void StyleNumericBig(NumericUpDown num)
        {
            if (num == null) return;
            num.Font = new Font("Segoe UI", 11f);
            num.BackColor = Color.FromArgb(250, 250, 250);
        }

        private void StylePrimaryButton(Button btn)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(37, 99, 235);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(29, 78, 216);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(37, 99, 235);
        }

        // =========================
        // ========= LOGIC ==========
        // =========================

        // ✅ تحميل الموردين + تحميل منتجات أول مورد
        private void LoadSuppliers()
        {
            string sql = "SELECT id_supplier, nom FROM dbo.Suppliers ORDER BY nom;";
            DataTable dt = d.GetData(sql, "suppliers");

            cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;

            cmbSupplier.DisplayMember = "nom";
            cmbSupplier.ValueMember = "id_supplier";
            cmbSupplier.DataSource = dt;

            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbSupplier.SelectedIndex = 0;
                LoadProductsForCurrentSupplier();
            }
            else
            {
                cmbProduct.DataSource = null;
                if (lblInfo != null) lblInfo.Text = "Aucun fournisseur.";
            }
        }

        // ✅ الطريقة الصح: اعتمد على SelectedItem (DataRowView) بدل SelectedValue
        private void LoadProductsForCurrentSupplier()
        {
            if (!(cmbSupplier.SelectedItem is DataRowView sdrv))
                return;

            int idSupplier = Convert.ToInt32(sdrv["id_supplier"]);

            string sql =
                "SELECT id_produit, nom " +
                "FROM dbo.Products " +
                "WHERE actif = 1 AND id_supplier = " + idSupplier + " " +
                "ORDER BY nom;";

            DataTable dt = d.GetData(sql, "productsOfSupplier");

            cmbProduct.DisplayMember = "nom";
            cmbProduct.ValueMember = "id_produit";
            cmbProduct.DataSource = dt;

            if (lblInfo != null)
            {
                if (dt == null || dt.Rows.Count == 0)
                    lblInfo.Text = "Ce fournisseur n'a aucun produit.";
                else
                    lblInfo.Text = "";
            }
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProductsForCurrentSupplier();
        }

        private void btnSaveRestock_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("Choisissez un produit.");
                return;
            }

            if (cmbProduct.SelectedValue is DataRowView)
            {
                MessageBox.Show("Veuillez re-sélectionner le produit.");
                return;
            }

            int idProduit = Convert.ToInt32(cmbProduct.SelectedValue);
            int qty = Convert.ToInt32(numQty.Value);

            // 1) Insert into Purchases
            string sqlPurchase =
                "DECLARE @newId INT; " +
                $"EXEC dbo.usp_Purchases_Insert @id_produit = {idProduit}, @date_achat = NULL, @quantite = {qty}, @new_id_purchase = @newId OUTPUT; " +
                "SELECT @newId AS id_purchase;";

            DataTable dtPurchase = d.GetData(sqlPurchase, "purchase");

            if (dtPurchase == null || dtPurchase.Rows.Count == 0)
            {
                MessageBox.Show("Erreur lors de l'enregistrement de l'achat.");
                return;
            }

            int idPurchase = Convert.ToInt32(dtPurchase.Rows[0]["id_purchase"]);

            // 2) Increase inventory
            string sqlInv =
                $"EXEC dbo.usp_Inventory_IncreaseOnPurchase @id_produit = {idProduit}, @quantite = {qty};";

            d.ExecuteActionCommand(sqlInv);

            // 3) Show new stock
            object newStock = d.GetValue($"SELECT quantite_actuelle FROM dbo.Inventory WHERE id_produit = {idProduit};");

            string prodName = "";
            if (cmbProduct.SelectedItem is DataRowView drv)
                prodName = drv["nom"].ToString();

            MessageBox.Show(
                $"Restock OK.\nPurchase ID = {idPurchase}\nProduit: {prodName}\n+{qty}\nNouveau stock: {(newStock == null ? "?" : newStock.ToString())}"
            );

            if (lblInfo != null)
                lblInfo.Text = "Nouveau stock: " + (newStock == null ? "?" : newStock.ToString());

            numQty.Value = 1;
        }
    }
}