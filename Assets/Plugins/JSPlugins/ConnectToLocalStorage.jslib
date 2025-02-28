mergeInto(LibraryManager.library, {

  // Get URL of the websocket by retrieving IP and port from LocalStorage
  getWsUrl: function() {
    const settings = JSON.parse(localStorage.getItem('settings'));
    const ip = settings && settings.ros && settings.ros.IP && settings.ros.IP !== "" ? settings.ros.IP : "localhost";
    const port = settings && settings.ros && settings.ros.port && settings.ros.port !== "" ? settings.ros.port : "9090";
    const url = "ws://" + ip + ":" + port;
    var bufferSize = lengthBytesUTF8(url) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(url, buffer, bufferSize);
    return buffer;
  },

  // Get joint states topic
  getTopic: function() {
    const settings = JSON.parse(localStorage.getItem('settings'));
    const topic = settings && settings.topics && settings.topics.humanJoints && settings.topics.humanJoints !== "" ? settings.topics.humanJoints : "/suit0/joints_state";
    var bufferSize = lengthBytesUTF8(topic) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(topic, buffer, bufferSize);
    return buffer;
  },

  // Get Character selected from LocalStorage
  getCharacterIndex: function() {
    const settings = JSON.parse(localStorage.getItem('settings'));
    const index = settings && settings.characterStyle && settings.characterStyle !== "" ? settings.characterStyle : 1;
    return index;
  },

  // Get custom character filepath to .glb file
  getCharacterFilePath: function() {
    const settings = JSON.parse(localStorage.getItem('settings'));
    const filepath = settings && settings.avatarFilepath && settings.avatarFilepath !== "" ? settings.avatarFilepath : "";
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