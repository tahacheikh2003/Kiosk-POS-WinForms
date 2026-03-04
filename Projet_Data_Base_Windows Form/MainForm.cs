using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MyNameSpace;

namespace Projet_Data_Base_Taha
{
    public partial class MainForm : Form
    {
        private Panel sidebar;
        private Panel header;
        private Panel content;

        private Color Bg = Color.FromArgb(245, 247, 251);
        private Color SidebarBg = Color.FromArgb(17, 24, 39);
        private Color SidebarHover = Color.FromArgb(31, 41, 55);
        private Color SidebarActive = Color.FromArgb(37, 99, 235);
        private Color TextLight = Color.White;
        private Color TextMuted = Color.FromArgb(209, 213, 219);

        private Button _activeBtn = null;

        public MainForm()
        {
            InitializeComponent();
            ApplyMainStyle_NoRename();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "Kiosk - " + Session.CurrentUserName + " (" + Session.CurrentUserRole + ")";

            if (Session.CurrentUserRole == "Caissier")
            {
                btnProducts.Visible = false;
                btnSuppliers.Visible = false;
                btnStock.Visible = false;
            }
        }

        private void ApplyMainStyle_NoRename()
        {
            this.BackColor = Bg;
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterScreen;

            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = SidebarBg
            };

            header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = Color.White
            };

            content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg
            };

            this.Controls.Add(content);
            this.Controls.Add(header);
            this.Controls.Add(sidebar);

            var menuButtons = new Button[]
            {
                btnPOS,
                btnShift,
                btnProducts,
                btnSuppliers,
                btnStock,
                btnDailyReport,
                Restock,
                btnLogout
            };

            foreach (var b in menuButtons)
            {
                if (b.Parent != null) b.Parent.Controls.Remove(b);
                sidebar.Controls.Add(b);
                StyleSidebarButton(b);
            }

            int top = 110;
            int h = 44;
            int gap = 10;

            BuildHeaderContent();

            var lblApp = new Label
            {
                Text = "KIOSK",
                ForeColor = TextLight,
                Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            sidebar.Controls.Add(lblApp);

            var lblUser = new Label
            {
                Text = Session.CurrentUserName,
                ForeColor = TextMuted,
                AutoSize = true,
                Location = new Point(20, 52)
            };
            sidebar.Controls.Add(lblUser);

            foreach (var b in menuButtons)
            {
                if (b == btnLogout) continue;

                b.SetBounds(12, top, sidebar.Width - 24, h);
                top += h + gap;
            }

            btnLogout.SetBounds(12, sidebar.Height - 60, sidebar.Width - 24, 44);
            btnLogout.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            SetActive(btnPOS);
        }

        private void BuildHeaderContent()
        {
            header.Controls.Clear();

            var lblTitle = new Label
            {
                Text = "Tableau de bord",
                ForeColor = Color.FromArgb(17, 24, 39),
                Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(18, 18)
            };

            var lblInfo = new Label
            {
                Text = Session.CurrentUserName + " • " + Session.CurrentUserRole,
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            header.Controls.Add(lblTitle);
            header.Controls.Add(lblInfo);

            header.Resize += (s, e) =>
            {
                lblInfo.Location = new Point(header.Width - lblInfo.Width - 18, 22);
            };

            lblInfo.Location = new Point(header.Width - lblInfo.Width - 18, 22);
        }

        private void StyleSidebarButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = SidebarBg;
            b.ForeColor = TextLight;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(14, 0, 0, 0);
            b.Cursor = Cursors.Hand;

            b.MouseEnter += (s, e) =>
            {
                if (_activeBtn != b) b.BackColor = SidebarHover;
            };

            b.MouseLeave += (s, e) =>
            {
                if (_activeBtn != b) b.BackColor = SidebarBg;
            };

            b.Click += (s, e) => SetActive(b);
        }

        private void SetActive(Button b)
        {
            if (_activeBtn != null)
            {
                _activeBtn.BackColor = SidebarBg;
                _activeBtn.ForeColor = TextLight;
            }

            _activeBtn = b;
            _activeBtn.BackColor = SidebarActive;
            _activeBtn.ForeColor = TextLight;
        }

        // =========================================================
        // ✅ SHIFT CHECK: prevent POS if no open shift
        // ✅ Uses your existing SP (same as ShiftForm)
        // =========================================================
        private bool HasOpenShift()
        {
            try
            {
                DataLayer dl = new DataLayer(@".\SQLEXPRESS", "KioskDB2");
                if (!dl.IsValid) return false;

                string sql = $"EXEC dbo.usp_Shift_GetOpenByUser @id_user = {Session.CurrentUserId};";
                DataTable dt = dl.GetData(sql, "openShiftCheck");

                return (dt != null && dt.Rows.Count > 0);
            }
            catch
            {
                return false;
            }
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            new SuppliersForm().ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Close();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            new ProductsForm().ShowDialog();
        }

        private void btnShift_Click(object sender, EventArgs e)
        {
            new ShiftForm().ShowDialog();
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            if (!HasOpenShift())
            {
                MessageBox.Show(
                    "Vous devez ouvrir un shift avant de faire une vente.",
                    "Shift fermé",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            new PosForm().ShowDialog();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            new StockForm().ShowDialog();
        }

        private void btnDailyReport_Click(object sender, EventArgs e)
        {
            new DailySalesReportForm().ShowDialog();
        }

        private void Restock_Click(object sender, EventArgs e)
        {
            new RestockForm().ShowDialog();
        }
    }
}