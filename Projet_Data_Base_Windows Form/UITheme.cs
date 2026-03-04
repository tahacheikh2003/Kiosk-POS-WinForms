using System.Drawing;
using System.Windows.Forms;

namespace Projet_Data_Base_Taha
{
    public static class UITheme
    {
        // Colors
        public static readonly Color Bg = Color.FromArgb(245, 247, 251);
        public static readonly Color Card = Color.White;
        public static readonly Color Primary = Color.FromArgb(37, 99, 235);
        public static readonly Color PrimaryHover = Color.FromArgb(29, 78, 216);
        public static readonly Color Text = Color.FromArgb(55, 65, 81);

        public static void ApplyBase(Form f)
        {
            f.BackColor = Bg;
            f.Font = new Font("Segoe UI", 10f);
            f.StartPosition = FormStartPosition.CenterScreen;
        }

        public static void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Primary;
            btn.ForeColor = Color.White;

            // Hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = PrimaryHover;
            btn.MouseLeave += (s, e) => btn.BackColor = Primary;
        }

        public static Panel CreateCard(Form f, int width, int height)
        {
            var card = new Panel
            {
                Size = new Size(width, height),
                BackColor = Card
            };
            f.Controls.Add(card);

            Center(card, f);
            f.Resize += (s, e) => Center(card, f);

            return card;
        }

        public static void Center(Control c, Form f)
        {
            c.Left = (f.ClientSize.Width - c.Width) / 2;
            c.Top = (f.ClientSize.Height - c.Height) / 2;
        }
    }
}