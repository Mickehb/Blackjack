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
using System.Windows.Shapes;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for player_add_window.xaml
    /// </summary>
    public partial class player_add_window : Window
    {
        public player_add_window()
        {
            InitializeComponent();
        }
        public player_add_window(int p)
        {
            InitializeComponent();
            player_name_TextBox.DataContext = Bj_interaction.instance().player_get_player(p);
            player_money_TextBox.DataContext = Bj_interaction.instance().player_get_player(p);
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression be = player_name_TextBox.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
            be = player_money_TextBox.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
            
            this.Close();
        }
    }
}
