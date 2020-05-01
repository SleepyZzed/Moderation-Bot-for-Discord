using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace Cerberus
{
    class Program
    {
        static async Task Main(string[] args)

            => await new botclient().InitializeAsync();
        
    }
}
