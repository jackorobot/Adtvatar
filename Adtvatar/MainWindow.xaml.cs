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
using System.IO;

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
            nations[0] = new Nation(0, "Bier");
            nations[1] = new Nation(1, "Fris");
            nations[2] = new Nation(2, "Apfelkorn");
            nations[3] = new Nation(3, "Bacardi");

            //TO DO: Find right IDs
            drinks = new Dictionary<string, Drink>();
            drinks.Add("Bier", new Drink(3, drinkTypes.Beer)); //Bier
            drinks.Add("%Meter%", new Drink(36, drinkTypes.Beer));
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

            saveToText();
        }

        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            initialiseData();
            setupConnection();

            if (!connector.getConsumptionToday("Bier").ToString().ToLower().Contains("error"))
            {
                timer = new DispatcherTimer(DispatcherPriority.Send);
                timer.Tick += Timer_Tick;
                //Set interval at 1 minute
                timer.Interval = new TimeSpan(0, 0, 30);
                timer.Start();

                btSOS.IsEnabled = true;

                foreach (KeyValuePair<string, Drink> drink in drinks)
                {
                    cbConsumptie.Items.Add(drink.Key);
                }

                btOpslaan.IsEnabled = true;
                cbConsumptie.IsEnabled = true;
                tbFactor.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Error making a connection");
            }
            
        }

        private void btSOS_Click(object sender, RoutedEventArgs e)
        {
            if (connector.isOpen())
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

        private void saveToText(){
            string fileName = @"D:\Users\Jesse\Google Drive\adtvatar\drinks.csv";
            var csv = new StringBuilder();

            foreach(Nation nation in nations){
                var id = nation.ID;
                var name = nation.Name;
                var points = nation.Points;
                var newLine = string.Format("{0},{1},{2};", id, name, points);
                csv.AppendLine(newLine);
            }

            try
            {
                File.WriteAllText(fileName, csv.ToString());
            }
            catch { MessageBox.Show("Error writing csv file"); }
        }

        private void cbConsumptie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tbFactor.Text = drinks[cbConsumptie.SelectedItem.ToString()].Factor.ToString();
            }
            catch { }
        }

        private void btOpslaan_Click(object sender, RoutedEventArgs e)
        {
            drinks[cbConsumptie.Text].Factor = Convert.ToInt32(tbFactor.Text);
        }
    }
}
