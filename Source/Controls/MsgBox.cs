using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// Code from:
// http://www.codeproject.com/script/articles/download.aspx?file=/KB/dialog/MessageBoxCenterOnParent/CenteredMessageboxDemo.zip&rp=http://www.codeproject.com/Articles/59483/How-to-make-MessageBoxes-center-on-their-parent-fo
//

namespace KLib.Controls
{
	public class MsgBox
	{
		private static Win32.WindowsHookProc _hookProcDelegate = null;
		private static int _hHook = 0;
		private static string _title = null;
		private static string _msg = null;
        private static IntPtr _parent = new IntPtr(0);

        private static string _yesText = "Yes";
        private static string _noText = "No";
        private static string _cancelText = "Cancel";

        public static DialogResult Show(string msg, string title, MessageBoxButtons btns, MessageBoxIcon icon, string yesText, string noText, string cancelText)
        {
            _yesText = yesText;
            _noText = noText;
            _cancelText = cancelText;

            return Show(msg, title, btns, icon, new IntPtr(0));
        }

        public static DialogResult Show(string msg, string title, MessageBoxButtons btns, MessageBoxIcon icon)
        {
            _yesText = "Yes";
            _noText = "No";
            _cancelText = "Cancel";

            return Show(msg, title, btns, icon, new IntPtr(0));
        }

        public static DialogResult Show(string msg, string title, MessageBoxButtons btns, MessageBoxIcon icon, IntPtr parent)
		{
			// Create a callback delegate
			_hookProcDelegate = new Win32.WindowsHookProc(HookCallback);

			// Remember the title & message that we'll look for.
			// The hook sees *all* windows, so we need to make sure we operate on the right one.
			_msg = msg;
			_title = title;
            _parent = parent;

			// Set the hook.
			// Suppress "GetCurrentThreadId() is deprecated" warning. 
			// It's documented that Thread.ManagedThreadId doesn't work with SetWindowsHookEx()
#pragma warning disable 0618
			_hHook = Win32.SetWindowsHookEx(Win32.WH_CBT, _hookProcDelegate, IntPtr.Zero, AppDomain.GetCurrentThreadId());
#pragma warning restore 0618

			// Pop a standard MessageBox. The hook will center it.
			DialogResult rslt = MessageBox.Show(msg, title, btns, icon);

			// Release hook, clean up (may have already occurred)
			Unhook();

			return rslt;
		}

		private static void Unhook()
		{
			Win32.UnhookWindowsHookEx(_hHook);
			_hHook = 0;
			_hookProcDelegate = null;
			_msg = null;
			_title = null;
		}

		private static int HookCallback(int code, IntPtr wParam, IntPtr lParam)
		{
			int hHook = _hHook; // Local copy for CallNextHookEx() JIC we release _hHook

            if (code < 0)
            {
                return Win32.CallNextHookEx(hHook, code, wParam, lParam);
            }

            string cls = Win32.GetClassName(wParam);

            //var msgStruct = (Win32.CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(Win32.CWPRETSTRUCT));
            //if (msgStruct.message == Win32.WM_INITDIALOG)
            //{
            //    if (cls == "#32770")	// MessageBoxes are Dialog boxes
            //    {
            //        Win32.EnumChildWindows(wParam, MessageBoxEnumProc, IntPtr.Zero);
            //    }
            //}
            //if (code == Win32.HCBT_ACTIVATE)
			{
                // Look for HCBT_ACTIVATE, *not* HCBT_CREATEWND:
                //   child controls haven't yet been created upon HCBT_CREATEWND.
                if (cls == "#32770")	// MessageBoxes are Dialog boxes
				{
					string title = Win32.GetWindowText(wParam);
					string msg = Win32.GetDlgItemText(wParam, 0xFFFF);	// -1 aka IDC_STATIC
					if ((title == _title) && (msg == _msg))
					{
                       // var msgStruct = (Win32.CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(Win32.CWPRETSTRUCT));
                        System.Diagnostics.Debug.WriteLine(_title + ": code = " + code);
                        Win32.EnumChildWindows(wParam, MessageBoxEnumProc, IntPtr.Zero);
                        CenterWindowOnParent(wParam);
                        Unhook();	// Release hook - we've done what we needed
                    }
                }
			}
			return Win32.CallNextHookEx(hHook, code, wParam, lParam);
		}

		// Boilerplate window-centering code.
		// Split out of HookCallback() for clarity.
		private static void CenterWindowOnParent(IntPtr hChildWnd)
		{
			// Get child (MessageBox) size
			Win32.RECT rcChild = new Win32.RECT();
			Win32.GetWindowRect(hChildWnd, ref rcChild);
			int cxChild = rcChild.right - rcChild.left;
			int cyChild = rcChild.bottom - rcChild.top;

			// Get parent (Form) size & location
			IntPtr hParent = Win32.GetParent(hChildWnd);
            if (hParent == IntPtr.Zero)
            {
                hParent = _parent;
            }
			Win32.RECT rcParent = new Win32.RECT();
			Win32.GetWindowRect(hParent, ref rcParent);
			int cxParent = rcParent.right - rcParent.left;
			int cyParent = rcParent.bottom - rcParent.top;

			// Center the MessageBox on the Form
			int x = rcParent.left + (cxParent - cxChild) / 2;
			int y = rcParent.top + (cyParent - cyChild) / 2;
			uint uFlags = 0x15;	// SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE;
			Win32.SetWindowPos(hChildWnd, IntPtr.Zero, x, y, 0, 0, uFlags);
		}

        private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
        {
            var className = Win32.GetClassName(hWnd);
            if (className == "Button")
            {
                int ctlId = Win32.GetDlgCtrlID(hWnd);
                switch (ctlId)
                {
                    case Win32.MBCancel:
                        Win32.SetWindowText(hWnd, _cancelText);
                        break;
                    case Win32.MBYes:
                        Win32.SetWindowText(hWnd, _yesText);
                        break;
                    case Win32.MBNo:
                        Win32.SetWindowText(hWnd, _noText);
                        break;
                }
            }
            return true;
        }
    }
}
