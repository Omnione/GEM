using MySql.Data.MySqlClient;

namespace GEM_DB_FILLER
{
    public class DB
    {
        private static MySqlConnection connection = new MySqlConnection();
        private static string connection_info = "";

        public static MySqlConnection MySQLConnection
        {
            get { return connection; }
            set { connection = value; }

        }
        public static string C_INFO
        {
            get { return connection_info; }
            set { connection_info = value; }

        }

        public static void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public static void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

    }
}
