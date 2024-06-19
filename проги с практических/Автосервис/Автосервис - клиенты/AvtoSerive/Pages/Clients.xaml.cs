using AvtoSerive.BD;
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
using static AvtoSerive.MainWindow;

namespace AvtoSerive.Pages
{
    /// <summary>
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {
        Frame Frame;
        private int start = 0;
        private int fullCount = 0;
        private int order = 0;
        private int filterinfId = 0;
        private int iag = 10;
        private string fnd = string.Empty;
        public Clients(Frame frame)
        {
            Frame = frame;
            InitializeComponent();

            Load();
        }

        public void Load()
        {
            try
            {
                var temp = new List<Client>();
                List<Client> client = new List<Client>();
                var ag = helper.GetContext().Client.Where(Client => Client.LastName.Contains(fnd) || Client.FirstName.Contains(fnd) || Client.Patronymic.Contains(fnd) || Client.Phone.Contains(fnd) || Client.Email.Contains(fnd));
                client.Clear();
                foreach (Client clients in ag)
                {
                    clients.Name = clients.LastName + " " + clients.FirstName + " " + clients.Patronymic;
                    if (clients.PhotoPath == "")
                    {
                        clients.PhotoPath = "/Клиенты/1.jpg";
                        

                    }
                    else
                    {
                        clients.PhotoPath = @"\"+ clients.PhotoPath.Remove(0,1);
                    }


                }   

                if(order == 0)
                {
                    if(filterinfId == 0)
                    {
                        temp = ag.OrderBy(Client => Client.ID).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderBy(Client => Client.ID).Count();
                    }
                    else
                    {
                        temp = ag.Where(x => x.GenderCode == (filterinfId == 1 ? "м" : "ж")).OrderBy(Client => Client.ID).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.GenderCode == (filterinfId == 1 ? "м" : "ж")).OrderBy(Client => Client.ID).Count();
                    }
                    clientsGrid.ItemsSource = temp;
                }
                if(order == 1)
                {
                    if (filterinfId == 0)
                    {
                        temp = ag.OrderBy(Client => Client.LastName).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderBy(Client => Client.LastName).Count();
                    }
                    else
                    {
                        temp = ag.Where(x => x.GenderCode == (filterinfId == 1 ? "м" : "ж")).OrderBy(Client => Client.LastName).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.GenderCode == (filterinfId == 1 ? "м" : "ж")).OrderBy(Client => Client.LastName).Count();
                    }
                    clientsGrid.ItemsSource = temp;
                }
                if(order == 2)
                {
                    if(filterinfId == 0)
                    {
                        temp = ag.OrderByDescending(Client => Client.LastName).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.OrderByDescending(Client => Client.LastName).Count();
                    }
                    else
                    {
                        temp = ag.Where(x => x.GenderCode == (filterinfId == 1 ? "м" : "ж")).OrderByDescending(Client => Client.LastName).Skip(start * iag).Take(iag).ToList();
                        fullCount = ag.Where(x => x.GenderCode == (filterinfId == 1 ? "м" : "ж")).OrderByDescending(Client => Client.LastName).Count();
                    }
                    clientsGrid.ItemsSource = temp;
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

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            start++;
            Load();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = ((TextBox)sender).Text;
            Load();
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

        }

        private void agentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (clientsGrid.SelectedItems.Count > 0)
            {
                Client client = clientsGrid.SelectedItems[0] as Client;

                if (client != null)
                {
                    Frame.Content = new clientsEdit(client);
                }
            }
        }

        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void updateButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (clientsGrid.SelectedItems.Count > 0)
            {
                Client client = clientsGrid.SelectedItems[0] as Client;

                if (client != null)
                {
                    Tegochnaya tegochnaya = new Tegochnaya(client);
                    tegochnaya.ShowDialog();

                }
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = new clientsEdit(null);
        }

        private void cmb_count_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iag = cmb_count.SelectedIndex == 0 ? 10 : cmb_count.SelectedIndex == 1 ? 50 : 200;
            Load();
        }

        private void cb_filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            filterinfId = int.Parse(selectedItem.Tag.ToString());
            Load();
        }
    }
}
