using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KLib.Controls
{
    /// <summary>
    /// Centers a MessageBox over a specified owner window.
    /// Usage: using (new MessageBoxCenterer(this)) { MessageBox.Show(...); }
    /// </summary>
    public class MessageBoxCenterer : IDisposable
    {
        #region Native

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        private const int WH_CBT = 5;
        private const int HCBT_ACTIVATE = 5;

        #endregion

        private readonly IWin32Window _owner;
        private readonly HookProc _hookProc;   // must hold a reference to prevent GC
        private IntPtr _hook;

        public MessageBoxCenterer(IWin32Window owner)
        {
            _owner = owner;
            _hookProc = CbtProc;
            _hook = SetWindowsHookEx(WH_CBT, _hookProc, IntPtr.Zero, GetCurrentThreadId());
        }

        private IntPtr CbtProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == HCBT_ACTIVATE)
            {
                CenterOnOwner(wParam);
                UnhookWindowsHookEx(_hook);
                _hook = IntPtr.Zero;
            }
            return CallNextHookEx(_hook, nCode, wParam, lParam);
        }

        private void CenterOnOwner(IntPtr hWndMessageBox)
        {
            if (!GetWindowRect(hWndMessageBox, out RECT mbRect)) return;

            var ownerForm = _owner as Form;
            if (ownerForm == null) return;

            var ownerBounds = ownerForm.Bounds;

            int mbWidth = mbRect.Right - mbRect.Left;
            int mbHeight = mbRect.Bottom - mbRect.Top;

            int x = ownerBounds.Left + (ownerBounds.Width - mbWidth) / 2;
            int y = ownerBounds.Top + (ownerBounds.Height - mbHeight) / 2;

            // Clamp to screen so it can't vanish off an edge
            var screen = Screen.FromControl(ownerForm).WorkingArea;
            x = Math.Max(screen.Left, Math.Min(x, screen.Right - mbWidth));
            y = Math.Max(screen.Top, Math.Min(y, screen.Bottom - mbHeight));

            MoveWindow(hWndMessageBox, x, y, mbWidth, mbHeight, false);
        }

        public void Dispose()
        {
            if (_hook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hook);
                _hook = IntPtr.Zero;
            }
        }
    }
}