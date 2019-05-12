using Pizzaria1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KINOwpf
{
    class SingleWindow
    {
        public MainWindow MainWindow { get; set; }

        public void Launch(IPerson person, List<Film> allfilms)
        {
            MainWindow = MainWindow.getInstance(person, allfilms);

        }
    }
        
}
