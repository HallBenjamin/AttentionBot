# Attention! Bot for Discord
### Written using Discord.Net 1.0.2 and Microsoft.Net Framework 4.5.2
"Attention to the designated grid square! (E3)" for Discord (from War Thunder®, quite annoying when spammed but fun to spam). Do not compile and run (it's on a server, and you don't have the token).

##### Note: This bot is constantly evolving. With 2 commands and just under 100 lines of code when first created, it now has 16 commands (9 for the public) with over 600 lines of code.

Add this bot to your server! https://discordapp.com/oauth2/authorize?client_id=346064990152818690&scope=bot&permissions=220160

Prefix: \

Commands:

Useful:

- \help 3949
  - Lists available commands

- \changelog 3949
  - Sends a link to the version history (changelog) of the bot.
  
- \membercount
  - Lists number of users on the server by status.

Admins:

- \admin [role id]
  - SERVER OWNER ONLY! Sets the specified role as an administrative role for the bot's admin commands.

- \noadmin [role id]
  - SERVER OWNER OLNY: Removes the specified role from the list of administrative roles for the bot.

- \mentions [0/1]
  - ADMINS AND SERVER OWNERS ONLY! Enables (1) or disables (0) user mentions for the bot.

- \announce [channel id]
  - ADMINS AND SERVER OWNER ONLY! Sets the Channel with the specified ID as the channel for bot announcements.

Fun Spam:

- \attention [position (optional)] [user ID (optional)]
  - Position is one letter A-J (capitalization does not matter) and/or one number 1-10. Order does not matter.
  - User ID only works if \mentions is set to 1 and if a position is given. Set the User ID to the ID of the user you want the bot to mention.

- \gary [user ID (optional)]
  - User ID only works if \mentions is set to 1 and if a position is given. Set the User ID to the ID of the user you want the bot to mention.

- \bandits [user ID (optional)]
  - User ID only works if \mentions is set to 1 and if a position is given. Set the User ID to the ID of the user you want the bot to mention.

# Release Notes
## v1.6.0.0 (Alpha)
- Add command to ping the bot for performance analysis
- Modified ability for bot to send announcement when back online to be automated
- Add voice commands
- This is an alpha build. Features in this build are unofficial and may not be released in the final version.
## v1.5.5.2 (Beta)
- Improve formatting for \help to be an embed
## v1.5.5.1 (Stable)
- Changed Owner commands to not output that a cleanup finished every single time
- Renamed \usercount to \membercount
## v1.5.5.0
- Optimize code to use Dictionaries for server and channel IDs since they are linked anyways
  - This is a more permanent, more effective, and more efficient fix to the same bug v1.5.4.3 fixed
- Optimized code so things loop less, and common lines of code are outside of the if statements while the individually different statements remain within
- New command: \gary [user ID (optional)]
  - "We must save my family!"
  - Sword Art Online Abridged reference
- New command: \bandits [user ID (optional)]
  - "The bandits are coming!"
  - Sword Art Online Abridged reference
- New commands (Owner): \cleanup (cleans up the files that stores bot data), \close (closes the bot silently), \reload (reloads the bot)
- Improved formatting for \help
## v1.5.4.3
- Fixed a major bug where if a server or channel went missing (was deleted/bot was kicked), the bot would stop working altogether
## v1.5.4.2
- Updated \help to show the version of .NET Framework and Discord.Net the bot uses
## v1.5.4.1
- Fixed a bug where \usercount crashed the computer's network capabilities
## v1.5.4.0
- Changed \usercount layout to use inline fields with an EmbedBuilder, looks much cleaner and more organized now
## v1.5.3.0
- Changed \usercount to display more information:
  - Total: displays the total number of users and bots on the server
  - Users: displays the total number of users on the server
    - Online, Away, Do Not Disturb, Invisible, Offline: displays the number of users with that status on the server
  - Bots: displays the total number of bots on the server
    - Online, Offline: displays the number of bots with that status on the server
## v1.5.2.1
- Fixed an issue with the \usercount command crashing the bot's internal network connectivity on large servers
## v1.5.2.0
- New command: \changelog 3949 (sends a link to the version history / changelog of the bot)
## v1.5.1.0
- Removed the arrays in Program.cs since they were identical to the Lists (replace all instances of the arrays with the Lists)
- Added code to detect if an instance of the program is already running and, if it is, display a message saying it's already running and close the new instance; this prevents the bot from logging in a second time, resetting the bot token
## v1.5.0.1
- Fixed a bug where having mentions enabled on the server but not putting in a user ID would crash the bot
## v1.5.0.0
- New command: \usercount (lists the number of users on the server by status)
- Made various variables private, static, or constant (programming practices / security)
## v1.4.0.1
- Fixed Owner.cs commands to check if the bot has access to a channel and, if not, remove the channel and server frmo the database.
## v1.4.0.0 (uploaded to GitHub as v1.3.5.0)
- Data saved in txt files now saved as strings for easier management of bot errors
- Cleaned up the role function
- New command: "noadmin"
## v1.3.4.7
- Updated the Microsoft .NET XML dependency to v1.3.1 (old: v1.3.0)
## v1.3.4.6
- Fixed error where if bot was kicked from a server, it couldn't send announcements
## v1.3.4.5
- Fixed Owner \exit command saying "Shutting down" instead of "Restarting"
## v1.3.4.4
- Fixed a code-breaking bug with saving the \announce info to the text file
## v1.3.4.3
- Updated .NET Framework Components
- Updated code formatting
- Optimized some of the code
## v1.3.4.2
- Fixed output text of Owner \restart with the [length] parameter
## v1.3.4.1
- Changed syntax of Owner \restart
- Added parameters to Owner \restart
- Optimized and compressed code for Owner \exit
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
## v1.3.0.4 (Beta)
- Same release notes as v1.3.1.0
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
- New command (Server Owners): \admin (adds administrative roles)
- Changed \announce command to let admin roles from above use it
- Changed \announce command to only let one channel per server use it
- Removed unused comment
- Made external files for Channel, Server, and Role info save immediately in case of crash
- Updated \help
## v1.3.0.0 (Beta)
- Same release notes as v1.3.0.1
- v1.3.0.1 fixed a bug where the \announce command wouldn't let admins use it, breaking the entire bot
- v1.3.0.1 fixed a bug where the bot wouldn't finish starting up after reading the txt files
## v1.2.0.2
- Fixed a bug where announcements couldn't find the server ID
## v1.2.0.1
- Fixed \help to show up-to-date commands
- Changed \help formatting
## v1.2.0.0
- Made \restart message send a message to default or available channel on all servers connected
- New command (Server Owners): \announce command to set announcements channel
- Saved \announce channel arrays and lists to text file to load upon bot restart
## v1.1.0.1
- Fixed \restart order of parameters to specify time but not bot
- Fixed \exit command to shut down all bots on my machine
## v1.1.0.0
- New command (Owner): \restart command to give a warning to the user about a bot restart
## v1.0.5.0
- New command (Owner): \exit parameter to specify which bot to shut down
- Added parameter to \help to specify which bot's help information to show
## v1.0.4.0
- Fixed an issue with owner \exit command and optimized code
## v1.0.3.0
- New command: \help
## v1.0.2.0 and older
- Original release, unstable/low on features/slow. Bot runs on newest stable version (no letter after version number).
