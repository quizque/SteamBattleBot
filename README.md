## SteamBattleBot
Steam chat game Bot used to pass time

### Usage
**You will need an alt account to use this (Or just use your own and let a friend play)**

1. Start the application and sign in
  - If you got email verification it will ask you for the code.
 
2. Open the new admim.txt folder and replace the current text with your steamID64
  - You can find your steamID64 at [steamid.io](https://steamid.io/)
  - You can add as many steamID64 as you want, just make sure each one is on a new line
 
3. Send the bot account the follow command to setup/restart a new game: `!setup`
  - To send the bot account commands, simply open a chat with the bot and enter them there.
 
4. Send the bot account commands via steam chat (Commands below).

### Commands
Command | Action
------------ | -------------
`!setup` | Used to setup/reset a game
`!attack` | Used to attack the "monster".
`!state` | Used to view yours and the monsters state.
`!shutdown` | Used to shutdown the bot (Admin only)
`!help` | Display commands

### Features
- Auto login (Currently unsecure, mostly just for testing)
  - Create a creds.txt in the same folder as the program and put your username on the top line and your password on the line below then simply open the program.

### Need to do list
- [Check out the projects!](https://github.com/nickthegamer5/SteamBattleBot/projects)

### Libarys used
- [Fody.Costura](https://github.com/Fody/Costura) for packaging the DLLs
- [SteamKit2](https://github.com/SteamRE/SteamKit) for handling steam requests
