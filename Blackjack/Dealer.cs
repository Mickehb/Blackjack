﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Blackjack 
{
    public class Dealer : INotifyPropertyChanged
    {
        private int points;
        private double Xcoord;
        private double Ycoord;
        private double Xoffset;
        private string hidden;
        private Image card;
        private bool status_visibility;

        //Dealer hand
        private List<short> hand;
        private short hand_value;
        private short ace_high_value;
        private string status;
        private const short ACE_LOW = 1;    //Constants for logic
        private const short ACE_HIGH = 11;
        private const short BUST = 22;
        private bool blackjack;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;
        public Dealer()
        {
            hand = new List<short>();
            points = 0;
            Xcoord = 700;
            Ycoord = 250;
            Xoffset = 0;
            blackjack = false;
        }
      protected void OnPropertyChanged(string name)
      {
          
          PropertyChangedEventHandler handler = PropertyChanged;
          if (handler != null)
          {
              handler(this, new PropertyChangedEventArgs(name));
          }
      }
        public short Hand_Value
        {
            get { return hand_value; }
            set { hand_value = value; }
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

        public bool Status_Visibility
        {
            get { return status_visibility; }
            set
            {
                status_visibility = value;
                OnPropertyChanged("Status_Visibility");

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

        public void add_card(short s)
        {
            hand.Add(s);
        }
        
        //returns true if the dealer should take another card
        public bool logic()
        {
            bool b = blackjack;

            if (blackjack)
                return false;
            
            set_value();
            short s = ace_high_value;
            short sh = hand_value;

            if ((ace_high_value >= BUST) && (hand_value >= BUST))
            {
                Dealer_Status = "Bust";
                Status_Visibility = true;
                return false;
            }
            
            else if (hand_value == 17 || ace_high_value == 17)
            {                
                hand_value = ace_high_value;
                Dealer_Status = hand_value.ToString();
                Status_Visibility = true;
                return false;
            }

            else if (hand_value != ace_high_value)
            {
                if (ace_high_value >= BUST)
                {
                    if (hand_value < 17)
                    {
                        Dealer_Status = hand_value.ToString();
                        return true;
                    }
                    Dealer_Status = hand_value.ToString();
                    Status_Visibility = true;
                    return false;
                }
                else
                {
                    if (ace_high_value >= 17)
                    {
                        hand_value = ace_high_value;
                        Dealer_Status = hand_value.ToString();
                        Status_Visibility = true;
                        return false;
                    }
                    hand_value = ace_high_value;
                    Dealer_Status = hand_value.ToString();
                    return true;
                }
            }
            else
            {
                if (hand_value < 17)
                {
                    Dealer_Status = hand_value.ToString();
                    return true;
                }
                Dealer_Status = hand_value.ToString();
                Status_Visibility = true;
                return false;
            }

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
            blackjack = false;
        }

        public bool dealer_blackjack()
        {
            set_value();
            if (ace_high_value == 21)
            {
                Dealer_Status = "Dealer Blackjack!";
                blackjack = true;
                Status_Visibility = true;
                return true;
            }

            return false;

        }
    }
}
