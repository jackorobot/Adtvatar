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
            sqlConnection.Open();

            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    output += reader.GetString(0);
                }
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

        public int getConsumptionLastMinute(int drinkID)
        {
            DateTime date = new DateTime();
            date = DateTime.Today;

            String query = "SELECT SUM(Bestelling_AantalS)" + "FROM Bestelling WHERE Bestelling_Bon IN (SELECT Bon_ID FROM Bon WHERE Bon_datum = '" + date.ToLongDateString() + "' AND Bon_Time > '" + (date.Subtract(new TimeSpan(0, 1, 0))) + "') AND Bestelling_Wat =" + drinkID;

            try
            {
                return int.Parse(executeCommand(query));
            }
            catch
            {
                return 0;
            }

        }
    }
}
