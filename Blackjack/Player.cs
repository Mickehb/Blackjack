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
        List<Card>[] hand;
        
        private short active_hand;
        private short nr_of_hands;
        private Card active_card;

        private short[] value;
        private string[] status;
        private const short ACE_LOW = 1;
        private const short ACE_HIGH = 11;
        private const short BUST = 21;

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
          value = new short[4];
          status = new string[4];
          

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
          get { return status[active_hand]; }
          set
          {
              status[active_hand] = value;
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
          hand[active_hand].Add(c);                
      }

      public void split_add_card(Card c)
      {
          hand[active_hand+1].Add(c);
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

          tmp[0] = (Xcord + (hand[active_hand].Count() * Xoffset));
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
            

      internal void split_logic()
      {
          int i = hand[active_hand].Count -1;
          Card tmp = hand[active_hand].Last();
          hand[active_hand + 1] = new List<Card>();
          hand[active_hand + 1].Add(tmp);
          //hand[active_hand].Remove(tmp);
          hand[active_hand].RemoveAt(i);
          
          nr_of_hands++;

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
          short si = value[active_hand];
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
          short s = value[active_hand];
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
                  value[active_hand] += ACE_HIGH;
                  Player_Status = value[active_hand].ToString();
              }
              else
              {
                  value[active_hand] += ACE_LOW;
                  Player_Status = value[active_hand].ToString();
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
                  value[active_hand] += active_card.Card_Value;
                  Player_Status = value.ToString();
              }

              return true;
          }

      }

      internal void set_value()
      {
          value[active_hand] = 0;
          short s = active_hand;
          for (short i = 0; i < hand.Count() -2 ; ++i)
          {
              value[active_hand] += hand[active_hand].ElementAt(i).Card_Value;
          }

          Player_Status = value[active_hand].ToString();
          
      }
      private bool bust(short s)
      {
          if (value[active_hand] + s > BUST)
              return true;

          return false;
      }
 
    }
}
