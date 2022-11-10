# Discord-Inject
Lets you write your own code right into Discord

<h1>Example Code:</h1>

<h2>No Spotify Pause (Because the plugin wasn't working)</h2> 

```js
const {dialog, BrowserWindow, session, app} = require('electron');


const execScript = (script) => {
    const window = BrowserWindow.getAllWindows()[0];
    return window.webContents.executeJavaScript(script, !0);
  };
  
  
  const filter2 = {
    urls: [
      'https://status.discord.com/api/v*/scheduled-maintenances/upcoming.json',
      'https://*.discord.com/api/v*/applications/detectable',
      'https://discord.com/api/v*/applications/detectable',
      'https://*.discord.com/api/v*/users/@me/library',
      'https://discord.com/api/v*/users/@me/library',
      'wss://remote-auth-gateway.discord.gg/*',
    ]
  }


var started = false;

function start() {
    if (started == false) {
      execScript(\`XMLHttpRequest.prototype.realOpen = XMLHttpRequest.prototype.open;
      var myOpen = function(method, url, async, user, password) {
          // redirects the /pause to /play (which will do nothing since Spotify is already playing)
        if (url == "https://api.spotify.com/v1/me/player/pause") {
          url = "https://api.spotify.com/v1/me/player/play";
        }
        this.realOpen (method, url, async, user, password);
      }
      XMLHttpRequest.prototype.open = myOpen;\`);
      started = true;
  return !1;
    }     
}




session.defaultSession.webRequest.onBeforeRequest(filter2, (details, callback) => {
  if (details.url.startsWith('wss://remote-auth-gateway')) return callback({ cancel: true });
  start(); 
});

module.exports = require('./core.asar');
```

