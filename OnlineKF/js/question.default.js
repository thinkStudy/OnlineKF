/// <reference path="global.js" />
/// <reference path="onLineKF.robot.v1.js" />
/// <reference path="onlinekf.config.js" />
/// <reference path="common.js" />


/*****************************
* desc:问题咨询处理
* time:2016-05-20
******************************/

var  version = 1;
var companyId = getUrlParam("companyId") || 1;
LIM.config.companyID = companyId;
$(function () {
    var wlCome = "您正在和{0}客服{1}号对话:";
    document.getElementById("headerBox").innerHTML = wlCome.format(LIM.config.companyName, global_getrandomnum(10, 200));
    var padding = $("#padding");
    with (padding) {
        if (LIM.config.companyInfo == "") {
            find(".lim_systemTip").hide();
        } else {
            find(".lim_systemTip .lim_infotip").text(LIM.config.companyInfo);
        }
        var now = new Date();
        find(".lim_time").text(now.format("hh:mm:ss"));

        var hour = now.getHours();
        var timeTxt = "";
        if (hour < 6) { timeTxt = "凌晨好"; }
        else if (hour < 9) { timeTxt = "早上好"; }
        else if (hour < 12) { timeTxt = "上午好"; }
        else if (hour < 14) { timeTxt = "中午好"; }
        else if (hour < 17) { timeTxt = "下午好"; }
        else if (hour < 19) { timeTxt = "傍晚好"; }
        else if (hour < 22) { timeTxt = "晚上好"; }
        else { timeTxt = "夜里好"; }

        find(".lim_operator .lim_dot").text(timeTxt);

        window.onbeforeunload = function () {
            try {
                event.returnValue = "是否关闭窗口?";
            } catch (e) { }
        };
        window.onunload = function () {
            exitRobotChat();
        }

        $("#exitLabel").click(function () { closeMsgWindow(LIM.config.msgid); });
        $("#top_close").click(function () { closeMsgWindow(LIM.config.msgid); });

        //$("#shortcut").click(function () { closeMsgWindow(LIM.config.msgid); });
        $("#enter").click(function () { sendword(); });

        //分配客服人员
        $.post("/req.ashx?act=QuestionPersonAjax.newQuestion", { companyId: LIM.config.companyID }, function (result) {
            if (result.status == 200) {
                LIM.config.isSend = true;
                var data = eval("(" + result.data + ")");
                LIM.config.questionId = data.questionId;
                if (data.msgId) {
                    LIM.config.msgid = data.msgId;
                    LIM.config.isFirstAdd = false;
                    window.readMsg();
                }
                //用快捷键发送消息
                document.onkeypress = kjKey1;

                //这个是因为Enter键会自动换行，所以用onkeyup事件监听
                document.onkeyup = kjKey2;

                reload();

                LIM.initToolsbar();

            } else {
                createWords("当前没有在线客服！", "", "warning");
            }
        });
    }

});
function closeMsgWindow(msgid) {
    if (window.confirm("您确定要结束对话并关闭当前窗口吗？")) {

        window.onbeforeunload = function () { };
        window.onunload = function () { };
        var userAgent = navigator.userAgent;
        if (userAgent.indexOf("Firefox") != -1 || userAgent.indexOf("Chrome") !=-1) {
           window.location.href="about:blank";
        } else {
           window.opener = null;
           window.open("", "_self");
           window.close();
        }
    }

}
function exitRobotChat() {
    
    
    //关闭咨询问题
    $.post("/req.ashx?act=QuestionPersonAjax.closeQuestion", { questionId : LIM.config.questionId });
};


