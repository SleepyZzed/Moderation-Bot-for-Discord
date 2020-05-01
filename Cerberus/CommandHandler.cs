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
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;
        public int usersNum = 0;
        public CommandHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider services)
        {
            _client = client;
            _cmdService = cmdService;
            _services = services;

        }
        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _cmdService.Log += LogAsync;
            _client.MessageReceived += HandleMessageAsync;
          



            await _client.SetGameAsync("The Gates of Hell", null, ActivityType.Watching);
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
        }

      

        private async Task HandleMessageAsync(SocketMessage socketMessage)
        {
            //Console.WriteLine(socketMessage.Content);
            string prefix = "~";
            var argPos = 0;
            if (socketMessage.Author.IsBot) return;

            var userMessage = socketMessage as SocketUserMessage;
            var context = new SocketCommandContext(_client, userMessage);
            if (userMessage is null)
                return;
            if (context.User.IsBot)
                return;
            if (!userMessage.Attachments.Any() && userMessage.Channel.Id == 595668716096847902)
            {
                await userMessage.Author.SendMessageAsync("Deleted");
                await userMessage.DeleteAsync();
                
            }
            if (!userMessage.Attachments.Any() && userMessage.Channel.Id == 705713517310902272)
            {
                await userMessage.Author.SendMessageAsync("Deleted");
                await userMessage.DeleteAsync();

            }
            if (!userMessage.Attachments.Any() && userMessage.Channel.Id == 685800338879414286)
            {
                await userMessage.Author.SendMessageAsync("Deleted");
                await userMessage.DeleteAsync();
                
            }
            if (!userMessage.Attachments.Any() && userMessage.Channel.Id == 602302904669569064)
            {
                await userMessage.Author.SendMessageAsync("Deleted");
                await userMessage.DeleteAsync();
                
            }
            if (userMessage.ToString().ToLower().Contains("https://discord.gg/"))
            {
                List<string> RoleList = new List<string>();
                foreach (SocketRole role in ((SocketGuildUser)userMessage.Author).Roles)
                {
                    RoleList.Add(role.Name);


                }
                if (string.Join(", ", RoleList.ToArray()).Contains("Owner") || string.Join(", ", RoleList.ToArray()).Contains("Gate Keeper") || string.Join(", ", RoleList.ToArray()).Contains("Partner Manager"))
                {
                    //await userMessage.DeleteAsync();
                    //await context.Guild.AddBanAsync(userMessage.Author);
                    var staffchan = _client.GetGuild(595663383777247297).GetChannel(696106753628307506) as SocketTextChannel;
                   
                    EmbedBuilder builderJoin = new EmbedBuilder();
                    builderJoin.WithTitle("Invite posted");
                    builderJoin.WithDescription($"{userMessage.Author.Username}#{userMessage.Author.Discriminator} ({userMessage.Author.Mention}) has posted a server invite, they are authorised");


                   

                    builderJoin.WithCurrentTimestamp();

                    builderJoin.WithColor(Color.Purple);


                    await staffchan.SendMessageAsync("", false, builderJoin.Build());
                    await Task.Delay(1);
                }
                else
                {
                    var staffchan = _client.GetGuild(595663383777247297).GetChannel(696106753628307506) as SocketTextChannel;
                  
                    await userMessage.Author.SendMessageAsync("Banned for advertising");
                    EmbedBuilder builderJoin = new EmbedBuilder();
                    builderJoin.WithTitle("Banned for advertising");
                    builderJoin.WithDescription($"{userMessage.Author.Username}#{userMessage.Author.Discriminator} ({userMessage.Author.Mention}) has been banned for advertising");




                    builderJoin.WithCurrentTimestamp();

                    builderJoin.WithColor(Color.Purple);


                    await staffchan.SendMessageAsync("", false, builderJoin.Build());
                    await Task.Delay(1);
                    await context.Guild.AddBanAsync(userMessage.Author);
                }
            }


            if (!userMessage.HasStringPrefix(prefix, ref argPos))
                return;


            var result = await _cmdService.ExecuteAsync(context, argPos, _services);


        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;

        }
    }
}

