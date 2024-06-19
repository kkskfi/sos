using Microsoft.Win32;
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

    public partial class AddEditProductPage : Page
    {
        Product product = new Product(); 
        public AddEditProductPage(Product currentProduct)
        {
            InitializeComponent();

            if(currentProduct != null)
            {
                product = currentProduct;

                btnDeleteProduct.Visibility = Visibility.Visible;
                txtArticle.IsEnabled = false;
            }

            DataContext = product;
            cmbCategory.ItemsSource = CategoryList;
            cmbMaker.ItemsSource = MakerList;
        }
        
        private string[] CategoryList =
        {
            "Аксессуары",
            "Автозапчасти",
            "Автосервис",
            "Съемники подшипников",
            "Ручные инструменты",
            "Зарядные устройства",

        };

        private string[] MakerList =
        {
            "KOLNER",
            "AIRLINE",
            "BIG FIGHTER",
            "STV",
            "JONNESWAY",
            "BOSCH",
            "TCL",
            "JTC",
            "GRASS",
            "SMART",
            "CHAMPION",
            "ALCA",
            "MOBIL",
            "EXPERT",
            "HAMMER"
        };

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog();

            GetImageDialog.Filter = "Файлы изображений: (*.png, *.jpg, *.jpeg)| *.png, *.jpg, *.jpeg";
            GetImageDialog.InitialDirectory = "D:\\VSProjects\\RulUfimtsev\\RulUfimtsev\\Resources";

            if(GetImageDialog.ShowDialog() == true)
            {
                product.ProductImage= GetImageDialog.SafeFileName;
            }
        }

        

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show($"Вы хотите удалить {product.ProductName}?", "Внимание",MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    RulEntities.GetContext().Product.Remove(product);
                    RulEntities.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Товар не может быть удален, так как он находится в заказе!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private StringBuilder checkProduct(Product product)
        {
            StringBuilder errors = new StringBuilder();
            
            if(String.IsNullOrEmpty(product.ProductArticleNumber))
            {
                errors.AppendLine("Артикул не может быть пустой стрококй!");
            }

            if (String.IsNullOrEmpty(product.ProductName))
            {
                errors.AppendLine("Название не может быть пустой стрококй!");
            }

            if (String.IsNullOrEmpty(product.ProductDescription))
            {
                errors.AppendLine("Описание не может быть пустой стрококй!");
            }

            if (product.ProductCategory <=0)
            {
                errors.AppendLine("Выберите категорию товара!");
            }

            if (product.ProductCost <= 0)
            {
                errors.AppendLine("Стоимость не может быть меньше или равно нулю!");
            }

            if (product.MinCount < 0)
            {
                errors.AppendLine("Минимальное количество не может быть отрицательным!");
            }

            if (product.ProductDiscountAmount > product.MaxDiscountAmount)
            {
                errors.AppendLine("Действующая скидка на товар не может быть больше максимальной!");
            }

            return errors;
        }




        private Product productFromfields(Product currentProduct)
        {
            return new Product()
            {
                ProductArticleNumber = txtArticle.Text,
                ProductName = txtTilte.Text,
                ProductDescription = txtDescription.Text,
                ProductCategory = RulEntities.GetContext().Category.Where(p => p.NameCategory == cmbCategory.Text).First().ID,
                ProductImage = currentProduct?.ProductImage,
                ProductManufacturer = RulEntities.GetContext().Maker.Where(p => p.NameMaker == cmbMaker.Text).First().ID,
                ProductCost = Convert.ToInt32(txtCost.Text),
                ProductDiscountAmount = (byte?)Convert.ToInt32(txtDiscount.Text),
                ProductQuantityInStock = Convert.ToInt32(txtCountInStock.Text),
                ProductStatus = 1.ToString(),
                Unit = txtUnit.Text,
                MaxDiscountAmount = (byte)Convert.ToInt32(txtMaxDiscount.Text),
                Supplier = RulEntities.GetContext().Provider.Where(p => p.NameProvider == txtSupplier.Text).First().ID,
                CountInPack = Convert.ToInt32(txtCountInpack.Text),
                MinCount = Convert.ToInt32(txtMinCount.Text)
            };
            
        }


        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {

            if (product.ProductArticleNumber == "")
            {
                product = productFromfields(product);
            }

            var errors = checkProduct(product);

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if(product.ProductArticleNumber == null)
            {
                RulEntities.GetContext().Product.Add(product);
            }

            try
            {
                RulEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }

            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
