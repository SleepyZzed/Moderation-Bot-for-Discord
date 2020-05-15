using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.Webhook;
using Discord.WebSocket;
using Discord.Commands;
using Discord.API;
using Discord.Audio;
using Discord.Rest;
using Discord;

namespace Cerberus
{
    public class AdminModules
    {
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]

        public class AdminModule : ModuleBase<SocketCommandContext>
        {


            [Command("ban")]
            public async Task ban(SocketGuildUser user, [Remainder] string Reason)
            {

                if (string.IsNullOrWhiteSpace(Reason)) return;
                var allbans = await Context.Guild.GetBansAsync();
                bool isbanned = allbans.Select(b => b.User).Where(u => u.Username == user.Username).Any();

                if(!isbanned)
                {
                    var targethighest = (user as SocketGuildUser).Hierarchy;
                    var senderhighest = (Context.User as SocketGuildUser).Hierarchy;
                    if(targethighest < senderhighest)
                    {
                        await Context.Guild.AddBanAsync(user);
                        EmbedBuilder builderJoin = new EmbedBuilder();
                        builderJoin.WithTitle("User banned");
                        builderJoin.WithDescription($"{Context.User} has banned {user.Username} for {Reason}");




                        builderJoin.WithCurrentTimestamp();

                        builderJoin.WithColor(Color.Purple);


                        await Context.Channel.SendMessageAsync("", false, builderJoin.Build());
                        var staffchan = Context.Guild.GetChannel(696106753628307506) as SocketTextChannel;
                        await staffchan.SendMessageAsync("", false, builderJoin.Build());
                        await Task.Delay(1);
                        var dmchannel = await user.GetOrCreateDMChannelAsync();
                        await dmchannel.SendMessageAsync($"you were banned from {Context.Guild.Name} for {Reason}");
                        
                    }
                }

            }

