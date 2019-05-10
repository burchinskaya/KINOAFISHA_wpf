using Microsoft.Win32;
using Pizzaria1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KINOwpf
{
    /// <summary>
    /// Логика взаимодействия для Seancess.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        [DllImport("USER32.DLL")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("USER32.dll")]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        public GameControl()
        {
            InitializeComponent();
        }

        private void Flash_Quran_Load(object sender, EventArgs e)
        {
            Process process = Process.Start("QuranFull.exe");
            process.WaitForInputIdle();
            SetParent(process.MainWindowHandle, butt.Handle);
            MoveWindow(process.MainWindowHandle, 0, 0, (int)this.Width - 90, (int)this.Height, true);
        }
    }
}
