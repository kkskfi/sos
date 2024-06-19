using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

namespace conference1
{
    /// <summary>
    /// Логика взаимодействия для Events.xaml
    /// </summary>
    public partial class Events : Page
    {
        private int iag =0;
        Frame fr;
        //OrganizerWindow or;
        //OrganizerWindow organizers;

        public Events(Frame frame)
        {
            
            fr = frame;
            InitializeComponent();
            List<Direction> directions = new List<Direction> { };
            directions = helper.GetContext().Direction.ToList();
            directions.Add(new Direction { Title = "Все направления" });
            Directions.ItemsSource = directions.OrderBy(Direction => Direction.id);
            Load();

        }
        public void Load()
        {
            try
            {
                var ev = helper.GetContext().Events.OrderBy(Events => Events.id).ToList();
                if (iag == 0)
                {
                    eventGrid.ItemsSource = ev.OrderBy(Events => Events.id).ToList();
                }
                else
                {
                    var evv = ev.Where((Events => Events.Direction == iag));
                    eventGrid.ItemsSource = evv.OrderBy(Events => Events.id).ToList();

                }
            }
            catch
            {
                return;
            }
            
        }

        //public void Button(OrganizerWindow organizers)
        //{
        //    or = organizers;

        //    if (or != null)
        //    {
        //        addButton.Visibility = Visibility.Visible;
        //        authorizationButton.Visibility = Visibility.Hidden;
        //    }


        //}

        private void authorizationButton_Click(object sender, RoutedEventArgs e)
        {
            //fr.Content = new Authorization(null);
            NavigationService.Navigate(new Authorization(null));
        }

        private void Directions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iag = ((Direction)Directions.SelectedItem).id;
            Load();
        }

        private void eventGrid_MouseDown(object sender, MouseButtonEventArgs e)
        { 
            if (eventGrid.SelectedItems.Count > 0)
            {
                Events events = eventGrid.SelectedItems[0] as Events;

                if (events != null)
                {
                     fr.Content = new EventInformation(events);
                }
            }

        }

        //private void addButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //or = organizers;

        //    //if (or != null)
        //    //{
        //    //    addButton.Visibility = Visibility.Visible;
        //    //    authorizationButton.Visibility = Visibility.Hidden;
        //    //}
            
        //}

        private void add_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
