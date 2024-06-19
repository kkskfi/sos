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
using static conference1.MainWindow;

namespace conference1
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        Organizers organizers;
        private int curCountry;
        private int curGender;
        public Profile(Organizers or)
        {
            InitializeComponent();
            try
            {
                Country.ItemsSource = helper.GetContext().Countries.ToList();
                Gender.ItemsSource = helper.GetContext().Genders.ToList();
            }
            catch { };
            if (or != null)
            {
                organizers = or;
                Country.SelectedItem = or.Countries;
                Gender.SelectedItem = or.Genders;
                this.Name.Text = $"{or.LastName} {or.FirstName} {or.Patronymic}";
                this.Birthday.Text = or.Birthday.ToString();
                this.Phone.Text = or.Phone;
                this.Email.Text = or.Email;
            }
            else
            {

            }
            this.DataContext = organizers;

        }

        private void Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            curGender = ((Genders)Gender.SelectedItem).id;
        }

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string fnd = ((ComboBox)sender).Text;
            //try
            //{
            //    Country.ItemsSource = helper.GetContext().Countries.Where(Countries => Countries.Title.Contains(fnd)).ToList();
            //}
            //catch { };
            curCountry = ((Countries)Country.SelectedItem).Code;
        }

        private void ok_Button_Click(object sender, RoutedEventArgs e)
        {
            if (curCountry == 0) return;
            if (curGender == 0) return;
            string fullName = Name.Text;
            string[] nameComponents = fullName.Split(' ');
            if (nameComponents.Length == 3)
            {
                string lastName = nameComponents[0];
                string firstName = nameComponents[1];
                string patronymic = nameComponents[2];

                // Присвоение значения атрибутам таблицы с использованием разделенных компонентов ФИО
                organizers.LastName = lastName;
                organizers.FirstName = firstName;
                organizers.Patronymic = patronymic;
            }
            if (!(new Regex(@"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$")).IsMatch(this.Phone.Text)) return;
            if ((this.Email.Text != "") && (!(new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")).IsMatch(this.Email.Text))) return;
            if (!(new Regex(@"^(\d{2})\.(\d{2})\.(\d{4})$")).IsMatch(this.Birthday.Text)) return;
            organizers.Country = curCountry;
            organizers.Gender = curGender;
            //organizers.Birthday = this.Birthday.Text;
            organizers.Phone = this.Phone.Text;
            organizers.Email = this.Email.Text;

            if ((bool)changePassword_checkBox.IsChecked)
            {
                organizers.Password = this.repeatPassword_textBox.Text;
            }
            try
            {
                organizers.Birthday = Convert.ToDateTime(this.Birthday.Text);
            }
            catch
            {
                return;
            }
            try
            {
                if (organizers.id > 0)
                {
                    helper.GetContext().Entry(organizers).State = EntityState.Modified;
                    helper.GetContext().SaveChanges();
                    MessageBox.Show("Обновление информации об организаторе завершено");
                }
            }
            catch { };
            int id = organizers.id;
            NavigationService.Navigate(new OrganizerWindow(id));
            //btnDelAg.IsEnabled = true;
            //btnWritHi.IsEnabled = true;
            //btnDelHi.IsEnabled = true;

        }

        private void visiblePassword_checkBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                repeatPassword_textBox.Text = repeatPassword_passwordBox.Password; // скопируем в TextBox из PasswordBox
                repeatPassword_textBox.Visibility = Visibility.Visible; // TextBox - отобразить
                repeatPassword_passwordBox.Visibility = Visibility.Hidden; // PasswordBox - скрыть
            }
            else
            {
                repeatPassword_passwordBox.Password = repeatPassword_textBox.Text; // скопируем в PasswordBox из TextBox 
                repeatPassword_textBox.Visibility = Visibility.Hidden; // TextBox - скрыть
                repeatPassword_passwordBox.Visibility = Visibility.Visible; // PasswordBox - отобразить
            }
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)changePassword_checkBox.IsChecked)
            {
                ok_Button.IsEnabled = password_passwordBox.Password == repeatPassword_passwordBox.Password;
            }
            //ok_Button.IsEnabled = password_passwordBox.Password == repeatPassword_passwordBox.Password;
        }

        private void changePassword_checkBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                repeatPassword_passwordBox.Visibility = Visibility.Visible;
                password_passwordBox.Visibility = Visibility.Visible;
                //if (password_passwordBox.Password == repeatPassword_passwordBox.Password)
                //{
                //    organizers.Password = this.repeatPassword_textBox.Text;
                //    helper.GetContext().Entry(organizers).State = EntityState.Modified;
                //    helper.GetContext().SaveChanges();
                //    MessageBox.Show("Обновление информации об агенте завершено");
                //}

                //ok_Button.IsEnabled = password_passwordBox.Password == repeatPassword_passwordBox.Password;
                //organizers.Password = this.repeatPassword_textBox.Text;
                //try
                //{
                //    if (organizers.id > 0)
                //    {
                //        helper.GetContext().Entry(organizers).State = EntityState.Modified;
                //        helper.GetContext().SaveChanges();
                //        MessageBox.Show("Обновление информации об агенте завершено");
                //    }
                //}
                //catch { };
            }
            else
            {
                repeatPassword_passwordBox.Visibility = Visibility.Hidden;
                password_passwordBox.Visibility = Visibility.Hidden;
            }
        }

        private void cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            int id = organizers.id;
            NavigationService.Navigate(new OrganizerWindow(id));

        }

       
    }
}
    

