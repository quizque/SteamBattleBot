## SteamBattleBot
Steam chat game bot used to pass time

### Usage
 - You will need
   - An alt account

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
`!setup` | Used to setup/reset a game.
`!block` | Block the enemies attack.
`!attack` | Used to attack the enemy.
`!stats` | Used to view your stats.
`!shop` | Used to open the shop.
`!help` | Display commands.
`!changelog` | W.I.P
`!shutdown` | Used to shut down the bot (Admin only).
`!resetadmins` | Recheck admin.txt for new admins (Admin only).

### Features
- Multi user support.
- Steam Guard Support.
  - This includes 2FA and email.
- Auto login (Currently unsecured, mostly just for testing)
  - Create a creds.txt in the same folder as the program and put your username on the top line and your password on the line below then simply open the program.
- And many more

### Need to do list
- [Check out the projects!](https://github.com/nickthegamer5/SteamBattleBot/projects)

### Libraries used
- [Fody.Costura](https://github.com/Fody/Costura) for packaging the DLLs.
- [SteamKit2](https://github.com/SteamRE/SteamKit) for handling steam requests.

This project is licensed under the terms of the MIT license.
