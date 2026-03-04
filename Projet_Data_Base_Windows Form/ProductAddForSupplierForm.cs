using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class ProductAddForSupplierForm : Form
    {
        private readonly int supplierId;
        private DataLayer d;

        public ProductAddForSupplierForm(int supplierId)
        {
            InitializeComponent();
            this.supplierId = supplierId;
        }

        private void ProductAddForSupplierForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            LoadCategories();
            chkActif.Checked = true;
        }
        private void LoadCategories()
        {
            string sql = "SELECT id_categorie, libelle FROM dbo.Categories ORDER BY libelle;";
            DataTable dt = d.GetData(sql, "cats");
            cmbCategories.DisplayMember = "libelle";
            cmbCategories.ValueMember = "id_categorie";
            cmbCategories.DataSource = dt;
        }

        private string SqlStr(string s) => "N'" + s.Replace("'", "''") + "'";

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nom = txtNom.Text.Trim();
            if (string.IsNullOrWhiteSpace(nom))
            {
                MessageBox.Show("Nom obligatoire.");
                return;
            }

            if (!decimal.TryParse(txtPrix.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal prix))
            {
                MessageBox.Show("Prix invalide.");
                return;
            }

            if (!int.TryParse(txtStockMin.Text.Trim(), out int stockMin))
            {
                MessageBox.Show("Stock minimum invalide.");
                return;
            }

            if (cmbCategories.SelectedValue == null)
            {
                MessageBox.Show("Choisissez une catégorie.");
                return;
            }

            int idCat = Convert.ToInt32(cmbCategories.SelectedValue);
            int actifBit = chkActif.Checked ? 1 : 0;
            string prixSql = prix.ToString("0.00").Replace(',', '.');

            string sql =
                "DECLARE @newId INT; " +
                $"EXEC dbo.usp_Products_Insert " +
                $"@nom = {SqlStr(nom)}, @prix_vente = {prixSql}, @stock_minimum = {stockMin}, @actif = {actifBit}, " +
                $"@id_categorie = {idCat}, @id_supplier = {supplierId}, @new_id_produit = @newId OUTPUT; " +
                "SELECT @newId AS id_produit;";

            DataTable dt = d.GetData(sql, "newProd");
            if (dt != null && dt.Rows.Count > 0)
            {
                MessageBox.Show("Produit ajouté. ID=" + dt.Rows[0]["id_produit"].ToString());

                // OPTIONAL: تأمين row بالـ Inventory (إذا بدك تبدأ بـ 0)
                int newProdId = Convert.ToInt32(dt.Rows[0]["id_produit"]);
                d.ExecuteActionCommand($"IF NOT EXISTS(SELECT 1 FROM dbo.Inventory WHERE id_produit={newProdId}) " +
                                       $"INSERT INTO dbo.Inventory(id_produit, quantite_actuelle, date_maj) VALUES({newProdId}, 0, SYSDATETIME());");

                Close();
            }
        }
    }
}
    
    

