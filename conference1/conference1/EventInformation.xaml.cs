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

namespace conference1
{
    /// <summary>
    /// Логика взаимодействия для EventInformation.xaml
    /// </summary>
    public partial class EventInformation : Page
    {
        Events events;
        
        public EventInformation(Events ev)
        {
            InitializeComponent();
            if (ev != null)
            {
                events = ev;
                //this.Title.Text = ev.TitleEvents;
                //this.Adress.Text = ag.Address;
                //this.Inn.Text = ag.INN;
                //this.Kpp.Text = ag.KPP;
                //this.Director.Text = ag.DirectorName;
                //this.Phone.Text = ag.Phone;
                //this.Prioritet.Text = ag.Priority.ToString();
                //historyGrid.ItemsSource = helper.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == ag.ID).ToList();
            }
            //else
            //{
            //    agent = new Agent();
            //    btnDelAg.IsEnabled = false;
            //    btnWritHi.IsEnabled = false;
            //    btnDelHi.IsEnabled = false;
            //}
            this.DataContext = events;
        }

    }
}

