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
using System.Collections;

namespace Adtvatar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, Drink> drinks;
        SQLConnector connector;
        Nation[] nations;
        DispatcherTimer timer;


        public MainWindow()
        {
            InitializeComponent();
            Advatar ad = new Advatar();
            ad.Show();
        }

        ~MainWindow()
        {
            try
            {
                timer.Stop();
            }
            catch { };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            updateScores();

        }

        private void initialiseData()
        {
            nations = new Nation[4];
            nations[0] = new Nation(0, "Beer");
            nations[1] = new Nation(1, "Soda");
            nations[2] = new Nation(2, "Apfelkorn");
            nations[3] = new Nation(3, "Bacardi");

            //TO DO: Find right IDs
            drinks = new Dictionary<string, Drink>();
            drinks.Add("Bier", new Drink(3, drinkTypes.Beer)); //Bier
            drinks.Add("%Meter%", new Drink(39, drinkTypes.Beer));
            drinks.Add("%Pitcher%", new Drink(27, drinkTypes.Beer));
            drinks.Add("%pul%bier%", new Drink(5, drinkTypes.Beer)); //Pul bier
            drinks.Add("%pitcher%bier%", new Drink(5, drinkTypes.Beer)); //Pitcher

            drinks.Add("Fris", new Drink(1, drinkTypes.Soda)); //Fris

            drinks.Add("%Apfelkorn%", new Drink(20, drinkTypes.Apfelkorn));
            drinks.Add("%Bacardi%", new Drink(20, drinkTypes.Bacardi)); //Razz
        }

        void setupConnection()
        {
            //Setup Data entry (username, pass, etc.)
            connector = new SQLConnector(tbIP.Text, tbDatabase.Text, tbUsername.Text, pbPass.Password);
        }

        private void updateScores()
        {
            foreach (KeyValuePair<string, Drink> drink in drinks)
            {
                int score = connector.getConsumptionLastMinute(drink.Key);
                switch (drink.Value.DrinkType)
                {
                    case drinkTypes.Beer:
                        nations[0].Points += (score * drink.Value.Factor);
                        break;

                    case drinkTypes.Soda:
                        nations[1].Points += (score * drink.Value.Factor);
                        break;

                    case drinkTypes.Apfelkorn:
                        nations[2].Points += (score * drink.Value.Factor);
                        break;

                    case drinkTypes.Bacardi:
                        nations[3].Points += (score * drink.Value.Factor);
                        break;
                }

            }
        }

        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            btSOS.IsEnabled = true;

            initialiseData();
            setupConnection();

            timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Tick += Timer_Tick;
            //Set interval at 1 minute
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();

            Timer_Tick(null, null);
            
        }

        private void btSOS_Click(object sender, RoutedEventArgs e)
        {
            foreach (KeyValuePair<string, Drink> drink in drinks)
            {
                int score = connector.getConsumptionToday(drink.Key);
                switch (drink.Value.DrinkType)
                {
                    case drinkTypes.Beer:
                        nations[0].Points += (score * drink.Value.Factor);
                        break;

                    case drinkTypes.Soda:
                        nations[1].Points += (score * drink.Value.Factor);
                        break;

                    case drinkTypes.Apfelkorn:
                        nations[2].Points += (score * drink.Value.Factor);
                        break;

                    case drinkTypes.Bacardi:
                        nations[3].Points += (score * drink.Value.Factor);
                        break;
                }

            }
        }
    }
}
