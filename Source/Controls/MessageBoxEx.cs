using KLib.Controls;
using System.Windows.Forms;

public static class MessageBoxEx
{
    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
    {
        using (new MessageBoxCenterer(owner))
            return MessageBox.Show(owner, text, caption, buttons);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption)
    {
        using (new MessageBoxCenterer(owner))
            return MessageBox.Show(owner, text, caption);
    }

    public static void Show(IWin32Window owner, string text)
    {
        using (new MessageBoxCenterer(owner))
            MessageBox.Show(owner, text);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        using (new MessageBoxCenterer(owner))
            return MessageBox.Show(owner, text, caption, buttons, icon);
    }

    // Add further overloads (MessageBoxIcon, etc.) as you need them
}