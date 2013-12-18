using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
namespace Blackjack
{
    public class Player : INotifyPropertyChanged
    {
        public List<Card>[] hand;                  //array of 4 lists, one for each hand (max 4 splits allowed)

        private bool is_active;

        private int active_hand;          //hand that is currently being played
        public int nr_of_hands;          //nr_of_hands the player has         

        private int ace_high_value;       //counts values using ace value of high
        private int[] hand_value;         //by default counts values using ace value of 1        
        private string hand_status;       //text for player to see, i.e bust or handvalue (12)
        private string hand_status1;       //text for player to see, i.e bust or handvalue (12)
        private string hand_status2;       //text for player to see, i.e bust or handvalue (12)
        private string hand_status3;       //text for player to see, i.e bust or handvalue (12)
        bool blackjack;                     //true if we have blackjack
        private const int ACE_LOW = 1;    //Constants for logic
        private const int ACE_HIGH = 11;
        private const int BUST = 22;
        private const int MAX_SPLITS = 3;

        private string name;
        private int money;
        private int bet;                  //show to player
        public int[] bets;               //keep track of bets made per hand

        private double x_coord;
        private double y_coord;
        private double x_offset;
        private double y_offset;

        // Visibility bools
        private bool money_bet_name_visibility;
        private bool bet_grid_visibility;
        private bool add_button_visibility;

        private bool hand_visibility1;
        private bool hand_visibility2;
        private bool hand_visibility3;
        private bool hand_visibility4;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public Player()
        {
            active_hand = 0;
            nr_of_hands = 0;
            hand = new List<Card>[4];
            hand[0] = new List<Card>();
            hand_value = new int[4];
            bets = new int[] { 0, 0, 0, 0 };
            blackjack = false;
            is_active = false;

            money_bet_name_visibility = false;
            bet_grid_visibility = false;
            add_button_visibility = false;
            hand_visibility1 = false;
            hand_visibility2 = false;
            hand_visibility3 = false;
            hand_visibility4 = false;

            this.money = 100;
            this.name = "Player";
            this.bet = 0;
            this.y_coord = 600;

        }
        public List<Card> get_hand(int s)
        {
            return hand[s];
        }

        public void set_hand(List<Card> l, int s)
        {
            hand[s] = l;
        }
        public int get_bet(int s)
        {
            return bets[s];
        }

        public void set_bet(int s, int b)
        {
            bets[s] = b;
        }
        public Player(string value)
        {
            this.name = value;
            this.money = 100;
        }

        /*
         * Set and Get
         */
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public int Active_Hand
        {
            get { return active_hand; }
            set { active_hand = value; }
        }
        public int[] Hand_Value
        {
            get { return hand_value; }
            // set { hand_value = value; }
        }

        public bool Is_Active
        {
            get { return is_active; }
            set { is_active = value; }
        }

        public bool Player_Blackjack
        {
            get { return blackjack; }
            set { blackjack = value; }
        }
        public string get_player_status()
        {
            string s = "";
            switch (active_hand)
            {
                case 0:
                    s = Hand_Status;
                    break;
                case 1:
                    s = Hand_Status1;
                    break;
                case 2:
                    s = Hand_Status2;
                    break;
                case 3:
                    s = Hand_Status3;
                    break;

            }

            return s;

        }
        public void set_hand_status(string status)
        {
            switch (active_hand)
            {
                case 0:
                    Hand_Status = status;
                    Hand_Visibility1 = true;
                    break;
                case 1:
                    Hand_Status1 = status;
                    Hand_Visibility2 = true;
                    break;
                case 2:
                    Hand_Status2 = status;
                    Hand_Visibility3 = true;
                    break;
                case 3:
                    Hand_Status3 = status;
                    Hand_Visibility4 = true;
                    break;
            }
        }

