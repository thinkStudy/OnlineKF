/// <reference path="onlinekf.config.js" />
/// <reference path="jquery-1.9.1.min.js" />
/// <reference path="global.js" />
/// <reference path="plugins/bind_data_util.js" />
/// <reference path="read.message.js" />

$(function () {

    window.onbeforeunload = function () {
        try {
            event.returnValue = "是否关闭窗口?";
        } catch (e) { }
    };
    window.onunload = function () {
        exitRobotChat();
    }

    setTimeout(showQuestionPerson, LIM.config.readSpeed);

    AgileMessageUtils.loadAgileMsgData();

    $("#exitLabel").click(function () { closeMsgWindow(LIM.config.msgid); });
    $("#top_close").click(function () { closeMsgWindow(LIM.config.msgid); });

    //$("#shortcut").click(function () { closeMsgWindow(LIM.config.msgid); });
    $("#enter").click(function () { sendword(); });

    //心跳包，5分钟自动保存当前客服在线
    setTimeout(autoSaveOnline, 1000 * 300);

});

//验证当前客服是否在线
function autoSaveOnline() {
    $.post("/req.ashx?act=LoginAjax.autoSaveOnline", {}, function (result) {
        if (result.status == 500) {
            window.onbeforeunload = function () { };
            window.onunload = function () { };
            location.href = "/login.html";
        } else {
            setTimeout(autoSaveOnline, 1000 * 300);
        }
    });
}

function exitRobotChat() {

    //关闭咨询问题
    $.post("/req.ashx?act=LoginAjax.loginOut");
};
LIM.config.isFirstAdd = false;
LIM.config.reloadType = 1;
function loginOut() {
    //关闭咨询问题
    $.post("/req.ashx?act=LoginAjax.loginOut", {}, function () {
        location.href = "/login.html";
    });
}
var global_question_data;
//加载咨询人员的左侧栏
function showQuestionPerson() {
    $.post("/req.ashx?act=MessageDataAjax.servicePersonOnlyCount", {}, function (result) {
        if (result.status == 200) {
            var msgData = eval("(" + result.data + ")");
            global_question_data = msgData;
            var divPanl = $(".con-l>.tabs");
            divPanl.find(".onlycount").text("");
            $.each(msgData, function (i, model) {
                var oddLi = divPanl.find("li[msgid=" + model.id + "]");
                var name = "咨询客户" + (divPanl.find("li").length);
                var oddclass = "";
                if (oddLi && oddLi.length > 0) {
                    name = oddLi.find(".questionName").text();
                    oddclass = oddLi.attr("class");
                }
                oddLi.remove();
                var li = '<li msgid="' + model.id + '" lastTime="0" questionid="' + model.questionid + '" class="' + oddclass + '"  onclick ="showQuestionInfo(this)" > <p class="left-li1">' +
                            '<span class="questionName" >' + name +
                            '</span> <span class="onlycount">(' + model.onlycount + ')</span></p>' +
                            '<div class="agile_close" style="display:block" onclick="closeMsgWindow('+model.id+')">×</div>' +
                        '</li>';
                var liObj = $(li);
                
                divPanl.find("li:eq(0)").after(liObj);

            });

        }
        setTimeout(showQuestionPerson, LIM.config.readSpeed);
    });

}

//加载当前咨询人员的信息
function showQuestionInfo(objThis) {

    var liThis = $(objThis);
    objThis = $(objThis).find(".questionName");

    var divPanl = $(".con-l>.tabs");

    var divLeftPanl = $(".con-l>.tabs");
    var currentLi = divLeftPanl.find("li[msgid=" + liThis.attr("msgid") + "]");

    if (!currentLi.length) { return false; }

    var oddLi = divPanl.find(".current");
    if (oddLi && oddLi.length > 0) {
        oddLi.removeClass("current");
        oddLi.attr("lastTime", LIM.config.lastTime);
    }
    clearAllContent();

    liThis.addClass("current");
    LIM.config.msgid = liThis.attr("msgid");
    LIM.config.questionId = liThis.attr("questionid");
    LIM.config.lastTime = liThis.attr("lastTime");
    $("#body_wrap #nameBox").text(objThis.text());

    //显示聊天记录
    if (oldContentsArry[LIM.config.msgid] && LIM.config.lastTime > 0) {
        document.getElementById(chatDIV).innerHTML = oldContentsArry[LIM.config.msgid];
    }

    if (LIM.config.isFirstReady == true) {
        LIM.config.isFirstReady = false;

        window.readMsg();

        //用快捷键发送消息
        document.onkeypress = kjKey1;

        //这个是因为Enter键会自动换行，所以用onkeyup事件监听
        document.onkeyup = kjKey2;
        
        LIM.initToolsbar();
    }


}

