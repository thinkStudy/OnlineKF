/// <reference path="onlinekf.config.js" />
/// <reference path="question.default.js" />
/// <reference path="serviceperson.default.js" />


var firstSpeak = ""; //问候语
var inputField = "inputbox"; //问题输入框ID
var chatDIV = 'padding'; //显示聊天内容层ID
var username = '您说'; //用户名
var isAnswer = true; //判断用户的问题服务器是否做出了响应
var lastTime;  //用户最后一次问问题的时间
var txtSize = 200;  //提交的问题的最大长度
var isIE = false;  //浏览器设定是不是IE
var isMoz = false; //浏览器的版本是不是Opera
var old = "";
var oldContentsArry = {}; //记录咨询用户的聊天信息
var globalRobotState = null; // live800机器人的状态
var robotLangs = ["zh-CN", "zh-TW", "en", "ja", "ko", "vi"];

var globalChatHandle = {
    ieFixRadius: ieCopyright <= 8,
    lastShowTime: getCurrentTime()
};

function reload(welcomeWord) {

    //检测用户浏览器是哪中类型（支持两种  IE 和 Opera）
    var sUserAgent = navigator.userAgent;
    isIE = (sUserAgent.indexOf("compatible") > -1) && (sUserAgent.indexOf("MSIE") > -1);
    isMoz = sUserAgent.indexOf("Gecko") > -1;
    //当加载的时候记录用户登录的时间
    lastTime = getCurrentTime();
    //聚焦在文本输入框
    document.getElementById(inputField).focus();
};

//往聊天栏添加聊天内容或者警告
function createWords(words, model, type) {

    model ? model : (model = "machine"); //默认为机器人消息
    var old = document.getElementById(chatDIV).innerHTML,content;

    if (model == "machine") {
        //类型为警告
        if (type == 'warning') {
            var speakContent = constructHistStyle(words, "systemTip", (new Date()).getTime());
            old = old + speakContent;
        } else {
            old = old + constructHistStyle(words, "operator", (new Date()).getTime(), null, true);
        }
    } else {	//当用户问问题的时候就记录下他这次问问题的时间
        if (!words) return null;
        old = old + constructHistStyle(words, model, (new Date()).getTime());
    }
    document.getElementById(chatDIV).innerHTML = old;
    //控制滚动条
    controlScrrol();
};
function getCurrentTime() {
    return ((new Date).getTime());
}
function constructHistStyle(content, pclass, time, id, isFilterHTMl) {
    var str = "", idstr = "", timestr = "", handle = globalChatHandle, jsonObj = content;
    if (typeof id != "undefined" && id != null && id.length > 0) {
        idstr = "id='" + id + "'";
    }
    time = parseInt(time || getCurrentTime());
    if (LIM.config.mesBubble == "2") {//bubble
        //不允许显示空消息内容。
        if (content.replace(/[\s\u3000]*/g, '') == '') return false;
        if (pclass == "visitor" || pclass == 'operator') {
            if (time - handle.lastShowTime > 60000) {
                timestr = '<div class="lim_time" >' + (new Date(time)).format('hh:mm:ss') + '</div>';
                handle.lastShowTime = time;
            }
            str = timestr + '<div class="lim_' + pclass + ' lim_clearfloat "><div class="lim_bubble ' + (handle.ieFixRadius ? "" : "lim_shadow") + '"><div class="lim_dot">'
				+ content
				+ (handle.ieFixRadius ? '</div><div class="lim_radius lim_r2"></div><div class="lim_radius lim_r3"></div><div class="lim_radius lim_r4"></div></div><div class="lim_radius lim_r1"></div></div>' : '</div></div><div class="lim_tale"><div id="radiusborder"></div></div></div>');
        }
        else {
            var dir = "lim_operator";
            if (pclass == "userinfo") dir = "lim_visitor";
            if (pclass == "systemTip") dir = "lim_systemTip";
            str = '<div  class="' + dir + ' lim_clearfloat " ' + idstr + '><div class="lim_bubble ' + (handle.ieFixRadius ? "" : "lim_shadow") + '"><div class="lim_infotip">' + content
			+ (handle.ieFixRadius ? '</div><div class="lim_radius lim_r2"></div><div class="lim_radius lim_r3"></div><div class="lim_radius lim_r4"></div></div><div class="lim_radius lim_r1"></div></div>' : '</div></div><div class="lim_tale"><div id="radiusborder"></div></div></div></div>')
			+ '</span></div>'
        }	
    } else {
        timestr = (new Date(time)).format('hh:mm:ss');
        if (pclass == "visitor") {
            if (typeof (globalChatState) == "undefined" || globalChatState != "CHATER") {
                globalChatState = "CHATER";
                str = "<div class='visitor'><p class='say'>" + LIM.localRes["yousay"] + "<label class='time'>" + "(" + timestr + ")" + "</label></p><div class='dot'>" + content + "</div></div>";
            } else {
                str = "<div class='visitor series'><div class='dot'>" + content + "<div></div>";
            }
        }
        else if (pclass == "operator") {
            if (typeof (globalChatState) == "undefined" || globalChatState != "OPERATOR") {
                globalChatState = "OPERATOR";
                str = "<div class='operator'><p class='say'>" + globalChatHandle.operatorName + LIM.localRes["sayre"] + "<label class='time'>" + "(" + timestr + ")" + "</label></p><div class='dot'>" + content + "</div></div>";
            } else {
                str = "<div class='operator series'><div class='dot'>" + content + "</div></div>";
            }
        } else {
            str = "<div class='info' " + idstr + ">" + content + "</div>";
        }

    }
    return str;
}




