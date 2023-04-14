using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace roleplay
{
    class Events : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStarted()
        {
            mysql.InitConnection();
            DateTime moscowTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
            NAPI.World.SetTime(moscowTime.Hour, moscowTime.Minute, moscowTime.Second);
        }
        [ServerEvent(Event.PlayerConnected)]
        private void OnPlayerConnected(Player player)
        {
            
            player.SendChatMessage("Добро пожаловать на сервер ~g~Mono RP");
            NAPI.ClientEvent.TriggerClientEvent(player, "showAuthWindow");
        }

        [ServerEvent(Event.PlayerSpawn)]
        private void OnPlayerSpawn(Player player)
        {
            player.Health = 50;
            player.Armor = 50;
        }
    }
}