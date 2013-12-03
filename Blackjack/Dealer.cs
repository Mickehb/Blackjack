using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Blackjack
{
    class Dealer
    {
        private int points;
        private double Xcoord;
        private double Ycoord;
        private double Xoffset;
        private string hidden;
        private Image card; 

        public Dealer()
        {
            points = 0;
            Xcoord = 700;
            Ycoord = 250;
            Xoffset = 0;
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
            Xoffset += 110;
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
    }
}
