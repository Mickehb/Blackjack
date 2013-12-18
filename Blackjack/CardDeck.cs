using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Blackjack
{
    class CardDeck : INotifyPropertyChanged
    {
        private List<Card> deck;
        private List<Card> onTable;
        private List<Card> discard;
        private Card active;
        private int Zcoord;
        private double cardStartX;
        private double cardCordY;
        private double cardEndX;
        private string errortext;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public CardDeck()
        {
            deck = new List<Card>();
            onTable = new List<Card>();
            discard = new List<Card>();
            cardStartX = 1200;
            cardCordY = 50;
            cardEndX = 400;
            Zcoord = 1;
        }
        public List<Card> Deck
        {
            get { return deck; }
            set { deck = value; }
        }
        public List<Card> OnTable
        {
            get { return onTable; }
            set { onTable = value; }
        }
        public List<Card> Discard
        {
            get { return discard; }
            set { discard = value; }
        }
        public Card Active_Card
        {
            get { return active; }
            set { active = value; }
        }
        public string Errortext
        {
            get { return errortext; }
            set
            {
                errortext = value;
                OnPropertyChanged("Errortext");
            }
        }

        public int ZIndex
        {
            get { return Zcoord; }
            set { Zcoord = value; }
        }

        public double[] Start_coordinates()
        {
            double[] tmp = new double[2];

            tmp[0] = cardStartX;
            tmp[1] = cardCordY;

            return tmp;
        }

        public double[] End_coordinates()
        {
            double[] tmp = new double[2];

            tmp[0] = cardEndX;
            tmp[1] = cardCordY;

            return tmp;
        }
        // deck
        public void load()
        {
            /*
             * Loading from database comming soonish
             */
            using (var db = new Blackjack_DBEntities1())
            {
                var query = from deck in db.New_Deck_DB
                            select deck;
                foreach (var item in query)
                {
                    Console.WriteLine(item.card_value);
                    Card c = new Card((int)item.card_value, item.fname);
                    deck.Add(c);
                }

            }


        }

        public void shuffle()
        {
            List<Card> tmpL = new List<Card>();
            Card tmpC;
            Random r = new Random();
            int i;
            Errortext = "Shuffle";
            while (deck.Count != 0)
            {
                i = r.Next(0, deck.Count);
                tmpC = deck.ElementAt(i);
                deck.RemoveAt(i);
                tmpL.Add(tmpC);
                Errortext += ", " + i.ToString();
            }

            deck = tmpL;
        }

        private void shuffle_discard()
        {
            Card tmp;
            Random r = new Random();
            int i;
            Errortext = "Shuffle discard:\n";
            while (discard.Count != 0)
            {
                i = r.Next(0, discard.Count);
                tmp = discard.ElementAt(i);
                discard.RemoveAt(i);
                deck.Add(tmp);
            }

            Errortext += "discard.count = " + discard.Count.ToString() + "\ndeck.count = " + deck.Count.ToString();
        }
        public Image get_card_image(int i)
        {
            return deck.ElementAt(i).Card_Image;
        }

        public void set_card_image(int i, Image c)
        {
            deck.ElementAt(i).Card_Image = c;
        }

        public string get_image_name(int i)
        {
            return deck.ElementAt(i).Card_Filename;
        }
        public string get_image_name()
        {
            return active.Card_Filename;
        }

        public int size()
        {
            return deck.Count;
        }

        public Image get_next_image()
        {
            if (!deck.Any())
                shuffle_discard();

            active = deck.ElementAt(0);
            deck.RemoveAt(0);
            onTable.Add(active);
            Canvas.SetZIndex(active.Card_Image, Zcoord);
            Zcoord++;
            return active.Card_Image;
        }

        public Image get_active_image()
        {
            return active.Card_Image;
        }

        // onTable
        public Image get_onTable_image(int c)
        {
            return onTable.ElementAt(c).Card_Image;
        }

        public int onTable_size()
        {
            return onTable.Count;
        }

        public void clear_table()
        {
            Card tmp;

            while (onTable.Count != 0)
            {
                tmp = onTable.ElementAt(0);
                onTable.RemoveAt(0);
                discard.Add(tmp);
            }

            Zcoord = 1;
        }

        internal void set_coordinates(double Xcoord)
        {
            double column_width = Xcoord / 5;

            cardStartX = (3 * column_width) + (column_width / 2);
            cardEndX = column_width + (column_width / 2);
            cardCordY = 10;
        }

        internal void reset()
        {
            deck.Clear();
            onTable.Clear();
            discard.Clear();
        }
    }
}
