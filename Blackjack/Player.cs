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

        private short active_hand;          //hand that is currently being played
        public short nr_of_hands;          //nr_of_hands the player has         

        private short ace_high_value;       //counts values using ace value of high
        private short[] hand_value;         //by default counts values using ace value of 1        
        private string hand_status;       //text for player to see, i.e bust or handvalue (12)
        private string hand_status1;       //text for player to see, i.e bust or handvalue (12)
        private string hand_status2;       //text for player to see, i.e bust or handvalue (12)
        private string hand_status3;       //text for player to see, i.e bust or handvalue (12)
        bool blackjack;                     //true if we have blackjack
        private const short ACE_LOW = 1;    //Constants for logic
        private const short ACE_HIGH = 11;
        private const short BUST = 22;
        private const short MAX_SPLITS = 3;

        private string name;
        private short money;
        private short bet;                  //show to player
        public short[] bets;               //keep track of bets made per hand

        private double Xcord;
        private double Ycord;
        private const double Xoffset = 30;
        private const double Yoffset = 110;
               
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
            hand_value = new short[4];
            bets = new short[4];
            blackjack = false;
            
            is_active = false;

            hand_visibility1 = false;
            hand_visibility2 = false;
            hand_visibility3 = false;
            hand_visibility4 = false;

            this.money = 100;
            this.name = "Player";
            this.bet = 0;
            this.Ycord = 600;

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
        
        public short Active_Hand
        {
            get { return active_hand; }
            set { active_hand = value; }
        }
        public short[] Hand_Value
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
                    Hand_Visibility1 = true;
                    break;
                case 2:
                    Hand_Status2 = status;
                    Hand_Visibility1 = true;
                    break;
                case 3:
                    Hand_Status3 = status;
                    Hand_Visibility1 = true;
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

        public bool Hand_Visibility4
        {
            get { return hand_visibility4; }
            set { 
                    hand_visibility4 = value;
                    OnPropertyChanged("Hand_Visibility4");
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
        public string Player_Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Player_Name");
            }
        }

        public short Player_Money
        {
            get { return money; }
            set
            {
                money = value;
                OnPropertyChanged("Player_Money");
            }
        }

        public short Player_Bet
        {
            get { return bet; }
            set
            {
                bet = value;
                OnPropertyChanged("Player_Bet");
            }
        }

        public short Player_Hand
        {
            get { return active_hand; }
            set { active_hand = value; }
        }

        public void set_Xcord(double x)
        {
            Xcord = x;
        }
        public void set_Ycord(double y)
        {
            Ycord = y;
        }
        public double Player_Xcord
        {
            get { return Xcord; }
            set
            {
                Xcord = value;
                OnPropertyChanged("Player_Ycord");
            }
        }

        public double Player_Ycord
        {
            get { return Xcord; }
            set
            {
                Xcord = value;
                //OnPropertyChanged("Player_Xcord");
            }
        }

        /*
         * Functions
         */
        public void add_card(Card c)
        {
            short s = active_hand;
            hand[active_hand].Add(c);
            set_value();
        }

        public void split_add_card(Card c)
        {
            short s = nr_of_hands;
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

        public bool valid_bet()
        {
            return true;
        }

        public bool update_bet(short b)
        {
            short cur_bet = bets[active_hand];
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
            short b = bets[active_hand];
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

            tmp[0] = (Xcord + (hand[active_hand].Count * Xoffset));
            tmp[1] = (Ycord - (active_hand * Yoffset));

            return tmp;
        }

        internal double[] split_coordinates()
        {
            double[] tmp = new double[2];
            tmp[0] = Xcord;
            tmp[1] = (Ycord - (nr_of_hands * Yoffset));

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
            short first_card = hand[active_hand].ElementAt(0).Card_Value;
            short second_card = hand[active_hand].ElementAt(1).Card_Value;



            if ((nr_of_hands < MAX_SPLITS) && (first_card == second_card) && split_allowed())
            {
                int i = hand[active_hand].Count - 1;
                Card tmp = hand[active_hand].Last();
                hand[active_hand + (nr_of_hands - active_hand)] = new List<Card>();
                hand[active_hand + (nr_of_hands - active_hand)].Add(tmp);
                hand[active_hand].Remove(tmp);
                //hand[active_hand].RemoveAt(i);
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

            //update_bet(bets[active_hand]);


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
            //if ((ace_high_value == 21) || (hand_value[active_hand] == 21))
            //{
            //    set_hand_status("21");
            //    if (nr_of_hands > active_hand)
            //    {
            //        active_hand++;
            //        set_value();
            //        return true;
            //    }
            //    else
            //        return false;
            //}

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
            short h = active_hand;
            short nr = nr_of_hands;
            ace_high_value = 0;
            hand_value[active_hand] = 0;
            short card_value;
            for (short i = 0; i < hand[active_hand].Count; ++i)
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
        private bool bust(short s)
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

        public void calculate_win(short dealer_hand)
        {
            short cur_hand;
            short cur_bet;
            short total_bet = bet;
            for (short s = 0; s <= nr_of_hands; ++s)
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
                    else if(dealer_hand > hand_value[s])
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
