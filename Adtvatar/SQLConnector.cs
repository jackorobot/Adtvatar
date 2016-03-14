using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using System.Net;
using MySql.Data.MySqlClient;

namespace Adtvatar
{
    class SQLConnector
    {
        private MySqlConnection sqlConnection;
        private string ConnectionIP;
        private string ConnectionFileName;
        private string ConnectionUsername;
        private string ConnectionPassword;

        public SQLConnector(string connectionIP, string connectionFileName, string connectionUsername, string connectionPassword)
        {
            ConnectionIP = connectionIP;
            ConnectionFileName = connectionFileName;
            ConnectionUsername = connectionUsername;
            ConnectionPassword = connectionPassword;

            initialise();
        }

        private void initialise()
        {
            MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
            connectionString.Server = ConnectionIP;
            connectionString.UserID = ConnectionUsername;
            connectionString.Password = ConnectionPassword;
            connectionString.Database = ConnectionFileName;

            sqlConnection = new MySqlConnection(connectionString.ToString());
        }
        private string executeCommand(string query)
        {
            string output = null;
            MySqlCommand command = new MySqlCommand(query, sqlConnection);
            try
            {
                sqlConnection.Open();
            }
            catch
            {
                System.Windows.MessageBox.Show("Failed to make a connection to the database. \n Please check the login credentials.");
            }

            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    output += reader.GetString(0);
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("An problem occurred during the database update \n" + ex.Message);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();
            }
            return output;
        }

        public string connectionIP
        {
            get
            {
                return ConnectionIP;
            }

            set
            {
                ConnectionIP = value;
            }
        }

        public string connectionUsername
        {
            get
            {
                return ConnectionUsername;
            }

            set
            {
                ConnectionUsername = value;
            }
        }

        public string connectionPassword
        {
            get
            {
                return ConnectionPassword;
            }

            set
            {
                ConnectionPassword = value;
            }
        }

        public string connectionFileName
        {
            get
            {
                return ConnectionFileName;
            }

            set
            {
                ConnectionFileName = value;
            }
        }

        public int getConsumptionLastMinute(string drinkName)
        {
            DateTime date = new DateTime();
            date = DateTime.Today;

            string queryUnits = "SELECT SUM(Bestelling_AantalS)" + @"FROM Bestelling 
                    WHERE Bestelling_Bon IN (SELECT Bon_ID FROM Bon WHERE Bon_datum = '" + date.ToLongDateString() + @"'
                    AND Bon_Time > '" + (date.Subtract(new TimeSpan(0, 0, 30))) + "') AND Bestelling_Wat IN (SELECT Prijs_ID FROM barkasread.prijs WHERE Prijs_Naam LIKE '"+ drinkName + "'))";

            string queryFlessen = "SELECT SUM(Bestelling_AantalS50)" + @"FROM Bestelling 
                    WHERE Bestelling_Bon IN (SELECT Bon_ID FROM Bon WHERE Bon_datum = '" + date.ToLongDateString() + @"'
                    AND Bon_Time > '" + (date.Subtract(new TimeSpan(0, 0, 30))) + "') AND Bestelling_Wat IN (SELECT Prijs_ID FROM barkasread.prijs WHERE Prijs_Naam LIKE '" + drinkName + "'))";

            try
            {
                int score = int.Parse(executeCommand(queryUnits)) + int.Parse(executeCommand(queryFlessen)) * 20;
                return score;
            }
            catch
            {
                return 0;
            }

        }

        public int getConsumptionToday(string drinkName)
        {
            DateTime date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 20, 10, 0);

            string queryUnits = "SELECT SUM(Bestelling_AantalS)" + @"FROM Bestelling 
                    WHERE Bestelling_Bon IN (SELECT Bon_ID FROM Bon WHERE Bon_datum = '" + date.ToLongDateString() + @"'
                    AND Bon_Time > '" + (date.Subtract(new TimeSpan(0, 0, 30))) + "') AND Bestelling_Wat IN (SELECT Prijs_ID FROM barkasread.prijs WHERE Prijs_Naam LIKE '" + drinkName + "'))";

            string queryFlessen = "SELECT SUM(Bestelling_AantalS50)" + @"FROM Bestelling 
                    WHERE Bestelling_Bon IN (SELECT Bon_ID FROM Bon WHERE Bon_datum = '" + date.ToLongDateString() + @"'
                    AND Bon_Time > '" + (date.Subtract(new TimeSpan(0, 0, 30))) + "') AND Bestelling_Wat IN (SELECT Prijs_ID FROM barkasread.prijs WHERE Prijs_Naam LIKE '" + drinkName + "'))";

            try
            {
                int score = int.Parse(executeCommand(queryUnits)) + int.Parse(executeCommand(queryFlessen)) * 20;
                return score;
            }
            catch
            {
                return 0;
            }
        }
    }
}
