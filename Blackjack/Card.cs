using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Blackjack
{
    class Card
    {        
        private string filename;
        private Image card_image;
        private short c_value;
      
        public Card(short i, string f) 
        {
            c_value = i;
            filename = f;
        }

        public string Card_Filename
        {
            get { return filename; }
            set { filename = value; }
        }
         
        public Image Card_Image
        {
            get { return card_image; }
            set { card_image = value; }
        }
        
        public short Card_Value
        {
            get { return c_value; }
            set { c_value = value; }
        }

    }
}
