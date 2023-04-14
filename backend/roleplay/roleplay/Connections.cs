using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace roleplay
{
    class Connections : Script
    {
        [RemoteEvent("authOnRegister")]
        private void OnRegister(Player player, string login, string email, string password)
        {
            ulong socialClubID = NAPI.Player.GetPlayerSocialClubId(player);
            if (mysql.IsAccountRegistered(login))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "sendTextError", "Аккаунт с таким именем существует.");
                return;
            }
            if (mysql.IsEmailDataDuplicate(email))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "sendTextError", "Аккаунт с такой эл.почтой уже зарегестрирован.");
                return;
            }
            if (mysql.IsSocialClubIdDuplicate(socialClubID))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "sendTextError", "Social Club уже привязан к другому аккаунту.");
                return;
            }

            Account account = new Account(login, player);
            account.Register(login, email, password);
            NAPI.ClientEvent.TriggerClientEvent(player, "closeAuthWindow");
        }

        [RemoteEvent("authOnLogin")]
        private void OnLogin(Player player, string login, string password)
        {
            if (!mysql.IsAccountRegistered(login))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "sendTextError", "Аккаунт с таким именем не существует.");
                return;
            }

            if(!mysql.IsValidPassword(login, password))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "sendTextError", "Неверный пароль.");
                return;
            }

            Account account = new Account(login, player);
            account.Login(player, false);
            NAPI.ClientEvent.TriggerClientEvent(player, "closeAuthWindow");
        }

    }
}
