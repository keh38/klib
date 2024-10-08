﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KLib.Controls
{
	public class Win32
	{
		#region Values & structs

		public const int WH_CBT = 5;
		public const int HCBT_ACTIVATE = 5;

        public const int MF_BYPOSITION = 0x400;

        public const int WH_CALLWNDPROCRET = 12;
        public const int WM_DESTROY = 0x0002;
        public const int WM_INITDIALOG = 0x0110;
        public const int WM_TIMER = 0x0113;
        public const int WM_USER = 0x400;
        public const int DM_GETDEFID = WM_USER + 0;

        public const int MBOK = 1;
        public const int MBCancel = 2;
        public const int MBAbort = 3;
        public const int MBRetry = 4;
        public const int MBIgnore = 5;
        public const int MBYes = 6;
        public const int MBNo = 7;


        [StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}


        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };

        public delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);
        #endregion Values & structs


        #region Stock P/Invokes

        // Arg for SetWindowsHookEx()
        public delegate int WindowsHookProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, WindowsHookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetDlgCtrlID(IntPtr hwndCtl);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(IntPtr hWnd);


		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern uint GetDlgItemText(IntPtr hDlg, int nIDDlgItem, [Out] StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr handle, ref RECT r);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("User32")]
        public static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        public static extern int GetMenuItemCount(IntPtr hWnd);

        #endregion Stock P/Invokes

        #region Simplified interfaces

        public static string GetClassName(IntPtr hWnd)
		{
			StringBuilder ClassName = new StringBuilder(100);
			//Get the window class name
			int nRet = GetClassName(hWnd, ClassName, ClassName.Capacity);
			return ClassName.ToString();
		}

		public static string GetWindowText(IntPtr hWnd)
		{
			// Allocate correct string length first
			int length = GetWindowTextLength(hWnd);
			StringBuilder sb = new StringBuilder(length + 1);
			GetWindowText(hWnd, sb, sb.Capacity);
			return sb.ToString();
		}

		public static string GetDlgItemText(IntPtr hDlg, int nIDDlgItem)
		{
			IntPtr hItem = GetDlgItem(hDlg, nIDDlgItem);
			if (hItem == IntPtr.Zero)
				return null;
			int length = GetWindowTextLength(hItem);
			StringBuilder sb = new StringBuilder(length + 1);
			GetWindowText(hItem, sb, sb.Capacity);
			return sb.ToString();
		}

        // See: https://midnightprogrammer.net/post/disable-x-close-button-on-your-windows-form-application/
        public static void DisableXButton(IntPtr hWnd)
        {
            IntPtr hMenu = Win32.GetSystemMenu(hWnd, false);
            int menuItemCount = Win32.GetMenuItemCount(hMenu);
            Win32.RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }

        #endregion Simplified interfaces


    }
}
