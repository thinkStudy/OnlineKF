/// <reference path="onlinekf.config.js" />
/// <reference path="global.js" />
/// <reference path="onLineKF.robot.v1.js" />

var globalCommonChatHandle;
$(function () {
    globalCommonChatHandle = new LIM.ChatHandle(window);

    $(window).resize(function () {
        //修改所有图片到合适大小
        var imgs = $(globalCommonChatHandle.historyBox).find(".lim_bubble img");
        $.each(imgs, function (i, objImg) {
            objImg = $(objImg);
            var oddSize = objImg.attr("oddSize") || undefined;
            if (oddSize) {
                oddSize = oddSize.split("*");
                var width = oddSize[0];
                var heigth = oddSize[1];

                var newSize = globalCommonChatHandle.getShowImgSize(width, heigth);
                width = newSize.split("*")[0];
                heigth = newSize.split("*")[1];

                objImg.css({ width: width, height: heigth });
            }
        });

    });
})

LIM.localRes = {};
LIM.localRes.towaitre = "请稍等，正在执行中。。"
LIM.ChatHandle = function (inWindow) {
    this.config = LIM.config;
    this.chatStatus = "operator"
    this.chatWindow = inWindow;
    this.chatDocument = inWindow.document;
    this.historyBox = document.getElementById('padding');
    this.headBox = document.getElementById('headerBox');
    this.haltSurvey = false;
    this.overtimenoreplay = false;
    this.footBox = document.getElementById('footerBox');
    this.banner = document.getElementById('right_banner');
    this.soundObj = document.getElementById('sound');
    this.lastTimeStr = "";
    this.skin = this.config["skin"];

    this.lastTextContent = "";
    this.updateLocalTypingTimer = null;
    this.sendButtonShortcut = "Enter";
    this.lastMessageTimeString = "";
    this.remoteTypingTimer = null;
    this.isLeaveMessage = 0;
    this.firstFocus = true;
    this.hasChated = false;
    this.initMessageId = "0";
    this.closing = false;
    this.canPlaySound = true;
    this.currentMsger = "";
    this.autoStop = false;
    this.fileId = 1;
    this.fileOddName = "";
    this.isCallbackSending = false;
    this.windowKey = null;
    this.lastShowTime = -1;
    this.ieFixRadius = false;
    if ((document.all && !document.documentMode) || document.documentMode < 9) this.ieFixRadius = true
};
LIM.ChatHandle.prototype = {

    getInputBox: function () {
        if (UT.isSimpleInput()) {
            return document.getElementById('inputbox')
        } else {
            return frames['inputbox']
        }
    },
    updateFootBox: function (inTextContent) {
        this.footBox.innerHTML = inTextContent
    },
    updateHeadBox: function (inTextContent) {
        this.headBox.innerHTML = inTextContent
    },

    getFootBoxInnerHtml: function () {
        return this.footBox.innerHTML
    },

    checkInputText: function () {
        var value;
        if (UT.isSimpleInput()) {
            value = this.getInputBox().value
        } else {
            value = this.getInputBox().document.body.innerHTML.replace(/<[^>]*>/ig, "").replace(/&nbsp;/ig, " ")
        }
        if (value && value != this.lastTextContent) {
            this.lastTextContent = value;
            this.chatClient.updateLocalTyping(true);
            if (this.chatStatus == "INSTANT") {
                this.chatClient.sendInstantText(value)
            }
        } else {
            this.chatClient.updateLocalTyping(false)
        }
    },
    sendLeaveMessage: function () {
        var visitorName = document.getElementById("name");
        var visitorEmail = document.getElementById("email");
        var visitorSubject = document.getElementById("subject");
        var visitorFeed = document.getElementById("inputBox");
        if (visitorName.value.length == 0) {
            alert(typename);
            visitorName.focus();
            return false
        }
        if (visitorEmail.value.length == 0) {
            alert(typeemail);
            visitorEmail.focus();
            return false
        } else {
            if (!UT.isMail(visitorEmail.value)) {
                alert(typerightemail);
                visitorEmail.focus();
                return false
            }
        }
        if (visitorSubject.value == 0) {
            alert(leavemsgtitle);
            visitorSubject.focus();
            return false
        }
        if (visitorFeed.value == 0) {
            alert(leavecontent);
            visitorFeed.focus();
            return false
        }
        var name = UT.XMLEncode(visitorName.value);
        var email = visitorEmail.value;
        var subject = UT.XMLEncode(visitorSubject.value);
        var feed = UT.XMLEncode(visitorFeed.value);
        this.chatClient.sendLeaveMessage(name, email, subject, feed)
    },
    inputBoxOnKeyDown: function () { },
    inputBoxOnKeyUp: function () { },
    inputBoxOnClick: function () {
        if (this.firstFocus) {
            this.firstFocus = false;
            if (navigator.userAgent.toLowerCase().indexOf("edge") > 0) {
                LIM.initEditor();
                this.getInputBox().focus()
            } else {
                this.getInputBox().value = ""
            }
        }
    },
    inputBoxOnMouseOver: function () { },
    inputBoxOnMouseOut: function () { },
    appendToHistoryBox: createWords,


    saveHistory: function () {
        if (this.chatStatus == "WAIT") {
            alert(LIM.localRes["towaitre"]);
            return
        }
        var historyHtml = document.getElementById("padding").innerHTML;
        var temp = document.getElementById("temp"),
			downform = document.getElementById("downform"),
			text = document.getElementById("downHistory"),
			hFrame, chaterName;
        if (!document.getElementById("historyFrame")) {
            hFrame = document.createElement("iframe");
            hFrame.name = "historyFrame";
            hFrame.id = "historyFrame";
            temp.appendChild(hFrame)
        }
        if (!downform) {
            downform = document.createElement("form");
            text = document.createElement("textarea");
            chaterName = document.createElement("input");
            downform.id = "downform";
            downform.action = "/UI/SaveHistory.aspx";
            downform.method = "post";
            downform.target = "historyFrame";
            text.name = "downHistory";
            text.id = "downHistory";
            chaterName.name = "downChaterName";
            chaterName.value = "VHistor" + global_getrandomnum(0, 1000);
            downform.appendChild(text);
            downform.appendChild(chaterName);
            temp.appendChild(downform)
        }
        text.value = historyHtml;
        downform.submit()
    },
    printHistory: function () {
        if (this.chatStatus == "WAIT") {
            alert(LIM.localRes["towaitre"]);
            return
        }
        var historyHtml = document.getElementById("padding").innerHTML;
        var myWindow = open('', 'mychatHistory', 'height=1,width=1');
        myWindow.document.write('<html><head><base target="_blank"></head><body></body></html>');
        myWindow.document.write(historyHtml);
        myWindow.document.execCommand('print', '', savetitle);
        myWindow.close()
    },

    showUploadFileForm: function (framesrc) {
        if (this.chatStatus == "WAIT") {
            alert("文件正在上传中。。。");
            return
        }
        if (!document.getElementById('uploadFileFrame')) {
            document.getElementById('uploadFileBox').innerHTML = '<iframe id="uploadFileFrame"></iframe>'
        }
        document.getElementById('uploadFileFrame').src = framesrc;
        document.getElementById('uploadFileBox').style.display = "";
        document.getElementById("uploadFileBox").style.visibility = "visible"
    },
    hideUploadFileForm: function () {
        document.getElementById("uploadFileBox").style.display = "none"
    },
    showUploadFileStart: function (inUploadFilePath) {
        var uploadFileName = inUploadFilePath.substring(inUploadFilePath.lastIndexOf("\\") + 1);
        globalCommonChatHandle.fileOddName = uploadFileName;
        var htmlString = '正在传送文件&nbsp;&nbsp;<span class="fileUpLoad" id="uploadFile' + (++globalCommonChatHandle.fileId) + '"  >' + uploadFileName + '</span>';
        this.chatStatus = "WAIT";
        this.appendToHistoryBox(htmlString, 'userinfo')
    },
    showUploadFileEnd: function () { this.chatStatus = "Done"; },
    uploadFileOnSubmit: function (inFilePath) {
        var result;
        if (document.all) this.hideUploadFileForm();
        else {
            document.getElementById("uploadFileBox").style.visibility = "hidden"
        }
        this.showUploadFileStart(inFilePath);
        result = LIM.config.filePostPath;
        
        return result
    },
    uploadFileDone: function (savePath, state, imgInfo, oddName, fileId) {
        
        var currentLi = $(globalCommonChatHandle.historyBox).find("#uploadFile" + fileId);
        var parentLi = currentLi.parent();
        var htmlString;

        if (state == "SUCCESS") {
            currentLi = currentLi.clone();
            var srcPath = LIM.config.resourcePath + savePath;

            var lasShowTime = (new Date()).format("yyyyMMddhhmmss") + global_getrandomnum(0, 100);
            var type = 2, content = savePath + " " + oddName;
            //上传的如果是图片

            if (imgInfo) {
                type = 1;
                var width = parseInt(imgInfo.split("*")[0]);
                var heigth = parseInt(imgInfo.split("*")[1]);
                var newSize = this.getShowImgSize(width, heigth);
                width = newSize.split("*")[0];
                heigth = newSize.split("*")[1];
                htmlString = '<a fileLastTime="' + lasShowTime + '" href="' + srcPath + '" target="_blank" ><img src="' + srcPath + '" oddSize="' + width + "*" + heigth + '" style="width:' + width + 'px; height:' + heigth + 'px;" ></a>';
                content = savePath + " " + imgInfo;
            } else {
                htmlString = '文件上传成功：<a fileLastTime="' + lasShowTime + '" href="' + srcPath + '" target="_blank" >' + oddName + '</a>';
            }
            parentLi.html(htmlString);

            //添加数据到后台
            var postUrl = LIM.config.sendPostUrl;
            content = encodeURIComponent(content);
            var postData = { questionId: LIM.config.questionId, content: content, type: type, time: lasShowTime, isFirstAdd: LIM.config.isFirstAdd, addType: LIM.config.reloadType };
            justSendpage(postUrl, postData);

        } else {
            htmlString = state + '：<span class="fileUpLoad" >' + oddName + '</span>';
            parentLi.html(htmlString);
        }

    },
    getShowImgSize: function (oddWidth, oddHeigth) {
        var newWidth = oddWidth, newHeigth= oddHeigth;
        var boxWidth = $(globalCommonChatHandle.historyBox).width() * LIM.config.imgAvg;
        var chaNum = 1;
        if (oddWidth > boxWidth) {
            chaNum = boxWidth / oddWidth;
            newWidth = parseInt(oddWidth * chaNum);
            newHeigth = parseInt(oddHeigth * chaNum);
        }
        return newWidth + "*" + newHeigth;
    },
    popEvaluation: function () {

        this.createEvaluator()

    },
    createEvaluator: function () {
        var el = document.getElementById("sysTip"),
			tid = "tip" + new Date().getTime(),
			info = "";
        if (!!el) {
            info = "<div id='" + tid + "' class='sysInfo'>请对我们的服务进行评分，最高5分。" + UT.getEvaluationHtml(tid, "globalCommonChatHandle.replaceEvaluationTipText") + "<div class='closeTip' title='' style='cursor:pointer' onclick='globalCommonChatHandle.hideEvaluation(true);'></div></div>";
            el.innerHTML = info
        }
        if (!this.hasShown) {
            fadeShow(el);
            this.hasShown = true
        }
    },
    hideEvaluation: function (flag) {
        this.hasShown = false;
        if (flag) {
            document.getElementById('sysTip').style.display = "none"
        } else {
            fadeHide(document.getElementById('sysTip'))
        }
    },
    playSound: function () {
        try {
            if (this.canPlaySound) {
                if (document.all) this.soundObj.src = "sounds/sound.wav";
                else this.soundObj.play()
            }
        } catch (e) { }
    }


};


