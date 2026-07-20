using System;
using System.Drawing;
using System.Windows.Forms;

namespace KLib.Controls
{
    /// <summary>
    /// A MessageBox-style dialog with arbitrary button labels, centered on its owner.
    /// Usage: int choice = CustomMessageBox.Show(this, "Save changes?", "Confirm", "Yes", "No", "Restart");
    /// </summary>
    public static class CustomMessageBox
    {
        /// <summary>
        /// Shows the dialog and returns the 0-based index of the clicked button,
        /// or -1 if the dialog was dismissed via the [X] title-bar button.
        /// </summary>
        /// <param name="owner">Window to center over. If null, centers on the screen.</param>
        /// <param name="text">Message body.</param>
        /// <param name="caption">Title-bar caption.</param>
        /// <param name="buttons">One or more button labels, left to right.</param>
        public static int Show(IWin32Window owner, string text, string caption, params string[] buttons)
        {
            if (buttons == null || buttons.Length == 0)
                throw new ArgumentException("At least one button label is required.", nameof(buttons));

            int clicked = -1;

            using (var form = new Form
            {
                Text = caption,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = owner != null
                    ? FormStartPosition.CenterParent
                    : FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
            })
            {
                // Measure the message so the dialog fits its content, wrapping past a max width.
                const int maxTextWidth = 380;
                Size textSize = TextRenderer.MeasureText(
                    text, form.Font,
                    new Size(maxTextWidth, int.MaxValue),
                    TextFormatFlags.WordBreak);

                var label = new Label
                {
                    Text = text,
                    AutoSize = false,
                    Bounds = new Rectangle(15, 15, Math.Max(textSize.Width, 200), textSize.Height),
                };

                var panel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Dock = DockStyle.Bottom,
                    Padding = new Padding(10),
                    Height = 50,
                };

                // Add right-to-left so buttons[0] lands leftmost.
                for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    int index = i; // capture per-iteration for the closure
                    var btn = new Button
                    {
                        Text = buttons[i],
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        MinimumSize = new Size(80, 28),
                        Margin = new Padding(6, 0, 0, 0),
                    };
                    btn.Click += (s, e) => { clicked = index; form.Close(); };
                    panel.Controls.Add(btn);

                    if (i == 0) form.AcceptButton = btn; // Enter triggers the first button
                }

                form.Controls.Add(label);
                form.Controls.Add(panel);

                form.ClientSize = new Size(
                    Math.Max(label.Right + 15, 260),
                    label.Bottom + 15 + panel.Height);

                // CancelButton is intentionally unset: with no natural "cancel" among
                // Yes/No/Restart, Esc does nothing and only [X] returns -1.
                form.ShowDialog(owner);
            }

            return clicked;
        }
    }
}