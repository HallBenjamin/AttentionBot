const Discord = require("discord.js");

const client = new Discord.Client();

const token = 'MzQ2MDY0OTkwMTUyODE4Njkw.DHEZrg.PWW68ODzek95ma8lX8PmdcC2kaI';

const prefix = '\\';

var a = 0;
client.login(token, errorOutput);

function errorOutput(error, token) {
  if(error) {
    console.log('Error: ' + error);
    return;
  }
}

client.on('ready', () => {
  console.log('Running');

});

client.on('message', message => {

  if (message.content.startsWith(prefix)) {

    var Number = Math.trunc(Math.random() * 9 + 1);
    var Letter = ['A','B','C','D','E','F','G','H','I','J'];

    var Message = message.content.split('');

    var Offline = message.content.split('e')[1];
    if(Offline == 'xit' && message.author.id == 213057020390146048) {
      client.destroy();
      process.exit();
    }

    var number = Number.toString();

    for(var i = 1; i <= 9; i++) {

      if(parseInt(Message[2]) == i) {
        number = Message[2];
        break;
      } else if(parseInt(Message[1]) == i) {
        number = Message[1];
        break;
      }
    }

    var letter = Letter[Math.trunc(Math.random() * 10)];

    for(var i = 0; i <= 9; i++) {

      if(Message[1] == Letter[i]) {
        letter = Message[1];
        break;
      } else if(Message[2] == Letter[i]) {
        letter = Message[2];
        break;
      }
    }

    var Text = ["Attention to the designated grid square!","Attention to the designated grid zone!","Attention to the map!"];

    var text = Text[Math.trunc(Math.random() * 3)];

    message.channel.send(text + " (" + letter + number + ")");

  }
});