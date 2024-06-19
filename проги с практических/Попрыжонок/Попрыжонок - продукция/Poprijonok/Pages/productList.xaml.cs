using Poprijonok.BD;
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

namespace Poprijonok.Pages
{
    /// <summary>
    /// Логика взаимодействия для productList.xaml
    /// </summary>
    public partial class productList : Page
    {
        Frame Frame;
        private int start = 0;
        private int fullCount = 0;
        private int order = 0;
        private int iag = 10;
        private int filterinfId = 0;
        private string fnd = string.Empty;
        public productList(Frame frame)
        {
            Frame = frame;
            InitializeComponent();

            List<ProductType> productsTypes = new List<ProductType> { };
            productsTypes = helper.GetContext().ProductType.ToList();
            productsTypes.Add(new ProductType { Title = "Все типы" });
            Type.ItemsSource = productsTypes.OrderBy(ProductType => ProductType.ID);

            Load();
        }

        public void Load()
        {
            try
            {
                List<Product> products = new List<Product>();
                var pr = helper.GetContext().Product.Where(Product => Product.Title.Contains(fnd) || Product.ArticleNumber.Contains(fnd));
                if (filterinfId > 0) pr = helper.GetContext().Product.Where(Product => (Product.Title.Contains(fnd) || Product.ArticleNumber.Contains(fnd)) && (Product.idProductType == filterinfId));
                products.Clear();
                foreach (Product product in pr)
                {
                    products.Add(product);
                }

             

                if (order == 0) productGrid.ItemsSource = pr.OrderBy(Product => Product.ID).Skip(start * iag).Take(iag).ToList();
                if (order == 1) productGrid.ItemsSource = pr.OrderBy(Product => Product.Title).Skip(start * iag).Take(iag).ToList();
                if (order == 2) productGrid.ItemsSource = pr.OrderByDescending(Product => Product.Title).Skip(start * iag).Take(iag).ToList();
                if (order == 3) productGrid.ItemsSource = pr.OrderBy(Product => Product.ArticleNumber).Skip(start * iag).Take(iag).ToList();
                if (order == 4) productGrid.ItemsSource = pr.OrderByDescending(Product => Product.ArticleNumber).Skip(start * iag).Take(iag).ToList();


                fullCount = pr.Count();

                int ost = fullCount % iag;
                int pag = (fullCount - ost) / iag;
                if (ost > 0) pag++;
                pagin.Children.Clear();
                for (int i = 0; i < pag; i++)
                {
                    Button myButton = new Button();
                    myButton.Height = 30;
                    myButton.Content = i + 1;
                    myButton.Width = 20;
                    myButton.HorizontalAlignment = HorizontalAlignment.Center;
                    myButton.Tag = i;
                    myButton.Click += new RoutedEventHandler(paginButto_Click);
                    pagin.Children.Add(myButton);
                }

                full.Text = fullCount.ToString();
                turnButton();
            }
            catch
            {
                return;
            };


        }

        private void turnButton()
        {
            if (start == 0) { back.IsEnabled = false; }
            else { back.IsEnabled = true; };
            if ((start) * iag + iag >= fullCount) { forward.IsEnabled = false; }
            else { forward.IsEnabled = true; };
        }


        private void paginButto_Click(object sender, RoutedEventArgs e)
        {
            start = Convert.ToInt32(((Button)sender).Tag.ToString());
            Load();

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = new productEdit(null);
        }

        private void updateButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            start--;
            Load();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            start++;
            Load();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            order = Convert.ToInt32(selectedItem.Tag.ToString());
            Load();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = ((TextBox)sender).Text;
            Load();
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterinfId = ((ProductType)Type.SelectedItem).ID;
            Load();
        }

        private void productGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (productGrid.SelectedItems.Count > 0)
            {
                Product product = productGrid.SelectedItems[0] as Product;

                if (product != null)
                {
                    Frame.Content = new productEdit(product);
                }
            }
        }

        private void cmb_count_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iag = cmb_count.SelectedIndex == 0 ? 10 : cmb_count.SelectedIndex == 1 ? 50 : 200;
            Load();
        }
    }
}
