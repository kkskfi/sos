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
    /// Логика взаимодействия для agentsEdit.xaml
    /// </summary>
    public partial class agentsEdit : Page
    {
        Agent agent;
        private int curSelPr;
        private int curTypAg = 0;
        private bool isEdit = true;
        public agentsEdit(Agent ag)
        {
            InitializeComponent();

            try
            {
                Type.ItemsSource = helper.GetContext().AgentType.ToList();
                product.ItemsSource = helper.GetContext().Product.ToList();
            }
            catch { };
            if (ag != null)
            {
                isEdit = true;
                agent = ag;
                Type.SelectedItem = ag.AgentType;
                this.Title.Text = ag.Title;
                this.Adress.Text = ag.Address;
                this.Inn.Text = ag.INN;
                this.Kpp.Text = ag.KPP;
                this.Director.Text = ag.DirectorName;
                this.Phone.Text = ag.Phone;
                this.Email.Text = ag.Email;
                this.Logo.Text = ag.Logo;
                this.Prioritet.Text = ag.Priority.ToString();
                historyGrid.ItemsSource = helper.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == ag.ID).ToList();
            }
            else
            {
                isEdit=false;
                agent = new Agent();
                btnDelAg.IsEnabled = false;
                btnWritHi.IsEnabled = false;
                btnDelHi.IsEnabled = false;
            }
            this.DataContext = agent;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (curTypAg == 0) {
                curTypAg = Type.SelectedIndex+1;
                if(!isEdit)
                    agent.ID = helper.GetContext().Agent.OrderByDescending(p => p.ID).Select(p => p.ID).FirstOrDefault();
        }
            if (this.Title.Text == "")
            {
                MessageBox.Show("Пустое название!");
                return; 
            }
            if (!(new Regex(@"\d{10}|\d{12}")).IsMatch(this.Inn.Text))
            {
                MessageBox.Show("Неверно введен ИНН");
                return;
            }
            if (!(new Regex(@"\d{4}[\dA-Z][\dA-Z]\d{3}")).IsMatch(this.Kpp.Text) && this.Kpp.Text.Length > 9)
            {
                MessageBox.Show("Неверно введен КПП");
                return;
            }
            if (!(new Regex(@"^\+?\d{0,2}\-?\d{3}\-?\d{3}\-?\d{4}")).IsMatch(this.Phone.Text))
            {
                MessageBox.Show("Неверно введен номер телефона");
                    return;
            }
            if ((this.Email.Text != "") && (!(new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)")).IsMatch(this.Email.Text)))
            {
                MessageBox.Show("Неверно введена почта");
                return;
            }
            agent.Title = this.Title.Text;
            agent.idAgentType = curTypAg;
            agent.Address = this.Adress.Text;
            agent.INN = this.Inn.Text;
            agent.KPP = this.Kpp.Text;
            agent.Phone = this.Phone.Text;
            agent.DirectorName = this.Director.Text;
            agent.Phone = this.Phone.Text;
            agent.Email = this.Email.Text;
            try
            {
                agent.Priority = Convert.ToInt32(this.Prioritet.Text);
            }
            catch
            {
                return;
            }

                if (isEdit)
                {
                    helper.GetContext().Entry(agent).State = EntityState.Modified;
                    helper.GetContext().SaveChanges();
                    MessageBox.Show("Обновление информации об агенте завершено");
                }
                else
                {
                    helper.ent.Agent.Add(agent);
                    helper.ent.SaveChanges();
                    MessageBox.Show("Добавление информации об агенте завершено");
                }
      

            btnDelAg.IsEnabled = true;
            btnWritHi.IsEnabled = true;
            btnDelHi.IsEnabled = true;

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (agent.ProductSale.Count > 0)
            {
                MessageBox.Show("Удаление невозможно!");
                return;
            }
            foreach (Shop shop in agent.Shop)
            {
                helper.GetContext().Shop.Remove(shop);
            }
            foreach (AgentPriorityHistory apr in agent.AgentPriorityHistory)
            {
                helper.GetContext().AgentPriorityHistory.Remove(apr);
            }
            helper.GetContext().Agent.Remove(agent);
            helper.GetContext().SaveChanges();
            MessageBox.Show("Удаление информации об агенте завешено!");
            this.NavigationService.GoBack();
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
                pr.AgentID = agent.ID;
                pr.ProductID = curSelPr;
                pr.SaleDate = (DateTime)date.SelectedDate;
                pr.ProductCount = cnt;
                try
                {
                    helper.GetContext().ProductSale.Add(pr);
                    helper.GetContext().SaveChanges();
                    historyGrid.ItemsSource = helper.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == agent.ID).ToList();
                }
                catch
                {
                    return;
                }
            }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < historyGrid.SelectedItems.Count; i++)
            {
                ProductSale prs = historyGrid.SelectedItems[i] as ProductSale;
                if (prs != null)
                {
                    helper.GetContext().ProductSale.Remove(prs);
                }
            }
            try
            {
                helper.GetContext().SaveChanges();
                historyGrid.ItemsSource = helper.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == agent.ID).ToList();
            }
            catch { return; };
        }


        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curSelPr = ((Product)product.SelectedItem).ID;
        }

        private void count_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void date_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void historyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mask_TextChanged(object sender, TextChangedEventArgs e)
        {
            string fnd = ((TextBox)sender).Text;
            try
            {
                product.ItemsSource = helper.GetContext().Product.Where(Product => Product.Title.Contains(fnd)).ToList();
            }
            catch { };

        }
    }
}


