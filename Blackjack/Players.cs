using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Blackjack
{
    class Players
    {
        private Player[] players;
        private int active_player;
        private int active_players;
        private int max_players;

        public Players()
        {
            players = new Player[] { new Player(), new Player(), new Player(), new Player(), new Player() };
            active_player = 0;
            active_players = 0;
            max_players = 5;
        }

        public int Active_Player
        {
            get { return active_player; }
            set { active_player = value; }
        }

        
        public int Active_Players
        {
            get { return active_players; }
            set { active_players = value; }
        }

        public void add_player(int p)
        {          
            player_add_window addwindow = new player_add_window(p);
            players[p].activate();
            addwindow.ShowDialog();
            active_players++;
        }

        public void remove_player(int p)
        {
            players[p].reset();
            active_players--;
        }

        public Player get_player(int p)
        {
           return players[p];
        }

        public void update_player_bet(int p, int b)
        {
            players[p].update_bet(b);
        }

        public bool double_down_allowed()
        {
            return players[active_player].double_down_allowed();
        }
        public bool split_allowed()
        {
            return players[active_player].split_allowed();
        }
        public void clear_player_bet(int p)
        {
            players[p].clear_bet();
        }

        public bool is_active(int p)
        {
            return players[p].Is_Active;
        }

        public void add_card(Card c)
        {
            if (players[active_player] != null)
                players[active_player].add_card(c);
        }

        public void add_card(int s, Card c)
        {
            if (players[s] != null)
                players[s].add_card(c); 
        }

        public void add_card_hand(int p, int h, Card c)
        {
            if (is_active(p))
                players[p].add_card(h, c);
        }

        public void add_split_card(Card c)
        {
            players[active_player].split_add_card(c);
        }

        public void clear_hands()
        {
            for(int i = 0; i < max_players; ++i)
            {
                if (is_active(i))
                    players[i].clear_hands();                    
                
            }
        }

        internal Image get_active_image()
        {
            return players[active_player].get_active_image();
        }

        internal int get_active_hand()
        {
            return players[active_player].Player_Hand;
        }

        internal bool double_down()
        {
            return players[active_player].double_down_logic();
        }

        internal bool stand()
        {
            return players[active_player].stand_logic();
        }

        internal bool split()
        {
            return players[active_player].split_logic();
        }

        internal bool hit()
        {
            return players[active_player].hit_logic();
        }

        internal bool blackjack(int s)
        {
            return players[s].blackjack_logic();
        }
        internal void blackjack_win(int s)
        {
            players[s].blackjack_win();
        }
        internal void player_loss(int s)
        {
            players[s].loss();
        }

        internal void calculate_win(int dealer_hand)
        {
            for (int s = 0; s < max_players; ++s)
            {
                if(is_active(s))
                {
                    if (!players[s].Player_Blackjack)
                        players[s].calculate_win(dealer_hand);
                }
            }
        }

        // Animation
        public double[] player_coordinates()
        {
            return players[active_player].coordinates();
        }

        public double[] player_coordinates(int p)
        {
            return players[p].coordinates();
        }

        public double[] player_split_coord()
        {
            return players[active_player].split_coordinates();
        }

        public void set_coordinates(double Xsize, double Ysize)
        {
            double Xcoord = Xsize / 5;
            double card_height = Ysize / 6;
            double card_width = card_height / 1.5;
            double y = Ysize - card_height;
            double y_offset = card_height + 10;
            double x_offset = card_width / 3;

            for(int i = 0; i < 5; ++i)
            {
                    players[i].Player_Xcord = Xsize - ((i + 1) * Xcoord) + 10;
                    players[i].Player_Ycord = y;
                    players[i].Player_Yoffset = y_offset;
                    players[i].Player_Xoffset = x_offset;
            }
        }        

        internal bool set_active_player()
        {
            for(int i = active_player; i < max_players; ++i)
            {
                if (is_active(i) && !players[i].Player_Blackjack)
                {
                    active_player = i;
                    return true;
                }
                
            }

            //active_player = -1;
            return false;
        }

        internal void new_round()
        {
            for (int i = 0; i < max_players; ++i)
            {
                if (players[i].Is_Active)
                    players[i].new_round();
                else
                    players[i].Add_Button_Visibility = true;
            }
        }

        internal void deal()
        {
            for (int i = 0; i < max_players; ++i)
            {
                if (players[i].Is_Active)
                    players[i].Bet_Grid_Visibility = false;
                else
                    players[i].Add_Button_Visibility = false;
            }
        }

        internal void add_visibility()
        {
            foreach (Player p in players)
            {
                p.Add_Button_Visibility = true;
            }
        }

        internal void reset()
        {
            active_players = 0;
            active_player = 0;
            foreach (Player p in players)
            {
                p.reset();
            } 
        }
    }
}
