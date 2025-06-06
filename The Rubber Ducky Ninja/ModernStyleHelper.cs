using System.Drawing;
using System.Windows.Forms;

namespace The_Rubber_Ducky_Ninja
{
    /// <summary>
    /// Helper class that applies a simple modern style to WinForms controls.
    /// Currently it changes the font to Segoe UI and sets FlatStyle on buttons.
    /// </summary>
    public static class ModernStyleHelper
    {
        public static void Apply(Control root)
        {
            if (root == null) return;
            var baseFont = new Font("Segoe UI", root.Font.SizeInPoints);
            ApplyRecursive(root, baseFont);
            root.BackColor = Color.WhiteSmoke;
        }

        private static void ApplyRecursive(Control control, Font font)
        {
            if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
            }

            if (!(control is TextBox) && !(control is RichTextBox))
            {
                control.Font = new Font("Segoe UI", control.Font.SizeInPoints);
            }

            foreach (Control child in control.Controls)
            {
                ApplyRecursive(child, font);
            }
        }
    }
}
