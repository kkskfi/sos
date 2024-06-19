using RulUfimtsev.Entities;
using RulUfimtsev.Windows;
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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        User user = new User();
        public Admin(User currentUser)
        {
            InitializeComponent();
            var product = RulEntities.GetContext().Product.ToList();
            LViewProduct.ItemsSource = product;
            DataContext = this;

            txtAllAmount.Text = product.Count.ToString();

            user = currentUser;
            User();
        }

        private void User()
        {
            if (user != null)
            {
                txtFullname.Text = user.UserSurname.ToString() + " " + user.UserName.ToString() + " " + user.UserPatronymic.ToString();
            }

            else
            {
                txtFullname.Text = "Гость";
            }
        }

        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию"
        };

        public string[] FilterList { get; set; } =
        {
            "Все диапозоны",
            "0%-9,99%",
            "10%-14,99%",
            "15% и более"
        };

        private void UpdateData()
        {
            var result = RulEntities.GetContext().Product.ToList();

            switch (cmbSorting.SelectedIndex)
            {
                case 1:
                    result = result.OrderBy(p => p.ProductCost).ToList();
                    break;
                case 2:
                    result = result.OrderByDescending(p => p.ProductCost).ToList();
                    break;
            }

            switch (cmbFilter.SelectedIndex)
            {
                case 1:
                    result = result.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
                    break;
                case 2:
                    result = result.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
                    break;
                case 3:
                    result = result.Where(p => p.ProductDiscountAmount >= 15).ToList();
                    break;
            }
            result = result.Where(p => p.ProductName.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            LViewProduct.ItemsSource = result;

            txtResultAmount.Text = result.Count.ToString();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        List<Product> orderProducts = new List<Product>();

        

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderListPage());
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(LViewProduct.SelectedItem as Product));
        }

        private void btnAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null));

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                RulEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LViewProduct.ItemsSource = RulEntities.GetContext().Product.ToList();
                UpdateData();
            }
        }

       
    }
}
