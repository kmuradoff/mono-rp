
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace roleplay
{
    public class Account
    {
        public enum AdminRanks {Player, Helper, Moderator, Administrator};

        public const string _accountKey = "Player_Data";
        public int _id, _phoneNumber, _level, _adminLevel;
        public string _name, _email, _login;
        public long _cash;
        public Player _player;


        public Account(string name, Player player, long cash = 1000)
        {
            _name = name;
            _player = player;
            _cash = cash;
            _adminLevel = 0;
        }


        public bool IsPlayerLoggedIn(Player player) {
            if(player == null) return player.HasData(_accountKey);
            return false;
        }

        public bool IsPlayerHasAdminLevel(int adminLevel)
        {
            return adminLevel <= _adminLevel;
        }


        public void Register(string login, string email, string password)
        {
            ulong socialClubID = NAPI.Player.GetPlayerSocialClubId(_player);
            mysql.NewAccountRegistered(this, login, email, password, socialClubID);
            Login(_player, true);
        }

        public void Login(Player player, bool isFirstLogin)
        {
            mysql.LoadAccount(this);
            if (isFirstLogin)
            {
                player.SendChatMessage("Вы успешно зарегистрированы!");
            }
            else
            {
                player.SendChatMessage("Вы успешно авторизовались!");
            }

            player.SetData(_accountKey, this);
        }

        

    }
}
