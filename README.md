# Attention! Bot for Discord
### Written using Discord.Net 1.0.1 and Microsoft.Net Framework 4.6.1
"Attention to the designated grid square! (E3)" for Discord (from War Thunder®, quite annoying when spammed but fun to spam). Do not compile and run (it's on a server, and you don't have the token).

Add this bot to your server! https://discordapp.com/oauth2/authorize?client_id=346064990152818690&scope=bot&permissions=203776

Prefix: \

Commands:
- \help 3949

  - Lists available commands

- \announce [channel id]

  - SERVER OWNER ONLY! Sets the Channel with the specified ID as the channel for bot announcements. Channel ID is mandatory.

- \attention [position]

  - Position is one letter A-J (capitalization does not matter) and/or one number 1-9. Order does not matter. Position is not required for the command.

# Release Notes
## v1.2.1.0a
- Add command to ping the bot for performance analysis
- Add ability for bot to send announcement when back online
- This is an alpha build. Features in this build are unofficial and may not be released in the final version.
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
