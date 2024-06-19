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
    /// Логика взаимодействия для EditOrderPage.xaml
    /// </summary>
    public partial class EditOrderPage : Page
    {
        Order order;
        public EditOrderPage(Order currentorder)
        {
            InitializeComponent();
            order = currentorder;
            if (order.productList == null)
                order.productList = order.OrderProduct.Select(x => x.Product).ToList();
            cmbPickUpPoint.ItemsSource = RulEntities.GetContext().PickupPoint.ToList();
            cmbStatus.ItemsSource = RulEntities.GetContext().Status.ToList();
            DataContext = order;
            UpdateData();
        }

        private void UpdateData()
        {
            double costWithDiscount = 0;
            LViewProducts.ItemsSource = null;
            LViewProducts.ItemsSource = order.productList;
            foreach (Product product in order.productList)
                costWithDiscount += Convert.ToDouble(product.ProductCost) - (Convert.ToDouble(product.ProductCost) * Convert.ToDouble(product.ProductDiscountAmount / 100.00));
            txtTotal.Text = Math.Round(costWithDiscount, 2).ToString() + " рублей";
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.DataContext as Product;
            if (MessageBox.Show($"Вы точно хотите удалить {item.ProductName}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                order.productList.Remove(item);
                LViewProducts.ItemsSource = null;
                LViewProducts.ItemsSource = order.productList;
                txtTotal.Text = $" {Total} рублей";
            }
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите удалить заказ под номером {order.OrderID}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    var context = RulEntities.GetContext();
                    context.OrderProduct.RemoveRange(order.OrderProduct);
                    context.Order.Remove(order);
                    context.SaveChanges();
                    MessageBox.Show("Заказ удален!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (datePickerDeliveryDate.SelectedDate < order.OrderDate)
                errors.AppendLine("Некорректная дата доставки заказа!");

            if (cmbPickUpPoint.SelectedItem == null)
                errors.AppendLine("Выберите пункт выдачи!");

            if (cmbStatus.SelectedItem == null)
                errors.AppendLine("Выберите статус заказа!");
            else
                if (cmbStatus.SelectedIndex == 1 && datePickerDeliveryDate.SelectedDate > DateTime.Now)
                errors.AppendLine("Заказ не может быть завершен, т.к. ещё не наступил день доставки заказа!");

            if (order.productList.Count <= 0)
                errors.AppendLine("Выберите заказываемые продукты!");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                RulEntities.GetContext().OrderProduct.RemoveRange(order.OrderProduct);
                var productArticle = order.productList.Select(p => p.ProductArticleNumber).ToList();
                List<string> temp = new List<string>();
                foreach (var item in productArticle)
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
                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string Total
        {
            get
            {
                var total = order.productList.Sum(p => Convert.ToDouble(p.ProductCost) - Convert.ToDouble(p.ProductCost) * Convert.ToDouble(p.ProductDiscountAmount) / 100);
                return total.ToString();
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductOrderWindow addProductOrderWindow = new AddProductOrderWindow(order);
            bool? dialog = addProductOrderWindow.ShowDialog();
            if (dialog != null)
            {
                UpdateData();
            }
        }
    }
}
