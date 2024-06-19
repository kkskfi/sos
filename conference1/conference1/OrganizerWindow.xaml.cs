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
using System.Windows.Threading;
using static conference1.MainWindow;

namespace conference1
{
    /// <summary>
    /// Логика взаимодействия для OrganizerWindow.xaml
    /// </summary>
    public partial class OrganizerWindow : Page
    {
        //Frame fr;
        private DispatcherTimer showtimer;
        //Authorization authorization;
        //Authorization id;
        int IdUser = 0;
        Profile profile;
        Events events;
        public OrganizerWindow(int id)
        {

            InitializeComponent();

            showtimer = new DispatcherTimer();

            showtimer.Tick += new EventHandler(ShowCurTimer);

            showtimer.Interval = new TimeSpan(0, 0, 0, 1, 0);

            showtimer.Start();

            IdUser = id;
            //Load();

            var or = helper.GetContext().Organizers.OrderBy(Organizers => Organizers.id).ToList();
            organizergrid.DataContext = or.Where(p => p.id == IdUser);

            var organizer = helper.GetContext().Organizers.Where(o => o.id == IdUser).FirstOrDefault();
            name_textBlock.Text = $"{organizer.FirstName} {organizer.Patronymic}";
        }
        private void profile_button_Click(object sender, RoutedEventArgs e)
        {
            Organizers organizers = helper.GetContext().Organizers.Where(o => o.id == IdUser).FirstOrDefault();
            //fr.Content = new Profile(organizers);
            NavigationService.Navigate(new Profile(organizers));
        }
        private void ShowCurTimer(object sender, EventArgs e)
        {
            if (Convert.ToInt32(DateTime.Now.ToString("HH")) < 11 && Convert.ToInt32(DateTime.Now.ToString("HH")) >= 09)
            {
                greetings_textBlock.Text = "Доброе утро!";
            }
            else if (Convert.ToInt32(DateTime.Now.ToString("HH")) < 18 && Convert.ToInt32(DateTime.Now.ToString("HH")) >= 11)
            {
                greetings_textBlock.Text = "Добрый день!";
            }
            else if (Convert.ToInt32(DateTime.Now.ToString("HH")) < 24 && Convert.ToInt32(DateTime.Now.ToString("HH")) >= 18)
            {
                greetings_textBlock.Text = "Добрый вечер!";
            }
        }

        private void events_button_Click(object sender, RoutedEventArgs e)
        {
            //var events = helper.GetContext().Events.OrderBy(Events => Events.id).ToList();
            Organizers organizers = helper.GetContext().Organizers.Where(o => o.id == IdUser).FirstOrDefault();
            //fr.Content = new Profile(organizers);
            NavigationService.Navigate(new AddEvent(organizers));
        }

        private void members_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Участники");
        }

        private void jury_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Жюри");
        }

        //public void Load()
        //{
        //    //var or = helper.GetContext().Organizers.OrderBy(Organizers => Organizers.id).ToList();
        //    //organizergrid.DataContext = or.Where(p => p.id == IdUser);

        //    //var organizer = helper.GetContext().Organizers.Where(o => o.id == IdUser).FirstOrDefault();
        //    //name_textBlock.Text = $"{organizer.FirstName} {organizer.Patronymic}";

        //    //helper.GetContext().Organizers.OrderBy(Organizers => Organizers.id).ToList();
        //}

        //private void events_button_Click(object sender, RoutedEventArgs e/*, Frame frame*/)
        //{
        //    ////fr = frame;
        //    //Organizers organizers = helper.GetContext().Organizers.Where(o => o.id == IdUser).FirstOrDefault();
        //    //NavigationService.Navigate(new Events(organizers/*, frame*/));
        //}
    }
}
