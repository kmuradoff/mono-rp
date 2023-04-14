using System;
using System.Reflection.PortableExecutable;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;

namespace roleplay
{
    internal class mysql
    {
        private static MySqlConnection _connection;
        private String _host {get;set;}
        private String _username { get;set;}
        private String _password { get;set;}
        private String _database { get;set;}

        private mysql() { 
            this._host = "localhost";
            this._username = "root";
            this._password = "";
            this._database = "monorp";
        }

        public static void InitConnection()
        {
            mysql sql = new mysql();
            String SQLConnection = $"SERVER={sql._host}; DATABASE={sql._database}; UID={sql._username}; PASSWORD={sql._password}";
            _connection = new MySqlConnection(SQLConnection);

            try {
                _connection.Open();
                NAPI.Util.ConsoleOutput("Connected to database successfuly.");
            }
            catch(Exception ex) {
                NAPI.Util.ConsoleOutput("Error connecting to Database");
                NAPI.Util.ConsoleOutput("Exception: " + ex);
                NAPI.Task.Run(() => {
                    Environment.Exit(0);
                }, delayTime: 5000);
            }
        }

        public static bool IsAccountRegistered(string name) { 
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);

            using (MySqlDataReader reader = command.ExecuteReader()) { 
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
        }

        public static void NewAccountRegistered(Account accounts, string login, string email, string password, ulong socialClubID)
        {
            string saltPassword = BCrypt.HashPassword(password, BCrypt.GenerateSalt());

            try { 
                MySqlCommand mySqlCommand = _connection.CreateCommand();
                mySqlCommand.CommandText = "INSERT INTO accounts (password, name, cash, email, socialClubID) VALUES (@password, @name, @cash, @email, @socialClubID)";

                mySqlCommand.Parameters.AddWithValue("@password", saltPassword);
                mySqlCommand.Parameters.AddWithValue("@name", login);
                mySqlCommand.Parameters.AddWithValue("@cash", accounts._cash);
                mySqlCommand.Parameters.AddWithValue("@email",email);
                mySqlCommand.Parameters.AddWithValue("@socialClubID",socialClubID);

                mySqlCommand.ExecuteNonQuery();

                accounts._id = (int)mySqlCommand.LastInsertedId;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput("Exception: " + ex);
            }
        }

        public static void LoadAccount(Account account) 
        { 
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", account._name);

            using(MySqlDataReader reader  = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    account._id = reader.GetInt32("id");
                    account._cash = reader.GetInt64("cash");
                    account._adminLevel = reader.GetInt32("adminLevel");
                }
            }
        }

        public static void SaveAccount(Account accounts)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE accounts SET cash=@cash WHERE id=@id";
            command.Parameters.AddWithValue("@cash", accounts._cash);
            command.Parameters.AddWithValue("@id", accounts._id);
        }

        public static bool IsValidPassword(string name, string inputPassword) { 
            string tempPassword = " ";

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT password FROM accounts WHERE name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    tempPassword = reader.GetString("password");
                }
            }
            if(BCrypt.CheckPassword(inputPassword, tempPassword)) return true;
            return false;
        }
        
        public static bool IsEmailDataDuplicate(string email)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE email=@email LIMIT 1";
            command.Parameters.AddWithValue("@email", email);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSocialClubIdDuplicate(ulong socialClubID) {

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE socialClubID=@socialClubID LIMIT 1";
            command.Parameters.AddWithValue("@socialClubID", socialClubID);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
