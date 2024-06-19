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

using RulUfimtsev.Pages;
using RulUfimtsev.Entities;

namespace RulUfimtsev.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        List<Product> productList = new List<Product>();
        public OrderPage(List<Product> products, User user)
        {
            InitializeComponent();

            DataContext = this;
            productList = products;
            lViewOrder.ItemsSource = productList;

            cmbPickUpPoint.ItemsSource = RulEntities.GetContext().PickupPoint.ToList();

            if(user != null)
            {
                txtUser.Text = user.UserSurname.ToString() + " " + user.UserName.ToString() + " " + user.UserPatronymic?.ToString(); 
            }
            else
            {
                txtUser.Visibility= Visibility.Collapsed;
                txtBUser.Visibility= Visibility.Collapsed;
            }

            
        }
        public string Total
        {
            get
            {
                var total = productList.Sum(p => Convert.ToDouble(p.ProductCost) - Convert.ToDouble(p.ProductCost) * Convert.ToDouble(p.ProductDiscountAmount / 100.00));
                return total.ToString();
            }
        }

        private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.DataContext as Product;

            if (MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                productList.Remove(item);
                lViewOrder.ItemsSource = null;
                lViewOrder.ItemsSource = productList;
                txtTotal.Text = $"{Total} рублей";
            }
        }

        private void btnOrderSave_Click(object sender, RoutedEventArgs e)
        {
            var productArticle = productList.Select(p => p.ProductArticleNumber).ToList();

            Random random = new Random();
            var date = DateTime.Now;
            if (productList.Any(p => p.ProductQuantityInStock < 3))
            {
                date = date.AddDays(6);
            }
            else
            {
                date = date.AddDays(3);
            }
            if (productList.Count == 0)
            {
                MessageBox.Show("Заказ пустой!");
                
                return;
            }
            if (cmbPickUpPoint.SelectedItem == null)
            {
                MessageBox.Show("Выберите пункт выдачи!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Order order = new Order();

                order.OrderStatus = 1;
                order.OrderDate = DateTime.Now;
                order.OrderDeliveryDate = date;
                
                int ReceiptCode = random.Next(100, 1000);
                while(RulEntities.GetContext().Order.Where(p=>p.ReceiptCode == ReceiptCode).Any())
                {
                    ReceiptCode = random.Next(100, 1000);
                }

                order.ReceiptCode = ReceiptCode;
                order.CurrentFullName = txtUser.Text;
                order.OrderPickupPoint = cmbPickUpPoint.SelectedIndex + 1;

                RulEntities.GetContext().Order.Add(order);

                List<string> temp = new List<string>();

                foreach(var item in productArticle)
                {
                    if (!temp.Contains(item))
                    {
                        int count = productArticle.Where(p => p == item).Count();
                        OrderProduct newOrderProduct = new OrderProduct()
                        {
                            OrderID = order.OrderID,
                            ProductArticleNumber = item,
                            CountProduct = count
                        };
                        temp.Add(item);
                        RulEntities.GetContext().OrderProduct.Add(newOrderProduct);
                    }
                }

                RulEntities.GetContext().SaveChanges();
                MessageBox.Show("Заказ оформлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new OrderTicketPage(order, productList));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }
    }
}