LIM.hasInitTollbar = false;
LIM.initToolsbar = function () {
    if (!LIM.hasInitTollbar) {
        new LIM.Behavior();
        LIM.hasInitTollbar = true
    }
};

LIM.Behavior = function () {
    this.config = LIM.config;
    this.init()
};
LIM.Behavior.prototype = {

    init: function () {
        var behs = [];
        behs.push({
            "id": "save",
            "type": "click",
            "router": function () {
                if (globalCommonChatHandle.chatStatus != "ROBOT") {
                    globalCommonChatHandle.saveHistory()
                } else { }
            }
        });
        behs.push({
            "id": "activex",
            "type": "click",
            "router": function () {
                if (globalCommonChatHandle.chatStatus != "ROBOT") {
                    UI.download()
                } else { }
            }
        });
        behs.push({
            "id": "file",
            "type": "click",
            "router": function () {
                if (globalCommonChatHandle.chatStatus != "ROBOT") {
                    globalCommonChatHandle.showUploadFileForm(LIM.config.fileUploadPath);
                } else { }
            }
        });

        behs.push({
            "id": "history",
            "type": "click",
            "router": function () {
               // hideMenubar()
            }
        });


        behs.push({
            "id": "enter",
            "type": "click",
            "router": function () {
                sendword();
            }
        });
        behs.push({
            "id": "shortcut",
            "type": "click",
            "router": this.shortCutMenu
        });
        behs.push({
            "id": "shortKeyMenu",
            "type": "mouseover",
            "router": function () {
                document.getElementById('shortKeyMenu').style.display = ""
            }
        });
        behs.push({
            "id": "shortKeyMenu",
            "type": "mouseout",
            "router": function () {
                document.getElementById('shortKeyMenu').style.display = "none"
            }
        });


        behs.push({
            "id": "evaluation",
            "type": "click",
            "router": function () {
                if (globalCommonChatHandle.chatStatus != "ROBOT") {

                    globalCommonChatHandle.popEvaluation()

                }
            }
        });


        this.add(behs);
        this.loadShortKeyMenu()
    },
    add: function (behs) {
        var el, st;
        for (var i = 0, l = behs.length; i < l; i++) {
            el = document.getElementById(behs[i].id);
            if (!el) continue;
            st = el.style;
            if (st.display == "" || behs[i].type != "click") {

                UT.addEvent(el, behs[i].type, behs[i].router)
            }
        }
    },
    shortCutMenu: function () {
        var el = document.getElementById('shortKeyMenu'),
			s = el.style;
        if (s.display == "none") {
            s.display = ""
        } else {
            s.display = "none"
        }
    },

    loadShortKeyMenu: function () {
        var lis = document.getElementById('shortKeyMenu').getElementsByTagName("li");
        if (LIM.config.sendKjj == "E") {
            lis[0].innerHTML += " *"
        } else {
            lis[1].innerHTML += " *"
        }
        UT.addHoverEffect(lis[0]);
        UT.addHoverEffect(lis[1]);
        UT.addEvent(lis[0], 'click', function () {
            lis[0].innerHTML = lis[0].innerHTML.replace(" *", "");
            lis[1].innerHTML = lis[1].innerHTML.replace(" *", "");
            shortcut = 'Enter';
            typeof globalCommonChatHandle != "undefined" && (globalCommonChatHandle.sendButtonShortcut = 'Enter');
            lis[0].innerHTML += " *";
            LIM.config.sendKjj = "E";
            document.getElementById('shortKeyMenu').style.display = "none"
        });
        UT.addEvent(lis[1], 'click', function () {
            lis[0].innerHTML = lis[0].innerHTML.replace(" *", "");
            lis[1].innerHTML = lis[1].innerHTML.replace(" *", "");
            shortcut = 'Ctrl + Enter';
            typeof globalCommonChatHandle != "undefined" && (globalCommonChatHandle.sendButtonShortcut = 'Ctrl + Enter');
            lis[1].innerHTML += " *";
            LIM.config.sendKjj = "CE";
            document.getElementById('shortKeyMenu').style.display = "none"
        })
    }

};

