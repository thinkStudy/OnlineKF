/// <reference path="global.js" />
/// <reference path="question.default.js" />
/// <reference path="onLineKF.robot.v1.js" />
/// <reference path="jquery-1.9.1.min.js" />

/////////////////////////////////////////
// author: 左都谷
// description：获取聊天咨询内容
// date:2016.5.23
/////////////////////////////////////////
window.readMsg = function () {
    var reloadType = LIM.config.reloadType; //读取数据类型：0咨询人员 1 客服人员

    function reloadMsg() {
        if (LIM.config.msgid < 1) {
            setTimeout(reloadMsg, LIM.config.readSpeed);
            return false;
        }
        var url = "/req.ashx?act=MessageDataAjax.queryQuestionPerson";
        if (reloadType == 1) {
            url = "/req.ashx?act=MessageDataAjax.queryServicePersonMsg";
        }

        $.post(url, { msgid: LIM.config.msgid }, function (result) {
            if (result.status == 200 && result.data) {
                var data = eval("(" + result.data + ")");
                var msgdata = eval("[" + data.message + "]");
                var readIndex = 0;
                var allShow = LIM.config.lastTime == 0;
                $.each(msgdata, function (i, model) {
                    var readLast = LIM.config.lastTime;

                    var modelData = model.qm ? model.qm : model.sm;
                    var showType = model.qm ? "userinfo" : "operator";
                    var lastT = modelData.d;
                    if (allShow) {
                        questionShow(modelData, showType);
                    }
                    else if (lastT == readLast) {
                        readIndex++;
                    } else if (readIndex > 0) {
                        questionShow(modelData, showType);
                        readIndex++;
                    }

                });
            }
            setTimeout(reloadMsg, LIM.config.readSpeed);

        });
    }

    //咨询人员，显示咨询信息
    function questionShow(data, showType) {
        //{sm:{m:"客服发言",t:"[0 文字 1图片 2文件]"},qm:{m:"咨询人员发言",t:"[0 文字 1图片 2文件]"}} 
        //data = { sm: { m: "客服发言", t: "[0 文字 1图片 2文件]"} };
        switch (data.t) {
            case 0:
                createWords(decodeURIComponent(data.m), showType);
                LIM.config.lastTime = data.d;
                break;
            default:
                if ($(globalCommonChatHandle.historyBox).find('a[fileLastTime="' + data.d + '"]').length == 0) {
                    createWords(createFileHtml(data), showType);
                    LIM.config.lastTime = data.d;
                }
                break;

        }

    }
    function createFileHtml(data) {
        var msg = decodeURIComponent(data.m);
        msg = msg.split(" ");
        var srcPath = LIM.config.resourcePath;
        var htmlString = "";
        if (data.t == 1) {
            type = 1;
            //msg 存储图片方式： “editfile/2016-05-28/36f658c1-9f18-4f5b-aa93-259444759304.png 1617*318”

            var styleTxt = "";
            if (msg.length > 1) {
                srcPath += msg[0];
                var imgInfo = msg[1];
                var width = parseInt(imgInfo.split("*")[0]);
                var heigth = parseInt(imgInfo.split("*")[1]);
                var newSize = globalCommonChatHandle.getShowImgSize(width, heigth);
                width = newSize.split("*")[0];
                heigth = newSize.split("*")[1];

                styleTxt = ' oddSize="' + width + "*" + heigth + '" style="width:' + width + 'px; height:' + heigth + 'px;"';
            } else {
                srcPath += msg[0];
            }
            htmlString = '<a  href="' + srcPath + '" target="_blank" ><img src="' + srcPath + '" ' + styleTxt + '  ></a>';

        } else if (data.t == 2) {
            var fileName = "fileName";
            if (msg.length > 1) {
                srcPath += msg[0];
                fileName = msg[1];
            } else {
                srcPath += msg[0];
            }
            htmlString = '文件：<a  href="' + srcPath + '" target="_blank" > ' + fileName + ' </a>';
        }
        return htmlString;
    }
    setTimeout(reloadMsg, LIM.config.readSpeed);
}