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
using AvtoSerive.BD;
using AvtoSerive.Pages;

using static AvtoSerive.MainWindow;

namespace AvtoSerive.Pages
{
    /// <summary>
    /// Логика взаимодействия для clientsEdit.xaml
    /// </summary>
    public partial class clientsEdit : Page
    {
        Client client;
        private int curSelPr;
        private int curTypAg = 0;
        private bool isEdit = true;
        public string fnd;
        public clientsEdit(Client clients)
        {
            InitializeComponent();
            
            GenderCode.SelectedIndex = 0;

            try
            {
                serviceList.ItemsSource = helper.GetContext().Service.ToList();
            }
            catch (Exception ex) {}

            if (clients != null)
            {
                isEdit = true;
                client = clients;
                this.FirstName.Text = clients.FirstName;
                this.LastName.Text = clients.LastName;
                this.Patronymic.Text = clients.Patronymic;
                this.Phone.Text = clients.Phone;
                this.Photo.Text = clients.PhotoPath;
                this.Email.Text = clients.Email;
                this.dateBirtday.Text = client.Birthday;
                if(clients.GenderCode == "ж")
                    GenderCode.SelectedIndex = 1;

                Load();
                btnWriteAg.Content = "Изменить";
            }
            else
            {
                isEdit = false;
                client = new Client();
                btnDelAg.IsEnabled = false;
                btnWritHi.IsEnabled = false;
                btnDelHi.IsEnabled = false;
            }
        }

        public void Load()
        {
            var tempValue = helper.GetContext().ClientService.Where(ClientService => ClientService.ClientID == client.ID).ToList();
            historyGrid.ItemsSource = tempValue;
            foreach (ClientService item in tempValue)
            {
                item.count = $"Всего файлов = {item.DocumentByService.Count.ToString()}";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // изменение/добавление клиента
        {
            if (FirstName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Имя не заполнено!");
                return;
            }
            if (LastName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Фамилия не заполнена!");
                return;
            }
            if (Photo.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Отсутствует путь к картинки!");
                return;
            }
            if (!(new Regex(@"^\+?\d{0,2}\-?\d{3}\-?\d{3}\-?\d{4}")).IsMatch(this.Phone.Text))
            {
                MessageBox.Show("Неверно введен номер телефона!");
                return;
            }
            if ((this.Email.Text != "") && (!(new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)")).IsMatch(this.Email.Text)))
            {
                MessageBox.Show("Неверно введена почта!");
                return;
            }
            if (dateBirtday.Text == string.Empty)
            {
                MessageBox.Show("Не вверно введена дата рождения!");
                return;
            }
            client.FirstName = this.FirstName.Text.Trim();
            client.LastName = this.LastName.Text.Trim();
            client.Patronymic = this.Patronymic.Text.Trim();
            client.Phone = this.Phone.Text.Trim();
            client.Email = this.Email.Text.Trim();
            client.Birthday = this.dateBirtday.Text.Trim();
            client.RegistrationDate = DateTime.Now.ToString();
            client.GenderCode = this.GenderCode.Text.Trim();
            client.PhotoPath = this.Photo.Text.Trim();
            try
            {
                if (isEdit)
                {
                    helper.GetContext().Entry(client).State = EntityState.Modified;
                    helper.GetContext().SaveChanges();
                    MessageBox.Show("Обновление информации об клиенте завершено");
                }
                else
                {
                    helper.ent.Client.Add(client);
                    helper.ent.SaveChanges();
                    MessageBox.Show("Добавление информации об клиенте завершено");
                }
            }
            catch { };
            btnDelAg.IsEnabled = true;
            btnWritHi.IsEnabled = true;
            btnDelHi.IsEnabled = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //добавление посещений
        {
            try
            {
                if (curSelPr > 0 && date.ToString() != string.Empty)
                {
                    ClientService cS = new ClientService();
                    cS.StartTime = Convert.ToDateTime(date.ToString());
                    cS.ClientID = client.ID;
                    cS.ServiceID = curSelPr;
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

        private void Button_Click_3(object sender, RoutedEventArgs e) // удаление посещений
        {
            for (int i = 0; i < historyGrid.SelectedItems.Count; i++)
            {
                ClientService prs = historyGrid.SelectedItems[i] as ClientService;
                if (prs != null)
                {
                    if (helper.GetContext().DocumentByService.Where(x=> x.ClientServiceID == prs.ID).Count() > 0)
                    {
                        MessageBox.Show("Удаление не возможно! Существуют документы.");
                    }
                    helper.GetContext().ClientService.Remove(prs);
                }
            }
            try
            {
                helper.GetContext().SaveChanges();
                historyGrid.ItemsSource = helper.GetContext().ClientService.Where(ClientService => ClientService.ClientID == client.ID).ToList();
            }
            catch { return; };

        }

        private void historyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //удаление клиента
        {
            if (client.ClientService.Count > 0)
            {
                MessageBox.Show("Удаление невозможно!");
                return;
            }
            foreach (ClientService cs in client.ClientService)
            {
                helper.GetContext().ClientService.Remove(cs);
            }
            helper.GetContext().Client.Remove(client);
            helper.GetContext().SaveChanges();
            MessageBox.Show("Удаление информации об агенте завешено!");
            this.NavigationService.GoBack();
        }

        private void mask_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = ((TextBox)sender).Text;
            try
            {
                serviceList.ItemsSource = helper.GetContext().Service.Where(Service => Service.Title.Contains(fnd)).ToList();
            }
            catch { };
        }


        private void serviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var sL = serviceList.SelectedItem as Service;
                if(sL != null)
                    curSelPr = sL.ID;
            }
            catch (Exception ex) { }
        }
    }
}
