using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Calculater
{
    public static class WinAPI
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowExA(IntPtr hWndParent, IntPtr hWndChildAfter, string lpClassName,
            string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, Delegate lpEnumFunc, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowInfo(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
    }

    public class Calculater
    {
        private static string _appName = "Calculator";

        private delegate IntPtr Callback(IntPtr hWnd, IntPtr lParam);

        public static void Test()
        {
            // var mainForm = WinAPI.FindWindow("Windows.UI.Core.CoreWindow", _appName);
            var mainForm = WinAPI.FindWindow(null, _appName);
            if (mainForm == null)
            {
                throw new Exception($"{_appName} is not running.");
            }

            EnumChildClasses(mainForm);
        }

        private static void EnumChildClasses(IntPtr parent)
        {
            if (parent == IntPtr.Zero) return;

            WinAPI.EnumChildWindows(
                parent,
                new Callback((hWnd, lParam) =>
                {
                    Console.WriteLine(GetClassName(hWnd));
                    EnumChildClasses(hWnd);
                    return new IntPtr(1);
                }),
                IntPtr.Zero
            );
        }

        private static string GetClassName(IntPtr hWnd)
        {
            var childClassName = new StringBuilder(256);
            WinAPI.GetClassName(hWnd, childClassName, 256);
            return childClassName.ToString();
        }
    }
}
