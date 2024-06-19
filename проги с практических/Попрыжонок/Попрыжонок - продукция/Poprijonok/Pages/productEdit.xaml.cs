using Poprijonok.BD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для productEdit.xaml
    /// </summary>
    public partial class productEdit : Page
    {
        Product Product;
        private int curSelPr;
        private int curTypPr = 0;
        private bool isEdit = true;
        public productEdit(Product product)
        {
            InitializeComponent();

            try
            {
                Type.ItemsSource = helper.GetContext().ProductType.ToList();
                agent.ItemsSource = helper.GetContext().Agent.ToList();
            }
            catch { };

            if (product != null)
            {
                isEdit = true;
                Product = product;
                Type.SelectedIndex = product.idProductType - 1;
                this.Title.Text = product.Title;
                this.ArticleNumber.Text = product.ArticleNumber;
                this.ProductionPersonCount.Text = product.ProductionPersonCount.ToString();
                this.ProductionWorkshopNumber.Text = product.ProductionWorkshopNumber.ToString();
                this.MinCostForAgent.Text = product.MinCostForAgent.ToString();

                agentGrid.ItemsSource = helper.GetContext().ProductSale.Where(ProductSale => ProductSale.ProductID == product.ID).ToList();
            }
            else
            {
                isEdit = false;
                Product = new Product();
                btnDelAg.IsEnabled = false;
                btnWritHi.IsEnabled = false;
                btnDelHi.IsEnabled = false;
            }
            this.DataContext = product;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) 
        {
            int cnt = 0;
            try
            {
                cnt = Convert.ToInt32(count.Text);
            }
            catch
            {
                MessageBox.Show("ТоЛьКо ЦиФрЫ! оТ 1 дО бЕсКоНеЧнОсТи. (*_*)");
                return;
            }
            string dt = date.ToString();
            if (curSelPr > 0 && dt != "" && cnt > 0)
            {
                ProductSale pr = new ProductSale();
                pr.AgentID = curSelPr;
                pr.ProductID = Product.ID;
                pr.SaleDate = (DateTime)date.SelectedDate;
                pr.ProductCount = cnt;
                try
                {
                    helper.GetContext().ProductSale.Add(pr);
                    helper.GetContext().SaveChanges();
                    agentGrid.ItemsSource = helper.GetContext().ProductSale.Where(ProductSale => ProductSale.ProductID == Product.ID).ToList();
                }
                catch
                {
                    return;
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < agentGrid.SelectedItems.Count; i++)
            {
                ProductSale prs = agentGrid.SelectedItems[i] as ProductSale;
                if (prs != null)
                {
                    helper.GetContext().ProductSale.Remove(prs);
                }
            }
            try
            {
                helper.GetContext().SaveChanges();
                agentGrid.ItemsSource = helper.GetContext().ProductSale.Where(ProductSale => ProductSale.ProductID == Product.ID).ToList();
            }
            catch { return; };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // удаление продукта
        {
            if (Product.ProductSale.Count > 0)
            {
                MessageBox.Show("Удаление невозможно!");
                return;
            }

            helper.GetContext().Product.Remove(Product);
            helper.GetContext().SaveChanges();
            MessageBox.Show("Удаление информации о продукте завершено!");
            this.NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (curTypPr == 0)
            {
                curTypPr = Type.SelectedIndex + 1;
            }
            if (this.Title.Text.Trim() == "")
            {
                MessageBox.Show("Пустое название!");
                return;
            }
            if (this.ArticleNumber.Text.Trim() == "")
            {
                MessageBox.Show("Пустое артикул!");
                return;
            }

            Product.Title = this.Title.Text;
            Product.idProductType = curTypPr;
            Product.ArticleNumber = this.ArticleNumber.Text;
            Product.ProductionPersonCount = int.Parse(this.ProductionPersonCount.Text);
            Product.ProductionWorkshopNumber = int.Parse(this.ProductionWorkshopNumber.Text);
            Product.MinCostForAgent = decimal.Parse(this.MinCostForAgent.Text.Replace(".", ","));


            if (isEdit)
            {
                helper.GetContext().Entry(Product).State = EntityState.Modified;
                helper.GetContext().SaveChanges();
                MessageBox.Show("Обновление информации о продукте завершено");
            }
            else
            {
                helper.ent.Product.Add(Product);
                helper.ent.SaveChanges();
                MessageBox.Show("Добавление информации о продукте завершено");
            }


            btnDelAg.IsEnabled = true;
            btnWritHi.IsEnabled = true;
            btnDelHi.IsEnabled = true;

        }

        private void count_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void mask_TextChanged(object sender, TextChangedEventArgs e)
        {
            string fnd = ((TextBox)sender).Text;
            try
            {
                agent.ItemsSource = helper.GetContext().Agent.Where(Agent => Agent.Title.Contains(fnd)).ToList();
            }
            catch { };
        }

        private void product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void agent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curSelPr = ((Agent)agent.SelectedItem).ID;
        }
    }
}
