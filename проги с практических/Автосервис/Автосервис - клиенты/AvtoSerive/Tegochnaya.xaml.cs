using AvtoSerive.BD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using static AvtoSerive.MainWindow;

namespace AvtoSerive
{
    /// <summary>
    /// Логика взаимодействия для Tegochnaya.xaml
    /// </summary>
    /// 
    public partial class Tegochnaya : Window
    {
        Client client;

        public Tegochnaya(Client clt)
        {
            InitializeComponent();

            client = clt;

            cb_tag.ItemsSource = helper.GetContext().Tag.Select(x => x.Title).ToList();

            Load();
        }

        public void Load()
        {
            grid_tagClient.ItemsSource = client.Tag.ToList();
        }

        private void btn_addTag_Click(object sender, RoutedEventArgs e)
        {

            if(cb_tag.SelectedIndex == -1)
            {
                MessageBox.Show("Тэг не выбран!");
                return;

            }

            var tag = helper.GetContext().Tag.FirstOrDefault(t => t.ID == cb_tag.SelectedIndex+1);

            foreach(var item in grid_tagClient.Items)
            {
                if(item == tag)
                {
                    MessageBox.Show("Данный тэг уже присвоен!");
                    return;
                }
            }

            client.Tag.Add(tag);
            helper.GetContext().SaveChanges();
            Load();
        }


        private void btn_deleteTag_Click(object sender, RoutedEventArgs e)
        {
            if(grid_tagClient.SelectedItems.Count == 0)
            {
                MessageBox.Show("Не выбран тэг!");
                return;
            }
            var tag = grid_tagClient.SelectedItems[0] as Tag;
            client.Tag.Remove(tag);
            helper.GetContext().SaveChanges();
            Load();
        }
    }
}