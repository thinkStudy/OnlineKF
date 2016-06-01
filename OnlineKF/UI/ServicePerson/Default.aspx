<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OnlineKF.UI.ServicePerson.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>在线咨询-客服人员</title>
    <link href="/css/structure.css" rel="stylesheet" type="text/css" />
    <link href="/css/serviceperson.default.css" rel="stylesheet" type="text/css" />
    <script src="/js/onlinekf.config.js" type="text/javascript"></script>
    <script src="/js/onLineKF.robot.v1.js" type="text/javascript"></script>
    <script src="/js/common.js" type="text/javascript"></script>
    <script src="/js/read.message.js" type="text/javascript"></script>
    <script src="/js/serviceperson.default.js" type="text/javascript"></script>
</head>
<body>
    <!-- 左侧消息接收 -->
    <div class="con-l pull-left menu_list">
        <ul class="tabs">
            <li style="text-align: center; padding-top: 10px;">客服名称：<asp:Label ID="serviceName"
                runat="server" Text=""></asp:Label>
                &nbsp;&nbsp;&nbsp; <a href="javascript:loginOut()">退出</a> </li>
        </ul>
    </div>
    <div class="wrap con-c" id="chatwrap">
        <div id="body_wrap" class="themeborder hidLeft">
            <div id="header" class="themes" style="">
                <div id="logo" style="">
                    <div id="nameBox">
                    </div>
                </div>
                <div id="top_close" class="">
                    ×</div>
            </div>
            <div id="content_wrap">
                <div id="history" class="themeborder">
                    <div id="padding">
                    </div>
                </div>
                <div id="entry-bar">
                    <div class="menubar">
                        <ul id="toolsbar">
                            <li id="save" class="show" title="保存记录"></li>
                            <li id="activex"  lim:status="show" class="hide" title="截 屏"></li>
                            <li id="file" lim:status="show" class="show" title="传送文件"></li>
                            <li id="emotion" class="hide" onclick="return false;" style="" title="表情"></li>
                            <li id="switch" class="hide" title="关闭提示音"></li>
                            <li id="evaluation" class="show" title="客服评分"></li>
                        </ul>
                    </div>
                    <div class="inputbox themeborder">
                        <div class="fix">
                            <textarea id="inputbox" name="inputbox" placeholder="请输入"></textarea>
                        </div>
                    </div>
                    <div class="actionbar themeborder">
                        <div id="exitChat" class="themeborder" style="" title="关闭">
                            <div class="hiLight">
                            </div>
                            <span id="exitLabel">关闭</span>
                        </div>
                        <div class="enter normal themeborder" id="enterBt">
                            <span id="enter">发送</span> <span class="shortcut" id="shortcut" style="color: blue">
                            </span>
                        </div>
                    </div>
                    <div id="sysTip" style="display: none; opacity: 1.01;">
                    </div>
                    <div id="uploadFileBox" style="display: none;">
                    </div>
                    <div id="emotionsBox" style="display: none;" class="themeborder">
                    </div>
                    <div id="shortKeyMenu" class="themeborder" style="display: none;">
                        <div class="backdrop">
                        </div>
                        <ul id="shortKeyMenuUl">
                            <li class="">按Enter键发送消息 </li>
                            <li class="">按Ctrl+Enter键发送消息</li></ul>
                    </div>
                    <div class="backdrop">
                    </div>
                </div>
            </div>
            <div id="right_banner" class="themeborder">
                <div class="banContent">
                </div>
            </div>
        </div>
    </div>
    <div class="con-r  pull-left menu_list">
        <ul class="tabs">
            <li style="text-align: center; padding-top: 10px;">快捷回复 &nbsp;&nbsp;&nbsp;<a href="javascript:AgileMessageUtils.addAgileMsg()">添加</a>
            </li>
        </ul>
    </div>
    <div id="recieveWrap">
        <audio id="sound" preload="auto">
               <%-- <source src="sounds/sound.wav">
                <source src="sounds/sound.ogg">--%>
            </audio>
    </div>
    <div id="robotWrap">
    </div>
    <div id="temp">
    </div>
    <div style="visibility: hidden;">
        <textarea id="editor_temp_text"></textarea></div>
    <div id="divtemp" style="display: none">
    </div>
    <div id="domConverter" style="display: none">
    </div>
</body>
</html>
