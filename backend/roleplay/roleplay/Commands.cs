using GTANetworkAPI;
using System;
using System.Security.Policy;

namespace roleplay
{
    internal class Commands : Script
    {
        [Command("veh", "veh или vehicle спавнит транспорт в координатах игрока", Alias = "vehicle")]
        private void spawnVehicle(Player player, String vehname, int color1, int color2) {
            try
            {
                Account account = player.GetData<Account>(Account._accountKey);
                if (!account.IsPlayerHasAdminLevel((int)Account.AdminRanks.Helper))
                {
                    player.SendNotification("~r~У вас нет доступа к данной команде.");
                    return;
                }

                uint vhash = NAPI.Util.GetHashKey(vehname);
                if (vhash <= 0)
                {
                    player.SendChatMessage("~r~Неверная модель т.с.");
                }

                Vehicle veh = NAPI.Vehicle.CreateVehicle(vhash, player.Position, player.Heading, color1, color2);
                veh.Locked = false;
                veh.EngineStatus = true;
                player.SetIntoVehicle(veh, (int)VehicleSeat.Driver);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("weather", "Команда weather меняет погоду в игре.")]
        private void setWeather(Player player, byte weatherId) {
            
            string weatherType = " ";
            try {
                Account account = player.GetData<Account>(Account._accountKey);
                if (!account.IsPlayerHasAdminLevel((int)Account.AdminRanks.Moderator))
                {
                    player.SendNotification("~r~У вас нет доступа к данной команде.");
                    return;
                }
                
                switch (weatherId) {
                    case 0:
                        weatherType = "EXTRASUNNY";
                        break;
                    case 1:
                        weatherType = "CLEAR";
                        break;
                    case 2:
                        weatherType = "CLOUDS";
                        break;
                    case 3:
                        weatherType = "SMOG";
                        break;
                    case 4:
                        weatherType = "FOGGY";
                        break;
                    case 5:
                        weatherType = "OVERCAST";
                        break;
                    case 6:
                        weatherType = "RAIN";
                        break;
                    case 7:
                        weatherType = "THUNDER";
                        break;
                    case 8:
                        weatherType = "CLEARING";
                        break;
                    case 9:
                        weatherType = "NEUTRAL";
                        break;
                    case 10:
                        weatherType = "SNOW";
                        break;
                    case 11:
                        weatherType = "BLIZZARD";
                        break;
                    case 12:
                        weatherType = "SNOWLIGHT";
                        break;
                    case 13:
                        weatherType = "XMAS";
                        break;
                    case 14:
                        weatherType = "HALLOWEEN";
                        break;
                    default:
                        weatherType = "CLEAR";
                        break;
                }
                NAPI.World.SetWeather(weatherType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
