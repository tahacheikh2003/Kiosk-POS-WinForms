using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class PosForm : Form
    {
        private DataLayer d;
        private DataTable cart; // sack de sell

        // ===== عناصر للستايل (ما بتأثر على اللوجيك)
        private Panel card;
        private Panel header;

        public PosForm()
        {
            InitializeComponent();
            ApplyPosStyle_NoRename(); // ✅ ستايل فقط
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 800);
        }

        private void ApplyPosStyle_NoRename()
        {
            // ===== ستايل عام للفورم
            this.Text = "POS";
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterParent;

            // ===== هيدر فوق (عنوان بسيط)
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
                Text = "Nouvelle vente",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(18, 18)
            };
            header.Controls.Add(lblTitle);

            // ===== كارد (منطقة الشغل)
            card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            this.Controls.Add(card);
            card.BringToFront();

            // ===== ننقل كل الكنترولز (ما عدا الهيدر) على الكارد
            // ملاحظة: هيك الليبلز تبعك بتضل طالعة ومكانها منضبط جوّا الكارد
            var toMove = this.Controls.Cast<Control>()
                .Where(c => c != header && c != card)
                .ToList();

            foreach (var c in toMove)
            {
                this.Controls.Remove(c);
                card.Controls.Add(c);
            }

            // ===== ستايل Inputs (بدون تغيير أسماء)
            if (cmbProducts != null)
            {
                cmbProducts.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbProducts.Font = new Font("Segoe UI", 10f);
            }

            if (txtPrice != null)
            {
                txtPrice.BorderStyle = BorderStyle.FixedSingle;
                txtPrice.Font = new Font("Segoe UI", 10f);
                txtPrice.BackColor = Color.FromArgb(243, 244, 246); // رمادي خفيف
                txtPrice.ReadOnly = true;
            }

            if (numQty != null)
            {
                numQty.Font = new Font("Segoe UI", 10f);
            }

            // ===== ستايل DataGridView
            if (dgvCart != null)
            {
                dgvCart.BackgroundColor = Color.White;
                dgvCart.BorderStyle = BorderStyle.FixedSingle;
                dgvCart.GridColor = Color.FromArgb(229, 231, 235);
                dgvCart.RowHeadersVisible = false;
                dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCart.EnableHeadersVisualStyles = false;

                dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
                dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(17, 24, 39);
                dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

                dgvCart.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
                dgvCart.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
                dgvCart.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            }

            // ===== ستايل Buttons
            StylePrimaryButton(btnAdd);
            StylePrimaryButton(btnSave);
            StyleSecondaryButton(btnSaveAndPrint);
            StylePrimaryButton(btnRemove);

            // ===== ستايل Total label (إذا موجود)
            if (lblTotal != null)
            {
                lblTotal.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold);
                lblTotal.ForeColor = Color.FromArgb(17, 24, 39);
            }
            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.White
            };
            card.Controls.Add(footer);
            footer.BringToFront();

            // ننقل الأزرار + total للـ footer (بدون ما نغير أسماء)
            if (btnRemove.Parent != null) btnRemove.Parent.Controls.Remove(btnRemove);
            if (btnAdd.Parent != null) btnAdd.Parent.Controls.Remove(btnAdd);
            if (btnSave.Parent != null) btnSave.Parent.Controls.Remove(btnSave);
            if (btnSaveAndPrint.Parent != null) btnSaveAndPrint.Parent.Controls.Remove(btnSaveAndPrint);
            if (lblTotal.Parent != null) lblTotal.Parent.Controls.Remove(lblTotal);

            footer.Controls.Add(btnAdd);
            footer.Controls.Add(btnSave);
            footer.Controls.Add(btnSaveAndPrint);
            footer.Controls.Add(lblTotal);
            footer.Controls.Add(btnRemove);
            // نخليهم ثابتين تحت
            btnAdd.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnSave.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnSaveAndPrint.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblTotal.Anchor = AnchorStyles.Bottom;




            // أحجام (إذا بدك)
            btnAdd.Width = 180;
            btnSave.Width = 180;
            btnSaveAndPrint.Width = 180;

            btnAdd.Height = 40;
            btnSave.Height = 40;
            btnSaveAndPrint.Height = 40;

            // مكانهم
            int padding = 20;
            int y = (footer.Height - 40) / 2;


            btnRemove.Width = 180;
            btnRemove.Height = 40;
            btnRemove.Location = new Point(btnAdd.Right + 12, y);
            btnAdd.Location = new Point(padding, y);

            // Total بالنص
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(17, 24, 39);
            lblTotal.Location = new Point((footer.Width - lblTotal.Width) / 2, y + 8);

            // يمين: Save + Save&Print
            btnSaveAndPrint.Location = new Point(footer.Width - padding - btnSaveAndPrint.Width, y);
            btnSave.Location = new Point(btnSaveAndPrint.Left - 12 - btnSave.Width, y);

            // تحديث أماكنهم عند Resize
            footer.Resize += (s, e) =>
            {
                btnSaveAndPrint.Location = new Point(footer.Width - padding - btnSaveAndPrint.Width, y);
                btnSave.Location = new Point(btnSaveAndPrint.Left - 12 - btnSave.Width, y);
                lblTotal.Location = new Point((footer.Width - lblTotal.Width) / 2, y + 8);
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

            // Hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(243, 244, 246);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.White;
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

        // ===================== كودك الأصلي (بدون تغيير اللوجيك) =====================

        private void InitCartTable()
        {
            cart = new DataTable();
            cart.Columns.Add("id_produit", typeof(int));
            cart.Columns.Add("produit", typeof(string));
            cart.Columns.Add("prix_unitaire", typeof(decimal));
            cart.Columns.Add("quantite", typeof(int));
            cart.Columns.Add("sous_total", typeof(decimal));

            dgvCart.AutoGenerateColumns = true;
            dgvCart.DataSource = cart;

            dgvCart.ReadOnly = true;
            dgvCart.AllowUserToAddRows = false;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblTotal.Text = "Total: 0";
        }

        private void LoadProducts()
        {
            string sql =
                "SELECT p.id_produit, p.nom, p.prix_vente, i.quantite_actuelle " +
                "FROM dbo.Products p " +
                "INNER JOIN dbo.Inventory i ON i.id_produit = p.id_produit " +
                "WHERE p.actif = 1 " +
                "ORDER BY p.nom;";

            DataTable dt = d.GetData(sql, "products");
            cmbProducts.DisplayMember = "nom";
            cmbProducts.ValueMember = "id_produit";
            cmbProducts.DataSource = dt;
        }

        private void RefreshTotal()
        {
            decimal total = 0;
            foreach (DataRow r in cart.Rows)
                total += Convert.ToDecimal(r["sous_total"]);

            lblTotal.Text = "Total: " + total.ToString("0.00");
        }

        private void PosForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            InitCartTable();
            LoadProducts();

            numQty.Minimum = 1;
            numQty.Value = 1;

            // عرض السعر أول ما تختار منتج
            cmbProducts_SelectedIndexChanged(null, null);
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem is DataRowView drv)
            {
                decimal price = Convert.ToDecimal(drv["prix_vente"]);
                int stock = Convert.ToInt32(drv["quantite_actuelle"]);

                txtPrice.Text = price.ToString("0.00");

                numQty.Maximum = Math.Max(1, stock);
                if (numQty.Value > numQty.Maximum) numQty.Value = numQty.Maximum;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!(cmbProducts.SelectedItem is DataRowView drv))
            {
                MessageBox.Show("Choisissez un produit.");
                return;
            }

            int idProduit = Convert.ToInt32(drv["id_produit"]);
            string nom = drv["nom"].ToString();
            decimal price = Convert.ToDecimal(drv["prix_vente"]);
            int stock = Convert.ToInt32(drv["quantite_actuelle"]);
            int qty = Convert.ToInt32(numQty.Value);

            if (stock <= 0)
            {
                MessageBox.Show("Stock insuffisant (0).");
                return;
            }

            // إذا المنتج موجود بالسلة: زيد الكمية
            DataRow existing = cart.AsEnumerable()
                .FirstOrDefault(r => Convert.ToInt32(r["id_produit"]) == idProduit);

            int qtyInCart = existing == null ? 0 : Convert.ToInt32(existing["quantite"]);
            if (qtyInCart + qty > stock)
            {
                MessageBox.Show($"Stock insuffisant. Disponible: {stock}, Dans le panier: {qtyInCart}");
                return;
            }

            if (existing == null)
            {
                DataRow r = cart.NewRow();
                r["id_produit"] = idProduit;
                r["produit"] = nom;
                r["prix_unitaire"] = price;
                r["quantite"] = qty;
                r["sous_total"] = qty * price;
                cart.Rows.Add(r);
            }
            else
            {
                int newQty = qtyInCart + qty;
                existing["quantite"] = newQty;
                existing["sous_total"] = newQty * price;
            }

            RefreshTotal();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cart.Rows.Count == 0)
            {
                MessageBox.Show("Panier vide.");
                return;
            }

            decimal total = 0;
            foreach (DataRow r in cart.Rows)
                total += Convert.ToDecimal(r["sous_total"]);

            string totalSql = total.ToString("0.00").Replace(',', '.');

            string sqlSale =
                "DECLARE @newId INT; " +
                $"EXEC dbo.usp_Sales_Insert @id_user = {Session.CurrentUserId}, @date_heure = NULL, @montant_total = {totalSql}, @new_id_sale = @newId OUTPUT; " +
                "SELECT @newId AS id_sale;";

            DataTable dtSale = d.GetData(sqlSale, "sale");
            if (dtSale == null || dtSale.Rows.Count == 0)
            {
                MessageBox.Show("Erreur lors de la création de la vente.");
                return;
            }

            int idSale = Convert.ToInt32(dtSale.Rows[0]["id_sale"]);

            foreach (DataRow r in cart.Rows)
            {
                int idProduit = Convert.ToInt32(r["id_produit"]);
                int qty = Convert.ToInt32(r["quantite"]);
                decimal pu = Convert.ToDecimal(r["prix_unitaire"]);
                string puSql = pu.ToString("0.00").Replace(',', '.');

                string sqlLine =
                    "DECLARE @newLineId INT; " +
                    $"EXEC dbo.usp_SaleDetails_Insert @id_sale = {idSale}, @id_produit = {idProduit}, @quantite = {qty}, @prix_unitaire = {puSql}, @new_id_sale_detail = @newLineId OUTPUT;";

                d.ExecuteActionCommand(sqlLine);

                string sqlInv =
                    $"EXEC dbo.usp_Inventory_DecreaseOnSale @id_produit = {idProduit}, @quantite = {qty}";

                d.ExecuteActionCommand(sqlInv);
            }

            MessageBox.Show($"Vente enregistrée. ID Sale = {idSale}");

            cart.Rows.Clear();
            RefreshTotal();

            LoadProducts();


        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            int idSale = SaveSaleAndReturnId();
            if (idSale == -1) return;

            InvoiceForm.PrintInvoiceSilent(idSale);
        }
        private int SaveSaleAndReturnId()
        {
            if (cart.Rows.Count == 0)
            {
                MessageBox.Show("Panier vide.");
                return -1;
            }

            decimal total = 0;
            foreach (DataRow r in cart.Rows)
                total += Convert.ToDecimal(r["sous_total"]);

            string totalSql = total.ToString("0.00").Replace(',', '.');

            string sqlSale =
                "DECLARE @newId INT; " +
                $"EXEC dbo.usp_Sales_Insert @id_user = {Session.CurrentUserId}, @date_heure = NULL, @montant_total = {totalSql}, @new_id_sale = @newId OUTPUT; " +
                "SELECT @newId AS id_sale;";

            DataTable dtSale = d.GetData(sqlSale, "sale");
            if (dtSale == null || dtSale.Rows.Count == 0)
            {
                MessageBox.Show("Erreur lors de la création de la vente.");
                return -1;
            }

            int idSale = Convert.ToInt32(dtSale.Rows[0]["id_sale"]);

            foreach (DataRow r in cart.Rows)
            {
                int idProduit = Convert.ToInt32(r["id_produit"]);
                int qty = Convert.ToInt32(r["quantite"]);
                decimal pu = Convert.ToDecimal(r["prix_unitaire"]);
                string puSql = pu.ToString("0.00").Replace(',', '.');

                string sqlLine =
                    "DECLARE @newLineId INT; " +
                    $"EXEC dbo.usp_SaleDetails_Insert @id_sale = {idSale}, @id_produit = {idProduit}, @quantite = {qty}, @prix_unitaire = {puSql}, @new_id_sale_detail = @newLineId OUTPUT;";

                d.ExecuteActionCommand(sqlLine);

                string sqlInv =
                    $"EXEC dbo.usp_Inventory_DecreaseOnSale @id_produit = {idProduit}, @quantite = {qty}";

                d.ExecuteActionCommand(sqlInv);
            }

            // reset cart + refresh UI
            cart.Rows.Clear();
            RefreshTotal();
            LoadProducts();

            return idSale;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez une ligne dans le panier.");
                return;
            }

            // بما إن dgvCart مربوط بـ DataTable cart
            int rowIndex = dgvCart.CurrentRow.Index;

            if (rowIndex < 0 || rowIndex >= cart.Rows.Count)
                return;

            // تأكيد (اختياري)
            var prodName = cart.Rows[rowIndex]["produit"].ToString();
            var res = MessageBox.Show($"Supprimer '{prodName}' du panier ?", "Confirmation",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            cart.Rows.RemoveAt(rowIndex);
            RefreshTotal();
        }
    }
}