            [Command("mute")]
            public async Task Mute(SocketGuildUser user)
            {
                var targethighest = (user as SocketGuildUser).Hierarchy;
                var senderhighest = (Context.User as SocketGuildUser).Hierarchy;
                if (targethighest > senderhighest)
                {
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("Invalid");
                    builderleave.WithDescription("You do not have permission to do this");
                    

                    builderleave.WithColor(Color.Purple);


                 
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());

                    return;
                }
                    var account = UserProfiles.UserAccount.GetAccounts((user));
                if (account.Muted == false)
                {
                   // await Context.Channel.SendMessageAsync($"{user} has been muted");
                    UserProfiles.UserAccount.GetOrCreateAccount(user.Id, user);
                    account.Muted = true;
                    account.Mutes = +1;
                    EmbedBuilder builderleave11 = new EmbedBuilder();

                    builderleave11.WithTitle("Muted");
                    builderleave11.WithDescription($"You have been muted");



                    builderleave11.WithCurrentTimestamp();

                    builderleave11.WithColor(Color.Purple);
                    
                    
                    UserProfiles.UserAccount.SaveAccounts();
                    List<string> RoleList = new List<string>();
                    foreach (SocketRole role in ((SocketGuildUser)user).Roles)
                    {
                        RoleList.Add(role.Name);
                        if (role.Name == "@everyone" || role.Name == "Nitro Booster")
                        {
                            continue;
                        }
                        await Task.Delay(250);
                        await user.RemoveRoleAsync(role);


                    }


                    ulong roleId = 601180170975445046;
                    var roles = Context.Guild.GetRole(roleId);
                    await user.AddRoleAsync(roles);
                    var result = String.Join(", ", RoleList.ToArray());
                    string avurlid = user.GetAvatarUrl();
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("user muted");
                    builderleave.WithDescription(user.Mention + "(" + user.Username + ")" + "Has been muted");
                    builderleave.AddField(result, true);

                    builderleave.WithThumbnailUrl(avurlid);
                    builderleave.WithCurrentTimestamp();

                    builderleave.WithColor(Color.Purple);

                    


                    var staffchan = Context.Guild.GetChannel(696106753628307506) as SocketTextChannel;
                    await staffchan.SendMessageAsync("", false, builderleave.Build());
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());
                    
                    
                    await user.SendMessageAsync("", false, builderleave11.Build());
                    
                 
                    


                }
                else
                {
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("invalid");
                    builderleave.WithDescription(user.Mention + "(" + user.Username + ")" + "is already muted");
                   

                   
                    builderleave.WithCurrentTimestamp();

                    builderleave.WithColor(Color.Purple);
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());
                }
               
                }


            [Command("Warn")]
            [Alias("Strike", "Punish")]
            public async Task Warn(SocketGuildUser user, [Remainder] string reason)
            {
                var account = UserProfiles.UserAccount.GetAccounts((user));
                var targethighest = (user as SocketGuildUser).Hierarchy;
                var senderhighest = (Context.User as SocketGuildUser).Hierarchy;
                if (targethighest > senderhighest)
                {
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("Invalid");
                    builderleave.WithDescription("You do not have permission to do this");


                    builderleave.WithColor(Color.Purple);



                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());

                    return;
                }
                account.Warns += 1;
                if (account.Warns == 5 && account.Muted == false)
                {
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("user warned");
                    builderleave.WithDescription($"{user.Mention}Has been warned \n Reason: {reason} ");



                    builderleave.WithCurrentTimestamp();

                    builderleave.WithColor(Color.Purple);


                    var staffchan = Context.Guild.GetChannel(696106753628307506) as SocketTextChannel;
                    await staffchan.SendMessageAsync("", false, builderleave.Build());
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());



                    account.WarnreasonList.Add(reason);
                    List<string> rolelist = account.WarnreasonList.ToList();
                    var warnreason = String.Join("\n ", rolelist.ToArray());
                    account.warnreason = warnreason.ToString();
                    UserProfiles.UserAccount.SaveAccounts();
                    account.Muted = true;
                    account.Mutes = +1;
                    await Task.Delay(3000);
                    EmbedBuilder builderleave11 = new EmbedBuilder();

                    builderleave11.WithTitle("You have been muted");
                    builderleave11.WithDescription($"You have been muted for the following reason: {reason}");
                   
                  

                    builderleave11.WithCurrentTimestamp();

                    builderleave11.WithColor(Color.Purple);
                    UserProfiles.UserAccount.SaveAccounts();
                    List<string> RoleList = new List<string>();
                    foreach (SocketRole role in ((SocketGuildUser)user).Roles)
                    {
                        RoleList.Add(role.Name);
                        if (role.Name == "@everyone" || role.Name == "Nitro Booster")
                        {
                            continue;
                        }
                        await Task.Delay(250);
                        await user.RemoveRoleAsync(role);


                    }

                    ulong roleId = 601180170975445046;
                    var roles = Context.Guild.GetRole(roleId);
                    await user.AddRoleAsync(roles);
                    var result = String.Join(", ", RoleList.ToArray());
                    string avurlid = user.GetAvatarUrl();
                    EmbedBuilder builderleave1 = new EmbedBuilder();

                    builderleave1.WithTitle("user muted");
                    builderleave1.WithDescription(user.Mention + "(" + user.Username + ")" + "Has been muted");
                    builderleave1.AddField(result, true);

                    builderleave1.WithThumbnailUrl(avurlid);
                    builderleave1.WithCurrentTimestamp();

                    builderleave1.WithColor(Color.Purple);

                   



                    await staffchan.SendMessageAsync("", false, builderleave.Build());
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());
                    await staffchan.SendMessageAsync("", false, builderleave1.Build());
                    await Context.Channel.SendMessageAsync("", false, builderleave1.Build());
                    await user.SendMessageAsync("", false, builderleave11.Build());

                }

                else {
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("user warned");
                    builderleave.WithDescription($"{user.Mention}Has been warned \n Reason: {reason} ");
                   

               
                    builderleave.WithCurrentTimestamp();

                    builderleave.WithColor(Color.Purple);

                    EmbedBuilder builderleave1 = new EmbedBuilder();

                    builderleave1.WithTitle("You have been warned");
                    builderleave1.WithDescription($"you have been warned for the following reason: {reason} ");



                    builderleave1.WithCurrentTimestamp();

                    builderleave1.WithColor(Color.Purple);


                    var staffchan = Context.Guild.GetChannel(696106753628307506) as SocketTextChannel;
                    await staffchan.SendMessageAsync("", false, builderleave.Build());
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());
                   

                  
                    account.WarnreasonList.Add(reason);
                    List<string> rolelist = account.WarnreasonList.ToList();
                    var warnreason = String.Join("\n ", rolelist.ToArray());
                    account.warnreason = warnreason.ToString();
                    UserProfiles.UserAccount.SaveAccounts();
                    await user.SendMessageAsync("", false, builderleave1.Build());
                }
                






            }
            [Command("punishments")]
            [Alias("sttrikes", "warnings")]
            public async Task test1(SocketGuildUser user)
            {
                var account = UserProfiles.UserAccount.GetAccounts((user));
                if(account.Warns == 0)
                {
                    EmbedBuilder builderleave1 = new EmbedBuilder();

                    builderleave1.WithTitle("invalid");
                    builderleave1.WithDescription($"{user.Mention} has no warnings");



                    builderleave1.WithCurrentTimestamp();

                    builderleave1.WithColor(Color.Purple);


                   
                  
                    await Context.Channel.SendMessageAsync("", false, builderleave1.Build());

                    return;
                }


                EmbedBuilder builderleave = new EmbedBuilder();

                builderleave.WithTitle($"All {user.Mention}'s Warnings");
                builderleave.WithDescription($"{user.Mention} Warnings:\n {account.warnreason} ");



                builderleave.WithCurrentTimestamp();

                builderleave.WithColor(Color.Purple);


                
                await Context.Channel.SendMessageAsync("", false, builderleave.Build());





            }

            [Command("delpunishments")]
            [Alias("delwarnings", "delstrikes")]
            public async Task DelPunishments(SocketGuildUser user)
            {
                var account = UserProfiles.UserAccount.GetAccounts((user));
                if (account.Warns > 0)
                {
                    EmbedBuilder builderleave1 = new EmbedBuilder();

                    builderleave1.WithTitle("Warnings Deleted");
                    builderleave1.WithDescription($"{user.Mention} wanings removed");



                    builderleave1.WithCurrentTimestamp();

                    builderleave1.WithColor(Color.Purple);




                    await Context.Channel.SendMessageAsync("", false, builderleave1.Build());
                    account.Warns = 0;
                    account.WarnreasonList.Clear();
                    account.warnreason = "";
                    UserProfiles.UserAccount.SaveAccounts();
                    return;
                }
                else
                {
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("user warned");
                    builderleave.WithDescription($"{user.Mention} has no warnings");



                    builderleave.WithCurrentTimestamp();

                    builderleave.WithColor(Color.Purple);

                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());
                }

               





            }

            [Command("unmute")]
            public async Task unmute(SocketGuildUser user)
            {
                var account = UserProfiles.UserAccount.GetAccounts((user));

                if (account.Muted == true)
                {
                    var role1 = ((ITextChannel)Context.Channel).Guild.Roles.FirstOrDefault(x => x.Name == "Muted");
                    await user.RemoveRoleAsync(role1);
                    List<string> rolelist = account.ListofRoles.ToList();

                    foreach (var i in rolelist)
                    {   if(i.ToString().Equals("Nitro Booster"))
                        {
                            continue;
                        }
                        var role = ((ITextChannel)Context.Channel).Guild.Roles.FirstOrDefault(x => x.Name == i.ToString());
                        await Task.Delay(250);
                        await user.AddRoleAsync(role);
                       
                    }
                    
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("Unmuted");
                    builderleave.WithDescription(user.Mention + "(" + user.Username + ")" + "unmuted");



                    builderleave.WithCurrentTimestamp();

                    builderleave.WithColor(Color.Purple);
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());
                    var staffchan = Context.Guild.GetChannel(696106753628307506) as SocketTextChannel;
                    await staffchan.SendMessageAsync("", false, builderleave.Build());
                    account.Muted = false;
                    UserProfiles.UserAccount.SaveAccounts();
                }
                else
                {
                    EmbedBuilder builderleave = new EmbedBuilder();

                    builderleave.WithTitle("invalid");
                    builderleave.WithDescription(user.Mention + "(" + user.Username + ")" + "is not muted");



                    builderleave.WithCurrentTimestamp();

                    builderleave.WithColor(Color.Purple);
                    await Context.Channel.SendMessageAsync("", false, builderleave.Build());
                }
            }

            [Command("del")]
            [Alias("purge", "remove")]
            public async Task del(int amount)
                {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
                const int delay = 3000;
               
                EmbedBuilder builderJoin = new EmbedBuilder();
                builderJoin.WithTitle("Deleted Messages");
                builderJoin.WithDescription($"{amount} messages deleted");




                builderJoin.WithCurrentTimestamp();

                builderJoin.WithColor(Color.Purple);


                IUserMessage m = await Context.Channel.SendMessageAsync("", false, builderJoin.Build());

                await Task.Delay(1);
                await Task.Delay(delay);
                await m.DeleteAsync();
            }
            







            }

        }

    }