function htmlFilter(content) {
    var t = content.replace(/<(p|li|h[1-6])*>/ig, "<br>");
    t = t.replace(/(\r|\n)/ig, "");
    t = t.replace(/<(?!(img|a|\/a|br))[^<>]*>/ig, ""); //去掉除img,a,br之外的所有标签;
    return t;
}

//从表单发送问题
function sendword(content, type) {
    if (!LIM.config.msgid && !LIM.config.isFirstAdd) { return false; }
    if (globalRobotState == "END") {
        globalRobotState = null;
    }
     type = type || LIM.config.sendType.txt;
    var txt = content || document.getElementById(inputField).value;
    txt = trim(txt);
    if (txt.length >= txtSize) {
        var warning = "您好！问题的长度超过了最大限制，请简化一下再提交。";
        createWords(warning, 'machine', 'warning');
        return;
    }
    //判断是否输入为空
    if (txt == "") {
        return;
    }
    //过滤除img以外的所有html节点,非节点内容照常发送
    txt = (function (s) {
        //s = s.replace(/<\/(div|p|DIV|P)>/g,"\n");
        s = s.replace(/<([^img])[^>]*>/ig, "");
        s = s.replace(/&nbsp;/ig, " ");
        return s;
    })(txt);
    //经过上面方法过滤后内容为空的话，不发送信息，也不提示用户。
    if (txt == "") {
        return;
    }
    if (!content) {
        document.getElementById(inputField).value = "";
        document.getElementById(inputField).focus();
    }
    var newTxt = txt;
    for (var i = 0; i < txt.length; i++) {
        newTxt = newTxt.replace("\n", "<br />");
    }

    var modelType = LIM.config.reloadType == 0 ? "userinfo" : "operator";
    createWords(newTxt, modelType);
    //将用户输入内容先编码，然后发送

    txt = encodeURIComponent(txt);

    lastTime = getCurrentTime();

    LIM.config.lastTime = (new Date()).format("yyyyMMddhhmmss")+global_getrandomnum(0,100);
    var postUrl = LIM.config.sendPostUrl;
    var postData = { questionId: LIM.config.questionId, content: txt, type: type, time: LIM.config.lastTime, isFirstAdd: LIM.config.isFirstAdd,addType:LIM.config.reloadType };
    justSendpage(postUrl, postData);
    if (LIM.config.reloadType == 1) {
        //记录快捷回复的使用次数
        var inputBox = $("#" + inputField);
        var agileId = inputBox.attr("agileId")
        if (agileId > 0) {
            var useNumber = inputBox.attr("useNumber")
            useNumber = parseInt(useNumber) + 1;

            $.post("req.ashx?act=AgileMessageAjax.logUseNumber", { id: agileId, usenumber: useNumber });

            $(".con-r li[agileid=" + agileId + "]").attr("usenumber",useNumber);
            clearInputBox();
        }
    }
};

