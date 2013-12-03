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
        
        short active_hand;
        short nr_of_hands;

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
 
    }
}
