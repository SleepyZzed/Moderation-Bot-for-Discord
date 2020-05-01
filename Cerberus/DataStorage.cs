using Cerberus.UserProfiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cerberus
{
    public static class DataStorage
    {
        public static void SavedUserAccounts(IEnumerable<UserAccounts> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);

        }

        public static IEnumerable<UserAccounts> LoadUserAccounts(string filePath)
        {

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccounts>>(json);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
