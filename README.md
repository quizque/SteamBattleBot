## SteamBattleBot
Steam chat Battle Bot used to pass time

### Usage
**You will need an alt account to use this (Or just use your own and let a friend play)**
1. Start the application and sign in
 - If you got email verification it will ask you for the code. (2SA does not work at the moment, sorry!)
2. Open the new admim.txt folder and replace the current text with your steamID64
 - You can find your steamID64 at [steamid.io](https://steamid.io/)
3. Send the bot account the follow command to setup/restart a new game: `!setup`
 - To send the bot account commands, simply open a chat with the bot and enter them there.
4. Send the bot account commands. (Commands below)

### Commands
- `!attack` - Used to attack the "monster".
- `!state` - Used to view yours and the monsters state.
- `!setup` - Used to setup/reset a game (Admin only command!)

### Need to do list
- Important
 - Add help command
 - Add 2SA support
 - Put in better argument system
 - Let more than one player play at a time
 - Organize files
- Semi-Important
 - Allow more than one admin
 - replace the `!setup` the command with a better system
 - Better file system for storing data
 - Allow multiplayer
- Ideas
 - Add more account based commands (Change username, friend user, etc)
 - Add more games
 - Move usage to the wiki
 - Change console to GUI

### Libarys used
- [Fody.Costura](https://github.com/Fody/Costura)
- [SteamKit2](https://github.com/SteamRE/SteamKit)