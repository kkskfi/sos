using System;
using System.Collections.Generic;
using System.Linq;
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

namespace conference1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class helper
        {
            public static Entities ent1;
            public static Entities GetContext()
            {
                if (ent1 == null)
                {
                    ent1 = new Entities();
                }
                return ent1;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            frame.Content = new Events(frame);
        }

        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }
    }
}
