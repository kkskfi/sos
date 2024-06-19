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
    /// Логика взаимодействия для OrderTicketPage.xaml
    /// </summary>
    public partial class OrderTicketPage : Page
    {
        List<Product> productList = new List<Product>();
        public OrderTicketPage(Order currentOrder, List<Product> products)
        {
            InitializeComponent();
            productList = products;
            DataContext= currentOrder;

            txtPickUpPoint.Text = currentOrder.PickupPoint.Address;
            
            var result = "";
            foreach(var pl in productList)
            {
                result += (result == "" ? "" : ",") + pl.ProductName.ToString();
            }

            LViewProductList.Text = MinimalizeText(result);

            var total = productList.Sum(p => Convert.ToDouble(p.ProductCost) - Convert.ToDouble(p.ProductCost) * Convert.ToDouble(p.ProductDiscountAmount / 100.00));
            txtCost.Text = total.ToString() + " рублей";

            var temp = Convert.ToDouble(productList.Sum(p => p.ProductCost));
            txtDiscountSum.Text = (Math.Round((1 - (total / temp)) * 100, 2)).ToString() + " %";
        }

        private string MinimalizeText(string text)
        {
            List<string> NewText = text.Split(',').ToList();
            List<string> Temp = new List<string>();
            List<string> Result = new List<string>();
            var result = "";
            foreach(var item in NewText)
            {
                if (!Temp.Contains(item))
                {
                    int count = NewText.Where(p => p == item).Count();
                    if (count > 1)
                    {
                        Result.Add(item.ToString() + $" x{count}");
                    }
                    else
                    {
                        Result.Add(item.ToString());
                    }
                    
                    Temp.Add(item);
                }
                
            }
            foreach (var word in Result)
            {
                result += (result == "" ? "" : ",\n") + word;
            }

            return result;
        }

        private void btnSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if(pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idp = flowDoc;
                pd.PrintDocument(idp.DocumentPaginator, Title);
            }
        }
    }
}
