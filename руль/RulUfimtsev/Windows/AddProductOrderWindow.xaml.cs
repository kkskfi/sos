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
using System.Windows.Shapes;

namespace RulUfimtsev.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddProductOrderWindow.xaml
    /// </summary>
    public partial class AddProductOrderWindow : Window
    {
        Order order;
        public AddProductOrderWindow(Order currentOrder)
        {
            InitializeComponent();
            var product = RulEntities.GetContext().Product.ToList();//Общаемся к таблице "Продукты"
            LViewProduct.ItemsSource = product; //Передаем таблицу в лист
            DataContext = this; //Привязываем контекст данных к коду, чтобы обратиться к массивам

            txtAllAmount.Text = product.Count().ToString();//Передаем количество всех записей из таблицы

            order = currentOrder;

            UpdateData();

        }

        public string[] SortingList { get; set; } =
       {
            "Без сортировки",
            "Стоимость по возврастанию",
            "Стоимость по убыванию"
        };
        public string[] FilterList { get; set; } =
        {
            "Все диапазоны",
            "0%-9,99%",
            "10%-14,99%",
            "15% и более"
        };

        private void UpdateData()
        {
            var result = RulEntities.GetContext().Product.ToList().Where(x => x.ProductQuantityInStock > 0); //Вводим переменную, которая принимает данные из таблицы товаров
            txtAllAmount.Text = result.Count().ToString();
            if (cmbSorting.SelectedIndex == 1)                           //Реализация сортировки
                result = result.OrderBy(p => p.ProductCost).ToList();   //С помощью запросов на сортировку по возрастанию
            if (cmbSorting.SelectedIndex == 2)                           //И убывание цены
                result = result.OrderByDescending(p => p.ProductCost).ToList();

            if (cmbFilter.SelectedIndex == 1)
                result = result.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
            if (cmbFilter.SelectedIndex == 2)                                                                            //Реализация фильтрации
                result = result.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();     //С помощью запросов на выборку
            if (cmbFilter.SelectedIndex == 3)                                                                           //По условию задания
                result = result.Where(p => p.ProductDiscountAmount >= 15).ToList();

            result = result.Where(p => p.ProductName.ToLower().Contains(txtSearch.Text.ToLower())).ToList();            //Реализация поиска
            LViewProduct.ItemsSource = result;

            txtResultAmount.Text = result.Count().ToString();//Передаем количество записей после применения поиска, сортировки, фильтрации
        }

        private void btnAddProductOrder_Click(object sender, RoutedEventArgs e)
        {
            if (order.productList.Contains(LViewProduct.SelectedItem as Product))
            {
                MessageBox.Show("Данный продукт уже добавлен!");
            }
            else
            {
                order.productList.Add(LViewProduct.SelectedItem as Product);
            }
        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
    }
}