function justSendpage(url, postData) {

    try {
        $.post(url, postData, function (result) {
            if (result.status == 200) {
                
                if (LIM.config.isFirstAdd) {
                    var msgId = eval("(" + result.data + ")").id;
                    LIM.config.msgid = msgId;
                    window.readMsg();
                    LIM.config.isFirstAdd = false;
                }

            }
        });

    } catch (e) {
        robotOnError();
    }
};
function robotOnError() {
    createWords("<p>发生错误，请与技术支持人员联系!</p>", 'machine');
    //将问题设置为已回答状态
    isAnswer = true;
};
function clearInputBox() {
    var inputBox = $("#" + inputField);
    inputBox.attr("agileId", 0)
    inputBox.attr("useNumber", 0)
    inputBox.val("");
}

function clearAllContent() {
    
    oldContentsArry[LIM.config.msgid] = document.getElementById(chatDIV).innerHTML;

    document.getElementById(chatDIV).innerHTML = "";

    $("#body_wrap #nameBox").text("");

    clearInputBox();
}

function d() {
    document.getElementById("enter").innerHTML = g.send_button;
    document.getElementById("exitLabel").innerHTML = g.close;
    Holder.changeHolder({
        element: document.getElementById("inputbox"),
        holder: g.inputboxTip,
        attributes: {
            name: "id",
            value: "holderTip"
        }
    });
    if (typeof globalChatHandle == "undefined") {
        return
    }
    var i = document.getElementById("shortKeyMenuUl").getElementsByTagName("li");
    if (globalChatHandle.sendButtonShortcut == "Enter") {
        i[0].innerHTML = g.shortkey1 + " *";
        i[1].innerHTML = g.shortkey2
    } else {
        i[0].innerHTML = g.shortkey1;
        i[1].innerHTML = g.shortkey2 + " *"
    }
    var k = document.getElementById("headerBox").innerText;
    var j = parseInt(k.replace(/[^0-9]+/g, ""));
    if (j != NaN && j != null && j >= 1) {
        if (globalChatHandle.chatStatus == "WAIT") {
            document.getElementById("headerBox").innerHTML = g.psinfo.replace(/ps/, j)
        }
    } else {
        if (globalChatHandle.chatStatus == "WAIT") {
            document.getElementById("headerBox").innerHTML = g.towaiting
        }
    }
    if (globalChatHandle.chatStatus == "CHAT" || globalChatHandle.chatStatus == "INSTANT") {
        document.getElementById("headerBox").innerHTML = g.italking.replace(/xx/g, globalChatHandle.operatorName)
    }
    if (globalChatHandle.chatStatus == "END") {
        document.getElementById("headerBox").innerHTML = g.dialogovered
    }
}






//监听的onkeypress事件
function kjKey1() {
    if (event && event.keyCode) {
        if (event.keyCode == getkjjKeyCode()) {
            sendword();
        }
    }
};

//监听onkeyup事件
function kjKey2() {
    if (event && event.keyCode) {
        if (event.keyCode == getkjjKeyCode()) {
            sendword();
        }
    }
};

//找出快捷键在IE浏览器下的键值
function getkjjKeyCode() {
    if (LIM.config.sendKjj  == "CE") {
        //Ctrl + Enter的键值
        return 10;
    } else {
        //Enter的键值
        return 13;
    }
};



//去掉空格
function trim(str) {
    return str.replace(/^\s*|\s*$|\n*$|\r*$|\r\n*$/g, "");
};
//去掉 & 符号
function hideSomeSign(str) {
    while (str.indexOf('&') >= 0) {
        str = str.replace('&', '');
    }
    return str;
};

//给用户一个随机名
function getUsername() {
    var num = parseInt(10000 * Math.random());
    var name = "访客" + num;
    return name;
};

//控制层的滚动条总是在最底端
function controlScrrol() {
    if (typeof isMobile != "undefined" && isMobile) {
        globalChatHandle.scrollHistoryToBottom();
        return;
    }
    var chatDivObj = document.getElementById(chatDIV).parentNode;
    var h = chatDivObj.clientHeight + chatDivObj.scrollHeight;
    if (h > 0) {
        chatDivObj.scrollTop = h;
    }

};


