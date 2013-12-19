using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Blackjack
{
    // handles all interaction between main window and code-behind classes with data and logic

    class Bj_interaction : INotifyPropertyChanged
    {
        int s_id; //save id rowguid for current game being saved or loaded
        int p_id; //player id rowguid for current player being saved or loaded
        int p_nr; //player number for which player is being saved or loaded  

        public int bets_placed;
        private Players players;
        public CardDeck deck { get; set; }               // Warning Puclic
        private Dealer dealer;
        private static Bj_interaction instance_variable;

        private int player_column;

        private string save_name;

        private bool move_visibility;
        private bool done_button_visibility;
        private bool deal_button_visibility;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        private Bj_interaction()
        {
            player_column = 0;
            move_visibility = false;
            done_button_visibility = false;
            deal_button_visibility = false;
            bets_placed = 0;
            players = new Players();
            deck = new CardDeck();
            dealer = new Dealer();
        }

        public static Bj_interaction instance()
        {
            if (instance_variable == null)
            {
                instance_variable = new Bj_interaction();
                return instance_variable;
            }

            else
                return instance_variable;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public string Save_Name
        {
            get { return save_name; }
            set
            {
                save_name = value;
                OnPropertyChanged("Save_Name");
            }
        }

        internal void yo()
        { }
        public bool Move_Visibility
        {
            get { return move_visibility; }
            set
            {
                move_visibility = value;
                OnPropertyChanged("Move_Visibility");
            }
        }
        public bool Done_Button_Visibility
        {
            get { return done_button_visibility; }
            set
            {
                done_button_visibility = value;
                OnPropertyChanged("Done_Button_Visibility");
            }
        }
        public bool Deal_Button_Visibility
        {
            get { return deal_button_visibility; }
            set
            {
                deal_button_visibility = value;
                OnPropertyChanged("Deal_Button_Visibility");
            }
        }
        public int Player_Column
        {
            get { return player_column; }
            set
            {
                player_column = value;
                OnPropertyChanged("Player_Column");
            }
        }
        public Dealer get_dealer()
        {
            return dealer;

        }
        public void new_round()
        {
            Done_Button_Visibility = false;
            deck.clear_table();
            dealer.reset_Xoffset();
            //players.reset_Xoffset();
            players.Active_Player = 0;
            bets_placed = 0;
            players.new_round();
            dealer.reset();
            dealer.Status_Visibility = false;
        }

        /*
         * Deck functions
         */
        public void deck_load()
        {
            deck.load();
        }

        public Image deck_get_card_image(int i)
        {
            return deck.get_card_image(i);
        }

        public void deck_set_card_image(int i, Image c)
        {
            deck.set_card_image(i, c);
        }

        public string deck_get_image_name(int i)
        {
            return deck.get_image_name(i);
        }

        public string deck_get_image_name()
        {
            return deck.get_image_name();
        }

        public int deck_size()
        {
            return deck.size();
        }

        public void deck_shuffle()
        {
            deck.shuffle();
        }

        public Image deck_get_next_image()
        {
            return deck.get_next_image();
        }

        public Image deck_get_active_image()
        {
            return deck.get_active_image();
        }

        public Image deck_get_onTable_image(int i)
        {
            return deck.get_onTable_image(i);
        }

        public int deck_onTable_size()
        {
            return deck.onTable_size();
        }

        public double[] deck_get_start_coordinates()
        {
            return deck.Start_coordinates();
        }

        public double[] deck_get_end_coordinates()
        {
            return deck.End_coordinates();
        }

        /*
         * Player functions
         */
        internal void player_create(int p)
        {
            players.add_player(p);
            player_valid_deal();
        }

        internal void player_remove(int p)
        {
            players.remove_player(p);
            player_valid_deal();
        }

        internal void player_update_bet(int p, int b)
        {
            players.update_player_bet(p, b);
        }

        internal void player_clear_bet(int player)
        {
            players.clear_player_bet(player);
        }

        internal Player player_get_player(int p)
        {
            return players.get_player(p);
        }

        internal Player player_get_active()
        {
            return players.get_player(players.Active_Player);
        }

        internal bool player_isactive(int p)
        {
            return players.is_active(p);
        }

        internal bool player_valid_deal()
        {
            if (players.Active_Players > 0 && (players.Active_Players == bets_placed))
            {
                Deal_Button_Visibility = true;
                return true;
            }
            else
            {
                Deal_Button_Visibility = false;
                return false;
            }
        }

        internal bool player_double_down_allowed()
        {
            return players.double_down_allowed();
        }

        internal bool player_split_allowed()
        {
            return players.split_allowed();
        }


        public bool player_change()
        {
            players.Active_Player++;
            return set_active_player();
        }

        internal bool set_active_player()
        {
            if (players.set_active_player())
            {
                Move_Visibility = true;
                Player_Column = 4 - players.Active_Player;
                return true;
            }
            Move_Visibility = false;
            return false;
        }

        internal int player_get_active_player_nr()
        {
            return players.Active_Player;
        }

        internal int player_get_active_hand()
        {
            return players.get_active_hand();
        }

        internal bool player_place_bet(int p)
        {

            if (players.get_player(p).Player_Bet > 0)
            {
                players.get_player(p).Bet_Grid_Visibility = false;
                bets_placed++;
                player_valid_deal();
                return true;
            }

            return false;
        }

        internal double[] player_coordinates()
        {
            return players.player_coordinates();
        }

        internal double[] player_coordinates(int p)
        {
            return players.player_coordinates(p);
        }

        internal void set_coordinates(double Xcoord, double Ycoord)
        {
            dealer.set_coordinates(Xcoord);
            deck.set_coordinates(Xcoord);
        }
        internal void player_set_coordinates(double Xcoord, double Ycoord)
        {
            players.set_coordinates(Xcoord, Ycoord);
        }


        internal void player_add_card()
        {
            players.add_card(deck.Active_Card);
        }

        internal void player_add_card(int s)
        {
            players.add_card(s, deck.Active_Card);
        }
        internal void player_add_split_card()
        {
            players.add_split_card(deck.Active_Card);
        }

        internal Image player_get_active_image()
        {
            return players.get_active_image();
        }

        internal bool player_double_down()
        {
            return players.double_down();
        }

        internal bool player_stand()
        {
            return players.stand();
        }

        internal bool player_hit()
        {
            return players.hit();
        }
        internal bool player_split()
        {
            return players.split();
        }

        internal double[] player_split_coordinates()
        {
            return players.player_split_coord();
        }

        /*
         * Dealer functions 
         */
        internal double[] dealer_coordinates()
        {
            return dealer.coordinates();
        }

        internal string dealer_get_hidden()
        {
            return dealer.Hidden;
        }

        internal void dealer_set_hidden(string h)
        {
            dealer.Hidden = h;
        }

        internal void dealer_set_card(Image c)
        {
            dealer.Hidden_Card = c;
        }

        internal Image dealer_get_card()
        {
            return dealer.Hidden_Card;
        }

        internal void dealer_add_card()
        {
            dealer.add_card(deck.Active_Card.Card_Value);
        }

        internal bool dealer_logic()
        {
            return dealer.logic();
        }

        internal bool blackjack_logic()
        {
            //set blackjack for players
            if (dealer.dealer_blackjack())
            {
                for (int s = 0; s < 5; ++s)
                {
                    if (players.is_active(s))
                    {
                        if (!players.blackjack(s))
                            players.player_loss(s);

                    }
                }
                return true;
            }
            else
            {
                for (int s = 0; s < 5; ++s)
                {
                    if (players.is_active(s))
                    {
                        if (players.blackjack(s))
                            players.blackjack_win(s);
                    }
                }
                return false;
            }
        }

        internal void calculate_win()
        {
            players.calculate_win(dealer.Hand_Value);
            Done_Button_Visibility = true;
        }

        internal void deal()
        {
            Deal_Button_Visibility = false;
            players.deal();
        }

        internal void new_game(double canvas_width, double canvas_height)
        {
            players.reset();
            deck.reset();
            dealer.reset();
            deck_load();
            bets_placed = 0;
            set_coordinates(canvas_width, canvas_height);
            player_set_coordinates(canvas_width, canvas_height);
            Move_Visibility = false;
            Done_Button_Visibility = false;
            players.add_visibility();
        }

        internal void reset_game()
        {
            players.reset();
            deck.reset();
            dealer.reset();
            bets_placed = 0;
            Move_Visibility = false;
            Done_Button_Visibility = false;
            players.add_visibility();
        }
        internal void save_game()
        {
            save_Saves_DB();
            save_Deck_DB();
            save_Discard_DB();
            save_Ontable_DB();
            save_Dealer_DB();
            save_Player_DB();
        }

        internal void load_game(double canvas_width, double canvas_height)
        {

            using (var db = new Blackjack_DBEntities1())
            {

                var query = from s in db.Saves_DB
                            //where s.save_name == Save_Name; WHY THE FUCK DOESN'T THIS WORK?
                            select s;
                foreach (var item in query)
                {
                    if (Save_Name == item.save_name)
                    {
                        s_id = item.id;
                        players.Active_Players = item.active_players;
                        players.Active_Player = item.active_player;
                        Deal_Button_Visibility = item.deal_visibility;
                        Done_Button_Visibility = item.done_visibility;
                        Move_Visibility = item.move_visibility;
                    }
                }
            }

            load_Deck_DB();
            load_Ontable_DB();
            load_Discard_DB();
            load_Players_DB();
            load_Dealer_DB();
            set_coordinates(canvas_width, canvas_height);
            player_set_coordinates(canvas_width, canvas_height);            
            set_visibility();

            if (Move_Visibility == true || Done_Button_Visibility == true)
                set_active_player();
        }

        internal void set_visibility()
        {
            
            for (int i = 0; i < 5; ++i)
            {
                players.get_player(i).Add_Button_Visibility = false;
                if (players.get_player(i).Is_Active && i <= players.Active_Player)
                {
                    for (int j = 0; j <= players.get_player(i).Active_Hand; ++j)
                        players.get_player(i).set_value();
                }
            }

        }

        internal void load_Dealer_DB()
        {
            int d_id = 3;
            using (var db = new Blackjack_DBEntities1())
            {
                var query = from d in db.Dealer_DB
                            where d.save_id == s_id
                            select d;

                foreach (var item in query)
                {
                    d_id = item.Id;
                    dealer.Xoffset = item.x_offset;
                    dealer.Hand_Value = item.hand_value;
                }

                var query1 = from d in db.Dealer_Hand_DB
                             where d.d_id == d_id
                             select d;

                foreach (var item in query1)
                    dealer.add_card(item.c_value);


            }
        }

        
        internal void load_Players_DB()
        {

            using (var db = new Blackjack_DBEntities1())
            {
                var query = from d in db.Players_DB
                            where d.save_id == s_id
                            select d;


                foreach (var item in query)
                {
                    players.get_player(item.player_nr).Player_Bet = item.total_bet;
                    players.get_player(item.player_nr).Player_Money = item.money;
                    players.get_player(item.player_nr).Player_Name = item.name;
                    players.get_player(item.player_nr).set_bet(0, item.bet0);
                    players.get_player(item.player_nr).set_bet(1, item.bet1);
                    players.get_player(item.player_nr).set_bet(2, item.bet2);
                    players.get_player(item.player_nr).set_bet(3, item.bet3);
                    players.get_player(item.player_nr).Active_Hand = item.active_hand;
                    players.get_player(item.player_nr).nr_of_hands = item.nr_of_hands;
                    players.get_player(item.player_nr).Is_Active = true;
                    p_nr = item.player_nr;
                    p_id = item.player_id;

                    players.get_player(item.player_nr).Money_Bet_Name_Visibility = true;

                    var query1 = from d in db.Player_hands_DB
                                 where d.player_id == p_id
                                 select d;


                    foreach (var item1 in query1)
                    {
                        foreach (Card c in deck.OnTable)
                        {
                            if (c.Image_Name == item1.image_name)
                            {
                                players.add_card_hand(p_nr, item1.hand, c);
                                break;
                            }
                        }
                    }
                }


            }
        }
        internal void load_Deck_DB()
        {
            using (var db = new Blackjack_DBEntities1())
            {
                var query = from d in db.Deck_DB
                            where d.save_id == s_id
                            select d;

                foreach (var item in query)
                {
                    Card c = new Card(item.c_value, item.fname);

                    Uri src = new Uri("pack://application:,,,/Images/Deck/" + item.fname);
                    BitmapImage img = new BitmapImage(src);

                    Image card = new Image();
                    card.Source = img;
                    card.Name = item.image_name;
                    c.Image_Name = item.image_name;
                    c.Card_Image = card;
                    deck.Deck.Add(c);
                    Canvas.SetLeft(card, item.x_coord);
                    Canvas.SetTop(card, item.y_coord);
                    Canvas.SetZIndex(card, item.z_coord);

                }
            }
        }

        internal void load_Ontable_DB()
        {
            using (var db = new Blackjack_DBEntities1())
            {
                var query = from d in db.Ontable_DB
                            where d.save_id == s_id
                            select d;

                foreach (var item in query)
                {
                    Card c = new Card((int)item.c_value, item.fname);

                    Uri src = new Uri("pack://application:,,,/Images/Deck/" + item.fname);
                    BitmapImage img = new BitmapImage(src);

                    Image card = new Image();
                    card.Source = img;
                    card.Name = item.image_name;
                    c.Image_Name = item.image_name;
                    c.Card_Image = card;
                    deck.OnTable.Add(c);
                    Canvas.SetLeft(card, item.x_coord);
                    Canvas.SetTop(card, item.y_coord);
                    Canvas.SetZIndex(card, item.z_coord);

                }
            }
        }

        internal void load_Discard_DB()
        {
            using (var db = new Blackjack_DBEntities1())
            {
                var query = from d in db.Discard_DB
                            where d.save_id == s_id
                            select d;

                foreach (var item in query)
                {
                    Card c = new Card((int)item.c_value, item.fname);

                    Uri src = new Uri("pack://application:,,,/Images/Deck/" + item.fname);
                    BitmapImage img = new BitmapImage(src);

                    Image card = new Image();
                    card.Source = img;
                    card.Name = item.image_name;
                    c.Image_Name = item.image_name;
                    c.Card_Image = card;
                    deck.Discard.Add(c);
                    Canvas.SetLeft(card, item.x_coord);
                    Canvas.SetTop(card, item.y_coord);
                    Canvas.SetZIndex(card, item.z_coord);

                }
            }
        }

        internal void save_Saves_DB()
        {
            using (var db = new Blackjack_DBEntities1())
            {
                s_id = (from s in db.Saves_DB
                            select s).Count();

                Saves_DB new_save = new Saves_DB()
                {
                    save_name = Save_Name,
                    id = s_id,
                    active_player = players.Active_Player,
                    active_players = players.Active_Players,
                    deal_visibility = Deal_Button_Visibility,
                    move_visibility = Move_Visibility,
                    done_visibility = Done_Button_Visibility
                };

                db.Saves_DB.Add(new_save);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }
            }
        }

        internal void save_Deck_DB()
        {
            int a = s_id;
            using (var db = new Blackjack_DBEntities1())
            {
                int pk = (from s in db.Deck_DB
                          select s).Count();
                Deck_DB deck_db;
                foreach (Card c in deck.Deck)
                {
                    deck_db = new Deck_DB()
                    {
                        Id = pk,
                        save_id = s_id,
                        image_name = c.Card_Image.Name,
                        c_value = c.Card_Value,
                        fname = c.Card_Filename,
                        x_coord = Canvas.GetLeft(c.Card_Image),
                        y_coord = Canvas.GetTop(c.Card_Image),
                        z_coord = Canvas.GetZIndex(c.Card_Image)
                    };
                    db.Deck_DB.Add(deck_db);
                    pk++;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }
            }
        }

        internal void save_Discard_DB()
        {            
            using (var db = new Blackjack_DBEntities1())
            {
                int pk = (from s in db.Discard_DB
                          select s).Count();
                Discard_DB discard_db;
                foreach (Card c in deck.Discard)
                {
                    discard_db = new Discard_DB()
                    {
                        Id = pk,
                        save_id = s_id,
                        image_name = c.Card_Image.Name,
                        c_value = c.Card_Value,
                        fname = c.Card_Filename,
                        x_coord = Canvas.GetLeft(c.Card_Image),
                        y_coord = Canvas.GetTop(c.Card_Image),
                        z_coord = Canvas.GetZIndex(c.Card_Image)
                    };
                    db.Discard_DB.Add(discard_db);
                    pk++;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }

            }
        }

        internal void save_Ontable_DB()
        {
            int a = s_id;
            using (var db = new Blackjack_DBEntities1())
            {
                int pk = (from s in db.Ontable_DB
                          select s).Count();
                Ontable_DB ontable_db;
                foreach (Card c in deck.OnTable)
                {
                    ontable_db = new Ontable_DB()
                    {
                        Id = pk,
                        save_id = s_id,
                        image_name = c.Card_Image.Name,
                        c_value = c.Card_Value,
                        fname = c.Card_Filename,
                        x_coord = Canvas.GetLeft(c.Card_Image),
                        y_coord = Canvas.GetTop(c.Card_Image),
                        z_coord = Canvas.GetZIndex(c.Card_Image)
                    };
                    db.Ontable_DB.Add(ontable_db);
                    pk++;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }

            }
        }

        internal void save_Dealer_DB()
        {
            using (var db = new Blackjack_DBEntities1())
            {
                int pk = (from s in db.Dealer_DB
                          select s).Count();
                Dealer_DB dealer_db = new Dealer_DB()
                {
                    Id = pk,
                    save_id = s_id,
                    hand_value = dealer.Hand_Value,
                    x_offset = dealer.Xoffset
                };
                db.Dealer_DB.Add(dealer_db);

                int fk = pk;
                pk = (from s in db.Dealer_Hand_DB
                      select s).Count();
                Dealer_Hand_DB dealer_hand_db;
                foreach (int i in dealer.Dealer_Hand)
                {
                    dealer_hand_db = new Dealer_Hand_DB()
                    {
                        Id = pk,
                        d_id = fk,
                        c_value = i
                    };
                    pk++;
                    db.Dealer_Hand_DB.Add(dealer_hand_db);
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }
            }
        }

        internal void save_Player_DB()
        {
            using (var db = new Blackjack_DBEntities1())
            {
                int pk = (from s in db.Player_hands_DB
                          select s).Count();

                Players_DB players_db;
                Player_hands_DB player_hands_db;
                string player_rowguid;

                for (int i = 0; i < 5; ++i)
                {
                    if (players.is_active(i))
                    {
                        player_rowguid = s_id.ToString() + i.ToString();
                        int p_id = Convert.ToInt32(player_rowguid);
                        players_db = new Players_DB()
                        {
                            player_id = p_id,
                            save_id = s_id,
                            name = players.get_player(i).Player_Name,
                            money = players.get_player(i).Player_Money,
                            total_bet = players.get_player(i).Player_Bet,
                            bet0 = players.get_player(i).get_bet(0),
                            bet1 = players.get_player(i).get_bet(1),
                            bet2 = players.get_player(i).get_bet(2),
                            bet3 = players.get_player(i).get_bet(3),
                            active_hand = players.get_player(i).Active_Hand,
                            nr_of_hands = players.get_player(i).nr_of_hands,
                            player_nr = i

                        };
                        db.Players_DB.Add(players_db);

                        for (int s = 0; s <= players.get_player(i).nr_of_hands; ++s)
                        {

                            foreach (Card c in players.get_player(i).get_hand(s))
                            {
                                player_hands_db = new Player_hands_DB()
                                {
                                    id = pk,
                                    player_id = p_id,
                                    hand = s,
                                    image_name = c.Card_Image.Name
                                };
                                db.Player_hands_DB.Add(player_hands_db);
                                pk++;
                            }
                        }
                    }

                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                }
                catch(DbUpdateException e)
                {
                    Console.WriteLine("Exception: ", e.Message);
                    Console.WriteLine("Inner exception: ", e.InnerException);                    
                }

            }

        }
        
    }
}
