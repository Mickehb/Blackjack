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
    class Dealer : INotifyPropertyChanged
    {
        private int points;
        private double Xcoord;
        private double Ycoord;
        private double Xoffset;
        private string hidden;
        private Image card;

        //Dealer hand
        List<short> hand;
        short hand_value;
        short ace_high_value;
        string status;
        private const short ACE_LOW = 1;    //Constants for logic
        private const short ACE_HIGH = 11;
        private const short BUST = 22;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;
        public Dealer()
        {
            hand = new List<short>();
            points = 0;
            Xcoord = 700;
            Ycoord = 250;
            Xoffset = 0;
        }
      protected void OnPropertyChanged(string name)
      {
          
          PropertyChangedEventHandler handler = PropertyChanged;
          if (handler != null)
          {
              handler(this, new PropertyChangedEventArgs(name));
          }
      }
           
        public string Dealer_Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Dealer_Status");
            }
        }
        public int Points
        {
            get { return points; }
            set { points = value; }
        }
        public string Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }

        public Image Hidden_Card
        {
            get { return card; }
            set { card = value; }
        }

        public double[] coordinates()
        {
            double[] tmp = new double[2];
            tmp[0] = (Xcoord + Xoffset);
            tmp[1] = Ycoord;
            Xoffset += 30;
            return tmp;
        }

        private double Dealer_Xcoord
        {
            get { return Xcoord; }
            set { Xcoord = value; }
        }
        private double Dealer_Ycoord
        {
            get { return Ycoord; }
            set { Ycoord = value; }
        }
        public void reset_Xoffset()
        {
            Xoffset = 0;
        }

        internal void set_coordinates(double Xsize)
        {
            double x = Xsize / 5;
            Xcoord = (2 * x) + 10;
            Ycoord = 10;

        }

        internal void add_card(short s)
        {
            hand.Add(s);
        }
        
        internal bool logic()
        {
            set_value();
            short s = ace_high_value;
            short sh = hand_value;

            if ((ace_high_value >= BUST) && (hand_value >= BUST))
            {
                Dealer_Status = "Bust";
                return false;
            }
            
            else if ((hand_value != ace_high_value) && ace_high_value == 17)
            {                
                return true;
            }
            else if (hand_value >= 17)
            {
                if (hand_value >= BUST)
                    Dealer_Status = "Bust";
                else
                    Dealer_Status = hand_value.ToString();
                return false;
            }
            else if(ace_high_value > 17)
            {
                if (ace_high_value >= BUST)
                    Dealer_Status = "Bust";
                else
                    Dealer_Status = ace_high_value.ToString();
                return false;
            }
            
            else if (hand_value >= BUST)
            {
                Dealer_Status = "Bust";
                return false;
            }

            return true;

        }
        internal void set_value()
        {
            
            ace_high_value = 0;
            hand_value = 0;
            short card_value;
            for (short i = 0; i < hand.Count; ++i)
            {
                card_value = hand.ElementAt(i);

                if (card_value == ACE_LOW)
                {
                    if (ace_high_value + ACE_HIGH < BUST)
                        ace_high_value += ACE_HIGH;
                    else
                        ace_high_value += card_value;
                }
                else
                    ace_high_value += card_value;

                hand_value += card_value;
            }         

        }


        internal void clear_hand()
        {
            hand.Clear();
            Dealer_Status = "";
        }
    }
}
