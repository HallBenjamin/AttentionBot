# Attention! Bot for Discord
### Written using Discord.Net 1.0.2 and Microsoft.Net Framework 4.6.1
"Attention to the designated grid square! (E3)" for Discord (from War Thunder®, quite annoying when spammed but fun to spam). Do not compile and run (it's on a server, and you don't have the token).

##### Note: This bot is constantly evolving. With 2 commands and just under 100 lines of code when first created, it now has 9 commands (5 for the public) with 450 lines of code.

Add this bot to your server! https://discordapp.com/oauth2/authorize?client_id=346064990152818690&scope=bot&permissions=104012800

Prefix: \

Commands:
- \help 3949

  - Lists available commands

- \admin [role id]

  - SERVER OWNER ONLY! Sets the specified role as an administrative role for the bot's admin commands.

- \mentions [0/1]

  - ADMINS AND SERVER OWNERS ONLY! Enables (1) or disables (0) user mentions for the bot.

- \announce [channel id]

  - ADMINS AND SERVER OWNER ONLY! Sets the Channel with the specified ID as the channel for bot announcements. Channel ID is mandatory.

- \attention [position] [user ID]

  - Position is one letter A-J (capitalization does not matter) and/or one number 1-10. Order does not matter. Position is not required for the command.
  
  - User ID only works if \mentions is set to 1 and if a position is given. Set the User ID to the ID of the user you want the bot to mention. User ID is optional.

# Release Notes
## v2.0.0.0e
- Transfer into universal bot for more commands
- This is a far-fetched, experimental idea that may never be created.
## v1.3.5.0a
- Add command to ping the bot for performance analysis
- Modified ability for bot to send announcement when back online to be automated
- Add voice commands
- This is an alpha build. Features in this build are unofficial and may not be released in the final version.
## v1.3.4.0
- Added parameters to \offline
- Changed BinaryFile variable names for readability
## v1.3.3.1
- Fixed an issue where multiple channels on the server could recieve bot announcements
- Fixed an issue where \mention 0 wouldn't save
## v1.3.3.0
- Added ability for bot to mention user
- Added \mentions command for Admins to control if user mentions are enabled for the bot
## v1.3.2.0
- Updated to Discord.Net 1.0.2
- Changed null to "all" for bot exit
- Added length of time bot will be offline if for server shutdown
## v1.3.1.1
- Fixed issue where position number couldn't be 10
- Fixed \help to show position number 10 as an option
## v1.3.1.0
- Added ability for bot to send announcement when back online
- Added ability for bot owner to send announcements
- Fixed bug where admin commands wouldn't execute
- Transfered Owner commands to separate C# file
## v1.3.0.4 (beta tested)
- Same release notes for v1.3.1.0
- v1.3.1.0 Fixed the bug
- v1.3.1.0 added the announcements feature
## v1.3.0.3
- Put Admin Commands in separate file
- Removed unused usings
- Made more public static variables local
## v1.3.0.2
- Made some public static variables local
- Fixed \help
## v1.3.0.1
- Added \admin command to add administrative roles
- Changed \announce command to let admin roles from above use it
- Changed \announce command to only let one channel per server use it
- Removed unused comment
- Made external files for Channel, Server, and Role info save immediately in case of crash
- Updated \help
## v1.3.0.0 (beta tested)
- Same release notes for v1.3.0.1
- v1.3.0.1 fixed a bug where the \announce command wouldn't let admins use it, breaking the entire bot
- v1.3.0.1 fixed a bug where the bot wouldn't finish starting up after reading the txt files
## v1.2.0.2
- Fixed a bug where announcements couldn't find the server ID
## v1.2.0.1
- Fixed \help to show up-to-date commands
- Changed \help formatting
## v1.2.0.0
- Made \restart message send a message to default or available channel on all servers connected
- Add Server Owner \announce command to set announcements channel
- Saved \announce channel arrays and lists to text file to load upon bot restart
## v1.1.0.1
- Fixed \restart order of parameters to specify time but not bot
- Fixed \exit command to shut down all bots on my machine
## v1.1.0.0
- Added owner \restart command to give a warning to the user about a bot restart
## v1.0.5.0
- Added owner \exit parameter to specify which bot to shut down
- Added \help parameter to specify which bot's help information to show
## v1.0.4.0
- Fixed an issue with owner \exit command and optimized code
## v1.0.3.0
- Added \help command
## v1.0.2.0 and older
- Original release, unstable/low on features/slow. Bot runs on newest stable version (no letter after version number).
