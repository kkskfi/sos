using RulUfimtsev.Entities;
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

namespace RulUfimtsev.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderListPage.xaml
    /// </summary>
    public partial class OrderListPage : Page
    {
       
        public OrderListPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateData();
            


        }

        

        private void UpdateData()
        {
            var result = RulEntities.GetContext().Order.ToList();               
            LViewOrder.ItemsSource = result;   
        }

     

        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditOrderPage(LViewOrder.SelectedItem as Order));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateData();
        }
    }
}
