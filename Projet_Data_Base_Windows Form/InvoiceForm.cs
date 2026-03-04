using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class InvoiceForm : Form
    {
        private readonly int _idSale;
        private DataLayer d;

        private DataTable header;
        private DataTable lines;

        private PrintDocument pd;
        public InvoiceForm(int idSale)
        {
            InitializeComponent();
            _idSale = idSale;
        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                Close();
                return;
            }

            LoadInvoiceData();
            lblInfo.Text = "Facture N° " + _idSale;
        }
        private void LoadInvoiceData()
        {
            // SP ترجع 2 result sets، وDataLayer ما بتدعمهم مباشرة
            // فنعمل استعلامين: واحد للهيدر، واحد للـ lines
            string sqlHeader =
                $"SELECT s.id_sale, s.date_heure, s.montant_total, (u.nom + ' ' + u.prenom) AS caissier " +
                $"FROM dbo.Sales s INNER JOIN dbo.Users u ON u.id_user = s.id_user " +
                $"WHERE s.id_sale = {_idSale};";

            string sqlLines =
                $"SELECT p.nom AS produit, sd.quantite, sd.prix_unitaire, sd.sous_total " +
                $"FROM dbo.SaleDetails sd INNER JOIN dbo.Products p ON p.id_produit = sd.id_produit " +
                $"WHERE sd.id_sale = {_idSale} ORDER BY sd.id_sale_detail;";

            header = d.GetData(sqlHeader, "header");
            lines = d.GetData(sqlLines, "lines");

            if (header == null || header.Rows.Count == 0 || lines == null)
                MessageBox.Show("Facture introuvable ou données manquantes.");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (header == null || header.Rows.Count == 0)
            {
                MessageBox.Show("Aucune donnée à imprimer.");
                return;
            }

            PrintDialog dlg = new PrintDialog();
            pd = new PrintDocument();
            dlg.Document = pd;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pd.PrinterSettings = dlg.PrinterSettings;
                pd.PrintPage += Pd_PrintPage;
                pd.Print();
            }

        }
        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font normalFont = new Font("Arial", 10);
            Font boldFont = new Font("Arial", 10, FontStyle.Bold);

            int left = 60;
            int top = 50;
            int lineH = 20;

            // Header info
            DataRow h = header.Rows[0];
            string caissier = h["caissier"].ToString();
            string dateStr = Convert.ToDateTime(h["date_heure"]).ToString("dd/MM/yyyy HH:mm");
            string totalStr = Convert.ToDecimal(h["montant_total"]).ToString("0.00");

            g.DrawString("FACTURE / INVOICE", titleFont, Brushes.Black, left, top);
            top += 40;

            g.DrawString("N°: " + _idSale, boldFont, Brushes.Black, left, top); top += lineH;
            g.DrawString("Date: " + dateStr, normalFont, Brushes.Black, left, top); top += lineH;
            g.DrawString("Caissier: " + caissier, normalFont, Brushes.Black, left, top); top += (lineH + 10);

            // Table header
            Pen pen = new Pen(Color.Black, 1);
            int colProd = 260, colQty = 70, colPU = 90, colST = 90;
            int rowH = 25;

            Rectangle rProd = new Rectangle(left, top, colProd, rowH);
            Rectangle rQty = new Rectangle(left + colProd, top, colQty, rowH);
            Rectangle rPU = new Rectangle(left + colProd + colQty, top, colPU, rowH);
            Rectangle rST = new Rectangle(left + colProd + colQty + colPU, top, colST, rowH);

            g.DrawRectangle(pen, rProd);
            g.DrawRectangle(pen, rQty);
            g.DrawRectangle(pen, rPU);
            g.DrawRectangle(pen, rST);

            g.DrawString("Produit", boldFont, Brushes.Black, rProd);
            g.DrawString("Qte", boldFont, Brushes.Black, rQty);
            g.DrawString("PU", boldFont, Brushes.Black, rPU);
            g.DrawString("Sous-total", boldFont, Brushes.Black, rST);

            top += rowH;

            // Lines
            foreach (DataRow r in lines.Rows)
            {
                string prod = r["produit"].ToString();
                string qte = r["quantite"].ToString();
                string pu = Convert.ToDecimal(r["prix_unitaire"]).ToString("0.00");
                string st = Convert.ToDecimal(r["sous_total"]).ToString("0.00");

                rProd = new Rectangle(left, top, colProd, rowH);
                rQty = new Rectangle(left + colProd, top, colQty, rowH);
                rPU = new Rectangle(left + colProd + colQty, top, colPU, rowH);
                rST = new Rectangle(left + colProd + colQty + colPU, top, colST, rowH);

                g.DrawRectangle(pen, rProd);
                g.DrawRectangle(pen, rQty);
                g.DrawRectangle(pen, rPU);
                g.DrawRectangle(pen, rST);

                g.DrawString(prod, normalFont, Brushes.Black, rProd);
                g.DrawString(qte, normalFont, Brushes.Black, rQty);
                g.DrawString(pu, normalFont, Brushes.Black, rPU);
                g.DrawString(st, normalFont, Brushes.Black, rST);

                top += rowH;
            }

            // Total
            top += 15;
            g.DrawString("TOTAL: " + totalStr, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, left + colProd + colQty + colPU - 30, top);

            e.HasMorePages = false;
        }
        public static void PrintInvoiceSilent(int idSale)
        {
            DataLayer d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                return;
            }

            // ===== Load data (نفس الكود)
            string sqlHeader =
                $"SELECT s.id_sale, s.date_heure, s.montant_total, (u.nom + ' ' + u.prenom) AS caissier " +
                $"FROM dbo.Sales s INNER JOIN dbo.Users u ON u.id_user = s.id_user " +
                $"WHERE s.id_sale = {idSale};";

            string sqlLines =
                $"SELECT p.nom AS produit, sd.quantite, sd.prix_unitaire, sd.sous_total " +
                $"FROM dbo.SaleDetails sd INNER JOIN dbo.Products p ON p.id_produit = sd.id_produit " +
                $"WHERE sd.id_sale = {idSale} ORDER BY sd.id_sale_detail;";

            DataTable header = d.GetData(sqlHeader, "header");
            DataTable lines = d.GetData(sqlLines, "lines");

            if (header == null || header.Rows.Count == 0 || lines == null)
            {
                MessageBox.Show("Facture introuvable.");
                return;
            }

            // ===== Print مباشرة (default printer)
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (s, e) =>
            {
                PrintInvoicePage(e, idSale, header, lines);
            };

            pd.Print();
        }
        private static void PrintInvoicePage(
    PrintPageEventArgs e,
    int idSale,
    DataTable header,
    DataTable lines)
        {
            Graphics g = e.Graphics;

            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font normalFont = new Font("Arial", 10);
            Font boldFont = new Font("Arial", 10, FontStyle.Bold);

            int left = 60;
            int top = 50;
            int lineH = 20;

            DataRow h = header.Rows[0];
            string caissier = h["caissier"].ToString();
            string dateStr = Convert.ToDateTime(h["date_heure"]).ToString("dd/MM/yyyy HH:mm");
            string totalStr = Convert.ToDecimal(h["montant_total"]).ToString("0.00");

            g.DrawString("FACTURE / INVOICE", titleFont, Brushes.Black, left, top);
            top += 40;

            g.DrawString("N°: " + idSale, boldFont, Brushes.Black, left, top); top += lineH;
            g.DrawString("Date: " + dateStr, normalFont, Brushes.Black, left, top); top += lineH;
            g.DrawString("Caissier: " + caissier, normalFont, Brushes.Black, left, top); top += lineH + 10;

            Pen pen = new Pen(Color.Black, 1);
            int colProd = 260, colQty = 70, colPU = 90, colST = 90;
            int rowH = 25;

            Rectangle rProd, rQty, rPU, rST;

            rProd = new Rectangle(left, top, colProd, rowH);
            rQty = new Rectangle(left + colProd, top, colQty, rowH);
            rPU = new Rectangle(left + colProd + colQty, top, colPU, rowH);
            rST = new Rectangle(left + colProd + colQty + colPU, top, colST, rowH);

            g.DrawRectangle(pen, rProd);
            g.DrawRectangle(pen, rQty);
            g.DrawRectangle(pen, rPU);
            g.DrawRectangle(pen, rST);

            g.DrawString("Produit", boldFont, Brushes.Black, rProd);
            g.DrawString("Qte", boldFont, Brushes.Black, rQty);
            g.DrawString("PU", boldFont, Brushes.Black, rPU);
            g.DrawString("Sous-total", boldFont, Brushes.Black, rST);

            top += rowH;

            foreach (DataRow r in lines.Rows)
            {
                rProd = new Rectangle(left, top, colProd, rowH);
                rQty = new Rectangle(left + colProd, top, colQty, rowH);
                rPU = new Rectangle(left + colProd + colQty, top, colPU, rowH);
                rST = new Rectangle(left + colProd + colQty + colPU, top, colST, rowH);

                g.DrawRectangle(pen, rProd);
                g.DrawRectangle(pen, rQty);
                g.DrawRectangle(pen, rPU);
                g.DrawRectangle(pen, rST);

                g.DrawString(r["produit"].ToString(), normalFont, Brushes.Black, rProd);
                g.DrawString(r["quantite"].ToString(), normalFont, Brushes.Black, rQty);
                g.DrawString(Convert.ToDecimal(r["prix_unitaire"]).ToString("0.00"), normalFont, Brushes.Black, rPU);
                g.DrawString(Convert.ToDecimal(r["sous_total"]).ToString("0.00"), normalFont, Brushes.Black, rST);

                top += rowH;
            }

            top += 15;
            g.DrawString("TOTAL: " + totalStr,
                new Font("Arial", 12, FontStyle.Bold),
                Brushes.Black,
                left + colProd + colQty + colPU - 30,
                top);

            e.HasMorePages = false;
        }


    }
}
