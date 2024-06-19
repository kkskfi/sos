using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
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
    /// Логика взаимодействия для AddEvent.xaml
    /// </summary>
    public partial class AddEvent : Page
    {
        Organizers organizers;
        Events events;
        private Events currEvents = new Events();
        private Activity currActivity = new Activity();
        private int curJury;
        private int curCity;
        private int curDir;

        public AddEvent(Organizers or)
        {
            
            InitializeComponent();
            try
            {
                //TitleEvent.ItemsSource = helper.GetContext().Events.ToList();
                TitleDirection.ItemsSource = helper.GetContext().Direction.ToList();
                City.ItemsSource = helper.GetContext().Cities.ToList();
                Jury.ItemsSource = helper.GetContext().Moderators.ToList();
            }
            catch { };

        }

        //private void Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    curEvent = ((Events)TitleEvent.SelectedItem).id;
        //}

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string fnd = ((ComboBox)sender).Text;
            //try
            //{
            //    City.ItemsSource = helper.GetContext().Countries.Where(Countries => Countries.Title.Contains(fnd)).ToList();
            //}
            //catch { };
            curCity = ((Cities)City.SelectedItem).id;
        }

        private void Direction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curDir = ((Direction)TitleDirection.SelectedItem).id;
        }
        private void ok_button_Click(object sender, RoutedEventArgs e)
        {

            if (curCity == 0) return;
            if (curDir == 0) return;
            if (TitleEvent.Text == "") return;
            DateTime startDate = (DateTime)StartTime.SelectedDate;
            DateTime finishDate = (DateTime)FinishTime.SelectedDate;

            if (startDate != null && finishDate != null)
            {
                TimeSpan difference = finishDate.Subtract(startDate);
                currEvents.Days = Convert.ToInt32(difference.TotalDays);
            }

            currEvents.Date = (DateTime)StartTime.SelectedDate;
            currEvents.TitleEvents = TitleEvent.Text;
            currEvents.Direction = curDir;
            currEvents.City = curCity;
            helper.GetContext().Events.Add(currEvents);
            helper.GetContext().SaveChanges();

            currActivity.Title = TitleActivity1.Text;
            currActivity.Title = TitleActivity2.Text;
            //currActivity.Title = TitleActivity3.Text;

            currActivity.StartTime = TimeSpan.ParseExact((string)TimeActivity1.SelectedItem, "hh\\:mm", CultureInfo.InvariantCulture);
            //currActivity.StartTime = TimeSpan.ParseExact((string)TimeActivity2.SelectedItem, "hh\\:mm", CultureInfo.InvariantCulture);

            //currActivity.StartTime = (TimeSpan)TimeActivity1.SelectedItem;
            //currActivity.StartTime = (TimeSpan)TimeActivity2.SelectedValue;
            //currActivity.StartTime = (TimeSpan)TimeActivity3.SelectedValue;
            currActivity.Moderator = curJury;
            var evv = helper.GetContext().Events.ToList().Where(p => p.TitleEvents == TitleEvent.Text && p.Date == StartTime.SelectedDate).Single();
            currActivity.id = evv.id;
            helper.GetContext().Activity.Add(currActivity);
            helper.GetContext().SaveChanges();
            MessageBox.Show("Мероприятие добавлено");

        }

        private void Jury_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string fnd = ((ComboBox)sender).Text;
            //try
            //{
            //    Jury1.ItemsSource = helper.GetContext().Jury.Where(Jury => Jury.LastName.Contains(fnd)).ToList();
            //}
            //catch { };
            curJury = ((Moderators)Jury.SelectedItem).id;
        }

        private void TimeActivity1_Loaded(object sender, RoutedEventArgs e)
        {
            TimeSpan timeInterval = new TimeSpan(1, 30, 0); //интервал 90 минут
            TimeSpan breakTime = new TimeSpan(0, 15, 0); //перерыв
            TimeSpan startTime = new TimeSpan(9, 30, 0);


            TimeSpan endTime = new TimeSpan(24, 0, 0);
            //startTime = endTime + breakTime; //время начала следующей
            //string activityTime = $"{startTime.ToString("HH:mm")} - {endTime.ToString("HH:mm")}";

            //if (startDate != null)
            //{
            //    if (TitleActivity1 != null)
            //    {
            while (startTime <= endTime)
            {
                TimeActivity1.Items.Add(startTime.ToString(@"hh\:mm")); // добавляем время в формате "чч:мм"
                startTime = startTime.Add(timeInterval); // увеличиваем время добавления на интервал

            }
        }

        private void TimeActivity2_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartTime.SelectedDate;
            TimeSpan timeInterval = new TimeSpan(1, 30, 0); //интервал 90 минут
            TimeSpan breakTime = new TimeSpan(0, 15, 0); //перерыв
            TimeSpan startTime = new TimeSpan(9, 30, 0);



            TimeSpan endTime = new TimeSpan(24, 0, 0);
            //TimeSpan startTime = endTime + breakTime; //время начала следующей
            //string activityTime = $"{startTime.ToString("HH:mm")} - {endTime.ToString("HH:mm")}";//длительность активности

            //if (startDate != null)
            //{
            //if (TimeActivity1.SelectedItem != null)
            //{
                while (startTime <= endTime)
                {
                    TimeActivity2.Items.Add(startTime.ToString(@"hh\:mm")); // добавляем время в формате "чч:мм"
                    startTime = startTime.Add(timeInterval); // увеличиваем время добавления на интервал

                }
            //}
            //}
        }

        private void TimeActivity3_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartTime.SelectedDate;
            TimeSpan timeInterval = new TimeSpan(1, 30, 0); //интервал 90 минут
            TimeSpan breakTime = new TimeSpan(0, 15, 0); //перерыв
            TimeSpan startTime = new TimeSpan(9, 30, 0);



            TimeSpan endTime = new TimeSpan(24, 0, 0);
            //TimeSpan startTime = endTime + breakTime; //время начала следующей
            //string activityTime = $"{startTime.ToString("HH:mm")} - {endTime.ToString("HH:mm")}";//длительность активности

            //if (startDate != null)
            //{
            //if (TimeActivity2.SelectedItem != null)
            //{
                //TimeSpan startTime = (TimeSpan)TimeActivity2.SelectedItem;
                while (startTime <= endTime)
                {
                    TimeActivity3.Items.Add(startTime.ToString(@"hh\:mm")); // добавляем время в формате "чч:мм"
                    startTime = startTime + timeInterval + breakTime; // увеличиваем время добавления на интервал

                }
            //}
            //}
        }

        private void csv_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("csv");
        }

        private void kandan_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Доска");
        }
    }

}        
    


    
