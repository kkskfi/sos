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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Avtoservice.BD;
using static Avtoservice.MainWindow;

namespace Avtoservice.Pages
{
    /// <summary>
    /// Логика взаимодействия для servicesEdit.xaml
    /// </summary>
    public partial class servicesEdit : Page
    {
        Service service;
        private int curSelPr;
        private int curTypAg = 0;
        private bool isEdit = true;
        public string fnd;
        public servicesEdit(Service services)
        {
            InitializeComponent();
            try
            {
                clientList.ItemsSource = helper.GetContext().Client.ToList();
            }
            catch (Exception ex) { }


            if (services != null)
            {
                isEdit = true;
                service = services;
                this.Title.Text = service.Title;
                this.Cost.Text = service.Cost.ToString();
                this.DurationInSeconds.Text = service.DurationInSeconds.ToString();

                Load();
                btnWriteAg.Content = "Изменить";
            }
            else
            {
                isEdit = false;
                service = new Service();
                btnDelAg.IsEnabled = false;
                btnWritHi.IsEnabled = false;
                btnDelHi.IsEnabled = false;
            }
        }

        public void Load()
        {
            clientGrid.ItemsSource = helper.GetContext().ClientService.Where(ClientService => ClientService.ServiceID == service.ID).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Title.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Имя не заполнено!");
                return;
            }
            if (Cost.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Фамилия не заполнена!");
                return;
            }
            if (DurationInSeconds.Text.Trim() == string.Empty && int.Parse(DurationInSeconds.Text) > 0)
            {
                MessageBox.Show("Длительность не заполнена!");
                return;
            }
            service.Title = this.Title.Text;
            service.Cost = decimal.Parse(this.Cost.Text);
            service.DurationInSeconds = int.Parse(this.DurationInSeconds.Text);
            try
            {
                if (isEdit)
                {
                    helper.GetContext().Entry(service).State = EntityState.Modified;
                    helper.GetContext().SaveChanges();
                    MessageBox.Show("Обновление информации о услуге завершено");
                }
                else
                {
                    helper.ent.Service.Add(service);
                    helper.ent.SaveChanges();
                    MessageBox.Show("Добавление информации о услуге завершено");
                }
            }
            catch { };
            btnDelAg.IsEnabled = true;
            btnWritHi.IsEnabled = true;
            btnDelHi.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (service.ClientService.Count > 0)
            {
                MessageBox.Show("Удаление невозможно!");
                return;
            }
            helper.GetContext().Service.Remove(service);
            helper.GetContext().SaveChanges();
            MessageBox.Show("Удаление услуги завешено!");
            this.NavigationService.GoBack();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (curSelPr > 0 && date.ToString() != string.Empty)
                {
                    ClientService cS = new ClientService();
                    cS.StartTime = Convert.ToDateTime(date.ToString());
                    cS.ClientID = curSelPr;
                    cS.ServiceID = service.ID;
                    try
                    {
                        helper.GetContext().ClientService.Add(cS);
                        helper.GetContext().SaveChanges();
                        Load();
                    }
                    catch
                    {
                        return;
                    }

                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < clientGrid.SelectedItems.Count; i++)
            {
                ClientService prs = clientGrid.SelectedItems[i] as ClientService;
                if (prs != null)
                {
                    if (helper.GetContext().DocumentByService.Where(x => x.ClientServiceID == prs.ID).Count() > 0)
                    {
                        MessageBox.Show("Удаление не возможно! Существуют документы.");
                    }
                    helper.GetContext().ClientService.Remove(prs);
                }
            }
            try
            {
                helper.GetContext().SaveChanges();
                Load();
            }
            catch { return; };
        }

        private void clientGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mask_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = ((TextBox)sender).Text;
            try
            {
                clientList.ItemsSource = helper.GetContext().Client.Where(Client => Client.LastName.Contains(fnd)).ToList();
            }
            catch { };
        }

        private void clientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var sL = clientList.SelectedItem as Client;
                if (sL != null)
                    curSelPr = sL.ID;
            }
            catch (Exception ex) { }
        }
    }
}
