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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();           // Must be first: controls are created here
            ApplyLoginStyle_NoRename();      // Apply UI styling after controls exist

            // Enter triggers Login
            this.AcceptButton = btnLogin;

            // Hide password by default
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string pass = txtPassword.Text;

            if (login == "" || pass == "")
            {
                MessageBox.Show("Veuillez saisir le login et le mot de passe.");
                return;
            }

            // Basic escape since SQL text is used
            string loginEsc = login.Replace("'", "''");
            string passEsc = pass.Replace("'", "''");

            DataLayer d = new DataLayer(@".\SQLEXPRESS", "KioskDB2");

            if (!d.IsValid)
            {
                MessageBox.Show("Connexion SQL invalide.");
                return;
            }

            string sql =
                $"EXEC dbo.usp_Auth_Login " +
                $"@login = N'{loginEsc}', " +
                $"@mot_de_passe = N'{passEsc}'";

            DataTable dt = d.GetData(sql, "auth");

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Login ou mot de passe incorrect.");
                return;
            }

            DataRow r = dt.Rows[0];

            Session.CurrentUserId = Convert.ToInt32(r["id_user"]);
            Session.CurrentUserRole = r["role"].ToString();
            Session.CurrentUserName = r["prenom"].ToString() + " " + r["nom"].ToString();

            MessageBox.Show("Bienvenue " + Session.CurrentUserName);

            // Open main form
            MainForm main = new MainForm();
            main.Show();
            this.Hide();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void txtLogin_TextChanged(object sender, EventArgs e) { }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Show/Hide password (single handler to avoid double execution)
            txtPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void ApplyLoginStyle_NoRename()
        {
            // ===== Theme
            this.BackColor = Color.FromArgb(245, 247, 251);
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Optional: block paste/copy shortcuts in password box
            // txtPassword.ShortcutsEnabled = false;

            // ===== Existing controls (no renaming)
            Label lblUser = label1;          // "User Name"
            Label lblPass = label2;          // "Password"
            TextBox txtUser = txtLogin;
            TextBox txtPass = txtPassword;
            CheckBox chkShow = checkBox1;
            Button btn = btnLogin;

            // ===== Create a centered card and move existing controls into it
            var card = new Panel
            {
                Size = new Size(380, 300),
                BackColor = Color.White
            };
            this.Controls.Add(card);

            card.Controls.Add(lblUser);
            card.Controls.Add(txtUser);
            card.Controls.Add(lblPass);
            card.Controls.Add(txtPass);
            card.Controls.Add(chkShow);
            card.Controls.Add(btn);

            // ===== Style controls
            lblUser.ForeColor = Color.FromArgb(55, 65, 81);
            lblPass.ForeColor = Color.FromArgb(55, 65, 81);

            txtUser.BorderStyle = BorderStyle.FixedSingle;
            txtPass.BorderStyle = BorderStyle.FixedSingle;

            chkShow.ForeColor = Color.FromArgb(55, 65, 81);

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(37, 99, 235);
            btn.ForeColor = Color.White;
            btn.Height = 40;

            // ===== Layout inside card
            int left = 40;
            int w = 300;

            lblUser.SetBounds(left, 30, w, 20);
            txtUser.SetBounds(left, 55, w, 32);

            lblPass.SetBounds(left, 105, w, 20);
            txtPass.SetBounds(left, 130, w, 32);

            chkShow.SetBounds(left, 180, w, 24);
            btn.SetBounds(left, 215, w, 42);

            // ===== Center the card on the form
            CenterControl(card);
            this.Resize += (s, e) => CenterControl(card);

            // UX: Enter triggers login
            this.AcceptButton = btn;
        }

        private void CenterControl(Control c)
        {
            c.Left = (this.ClientSize.Width - c.Width) / 2;
            c.Top = (this.ClientSize.Height - c.Height) / 2;
        }
    }

    // Session (current user info storage)
    public static class Session
    {
        public static int CurrentUserId { get; set; }
        public static string CurrentUserRole { get; set; }
        public static string CurrentUserName { get; set; }
    }
}