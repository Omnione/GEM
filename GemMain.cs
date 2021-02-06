/*
 * --------------------------------------------------------------------------------------------
 * Original code by MrSent of Topaz-next comunity
 * please do not sell this code or distribute as your own,
 * any and all modifications are at your own risk and i take no responsibilty for any damages.
 * use at your own risk.
 * --------------------------------------------------------------------------------------------
*/

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace GEM_DB_FILLER
{
    public partial class GemMain : Form
    {
        // Quantity of items to list on the AH.
        public int qty = 10;

        public GemMain()
        {
            InitializeComponent();
            Init();
            
        }

        private void Init()
        {
            // Hide and disable the fill AH button.
            fill_ah.Visible = false;
            fill_ah.Enabled = false;
        }

        // connection button function
        // this will test the connection, if sucsessful, will display connected,
        // else will display error message.
        // Also un hides the fill AH button.
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
            RunSingles();
        }

        private void RunSingles()
        {
            int count = 0;
            int res = 0;

            try
            {
                DB.OpenConnection();

                if (DB.MySQLConnection.State == ConnectionState.Open)
                {
                    string seller_id = "999999";
                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    string secondsSinceEpoch = t.TotalSeconds.ToString();
                    string query = "INSERT INTO " +
                        "auction_house(itemid, stack, seller, seller_name, date, price) " +
                        "SELECT itemid, 0, " + seller_id + ", 'GEM', " + secondsSinceEpoch + ", BaseSell " +
                        "FROM item_basic WHERE aH > 0 AND stackSize = 1 AND BaseSell > 0;";

                    while (count < qty)
                    {
                        MySqlCommand command_singles = new MySqlCommand(query, DB.MySQLConnection);
                        res = command_singles.ExecuteNonQuery();
                        count++;
                    } 
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DB.CloseConnection();
            }

            if (res < 0)
            {
                MessageBox.Show("Error inserting single items into Database!.");
            }
            else
            {
                RunStackSingles();
            }
        }

        private void RunStackSingles()
        {
            int count = 0;
            int res = 0;

            try
            {
                DB.OpenConnection();

                if (DB.MySQLConnection.State == ConnectionState.Open)
                {
                    string seller_id = "999999";
                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    string secondsSinceEpoch = t.TotalSeconds.ToString();
                    string query = "INSERT INTO " +
                        "auction_house(itemid, stack, seller, seller_name, date, price) " +
                        "SELECT itemid, 0, " + seller_id + ", 'GEM', " + secondsSinceEpoch + ", BaseSell " +
                        "FROM item_basic WHERE aH > 0 AND stackSize > 1 AND BaseSell > 0;";

                    while (count < qty)
                    {
                        MySqlCommand command_stack_singles = new MySqlCommand(query, DB.MySQLConnection);
                        command_stack_singles.ExecuteNonQuery();
                        count++;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DB.CloseConnection();
            }

            if (res < 0)
            {
                MessageBox.Show("Error inserting single stackable items into Database!.");
            }
            else
            {
                RunStacks();
            }
        }

        private void RunStacks()
        {
            int count = 0;
            int res = 0;

            try
            {
                DB.OpenConnection();

                if (DB.MySQLConnection.State == ConnectionState.Open)
                {
                    string seller_id = "999999";
                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    string secondsSinceEpoch = t.TotalSeconds.ToString();
                    string query = "INSERT INTO " +
                        "auction_house(itemid, stack, seller, seller_name, date, price) " +
                        "SELECT itemid, 1, " + seller_id + ", 'GEM', " + secondsSinceEpoch + ", BaseSell*stackSize " +
                        "FROM item_basic WHERE aH > 0 AND stackSize > 1 AND BaseSell > 0;";

                    while (count < qty)
                    {
                        MySqlCommand command_stacks = new MySqlCommand(query, DB.MySQLConnection);
                        res = command_stacks.ExecuteNonQuery();
                        count++;
                    } 
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DB.CloseConnection();
            }

            if (res < 0)
            {
                MessageBox.Show("Error inserting stacks of items into Database!.");
            }
            else
            {
                MessageBox.Show("AH Filled.");
            }
        }
    }
}
