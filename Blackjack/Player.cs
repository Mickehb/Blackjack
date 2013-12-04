using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
namespace Blackjack
{
    class Player : INotifyPropertyChanged
    {
        List<Card>[] hand;                  //array of 4 lists, one for each hand (max 4 splits allowed)
        
        private short active_hand;          //hand that is currently being played
        private short nr_of_hands;          //nr_of_hands the player has 
        private Card active_card;

        private short ace_high_value;       //counts values using ace value of high
        private short[] hand_value;         //by default counts values using ace value of 1        
        private string[] hand_status;       //text for player to see, i.e bust or handvalue (12)
        private const short ACE_LOW = 1;    //Constants for logic
        private const short ACE_HIGH = 11;
        private const short BUST = 21;
        private const short MAX_SPLITS = 3;

        private string name;
        private short money;
        private short bet;

        private double Xcord;
        private double Ycord;
        private const double Xoffset = 30;
        private const double Yoffset = 110;

        // Declare the event 
      public event PropertyChangedEventHandler PropertyChanged;

      public Player()
      {
          active_hand = 0;
          nr_of_hands = 0;
          hand = new List<Card>[4];
          hand[0] = new List<Card>();
          hand_value = new short[4];
          hand_status = new string[4];
          

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

      public string Player_Status
      {
          get { return hand_status[active_hand]; }
          set
          {
              hand_status[active_hand] = value;
              OnPropertyChanged("Player_Status");
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
          set { 
              Xcord = value;
              OnPropertyChanged("Player_Ycord");
          }
      }

      public double Player_Ycord
      {
          get { return Xcord; }
          set { 
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
      }

      public void split_add_card(Card c)
      {
          short s = nr_of_hands;
          hand[active_hand+nr_of_hands].Add(c);
      }

      public void clear_hands()
      {
          for(int i = 0; i < 4; ++i)
          {
              if (hand[i] != null)
              {
                  hand[i].Clear();
                  active_hand = 0;
                  nr_of_hands = 0;
              }
          }
      }

      public bool valid_bet()
      {
          return true;
      }

      public void update_bet(short b)
      {
          if (money >= b)
          {
              Player_Money -= b;
              Player_Bet += b;
          }
      }

      public void clear_bet()
      {
          Player_Money += bet;
          Player_Bet = 0;
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
      protected void OnPropertyChanged(string name)
      {
          PropertyChangedEventHandler handler = PropertyChanged;
          if (handler != null)
          {
              handler(this, new PropertyChangedEventArgs(name));
          }
      }
            

      internal bool split_logic()
      {
          //if split allowed
          short first_card = hand[active_hand].ElementAt(0).Card_Value;
          short second_card = hand[active_hand].ElementAt(1).Card_Value;
          

          if ((nr_of_hands < MAX_SPLITS) && first_card == second_card)
          {
              nr_of_hands++;
              int i = hand[active_hand].Count - 1;
              Card tmp = hand[active_hand].Last();
              hand[active_hand + nr_of_hands] = new List<Card>();
              hand[active_hand + nr_of_hands].Add(tmp);
              hand[active_hand].Remove(tmp);
              //hand[active_hand].RemoveAt(i);
              
              return true;
          }
          else
              return false;
          

      }

        /*
         * returns true if we have another hand to play
         */
      internal bool double_down_logic()
      {
          if (active_hand < nr_of_hands)
          {
              active_hand++;
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
          short s = active_hand;
          short si = hand_value[active_hand];
          string str = Player_Status;

          if (active_hand < nr_of_hands)
          {
              active_hand++;
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
          short s = hand_value[active_hand];
          short si = active_hand;
          string str = Player_Status;

          active_card = hand[active_hand].Last();
          if (active_card.Card_Value == ACE_LOW)
          {
              if (bust(ACE_LOW))
              {
                  Player_Status = "Bust\n!";
                  if (nr_of_hands > active_hand)
                  {
                      active_hand++;                      
                  }
                  else
                      return false;
              }

              else if (!bust(ACE_HIGH))
              {
                  hand_value[active_hand] += ACE_HIGH;
                  Player_Status = hand_value[active_hand].ToString();
              }
              else
              {
                  hand_value[active_hand] += ACE_LOW;
                  Player_Status = hand_value[active_hand].ToString();
              }
              return true;
          }
          else
          {
              if (bust(active_card.Card_Value))
              {
                  Player_Status = "Bust\n!";
                  if (nr_of_hands > active_hand)
                  {
                      active_hand++;               
                  }
                  else
                      return false;
              }
              else
              {
                  hand_value[active_hand] += active_card.Card_Value;
                  Player_Status = hand_value.ToString();
              }

              return true;
          }

      }

      internal void set_value()
      {
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

          Player_Status = hand_value[active_hand].ToString() + "/" + ace_high_value;
          
      }
      private bool bust(short s)
      {
          if (hand_value[active_hand] + s > BUST)
              return true;

          return false;
      }
 
    }
}
