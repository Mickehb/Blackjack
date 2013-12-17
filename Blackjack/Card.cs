using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Blackjack
{
    public class Card
    {        
        private string filename;
        private Image card_image;
        private int c_value;
        private string image_name;
        public Card(int i, string f) 
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
        
        public string Image_Name
        {
            get { return image_name; }
            set { image_name = value; }
        }
        public int Card_Value
        {
            get { return c_value; }
            set { c_value = value; }
        }

    }
}
