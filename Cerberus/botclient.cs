using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Cerberus
{
    public class botclient
    {
        private DiscordSocketClient _client;
        private CommandService _cmdService;
        private IServiceProvider _services;

        public botclient(DiscordSocketClient client = null, CommandService cmdService = null)
        {
            _client = client ?? new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = LogSeverity.Debug
            });

            _cmdService = cmdService ?? new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false
            });
        }
        public async Task InitializeAsync()
        {
            Console.WriteLine("Please Select:\n 1. Cerberus \n 2. Cerberus \n 3. Enter your own Token");
            int result = Convert.ToInt32(Console.ReadLine());
            switch (result)
            {
                case 1:
                    string ThirstBot = "Njk2MTAxNDI2NjI3MzQ2NDgy.Xoj1Og.oQYpEUt_VSDIOttIcjs4H_iWfNk";
                    Global.Ysmirr = ThirstBot;
                    break;
                case 2:
                    string ThirstBot1 = "Njk2OTcxMDk0MzQ2OTU2ODYw.Xowe8g.OWTTlp5i3P_s5Tu5mNxc43CXaec";
                    Global.Ysmirr = ThirstBot1;

                    break;
                case 3:
                    Console.WriteLine("enter token now:");
                    string result2 = Console.ReadLine().ToString();
                    Global.Ysmirr = result2;
                    break;
            }




            await _client.LoginAsync(TokenType.Bot, Global.Ysmirr);

            await _client.StartAsync();
            _client.Log += LogAsync;

            _services = SetupServices();
            var cmdHandler = new CommandHandler(_client, _cmdService, _services);

            await cmdHandler.InitializeAsync();



            await Task.Delay(-1);

        }


        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);

            return Task.CompletedTask;

        }






        private IServiceProvider SetupServices()
                => new ServiceCollection()
                    .AddSingleton(_client)
                    .AddSingleton(_cmdService)
                    .BuildServiceProvider();

    }

}