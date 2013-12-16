using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Blackjack
{
    // handles all interaction between main window and code-behind classes with data and logic

    class Bj_interaction : INotifyPropertyChanged
    {
        // testing Github får facks säjk

        public short bets_placed;
        private Players players;
        public CardDeck deck;               // Warning Puclic
        private Dealer dealer;
        private static Bj_interaction instance_variable;

        private int player_column;

        private bool move_visibility;
        private bool done_button_visibility;
        private bool deal_button_visibility;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;
        private Bj_interaction()
        {
            player_column = 0;
            move_visibility = false;
            done_button_visibility = false;
            deal_button_visibility = false;
            bets_placed = 0;
            players = new Players();
            deck = new CardDeck();
            dealer = new Dealer();
        }

        public static Bj_interaction instance()
        {
            if (instance_variable == null)
            {
                instance_variable = new Bj_interaction();
                return instance_variable;
            }
                
            else
                return instance_variable;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        internal void yo()
        { }
        public bool Move_Visibility
        {
            get { return move_visibility; }
            set
            {
                move_visibility = value;
                OnPropertyChanged("Move_Visibility");
            }
        }
        public bool Done_Button_Visibility
        {
            get { return done_button_visibility; }
            set
            {
                done_button_visibility = value;
                OnPropertyChanged("Done_Button_Visibility");
            }
        }
        public bool Deal_Button_Visibility
        {
            get { return deal_button_visibility; }
            set
            {
                deal_button_visibility = value;
                OnPropertyChanged("Deal_Button_Visibility");
            }
        }
        public int Player_Column
        {
            get { return player_column; }
            set
            {
                player_column = value;
                OnPropertyChanged("Player_Column");
            }
        }
        public Dealer get_dealer()
        {
            return dealer;
            
        }
        public void new_round()
        {
            Done_Button_Visibility = false;
            deck.clear_table();
            dealer.reset_Xoffset();
            //players.reset_Xoffset();
            players.Active_Player = 0;
            bets_placed = 0;
            players.new_round();
            dealer.reset();
            dealer.Status_Visibility = false;
        }

        /*
         * Deck functions
         */ 
        public void deck_load()
        {
            deck.load();            
        }

        public Image deck_get_card_image(short i)
        {
            return deck.get_card_image(i);
        }

        public void deck_set_card_image(short i, Image c)
        {
            deck.set_card_image(i, c);
        }

        public string deck_get_image_name(short i)
        {
            return deck.get_image_name(i);
        }

        public string deck_get_image_name()
        {
            return deck.get_image_name();
        }

        public int deck_size()
        {
            return deck.size();
        }

        public void deck_shuffle()
        {
            deck.shuffle();
        }

        public Image deck_get_next_image()
        {
            return deck.get_next_image();
        }

        public Image deck_get_active_image()
        {
            return deck.get_active_image();
        }

        public Image deck_get_onTable_image(short i)
        {
            return deck.get_onTable_image(i);
        }

        public int deck_onTable_size()
        {
            return deck.onTable_size();
        }

        public double[] deck_get_start_coordinates()
        {
            return deck.Start_coordinates();
        }

        public double[] deck_get_end_coordinates()
        {
            return deck.End_coordinates();
        }

        /*
         * Player functions
         */
        internal void player_create(short p)
        {
            players.add_player(p);
            player_valid_deal();
        }

        internal void player_remove(short p)
        {
            players.remove_player(p);
            player_valid_deal();
        }

        internal void player_update_bet(short p, short b)
        {
            players.update_player_bet(p,b);
        }

        internal void player_clear_bet(short player)
        {
            players.clear_player_bet(player);
        }

        internal Player player_get_player(short p)
        {
            return players.get_player(p);
        }

        internal Player player_get_active()
        {
            return players.get_player(players.Active_Player);
        }

        internal bool player_isactive(short p)
        {
            return players.is_active(p);
        }

        internal bool player_valid_deal()
        {
            if (players.Active_Players > 0 && (players.Active_Players == bets_placed))
            {
                Deal_Button_Visibility = true;
                return true;
            }
            else
            {
                Deal_Button_Visibility = false;
                return false;
            }
        }

        internal bool player_double_down_allowed()
        {
            return players.double_down_allowed();
        }

        internal bool player_split_allowed()
        {
            return players.split_allowed();
        }

       
        public bool player_change()
        {
            players.Active_Player++;            
            return set_active_player();
        }

        internal bool set_active_player()
        {
            if (players.set_active_player())
            {
                Move_Visibility = true;
                Player_Column = 4 - players.Active_Player;
                return true;
            }
            Move_Visibility = false;
            return false;
        }

        internal short player_get_active_player_nr()
        {           
           return players.Active_Player;
        }

        internal short player_get_active_hand()
        {
            return players.get_active_hand();
        }

        internal bool player_place_bet(short p)
        {

            if (players.get_player(p).Player_Bet > 0)
            {
                players.get_player(p).Bet_Grid_Visibility = false;
                bets_placed++;
                player_valid_deal();
                return true;
            }

            return false;
        }

        internal double[] player_coordinates()
        {
            return players.player_coordinates();
        }

        internal double[] player_coordinates(short p)
        {
            return players.player_coordinates(p);
        }

        internal void set_coordinates(double Xcoord, double Ycoord)
        {            
            dealer.set_coordinates(Xcoord);
            deck.set_coordinates(Xcoord);
        }
        internal void player_set_coordinates(double Xcoord, double Ycoord)
        {
            players.set_coordinates(Xcoord, Ycoord);
        }
        

        internal void player_add_card()
        {
            players.add_card(deck.Active_Card);
        }

        internal void player_add_card(short s)
        {
            players.add_card(s, deck.Active_Card);
        }
        internal void player_add_split_card()
        {
            players.add_split_card(deck.Active_Card);
        }

        internal Image player_get_active_image()
        {
            return players.get_active_image();
        }

        internal bool player_double_down()
        {
            return players.double_down();
        }

        internal bool player_stand()
        {
            return players.stand();
        }

        internal bool player_hit()
        {
            return players.hit();
        }
        internal bool player_split()
        {
            return players.split();
        }
       
        internal double[] player_split_coordinates()
        {
            return players.player_split_coord();
        }

        /*
         * Dealer functions 
         */
        internal double[] dealer_coordinates()
        {
            return dealer.coordinates();
        }

        internal string dealer_get_hidden()
        {
            return dealer.Hidden;
        }

        internal void dealer_set_hidden(string h)
        {
            dealer.Hidden = h;
        }

        internal void dealer_set_card(Image c)
        {
            dealer.Hidden_Card = c;
        }

        internal Image dealer_get_card()
        {
            return dealer.Hidden_Card;
        }

        internal void dealer_add_card()
        {
            dealer.add_card(deck.Active_Card.Card_Value);
        }
        
        internal bool dealer_logic()
        {
            return dealer.logic();
        }

        internal bool blackjack_logic()
        {
            //set blackjack for players
            

            if (dealer.dealer_blackjack())
            {
                for (short s = 0; s < 5; ++s)
                {
                    if (players.is_active(s))
                    {
                        if (!players.blackjack(s))
                            players.player_loss(s);
                        
                    }
                }                
                return true;
            }
            else
            {
                for (short s = 0; s < 5; ++s)
                {
                    if (players.is_active(s))
                    {
                        if (players.blackjack(s))
                            players.blackjack_win(s);
                    }
                }
                return false;
            }
        }

        internal void calculate_win()
        {
            players.calculate_win(dealer.Hand_Value);
            Done_Button_Visibility = true;
        }

        internal void deal()
        {
            Deal_Button_Visibility = false;
            players.deal();
        }

        internal void new_game()
        {
            players.reset();
            deck.reset();
            dealer.reset();
            deck_load();
            bets_placed = 0;
            set_coordinates(1000, 400);
            Move_Visibility = false;
            Done_Button_Visibility = false;           
            players.add_visibility();
        }
        internal void load_game()
        {

        }

        internal void save_game()
        {
            save_window save = new save_window();           
            save.ShowDialog();
        }
    }
}
