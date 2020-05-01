using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cerberus.UserProfiles
{
    public class UserAccount
    {
        private static List<UserAccounts> accounts;

        private static string accountsFile = "Resources/accounts.json";
        static UserAccount()
        {
            if (DataStorage.SaveExists(accountsFile))
            {
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
            }
            else
            {
                accounts = new List<UserAccounts>();
                SaveAccounts();
            }

        }

        public static void SaveAccounts()
        {
            DataStorage.SavedUserAccounts(accounts, accountsFile);
        }
        public static UserAccounts GetAccounts(SocketUser user)
        {
            
            return GetOrCreateAccount(user.Id, user);
        }

        public static UserAccounts GetOrCreateAccount(ulong id, SocketUser user)
        {
            var result = from a in accounts
                         where a.ID == id
                         select a;
            var account = result.FirstOrDefault();

            if (account == null) account = CreateUserAccount(id, user);
            return account;
        }

        private static UserAccounts CreateUserAccount(ulong id, SocketUser user)
        {
            List<string> RoleList = new List<string>();
            foreach (SocketRole role in ((SocketGuildUser)user).Roles)
            {
                
                if (role.Name == "@everyone")
                {
                    continue;
                }
                RoleList.Add(role.Name);


            }
            List<string> WarnReason = new List<string>();
            var result = String.Join(", ", RoleList.ToArray());
            var warnreason = String.Join(", ", WarnReason.ToArray());
            var newAccount = new UserAccounts()
            {
                ID = id,
                Warns = 0,
                Mutes = 0,
                Roles = result,
                Username = user.Username,
                ListofRoles = RoleList,
                Muted = false,
                WarnreasonList = WarnReason,
                warnreason = warnreason

            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}
