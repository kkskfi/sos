using Poprijonok.BD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для agentsList.xaml
    /// </summary>
    public partial class agentsList : Page
    {

        Frame Frame;
        private int start = 0;
        private int fullCount = 0;
        private int order = 0;
        private int filterinfId = 0;
        private int iag = 10;
        private string fnd = string.Empty;
        public agentsList(Frame frame)
        {
            Frame = frame;
            InitializeComponent();
            List<AgentType> agents = new List<AgentType> { };
            agents = helper.GetContext().AgentType.ToList();
            agents.Add(new AgentType { Title = "Все типы" });
            Type.ItemsSource = agents.OrderBy(AgentType => AgentType.ID);

            Load();
        }


        public void Load()
        {
            try
            {
                List<Agent> agents = new List<Agent>();
                var ag = helper.GetContext().Agent.Where(Agent => Agent.Title.Contains(fnd) || Agent.Phone.Contains(fnd) || Agent.Email.Contains(fnd));
                agents.Clear();
                foreach (Agent agent in ag)
                {
                    if (agent.Logo == "")
                    {
                        agent.Logo = "/agents/picture.png";
                    }
                    double sum = 0;
                    double fsum = 0;
                    foreach (ProductSale ps in agent.ProductSale)
                    {
                        List<ProductMaterial> mtr = new List<ProductMaterial> { };
                        mtr = helper.GetContext().ProductMaterial.Where(ProductMaterial => ProductMaterial.ProductID == ps.ProductID).ToList();
                        foreach (ProductMaterial mt in mtr)
                        {
                            double f = decimal.ToDouble(mt.Material.Cost);
                            fsum += f * (double)mt.Count;
                        }
                        fsum = fsum * ps.ProductCount;
                        if (ps.SaleDate.AddDays(365).CompareTo(DateTime.Today) > 0)
                            sum += ps.ProductCount;
                    }
                    agent.sale = sum;
                    agent.fsale = fsum;
                    agent.percent = 0;
                    if (fsum >= 10000 && fsum < 50000) 
                        agent.percent = 5;
                    if (fsum >= 50000 && fsum < 150000)
                        agent.percent = 10;
                    if (fsum >= 150000 && fsum < 500000)
                        agent.percent = 20;
                    if (fsum >= 500000) 
                        agent.percent = 25;
                    agents.Add(agent);
                }
                if (order == 0) 
                {
                    if(filterinfId == 0)
                    {
                        agentGrid.ItemsSource = ag.OrderBy(Agent => Agent.ID).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderBy(Agent => Agent.ID).Count();
                    }
                    else
                    {
                        agentGrid.ItemsSource = ag.Where(x => x.idAgentType == filterinfId).OrderBy(Agent => Agent.ID).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.idAgentType == filterinfId).OrderBy(Agent => Agent.ID).Count();
                    }
                }
                if (order == 1)
                {
                    if (filterinfId == 0)
                    {
                        agentGrid.ItemsSource = ag.OrderBy(Agent => Agent.Title).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderBy(Agent => Agent.Title).Count();
                    }
                    else
                    {
                        agentGrid.ItemsSource = ag.Where(x => x.idAgentType == filterinfId).OrderBy(Agent => Agent.Title).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.idAgentType == filterinfId).OrderBy(Agent => Agent.Title).Count();
                    }
                }
                if(order == 2) { 
                    if (filterinfId == 0)
                    {
                        agentGrid.ItemsSource = ag.OrderByDescending(Agent => Agent.Title).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderByDescending(Agent => Agent.Title).Count();
                    }
                    else
                    {
                        agentGrid.ItemsSource = ag.Where(x => x.idAgentType == filterinfId).OrderByDescending(Agent => Agent.Title).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.idAgentType == filterinfId).OrderByDescending(Agent => Agent.Title).Count();
                    }
                }
                if(order == 3)
                {
                    if (filterinfId == 0)
                    {
                        agentGrid.ItemsSource = ag.OrderBy(Agent => Agent.Priority).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderBy(Agent => Agent.Priority).Count();
                    }
                    else
                    {
                        agentGrid.ItemsSource = ag.Where(x => x.idAgentType == filterinfId).OrderBy(Agent => Agent.Priority).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.idAgentType == filterinfId).OrderBy(Agent => Agent.Priority).Count();
                    }
                }
                if(order == 4)
                {
                    if (filterinfId == 0)
                    {
                        agentGrid.ItemsSource = ag.OrderByDescending(Agent => Agent.Priority).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderByDescending(Agent => Agent.Priority).Count();
                    }
                    else
                    {
                        agentGrid.ItemsSource = ag.Where(x => x.idAgentType == filterinfId).OrderByDescending(Agent => Agent.Priority).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.idAgentType == filterinfId).OrderByDescending(Agent => Agent.Priority).Count();
                    }
                }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            start--;
            Load();
        }
        private void updateButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (agentGrid.SelectedItems.Count > 0)
            {
                int prt = 0;
                foreach (Agent agent in agentGrid.SelectedItems)
                {
                    if (agent.Priority > prt) prt = agent.Priority;
                }
                Window1 dlg = new Window1(prt);
                helper.prioritet = prt;
                helper.flag = false;
                dlg.ShowDialog();
                if (helper.flag)
                {
                    foreach (Agent agent in agentGrid.SelectedItems)
                    {
                        agent.Priority = helper.prioritet;
                        helper.GetContext().Entry(agent).State = EntityState.Modified;
                    }
                    helper.GetContext().SaveChanges();
                    Load();
                }
            }


        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = new agentsEdit(null);
        }

        private void revButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            start++;
            Load();
        }

        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Agent agent = (Agent)e.Row.DataContext;
            SolidColorBrush hb;
            if (agent.percent == 25)
            {
                hb = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                hb = new SolidColorBrush(Colors.White);
            };
            e.Row.Background = hb;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            order = Convert.ToInt32(selectedItem.Tag.ToString());
            Load();

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterinfId = ((ComboBox)sender).SelectedIndex; 
            Load();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = ((TextBox)sender).Text;
            Load();

        }

        private void agentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (agentGrid.SelectedItems.Count > 0)
            {
                Agent agent = agentGrid.SelectedItems[0] as Agent;

                if (agent != null)
                {
                    Frame.Content = new agentsEdit(agent);
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