function fadeShow(el) {
    var opacity = 0,
		showTimer;
    el.style.opacity = 0;
    el.style.filter = "alpha(opacity=" + 0 + ")";
    el.style.display = "block";
    showTimer = setInterval(function () {
        if (opacity <= 100) {
            opacity++;
            el.style.opacity = opacity / 100;
            el.style.filter = "alpha(opacity=" + opacity + ")"
        } else {
            clearInterval(showTimer)
        }
    }, 20)
};

function fadeHide(el) {
    var transparent = 0,
		hideTimer;
    if (el.hiding == 1) {
        return
    }
    hideTimer = setInterval(function () {
        if (transparent <= 100) {
            el.hiding = 1;
            transparent++;
            el.style.opacity = (100 - transparent) / 100;
            el.style.filter = "alpha(opacity=" + (100 - transparent) + ")"
        } else {
            el.style.display = "none";
            el.hiding = 0;
            globalCommonChatHandle.hasShown = false;
            clearInterval(hideTimer)
        }
    }, 20)
};

if (typeof UT == "undefined") window.UT = {};
UT.isSimpleInput = function () {
    return IE6SSL || (!richInputEnabled)
};
UT.changeToRichInput = function () {
    if (IE6SSL) {
        return
    }
    if (!richInputEnabled) {
        richInputEnabled = true;
        var inputedValue = document.getElementById('inputbox').value;
        LIM.initEditor();
        frames['inputbox'].document.body.innerHTML = inputedValue
    }
};
UT.evaluator = function (d, f, e) {
    var g = d,
		c = g.getElementsByTagName("input"),
		a = g.getElementsByTagName("li");
    for (var b = 0; b < a.length; b++) {
        a[b].onclick = function (j) {
            var i = this,
				h = i.getElementsByTagName("input")[0];
            h.checked = true
        }
    }
    document.getElementById("submiteval").onclick = function () {
        var l = null;
        for (var j = 0; j < c.length; j++) {
            if (c[j].getAttribute("name") == "eval" && c[j].checked) {
                l = LIM.evaluationValue = c[j].getAttribute("value");
                break
            }
        }
        

        $.post("/req.ashx?act=QuestionPersonAjax.setServiceLevel", { questionId: LIM.config.questionId, serviceLevel: l });

        globalCommonChatHandle.haltSurvey = true;

        globalCommonChatHandle.hideEvaluation(true);
    }
};
UT.addHoverEffect = function (element) {
    this.addEvent(element, "mouseover", function (e) {
        element.className += " hover"
    });
    this.addEvent(element, "mouseout", function (e) {
        element.className = element.className.replace(/\s?hover/, '')
    })
};
UT.JSONRequest = function (url, id, onload, onerror) {
    this.url = url;
    this.onload = onload;
    this.onerror = onerror ? onerror : this.defaultError;
    this.init(url, id)
};
UT.JSONRequest.prototype = {
    init: function (url, id) {
        this.script = document.createElement("script");
        this.script.setAttribute("type", "text/javascript");
        this.script.setAttribute("src", url);
        if (id) this.script.setAttribute("id", id);
        document.getElementsByTagName("head")[0].appendChild(this.script);
        var request = this;
        if (this.script) {
            if (document.all) {
                var script = this.script;
                this.script.onreadystatechange = function () {
                    var state = script.readyState;
                    if (state == "loaded" || state == "interactive" || state == "complete") {
                        request.onload.call(request)
                    }
                }
            } else {
                this.script.onload = function () {
                    request.onload.call(request)
                }
            }
        } else {
            request.onerror.call(this)
        }
    },
    defaultError: function () {
        E.report(jsResources.notice.constructError, "1101")
    }
};
UT.addEvent = function (element, eventName, eventFunction) {
    if (document.attachEvent) {
        element.attachEvent("on" + eventName, eventFunction)
    } else {
        element.addEventListener(eventName, eventFunction, false)
    }
};
UT.getElementLeft = function (obj) {
    var x = obj.offsetLeft;
    while (obj = obj.offsetParent) x += obj.offsetLeft;
    return x + 'px'
};

UT.getEvaluationHtml = function (c, b) {
    var a = '<ul class="evaluator" id="' + c + '" onmouseover="UT.evaluator(this,\'' + c + "'" + (b ? ("," + b) : "") + ')">';
    a += '<li><input type="radio" name="eval" value="5" checked="checked"/><span value="5" class="eval5 evallevel"></span><span>' + "5分</span></li>";
    a += '<li><input type="radio" name="eval" value="4"/><span value="4" class="eval4 evallevel"></span><span>' + "4分</span></li>";
    a += '<li><input type="radio" name="eval" value="3"/><span value="3" class="eval3 evallevel"></span><span>' + "3分</span></li>";
    a += '<li><input type="radio" name="eval" value="2"/><span value="2" class="eval2 evallevel"></span><span>' + "2分</span></li>";
    a += '<li><input type="radio" name="eval" value="1"/><span value="1" class="eval1 evallevel"></span><span>' + "1分</span></li>";
    a += '<li><input type="button" class="submiteval" id="submiteval" value="提交" /></li>';
    a += "</ul>";
    return a
};