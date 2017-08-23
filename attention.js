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

    var number;

    for(var i = 1; i <= 9; i++) {

      if(parseInt(Message[2]) == i) {
        number = Message[2];
        break;
      } else if(parseInt(Message[1]) == i) {
        number = Message[1];
        break;
      } else
        number = Number.toString();
    }

    var letter;

    for(var i = 0; i <= 9; i++) {

      if(Message[1] == Letter[i]) {
        letter = Message[1];
        break;
      } else if(Message[2] == Letter[i]) {
        letter = Message[2];
        break;
      } else
        letter = Letter[Math.trunc(Math.random() * 10)];
    }

    var Text = ["Attention to the designated grid square!","Attention to the designated grid zone!","Attention to the map!"];

    var text = Text[Math.trunc(Math.random() * 3)];

    message.channel.send(text + " " + "(" + letter + number + ")");

  }
});