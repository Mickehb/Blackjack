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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Storyboard mystoryboard;
        int cardHeight = 112;
        int cardWidth = 80;
        
        public MainWindow()
        {
            InitializeComponent();
            Bj_interaction.instance().deck_load();
            Bj_interaction.instance().set_coordinates(1600, 900);
            load_card_image();
            Bj_interaction.instance().deck_shuffle();
            set_datacontext();
        }

        /****************************************
         *          Animation functions         *
         * **************************************/
        private void one_card_animation(double[] from, double[] to, Image card)
        {
            /*
             * double[] holds coordinates
             * double[X,Y]
             */
            card.Visibility = Visibility.Visible;
            DoubleAnimation horizontal = new DoubleAnimation();
            DoubleAnimation vertical = new DoubleAnimation();
            mystoryboard = new Storyboard();
            Duration myduration = new Duration(TimeSpan.FromMilliseconds(250));
            PropertyPath canvasLeft = new PropertyPath(Canvas.LeftProperty);
            PropertyPath canvasTop = new PropertyPath(Canvas.TopProperty);

            // Horizontal animation
            horizontal.From = from[0];
            horizontal.To = to[0];
            horizontal.Duration = myduration;

            // Vertical animation
            vertical.From = from[1];
            vertical.To = to[1];
            vertical.Duration = myduration;

            mystoryboard.Children.Add(horizontal);
            mystoryboard.Children.Add(vertical);

            Storyboard.SetTargetName(horizontal, card.Name);
            Storyboard.SetTargetProperty(horizontal, canvasLeft);
            Storyboard.SetTargetName(vertical, card.Name);
            Storyboard.SetTargetProperty(vertical, canvasTop);

            mystoryboard.Begin(this);
        }

        private void clear_animation()
        {
            int decksize = Bj_interaction.instance().deck_onTable_size();
            double[] from = new double[2];
            double[] to = Bj_interaction.instance().deck_get_end_coordinates();
            Image tmp;

            for (short i = 0; i < decksize; i++)
            {
                tmp = Bj_interaction.instance().deck_get_onTable_image(i);
                from[0] = Canvas.GetLeft(tmp);
                from[1] = Canvas.GetTop(tmp);

                one_card_animation(from, to, tmp);
            }
        }

        private void deal_animation()
        {
            double[] from = Bj_interaction.instance().deck_get_start_coordinates();
            double[] to;
            Image card;

            for (int a = 0; a < 2; a++)
            {
                for (short i = 0; i < 5; i++)
                {
                    if (Bj_interaction.instance().player_isactive(i))
                    {
                        to = Bj_interaction.instance().player_coordinates(i);
                        card = Bj_interaction.instance().deck_get_next_image();
                        card.Visibility = Visibility.Visible;
                        one_card_animation(from, to, card);
                        Bj_interaction.instance().player_add_card(i);
                    }
                }
                if (a == 0) // Dealers 1st card
                {
                    to = Bj_interaction.instance().dealer_coordinates();
                    card = Bj_interaction.instance().deck_get_next_image();
                    one_card_animation(from, to, card);
                    Bj_interaction.instance().dealer_add_card();
                }
                else // Dealers 2nd card
                {
                    to = Bj_interaction.instance().dealer_coordinates();
                    card = Bj_interaction.instance().deck_get_next_image();
                    string filename = Bj_interaction.instance().deck_get_image_name();
                    Bj_interaction.instance().dealer_set_hidden(filename);
                    Uri src = new Uri("pack://application:,,,/Images/Deck/brv.png");
                    BitmapImage img = new BitmapImage(src);
                    card.Source = img;
                    Bj_interaction.instance().dealer_set_card(card);
                    one_card_animation(from, to, card);
                    Bj_interaction.instance().dealer_add_card();
                }
            }
        }

        private void show_dealer_hidden() //Shows dealers hidden card
        {
            string filename = Bj_interaction.instance().dealer_get_hidden();
            Uri src = new Uri("pack://application:,,,/Images/Deck/" + filename);
            BitmapImage img = new BitmapImage(src);
            Image card = Bj_interaction.instance().dealer_get_card();
            card.Source = img;
        }

        /********************************
         *          Load Images         *
         ********************************/
        private void load_card_image()
        {
            int decksize = Bj_interaction.instance().deck_size();
            double[] startCoords = Bj_interaction.instance().deck_get_start_coordinates();
            double[] endCoords = Bj_interaction.instance().deck_get_end_coordinates();
            Uri src;

            for (int i = 0; i < decksize; i++)
            {
                string filename = Bj_interaction.instance().deck_get_image_name((short)i);
                src = new Uri("pack://application:,,,/Images/Deck/" + filename);
                BitmapImage img = new BitmapImage(src);

                Image card = new Image();
                card.Source = img;
                card.Name = "Cardname" + i.ToString();
                this.RegisterName(card.Name, card);
                card.Width = cardWidth;
                card.Height = cardHeight;
                card.Visibility = Visibility.Visible;


                Bj_interaction.instance().deck_set_card_image((short)i, card);
                canvas1.Children.Add(card);

                Canvas.SetLeft(card, startCoords[0]);
                Canvas.SetTop(card, startCoords[1]);
            }

            src = new Uri("pack://application:,,,/Images/Deck/brv.png");

            BitmapImage startimg = new BitmapImage(src);
            Image startcard = new Image();
            startcard.Source = startimg;
            startcard.Name = "startpile";
            this.RegisterName(startcard.Name, startcard);
            startcard.Width = cardWidth;
            startcard.Height = cardHeight;
            startcard.Visibility = Visibility.Visible;
            Canvas.SetZIndex(startcard, (decksize + 2));
            canvas1.Children.Add(startcard);
            Canvas.SetLeft(startcard, startCoords[0]);
            Canvas.SetTop(startcard, startCoords[1]);

            BitmapImage endimg = new BitmapImage(src);
            Image endcard = new Image();
            endcard.Source = endimg;
            endcard.Name = "throwpile";
            this.RegisterName(endcard.Name, endcard);
            endcard.Width = cardWidth;
            endcard.Height = cardHeight;
            endcard.Visibility = Visibility.Visible;
            Canvas.SetZIndex(endcard, (decksize + 2));
            canvas1.Children.Add(endcard);
            Canvas.SetLeft(endcard, endCoords[0]);
            Canvas.SetTop(endcard, endCoords[1]);

        }

        private void change_player()
        {

            if(!Bj_interaction.instance().player_change())           
            {
                /*
                 * else dealer_logic()
                 */
                show_dealer_hidden();

                while (Bj_interaction.instance().dealer_logic())
                {
                    double[] to = Bj_interaction.instance().dealer_coordinates();
                    double[] from = Bj_interaction.instance().deck_get_start_coordinates();
                    Image card = Bj_interaction.instance().deck_get_next_image();
                    one_card_animation(from, to, card);
                    Bj_interaction.instance().dealer_add_card();
                }
                Bj_interaction.instance().calculate_win();

            }
        }

        private void set_datacontext()
        {

                    deal.DataContext = Bj_interaction.instance();
                    done.DataContext = Bj_interaction.instance();
                    p_moves.DataContext = Bj_interaction.instance();
                    dealer_hand.DataContext = Bj_interaction.instance().get_dealer();

                    p1_betting.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_add.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_money.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_name.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_bet.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_hand.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_hand1.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_hand2.DataContext = Bj_interaction.instance().player_get_player(0);
                    p1_hand3.DataContext = Bj_interaction.instance().player_get_player(0);

                    p2_betting.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_add.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_money.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_name.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_bet.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_hand.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_hand1.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_hand2.DataContext = Bj_interaction.instance().player_get_player(1);
                    p2_hand3.DataContext = Bj_interaction.instance().player_get_player(1);


                    p3_betting.DataContext = Bj_interaction.instance().player_get_player(2);
                    p3_add.DataContext = Bj_interaction.instance().player_get_player(2); 
                    p3_money.DataContext = Bj_interaction.instance().player_get_player(2);
                    p3_name.DataContext = Bj_interaction.instance().player_get_player(2);
                    p3_bet.DataContext = Bj_interaction.instance().player_get_player(2);
                    p3_hand.DataContext = Bj_interaction.instance().player_get_player(2);
                    p3_hand1.DataContext = Bj_interaction.instance().player_get_player(2);
                    p3_hand2.DataContext = Bj_interaction.instance().player_get_player(2);
                    p3_hand3.DataContext = Bj_interaction.instance().player_get_player(2);

                    p4_betting.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_add.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_money.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_name.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_bet.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_hand.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_hand1.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_hand2.DataContext = Bj_interaction.instance().player_get_player(3);
                    p4_hand3.DataContext = Bj_interaction.instance().player_get_player(3);

                    p5_betting.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_add.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_money.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_name.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_bet.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_hand.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_hand1.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_hand2.DataContext = Bj_interaction.instance().player_get_player(4);
                    p5_hand3.DataContext = Bj_interaction.instance().player_get_player(4);
        

        }

        /************************************
         *          EVENT HANDLERS          *
         * **********************************/
        private void deal_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().deal();
            Bj_interaction.instance().player_set_coordinates(1600, 900);
           
            deal_animation();

            //will work once blackjack_logic works
           if (Bj_interaction.instance().blackjack_logic())
            {
                show_dealer_hidden();
                Bj_interaction.instance().Done_Button_Visibility = true;
            }
            else
            {
                if (!Bj_interaction.instance().set_active_player())
                {
                    show_dealer_hidden();
                    Bj_interaction.instance().Done_Button_Visibility = true;
                    Bj_interaction.instance().calculate_win();
                }
            }
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            clear_animation();
            Bj_interaction.instance().new_round();
        }

        /*
         * Playing handlers
         */
        private void hit_Click(object sender, RoutedEventArgs e)
        {
            errorbox.DataContext = Bj_interaction.instance().deck;
            double[] from = Bj_interaction.instance().deck_get_start_coordinates();
            double[] to = Bj_interaction.instance().player_coordinates();
            Image card = Bj_interaction.instance().deck_get_next_image();
            one_card_animation(from, to, card);
            Bj_interaction.instance().player_add_card();

            if (!Bj_interaction.instance().player_hit())
                change_player();
        }

        private void stand_Click(object sender, RoutedEventArgs e)
        {
            /*
             * stand logic
             */
            short active_player = Bj_interaction.instance().player_get_active_player_nr();
            short active_hand = Bj_interaction.instance().player_get_active_hand();

            if (!Bj_interaction.instance().player_stand())
            {
                change_player();
            }
        }

        private void double_down_Click(object sender, RoutedEventArgs e)
        {
            /*
             *  double down logic
             *  take another card move to next player
             *  bet more money
             */
            if (Bj_interaction.instance().player_double_down_allowed())
            {
                errorbox.DataContext = Bj_interaction.instance().deck;
                double[] from = Bj_interaction.instance().deck_get_start_coordinates();
                double[] to = Bj_interaction.instance().player_coordinates();
                Image card = Bj_interaction.instance().deck_get_next_image();
                one_card_animation(from, to, card);
                Bj_interaction.instance().player_add_card();

                if (!Bj_interaction.instance().player_double_down())
                    change_player();
            }
        }

        private void split_Click(object sender, RoutedEventArgs e)
        {

            errorbox.Text = "Split not implemented";
            errorbox.DataContext = Bj_interaction.instance().deck;

            // get our current card_image
            Image card = Bj_interaction.instance().player_get_active_image();

            // if split allowed is allowed, do animations
            if (Bj_interaction.instance().player_split())
            {
                // get our current coordinates
                double[] from = Bj_interaction.instance().player_coordinates();


                double[] to = Bj_interaction.instance().player_split_coordinates();

                one_card_animation(from, to, card);

                from = Bj_interaction.instance().deck_get_start_coordinates();
                to = Bj_interaction.instance().player_coordinates();

                card = Bj_interaction.instance().deck_get_next_image();
                Bj_interaction.instance().player_add_card();
                one_card_animation(from, to, card);

                to = Bj_interaction.instance().player_split_coordinates();
                to[0] += 30;

                card = Bj_interaction.instance().deck_get_next_image();
                Bj_interaction.instance().player_add_split_card();
                one_card_animation(from, to, card);
            }


        }

        /*
         * Leave_Click event handlers
        */
        private void p1_leave_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_remove(0);
        }

        private void p2_leave_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_remove(1);
        }

        private void p3_leave_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_remove(2);
        }

        private void p4_leave_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_remove(3);
        }

        private void p5_leave_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_remove(4);
        }

        /*
         * Add_Click event handlers
         */
        private void p1_add_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_create(0);
        }
        private void p2_add_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_create(1);
        }

        private void p3_add_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_create(2);
        }

        private void p4_add_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_create(3);
        }

        private void p5_add_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_create(4);
        }

        /*
         * Betting  event handlers
         */
        // Bet 5
        private void p1_five_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(0, 5);
        }

        private void p2_five_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(1, 5);
        }

        private void p3_five_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(2, 5);
        }
        private void p4_five_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(3, 5);
        }

        private void p5_five_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(4, 5);
        }
        // Bet 10
        private void p1_ten_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(0, 10);
        }
        private void p2_ten_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(1, 10);
        }
        private void p3_ten_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(2, 10);
        }

        private void p4_ten_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(3, 10);
        }

        private void p5_ten_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(4, 10);
        }
        // Bet 20
        private void p1_twenty_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(0, 20);
        }

        private void p2_twenty_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(1, 20);
        }
        private void p3_twenty_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(2, 20);
        }

        private void p4_twenty_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(3, 20);
        }

        private void p5_twenty_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_update_bet(4, 20);

        }

        /*
         * Clear Bet event handlers
         */
        private void p1_clear_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_clear_bet(0);
        }

        private void p2_clear_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_clear_bet(1);
        }

        private void p3_clear_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_clear_bet(2);
        }

        private void p4_clear_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_clear_bet(3);
        }

        private void p5_clear_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_clear_bet(4);
        }

        /*
         * Place Bet event handlers
         */
        private void p1_place_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_place_bet(0);
        }

        private void p2_place_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_place_bet(1);
        }

        private void p3_place_bet_Click(object sender, RoutedEventArgs e)
        {           
            Bj_interaction.instance().player_place_bet(2);
        }

        private void p4_place_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_place_bet(3);
        }

        private void p5_place_bet_Click(object sender, RoutedEventArgs e)
        {
            Bj_interaction.instance().player_place_bet(4);
        }

        /*
         * Menu event handlers
         */
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

