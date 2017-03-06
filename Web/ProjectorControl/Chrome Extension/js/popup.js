hideAllUserUIElements(); // HIDE ALL ON LOAD

function hideAllUserUIElements() {
  $("#proj-controlbtn-holder").hide();
  $(".proj-show-off").hide();
  $(".proj-show-on").hide();
  $(".proj-show-turning-on").hide();
  $(".proj-show-mute-on").hide();
  $(".proj-show-mute-off").hide();
  $(".proj-show-freeze-on").hide();
  $(".proj-show-freeze-off").hide();
  $(".proj-status-holder > div").hide();
  $(".proj-show-status-on").hide();
}

function enableButtons() {
  $("button").attr("disabled", false);
  localStorage.cmdRunning = false;
}

function disableButtons() {
   $("button").attr("disabled", true);
   localStorage.cmdRunning = true;
}

addEventListener("unload", function (event) {
    chrome.runtime.sendMessage({method:"notRunning"});
}, true);
addEventListener("load", function (event) {
    chrome.runtime.sendMessage({method:"isRunning"});
}, true);


/*

RECEIVE INFO ON STARTUP

*/

var projectorInfo = {
  "on": false,
  "muted": false,
  "frozen": false
};

function getInfo() {
  chrome.runtime.sendMessage({method:"getInfo"},function(response){
    disableButtons();
    if(response) {
      projectorInfo = response;
      console.log(projectorInfo);
      setStatus();
    } else {
      setStatus(true);
    }
  });
}

getInfo(); // Run on startup

chrome.alarms.onAlarm.addListener(function(alarm){
  if(alarm.name == "checkProjTurningOnAlarm")
    getInfo(); // Check on status
});

function setStatus(turningOnMode,updaterRunning) {
  hideAllUserUIElements();
  $("#proj-controlbtn-holder").show();
  if(projectorInfo['on'] === true && turningOnMode !== true) {
    $(".proj-show-on").show();
    $(".proj-show-status-on").show();
    chrome.power.requestKeepAwake("display"); // KEEPS DISPLAY AWAKE AND SYSTEM RUNNING WHILE THE PROJECTOR IS ON
  } else {
    $(".proj-show-status-on").hide();
    if(turningOnMode) { // TURNING ON MODE
      $(".proj-show-turning-on").show();
      $("#proj-controlbtn-holder").hide();
      disableButtons();
      chrome.power.releaseKeepAwake(); // ALLOW POWER MANAGEMENT WHEN THE PROJECTOR IS OFF OR TURNING ON
      if(updaterRunning !== true)
        chrome.alarms.create("checkProjTurningOnAlarm", {when:Date.now()+2000});
    } else { // OFF MODE
      $(".proj-show-off").show();
      enableButtons();
    }
    return true; // No other options should be available
  }
  if(projectorInfo['muted'] === true) {
    $(".proj-show-mute-on").show();
    $(".proj-show-mute-off").hide();
    $(".proj-show-frozen").hide(); // HIDE FREEZE CONROLS WHEN MUTED TO AVOID CONFUSION
    $(".proj-show-status-on").hide(); // HIDE ON STATUS WHEN THERE'S OTHER FUNCTIONS ACTIVE
  } else {
    $(".proj-show-mute-on").hide();
    $(".proj-show-mute-off").show();
    $(".proj-show-frozen").show(); // SHOW FREEZE CONTROLS WHEN MUTE == OFF
  }
  if(projectorInfo['frozen'] === true) {
    $(".proj-show-freeze-on").show();
    $(".proj-show-freeze-off").hide();
    $(".proj-show-muted").hide(); // HIDE MUTE CONTROLS WHEN FROZEN TO LIMIT CONFUSION
    $(".proj-show-status-on").hide(); // HIDE ON STATUS WHEN THERE'S OTHER FUNCTIONS ACTIVE
  } else {
    $(".proj-show-freeze-on").hide();
    $(".proj-show-freeze-off").show();
    $(".proj-show-muted").show(); // SHOW MUTE CONTROLS WHEN FREEZE == OFF
  }
  enableButtons();
}

$("#proj-controlbtn-holder button.btn").click(function(e) {
  // Control button click!
  var $pCommand = $(this).attr("PPKGcmd");
  console.log("controlbtn clicked! trying to send PPKGcmd:" + $pCommand);
  sendCommand($pCommand);
});


/*

SEND COMMAND TO BACKGROUND.JS

*/
function sendCommand(cmd) {
  disableButtons();
  if(typeof(cmd) != 'undefined' && cmd == "on") setStatus(true,true); // TURNING ON MODE
  chrome.runtime.sendMessage({method:"sendCommand",command:cmd},function(response){
    if(response) {
      projectorInfo = response;
      console.log("sendCommand: success");
      setStatus();
    } else {
      setStatus(true,true);
    }
  });
}

/*

TOOLTIPS

*/

$(function () {
  $('[data-toggle="tooltip"]').tooltip();
});


/*

CONFIRM SHUTDOWN

*/
$('#proj-btn-off').unbind('click').on('click', function(e){
    e.preventDefault();
    $('#confirm').modal({ backdrop: 'static', keyboard: true, show:true })
        .one('click', '#confirm-turnoff', function (e) {
              console.log("Confirmed turn off!");
              sendCommand("off");
        });
});

/*

TEST

*/

chrome.tabs.getAllInWindow(null, function(tabs){
    for (var i = 0; i < tabs.length; i++) {
    chrome.tabs.sendRequest(tabs[i].id, { action: "xxx" });                         
    }
});