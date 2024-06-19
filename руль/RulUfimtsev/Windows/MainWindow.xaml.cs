using RulUfimtsev.Pages;
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

namespace RulUfimtsev
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Autho autho = new Autho();
        public MainWindow()
        {
            InitializeComponent();
            
            FrmMain.Navigate(autho);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            autho.clearFields();
            FrmMain.GoBack();
        }

        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Hidden;
            }
        }
    }
}