        public string Hand_Status
        {
            get { return hand_status; }
            set
            {
                hand_status = value;
                OnPropertyChanged("Hand_Status");
            }
        }
        public string Hand_Status1
        {
            get { return hand_status1; }
            set
            {
                hand_status1 = value;
                OnPropertyChanged("Hand_Status1");
            }
        }
        public string Hand_Status2
        {
            get { return hand_status2; }
            set
            {
                hand_status2 = value;
                OnPropertyChanged("Hand_Status2");
            }
        }
        public string Hand_Status3
        {
            get { return hand_status3; }
            set
            {
                hand_status3 = value;
                OnPropertyChanged("Hand_Status3");
            }
        }

        public bool Hand_Visibility1
        {
            get { return hand_visibility1; }
            set
            {
                hand_visibility1 = value;
                OnPropertyChanged("Hand_Visibility1");
            }
        }
        public bool Hand_Visibility2
        {
            get { return hand_visibility2; }
            set
            {
                hand_visibility2 = value;
                OnPropertyChanged("Hand_Visibility2");
            }
        }
        public bool Hand_Visibility3
        {
            get { return hand_visibility3; }
            set
            {
                hand_visibility3 = value;
                OnPropertyChanged("Hand_Visibility3");
            }
        }
        public bool Hand_Visibility4
        {
            get { return hand_visibility4; }
            set
            {
                hand_visibility4 = value;
                OnPropertyChanged("Hand_Visibility4");
            }
        }
        public bool Money_Bet_Name_Visibility
        {
            get { return money_bet_name_visibility; }
            set
            {
                money_bet_name_visibility = value;
                OnPropertyChanged("Money_Bet_Name_Visibility");
            }
        }

        public bool Bet_Grid_Visibility
        {
            get { return bet_grid_visibility; }
            set
            {
                bet_grid_visibility = value;
                OnPropertyChanged("Bet_Grid_Visibility");
            }
        }

        public bool Add_Button_Visibility
        {
            get { return add_button_visibility; }
            set
            {
                add_button_visibility = value;
                OnPropertyChanged("Add_Button_Visibility");
            }
        }

