using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace KINOwpf
{
    /// <summary>
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        [DllImport("USER32.DLL")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("USER32.dll")]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
        public Game()
        {
            InitializeComponent();
        }

        private void Flash_Quran_Load(object sender, EventArgs e)
        {
            Process process = Process.Start("QuranFull.exe");
            process.WaitForInputIdle();
            p.WaitForInputIdle();
            SetParent(process.MainWindowHandle, this.mainGrid.Handle);
            MoveWindow(process.MainWindowHandle, 0, 0, (int)this.Width - 90, (int)this.Height, true);
        }

    }
}