function closeMsgWindow(msgid) {
    
    if (!msgid) {return false;}
    var divLeftPanl = $(".con-l>.tabs");
    var currentLi = divLeftPanl.find("li[msgid=" + msgid + "]");
    
    //有未读消息时，禁止删除对话框
    if (currentLi.find(".onlycount").text() != "") {return false;}
    
    if (currentLi.hasClass("current")) {
        clearAllContent();
        LIM.config.msgid = 0;
    }
    currentLi.remove();
    
}

/*******************************
  desc:快捷回复操作类
*******************************/
var AgileMessageUtils = {
    loadAgileMsgData: function () {
        //加载快捷回复数据
        $.post("req.ashx?act=AgileMessageAjax.queryAgileMessage", {}, function (result) {
            if (result.status == 200) {

                var resultData = eval("(" + result.data + ")");
                $.each(resultData, function (i, data) {
                    var divPanl = $(".con-r>.tabs");
                    divPanl.find("li:eq(" + divPanl.find(".addAgileMsg").length + ")").after(AgileMessageUtils.createLiHtml(data));
                });
            }
        });
    },
    createLiHtml: function (data) {
        var li = '<li agileId="' + data.id + '" useNumber="' + data.usenumber + '"   onclick ="AgileMessageUtils.useAgileMsg(this)" > <p class="left-li1" >' +
                            '<span class="questionName" >' + data.messagetxt +
                            '</span> </p>' +
                            '<div class="agile_close" onclick="AgileMessageUtils.delAgileMsg(this)">×</div>' +
                        '</li>';
        var liObj = $(li);
        liObj.mouseover(function () {
            $(this).find(".agile_close").show();
        });
        liObj.mouseout(function () {
            $(this).find(".agile_close").hide();
        });
        return liObj;

    },
    //添加快捷回复
    addAgileMsg: function () {
        var divPanl = $(".con-r>.tabs");

        var li = '<li style="height:auto;    text-align: center;" > ' +
                        '<textarea class="addAgileMsg" placeholder="请输入"></textarea>' +
                        '<a href="javascript:void()"  onclick="AgileMessageUtils.execAddAgile(this)">确定</a>&nbsp;&nbsp;&nbsp;<a href="javascript:void()" onclick="$(this).parent().remove()">取消</a>' +
                 '</li>';

        divPanl.find("li:eq(0)").after($(li));

    },
    execAddAgile: function execAddAgile(objThis) {

        objThis = $(objThis);
        var liThis = objThis.parent();
        var msgtxt = $.trim(liThis.find(".addAgileMsg").val());
        if (msgtxt == "") {
            alert("请输入快捷回复内容！")
            return false;
        }
        $.post("req.ashx?act=AgileMessageAjax.addAgileMessage", { agileMsg: msgtxt }, function (result) {
            if (result.status == 200) {
                liThis.remove();
                var data = eval("(" + result.data + ")");
                data.messagetxt = msgtxt;
                data.usenumber = 0;

                var divPanl = $(".con-r>.tabs");
                divPanl.find("li:eq(" + divPanl.find(".addAgileMsg").length + ")").after(AgileMessageUtils.createLiHtml(data));
            } else {
                alert("添加失败！ " + result.msg);
            }
        });
    },
    useAgileMsg: function (objThis) {
        objThis = $(objThis);
        var sendBox = $("#body_wrap #inputbox");
        sendBox.val(objThis.find("span").text());
        sendBox.attr("useNumber", objThis.attr("useNumber"));
        sendBox.attr("agileId", objThis.attr("agileId"));
    },
    delAgileMsg: function (objThis) {
        objThis = $(objThis);
        var liThis = objThis.parent();
        var cf = confirm("是否确认删除！")
        if (cf == false) {
            return false;
        }
        var agileId = liThis.attr("agileId");
        $.post("req.ashx?act=AgileMessageAjax.delAgileMessage", { id: agileId }, function (result) {
            if (result.status == 200) {
                liThis.remove();
            } else {
                alert("添加失败！ " + result.msg);
            }
        });
    }

}