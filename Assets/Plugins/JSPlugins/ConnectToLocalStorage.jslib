mergeInto(LibraryManager.library, {

  // Get URL of the websocket by retrieving IP and port from LocalStorage
  getWsUrl: function() {
    const settings = JSON.parse(localStorage.getItem('settings'));
    const ip = settings && settings.websocket ? settings.websocket.ip : "localhost";
    const port = settings && settings.websocket ? settings.websocket.port : "9090";
    const url = "ws://" + ip + ":" + port;
    var bufferSize = lengthBytesUTF8(url) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(url, buffer, bufferSize);
    return buffer;
  },

  // Get Character selected from LocalStorage
  getCharacterIndex: function() {
    const settings = JSON.parse(localStorage.getItem('settings'));
    const index = settings.characterStyle || 1;
    return index;
  },

  // Get custom character filepath to .glb file
  getCharacterFilePath: function() {
    const settings = JSON.parse(localStorage.getItem('settings'));
    const filepath = settings.avatarFilepath || "";
    var bufferSize = lengthBytesUTF8(filepath) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(filepath, buffer, bufferSize);
    return buffer;
  },

  // Get webpage hostname and port
  getPageUrl: function() {
    var ip = window.location.hostname;
    var port = window.location.port;
    var url = "http://" + ip + (port ? ":" + port : "") + "/";
    var bufferSize = lengthBytesUTF8(url) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(url, buffer, bufferSize);
    return buffer;
  }

});
