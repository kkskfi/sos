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
using static conference1.MainWindow;
using static System.Net.Mime.MediaTypeNames;

namespace conference1
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        Authorization authorization;
        Events events;
        //Frame frame;
   
        public Authorization(Events events)
        {
            InitializeComponent();
 
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //string ID = Convert.ToString(dataGridView1[2, Convert.ToInt32(dataGridView1.CurrentRow.Index)].Value);
            //int ID = int.Parse(id_TextBox.ToString());
            //int ID = password_TextBox;
            //int ID = password_TextBox.CurrentRow.RowIndex;
            //int ID = int.Parse(id_TextBox.Text);
            int ID =0;
            if (int.TryParse(id_TextBox.Text, out ID))
            {
                ID = int.Parse(id_TextBox.Text);
            }
            else
            {
                MessageBox.Show("Только числовое значение");
            }
            string password = password_TextBox.Text;
            //var user = helper.GetContext().Organizers.Where(p => p.id == ID && p.Password == password).FirstOrDefault();
            var organizers = helper.GetContext().Organizers.Where(p => p.id == ID && p.Password == password).FirstOrDefault();
            var members = helper.GetContext().Members.Where(p => p.id == ID && p.Password == password).FirstOrDefault();
            var moderators = helper.GetContext().Moderators.Where(p => p.id == ID && p.Password == password).FirstOrDefault();
            var jury = helper.GetContext().Jury.Where(p => p.id == ID && p.Password == password).FirstOrDefault();

            if (id_TextBox.Text.Length > 0)
            {
                if (password_TextBox.Text.Length > 0)
                {
                    if(id_TextBox.Text.Length > 0 && password_TextBox.Text.Length > 0)
                    {
                        if (organizers != null)
                        {
                            int id = organizers.id;
                            NavigationService.Navigate(new OrganizerWindow(id));
                        }
                        else if (members !=null)

                        {
                            MessageBox.Show("ОК. участник");
                        }

                        else if (moderators != null)
                        {
                            MessageBox.Show("ОК. модератор");
                        }
                        else if (jury != null)
                        {
                            MessageBox.Show("ОК. жюри");
                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите id и пароль");
                    }
                }
                else
                {
                    MessageBox.Show("Введите пароль");
                }
                
            }
            else
            {
                MessageBox.Show("Введите id");
            }
            
        }
    }
}
