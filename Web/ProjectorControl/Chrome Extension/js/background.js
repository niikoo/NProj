var  $pStatus = {
  "on": false,
  "muted": false,
  "frozen": false
};

var $pfStatus = $pStatus; // For turning on mode

var popupRunning = false; // Is the popup window / browser_action popup running?

chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
  if(msg.method == "getInfo") {
    getInfo(sendResponse);
  } else if(msg.method == "isRunning") {
    popupRunning = true;
  } else if(msg.method == "notRunning") {
    popupRunning = false;
  } else if(msg.method == "sendCommand") {
    console.log("background.js info: sendCommand log - command received: " + msg.command);
    sendCommand(msg.command,sendResponse);
  }
  return true;
});

function getInfo(sendResponse) {
  if(checkProjStatusRunning) sendResponse(false);
   $.getJSON("http://127.0.0.1/rs-svg/rs-ppkg.php?cmd=status").done(function(json) {
      $pStatus = json.status;
      sendResponse($pStatus);
   }).fail(function(jqxhr, textStatus, error ) {
     var err = textStatus + ", " + error;
     console.log("CMD send failed: " + err);
     showNotification("Klarer ikke kommunisere med projektoren for å hente info.","circle_warning.png");
     sendResponse($pStatus);
   });
   return true;
}

var checkProjStatusResponse = false;
var checkProjStatus_waitForKey = "on";
var checkProjStatus_waitForValue = true;
var checkProjStatusRunning = false;
chrome.alarms.onAlarm.addListener(function(alarm){
  if(alarm.name == "checkProjStatusAlarm")
    checkProjStatus();
});
function checkProjStatus(){ // CHECK FOR PROJECTOR ON
  checkProjStatusRunning = true;
   $.getJSON("http://127.0.0.1/rs-svg/rs-ppkg.php?cmd=status").done(function(json) {
      $pStatus = json.status;
      if($pStatus[checkProjStatus_waitForKey] === checkProjStatus_waitForValue) {
        console.log("no need to wait anymore! status set to " + checkProjStatus_waitForKey + " === " + checkProjStatus_waitForValue);
        checkProjStatusRunning = false;
        checkProjStatusResponse($pStatus);
        checkProjStatusResponse = false;
      } else { 
        console.log("still waiting for the projector to change status to " + checkProjStatus_waitForKey + " === " + checkProjStatus_waitForValue + " -- current:  " + checkProjStatus_waitForKey + " === " + $pStatus[checkProjStatus_waitForKey]);
        chrome.alarms.create("checkProjStatusAlarm", {when:Date.now()+2000});
      }
   }).fail(function(jqxhr, textStatus, error ) {
     var err = textStatus + ", " + error;
     console.log("CMD send failed: " + err);
     showNotification("Klarer ikke kommunisere med projektoren for å hente info.","circle_warning.png");
     checkProjStatusResponse($pStatus);
     checkProjStatusRunning = false;
     checkProjStatusResponse = false;
   });
}
function sendCommand(cmd,sendResponse) {
  if(checkProjStatusRunning) sendResponse(false);
  if(cmd == "on") {
    showNotification("Projektoren slår seg på. Dette kan ta noen minutter","metro-projector.png");
    console.log("waiting until the projector is turned on");
    checkProjStatusResponse = sendResponse;
    checkProjStatus_waitForKey = "on";
    checkProjStatus_waitForValue = true;
    chrome.browserAction.setBadgeBackgroundColor({ color: [0,255, 0, 255] });
    chrome.browserAction.setBadgeText({text: 'ON'});
    checkProjStatus();
    sendResponse = function(doNotCare) { return true; }
  } else if(cmd == "off") {
    showNotification("Projektoren slår seg av","metro-shutdown.png");
    checkProjStatusResponse = sendResponse;
    checkProjStatus_waitForKey = "on";
    checkProjStatus_waitForValue = false;
    chrome.browserAction.setBadgeBackgroundColor({ color: [255, 0, 0, 255] });
    chrome.browserAction.setBadgeText({text: 'OFF'});
    checkProjStatus();
    sendResponse = function(doNotCare) { return true; }
  } else if(cmd == "mute") {
    // TOGLLE
    if($pStatus['muted']) {
      chrome.browserAction.setBadgeBackgroundColor({ color: [0,255, 0, 255] });
      chrome.browserAction.setBadgeText({text: 'ON'});
      showNotification("Skjuler ikke lenger bildet","metro-restart.png");
    } else {
      chrome.browserAction.setBadgeBackgroundColor({ color: [0, 0, 255, 255] });
      chrome.browserAction.setBadgeText({text: 'M'});
      showNotification("Skjuler bildet. (Viser kun en svart skjerm)","metro-sleep.png");
    }    
  } else if(cmd == "freeze") {
    // TOGLLE
    if($pStatus['frozen']) {
      chrome.browserAction.setBadgeBackgroundColor({ color: [0,255, 0, 255] });
      chrome.browserAction.setBadgeText({text: 'ON'});
      showNotification("Bildet er nå tint opp =)","metro-restart.png");
    } else {
      chrome.browserAction.setBadgeBackgroundColor({ color: [0, 0, 255, 255] });
      chrome.browserAction.setBadgeText({text: 'F'});
      showNotification("Bildet er nå fryst.","metro-lock.png");
    }
  }
  console.log("JSON REQUEST: " + "http://127.0.0.1/rs-svg/rs-ppkg.php?cmd=" + cmd);
  $.getJSON("http://127.0.0.1/rs-svg/rs-ppkg.php?cmd=" + cmd).done(function(json) {
    $pStatus = json.status;
    sendResponse($pStatus);
  }).fail(function(jqxhr, textStatus, error ) {
    console.log(jqxhr);
    var err = textStatus + ", " + error;
    console.log("CMD send failed: " + err);
    showNotification("Klarer ikke kommunisere med projektoren for å sende kommando.","circle_warning.png");
    sendResponse($pStatus);
  });
}

// Declare a variable to generate unique notification IDs
var notID = 0;

function showNotification(text,iconName) {
    var notification = {
      type: "basic",
      title: "Projektorstyring",
      message: text,
      iconUrl: "/image/notifications/" + iconName,
      priority: 2
    };
    chrome.notifications.create("id"+notID++, notification, function(notID){
      setTimeout(function() {
  			chrome.notifications.clear(notID, function(wasCleared) {
  				console.log("Notification " + notID + " cleared: " + wasCleared);
  			});
  		}, 3000);
    });
}

chrome.commands.onCommand.addListener(function(command) {
  if(popupRunning) {
    showNotification("Hurtigtastene er slått av så lenge kontrollene vises.","metro-projector.png");
  } else {
    if(command == "toggle-mute") {
      sendCommand("mute",function(){});
    } else if(command == "toggle-freeze") {
      sendCommand("freeze",function(){});
    }
  }
});

/*chrome.app.runtime.onLaunched.addListener(function() {
  chrome.app.window.create('html/window.html', {
    'bounds': {
      'width': 350,
      'height': 500
    }
  });
  */