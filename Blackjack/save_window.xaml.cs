using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for save_window.xaml
    /// </summary>
    public partial class save_window : Window
    {
        public ObservableCollection<string> saves { get; set; }
        public save_window()
        {
            InitializeComponent();
            saves = new ObservableCollection<string>();
            save_list.DataContext = saves;
            using (var db = new Blackjack_DBEntities1())
            {
                var query = from s in db.Saves_DB
                            select s.save_name;
                foreach (var item in query)
                {
                    saves.Add(item);
                }

            }

        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
    }
}
