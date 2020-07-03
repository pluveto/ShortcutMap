using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Pluvet.ShortcutMap.Helper
{
    public class WindowHelper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public static string GetActiveModuleName()
        {
            IntPtr hWnd = GetForegroundWindow();
            uint procId = 0;
            if (hWnd == IntPtr.Zero) return null;
            GetWindowThreadProcessId(hWnd, out procId);
            var proc = Process.GetProcessById((int)procId);
            if (null == proc) return null;
            return (proc.MainModule.ModuleName);
        }
    }
}