        public string Player_Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Player_Name");
            }
        }

        public int Player_Money
        {
            get { return money; }
            set
            {
                money = value;
                OnPropertyChanged("Player_Money");
            }
        }

        public int Player_Bet
        {
            get { return bet; }
            set
            {
                bet = value;
                OnPropertyChanged("Player_Bet");
            }
        }

        public int Player_Hand
        {
            get { return active_hand; }
            set { active_hand = value; }
        }

        public void set_Xcord(double x)
        {
            x_coord = x;
        }
        public void set_Ycord(double y)
        {
            y_coord = y;
        }
        public double Player_Xcord
        {
            get { return x_coord; }
            set
            {
                x_coord = value;
                OnPropertyChanged("Player_Ycord");
            }
        }

        public double Player_Ycord
        {
            get { return y_coord; }
            set
            {
                y_coord = value;
                //OnPropertyChanged("Player_Xcord");
            }
        }
        public double Player_Yoffset
        {
            get { return y_offset; }
            set { y_offset = value; }
        }
        public double Player_Xoffset
        {
            get { return x_offset; }
            set { x_offset = value; }
        }
        /*
         * Functions
         */
        public void add_card(Card c)
        {
            hand[active_hand].Add(c);
            set_value();
        }

        public void add_card(int h, Card c)
        {
            hand[h].Add(c);
        }

        public void split_add_card(Card c)
        {
            int s = nr_of_hands;
            hand[active_hand + (nr_of_hands - active_hand)].Add(c);
        }

        public void clear_hands()
        {
            for (int i = 0; i < 4; ++i)
            {
                if (hand[i] != null)
                {
                    hand[i].Clear();
                    active_hand = 0;
                    nr_of_hands = 0;
                    Hand_Status = "";
                    Hand_Status1 = "";
                    Hand_Status2 = "";
                    Hand_Status3 = "";
                    Player_Blackjack = false;
                    clear_bet();
                }
            }
        }

        public void hide_hand_values()
        {
            Hand_Visibility1 = false;
            Hand_Visibility2 = false;
            Hand_Visibility3 = false;
            Hand_Visibility4 = false;
        }

        public void activate()
        {
            Is_Active = true;
            Money_Bet_Name_Visibility = true;
            Add_Button_Visibility = false;
            Bet_Grid_Visibility = true;
        }

        public void reset()
        {
            Is_Active = false;
            Money_Bet_Name_Visibility = false;
            Add_Button_Visibility = true;
            Bet_Grid_Visibility = false;
            clear_hands();
            hide_hand_values();
            Player_Money = 100;
            Player_Bet = 0;
            Player_Name = "Player";
        }

        public void new_round()
        {
            clear_hands();
            hide_hand_values();
            Bet_Grid_Visibility = true;
        }

        public bool valid_bet()
        {
            return true;
        }

        public bool update_bet(int b)
        {
            int cur_bet = bets[active_hand];
            if (money >= b)
            {
                Player_Money -= b;
                Player_Bet += b;
                bets[active_hand] += b;
                cur_bet = bets[active_hand];
                return true;
            }
            return false;
        }
        public bool double_down_allowed()
        {
            if (hand[active_hand].Count == 2)
                return update_bet(bets[active_hand]);

            return false;
        }
        public bool split_allowed()
        {
            nr_of_hands++;
            int b = bets[active_hand];
            if (money >= b)
            {
                Player_Money -= b;
                Player_Bet += b;
                bets[active_hand + (nr_of_hands - active_hand)] += b;
                return true;
            }
            return false;
        }


        public void clear_bet()
        {
            Player_Money += bet;
            Player_Bet = 0;
            for (int i = 0; i < 4; ++i)
            {
                bets[i] = 0;
            }
        }

        public double[] coordinates()
        {
            double[] tmp = new double[2];

            tmp[0] = (x_coord + (hand[active_hand].Count * x_offset));
            tmp[1] = (y_coord - (active_hand * y_offset));

            return tmp;
        }

        internal double[] split_coordinates()
        {
            double[] tmp = new double[2];
            tmp[0] = x_coord;
            tmp[1] = (y_coord - (nr_of_hands * y_offset));

            return tmp;
        }
        internal Image get_active_image()
        {
            return hand[active_hand].Last().Card_Image;
        }
        // Create the OnPropertyChanged method to raise the event 


        public bool split_logic()
        {
            //if split allowed
            int first_card = hand[active_hand].ElementAt(0).Card_Value;
            int second_card = hand[active_hand].ElementAt(1).Card_Value;



            if ((nr_of_hands < MAX_SPLITS) && (first_card == second_card) && split_allowed())
            {
                int i = hand[active_hand].Count - 1;
                Card tmp = hand[active_hand].Last();
                hand[active_hand + (nr_of_hands - active_hand)] = new List<Card>();
                hand[active_hand + (nr_of_hands - active_hand)].Add(tmp);
                hand[active_hand].Remove(tmp);

                set_value();

                return true;
            }
            else
                return false;


        }

        /*
         * returns true if we have another hand to play
         */
        public bool double_down_logic()
        {
            set_value();
            //save our highest handvalue and set status
            if ((ace_high_value >= BUST) && (hand_value[active_hand] >= BUST))
            {
                set_hand_status("Bust");
            }
            else if (ace_high_value < BUST)
            {
                if (ace_high_value == hand_value[active_hand])
                    set_hand_status(hand_value[active_hand].ToString());
                else
                {
                    hand_value[active_hand] = ace_high_value;
                    set_hand_status(hand_value[active_hand].ToString());
                }
            }
            else
            {
                set_hand_status(hand_value[active_hand].ToString());

            }

            //check if we have another hand to play
            if (active_hand < nr_of_hands)
            {
                active_hand++;
                set_value();
                return true;
            }
            else
            {
                active_hand = 0;
                return false;
            }
        }

        /*
         * returns true if we have another hand to play
         */
        internal bool stand_logic()
        {
            set_value();

            string str = get_player_status();

            //save our highest handvalue
            if ((ace_high_value < BUST) && (ace_high_value != hand_value[active_hand]))
            {
                hand_value[active_hand] = ace_high_value;
                set_hand_status(ace_high_value.ToString());

            }
            else
                set_hand_status(hand_value[active_hand].ToString());

            //check if we have another hand to play
            if (active_hand < nr_of_hands)
            {
                active_hand++;
                set_value();
                return true;
            }
            else
            {
                active_hand = 0;
                return false;
            }
       }
        /*
         * returns true if we are to play again
         */
        public bool hit_logic()
        {

            set_value();

            if ((ace_high_value >= BUST) && (hand_value[active_hand] >= BUST))
            {
                set_hand_status("Bust");
                if (nr_of_hands > active_hand)
                {
                    active_hand++;
                    set_value();
                    return true;
                }
                else
                    return false;
            }
            else if (ace_high_value < BUST)
            {
                if (ace_high_value == hand_value[active_hand])
                    set_hand_status(hand_value[active_hand].ToString());
                else
                    set_hand_status(hand_value[active_hand].ToString() + "/" + ace_high_value);

                return true;
            }
            else
            {
                set_hand_status(hand_value[active_hand].ToString());
                return true;
            }

            return false;

        }

        public bool blackjack_logic()
        {
            set_value();
            if (ace_high_value == 21)
            {
                Player_Blackjack = true;
                set_hand_status("Blackjack!");
                bets[0] = 0;
                return true;
            }

            return false;

        }
        internal void set_value()
        {
            int h = active_hand;
            int nr = nr_of_hands;
            ace_high_value = 0;
            hand_value[active_hand] = 0;
            int card_value;
            for (int i = 0; i < hand[active_hand].Count; ++i)
            {
                card_value = hand[active_hand].ElementAt(i).Card_Value;

                if (card_value == ACE_LOW)
                {
                    if (ace_high_value + ACE_HIGH < BUST)
                        ace_high_value += ACE_HIGH;
                    else
                        ace_high_value += card_value;
                }
                else
                    ace_high_value += card_value;

                hand_value[active_hand] += card_value;
            }

            //if both values are equal, only print one
            if (ace_high_value == hand_value[active_hand])
                set_hand_status(hand_value[active_hand].ToString());
            else
                set_hand_status(hand_value[active_hand].ToString() + "/" + ace_high_value);

        }
        private bool bust(int s)
        {
            if (hand_value[active_hand] + s > BUST)
                return true;

            return false;
        }

        public void loss()
        {
            Player_Bet -= bets[0];
            bets[0] = 0;
        }
        public void blackjack_win()
        {
            Player_Bet *= 2;
            bets[0] = 0;
        }

        public void calculate_win(int dealer_hand)
        {
            int cur_hand;
            int cur_bet;
            int total_bet = bet;
            for (int s = 0; s <= nr_of_hands; ++s)
            {
                cur_hand = hand_value[s];
                cur_bet = bets[s];

                if (dealer_hand == 21)
                {
                    Player_Bet -= bets[s];
                    total_bet = Player_Bet;
                    bets[s] = 0;
                }

                else if (hand_value[s] > 21)
                {
                    Player_Bet -= bets[s];
                    total_bet = Player_Bet;
                    bets[s] = 0;
                }
                else
                {
                    if (dealer_hand > 21 || dealer_hand < hand_value[s])
                    {
                        Player_Bet += bets[s];
                        total_bet = Player_Bet;

                        bets[s] = 0;
                    }
                    else if (dealer_hand > hand_value[s])
                    {
                        Player_Bet -= bets[s];
                        total_bet = Player_Bet;
                        bets[s] = 0;
                    }
                    else
                    {
                        bets[s] = 0;
                    }
                }
            }
        }
    }
}
