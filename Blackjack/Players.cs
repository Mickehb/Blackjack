﻿using System;
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
        private short active_player;
        private short active_players;
        private short max_players;
        public Players()
        {
            players = new Player[] { new Player(), new Player(), new Player(), new Player(), new Player() };
            active_player = 0;
            active_players = 0;
            max_players = 5;
        }

        public short Active_Player
        {
            get { return active_player; }
            set { active_player = value; }
        }

        
        public short Active_Players
        {
            get { return active_players; }
            set { active_players = value; }
        }

        public void add_player(short p)
        {
            //Player player = new Player();
            //players[p] = player;            
            player_add_window addwindow = new player_add_window(p);
            players[p].Is_Active = true;
            addwindow.ShowDialog();
            active_players++;
        }

        public void remove_player(short p)
        {
            players[p].Is_Active = false;
            active_players--;
        }

        public Player get_player(short p)
        {
           return players[p];
        }

        public void update_player_bet(short p, short b)
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
        public void clear_player_bet(short p)
        {
            players[p].clear_bet();
        }

        public bool is_active(short p)
        {
            return players[p].Is_Active;
        }

        public void add_card(Card c)
        {
            if (players[active_player] != null)
                players[active_player].add_card(c);
        }

        public void add_card(short s, Card c)
        {
            if (players[s] != null)
                players[s].add_card(c); 
        }

        public void add_split_card(Card c)
        {
            players[active_player].split_add_card(c);
        }

        public void clear_hands()
        {
            for(short i = 0; i < max_players; ++i)
            {
                if (is_active(i))
                    players[i].clear_hands();                    
                
            }
        }

        internal Image get_active_image()
        {
            return players[active_player].get_active_image();
        }

        internal short get_active_hand()
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

        internal bool blackjack(short s)
        {
            return players[s].blackjack_logic();
        }
        internal void blackjack_win(short s)
        {
            players[s].blackjack_win();
        }
        internal void player_loss(short s)
        {
            players[s].loss();
        }

        internal void calculate_win(short dealer_hand)
        {
            for (short s = 0; s < max_players; ++s)
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

        public double[] player_coordinates(short p)
        {
            return players[p].coordinates();
        }

        public double[] player_split_coord()
        {
            return players[active_player].split_coordinates();
        }
        public void set_coordinates(double Xsize, double Ysize)
        {
            Player p;
            string s;
            double x, y;
            double Xcoord = Xsize / 5;                  
            for(short i = 0; i < 5; ++i)
            {
                if (is_active(i))
                {
                    p = players[i];
                    s = p.Player_Name;
                    x = Xsize - ((i + 1) * Xcoord);
                    y = Ysize - 385;
                    players[i].set_Xcord(x);
                    players[i].set_Ycord(y);
                    
                        

                }
            }

        }

        public void reset_Xoffset()
        {
            for (int i = 0; i < max_players; i++)
            {
                //if (players.ElementAt(i) != null)
                    //players.ElementAt(i).Player_Offset = 0;
            }
        }
        

        internal bool set_active_player()
        {
            for(short i = active_player; i < max_players; ++i)
            {
                if (is_active(i) && !players[i].Player_Blackjack)
                {
                    active_player = i;
                    return true;
                }
                
            }

            active_player = -1;
            return false;
        }

        internal void hide_hand_values()
        {
            for (short i = 0; i < max_players; ++i)
            {
                if(is_active(i))
                    players[i].hide_hand_values();
            }
        }
    }
}
