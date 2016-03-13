using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Net;

namespace Adtvatar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, drinkTypes> drinks;
        SQLConnector connector;
        Nation[] nations;
        DispatcherTimer timer;
        Nation Attacker;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        ~MainWindow()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            updateScores();

            Random rnd = new Random();
            if(rnd.Next(0, 10) == 7)
            {
                //TO DO
                animationUpdateAttacker();

                //Rotate throug all nations
                if (Attacker.ID == 3)
                    Attacker = nations[0];
                else
                    Attacker = nations[Attacker.ID++];
            }

        }

        void animationUpdateAttacker()
        {
            //TO DO
        }

        private void initialiseData()
        {
            nations = new Nation[4];
            nations[0] = new Nation(0, "Beer");
            nations[1] = new Nation(1, "Soda");
            nations[2] = new Nation(2, "Apfelkorn");
            nations[3] = new Nation(3, "Bacardi");

            //TO DO: Find right IDs
            drinks = new Dictionary<int, drinkTypes>();
            drinks.Add(1, drinkTypes.Beer); //Bier
            drinks.Add(2, drinkTypes.Beer); //Meter
            drinks.Add(3, drinkTypes.Beer); //Pull bier
            drinks.Add(4, drinkTypes.Beer); //Pull bier korting
            drinks.Add(5, drinkTypes.Beer); //Pitcher
        
            drinks.Add(6, drinkTypes.Soda); //Fris

            drinks.Add(7, drinkTypes.Apfelkorn);
            drinks.Add(8, drinkTypes.Bacardi); //Razz
            drinks.Add(9, drinkTypes.Bacardi); //Original
            drinks.Add(10, drinkTypes.Bacardi); //Lemon

            Attacker = nations[0];
        }

        void setupConnection()
        {
            //Setup Data entry (username, pass, etc.)
            connector = new SQLConnector(tbIP.Text, tbDatabase.Text, tbUsername.Text, pbPass.Password);
        }

        private void updateScores()
        {
            foreach(KeyValuePair<int, drinkTypes> drink in drinks)
            {
                if (drink.Value.ToString() == Attacker.Name)
                {
                    int score = connector.getConsumptionLastMinute(drink.Key);
                    switch(Attacker.ID)
                    {
                        case 0:
                            nations[0].Points += score;
                            break;

                        case 1:
                            nations[1].Points += score;
                            break;

                        case 2:
                            nations[2].Points += score;
                            break;

                        case 3:
                            nations[3].Points += score;
                            break;
                    }
                }

                #region Redundant
                //CODE FOR ALWAYS UPDATING SCORES
                //switch (drink.Value)
                //{
                //    case drinkTypes.Beer:
                //        nations[0].Points += score;
                //        break;

                //    case drinkTypes.Soda:
                //        nations[1].Points += score;
                //        break;

                //    case drinkTypes.Apfelkorn:
                //        nations[2].Points += score;
                //        break;

                //    case drinkTypes.Bacardi:
                //        nations[3].Points += score;
                //        break;
                //}
                #endregion

            }
        }

        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            initialiseData();
            setupConnection();

            timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Tick += Timer_Tick;
            //Set interval at 1 minute
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Start();

            Timer_Tick(null, null);
        }
    }
}
