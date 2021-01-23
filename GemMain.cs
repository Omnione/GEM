using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace GEM_DB_FILLER
{
    public partial class GemMain : Form
    {
        public GemMain()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            fill_ah.Visible = false;
            fill_ah.Enabled = false;
        }

        private void Connect_Button_Click_1(object sender, EventArgs e)
        {
            string ip = ip_textbox.Text;
            string port = port_textbox.Text;
            string database = database_textbox.Text;
            string user = user_textbox.Text;
            string password = password_texbox.Text;

            DB.C_INFO = "server=" + ip + ";port=" + port + ";Database=" + database + ";User=" + user + ";pwd=" + password;

            DB.MySQLConnection.ConnectionString = DB.C_INFO;

            try
            {
                DB.OpenConnection();

                if (DB.MySQLConnection.State == ConnectionState.Open)
                {
                    fill_ah.Visible = true;
                    fill_ah.Enabled = true;
                    ip_textbox.Enabled = false;
                    port_textbox.Enabled = false;
                    database_textbox.Enabled = false;
                    user_textbox.Enabled = false;
                    password_texbox.Enabled = false;

                    MessageBox.Show("Connection Successful!.");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            DB.CloseConnection();
        }

        private void fill_ah_Click(object sender, EventArgs e)
        {
            int seller_id = 999999;
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            try
            {
                DB.OpenConnection();

                if (DB.MySQLConnection.State == ConnectionState.Open)
                {
                    MySqlCommand command = new MySqlCommand("", DB.MySQLConnection);
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO auction_house(itemid, stack, seller, seller_name, date, price) SELECT itemid, stackSize, " + seller_id + ", 'GEM', " + secondsSinceEpoch + ", BaseSell FROM item_basic WHERE aH > 0 AND BaseSell > 0;";

                    int result = command.ExecuteNonQuery();

                    if (result < 0)
                    {
                        MessageBox.Show("Error inserting data into Database!.");
                    }
                    else
                    {
                        MessageBox.Show(string.Format("AH filled Successfully!\n" + result + " items added."));
                    }
                }
                DB.CloseConnection();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
