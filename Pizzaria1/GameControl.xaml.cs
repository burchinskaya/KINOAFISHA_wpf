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
        public GameControl()
        {
            InitializeComponent();
        }

        private void Flash_Quran_Load(object sender, EventArgs e)
        {
            /*Process process = Process.Start("QuranFull.exe");
            process.WaitForInputIdle();
            SбetParent(process.MainWindowHandle);
            MoveWindow(process.MainWindowHandle, 0, 0, (int)this.Width - 90, (int)this.Height, true);*/
        }
    }
}
