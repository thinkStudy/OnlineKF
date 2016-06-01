/*****************************
* desc:全局js文件
* time:2015-07-02
******************************/

/*************************cookie***************/
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD (Register as an anonymous module)
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node/CommonJS
        module.exports = factory(require('jquery'));
    } else {
        // Browser globals
        factory(jQuery);
    }
}
(function ($) {

    var pluses = /\+/g;

    function encode(s) {
        return config.raw ? s : encodeURIComponent(s);
    }

    function decode(s) {
        return config.raw ? s : decodeURIComponent(s);
    }

    function stringifyCookieValue(value) {
        return encode(config.json ? JSON.stringify(value) : String(value));
    }

    function parseCookieValue(s) {
        if (s.indexOf('"') === 0) {
            // This is a quoted cookie as according to RFC2068, unescape...
            s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
        }

        try {
            // Replace server-side written pluses with spaces.
            // If we can't decode the cookie, ignore it, it's unusable.
            // If we can't parse the cookie, ignore it, it's unusable.
            s = decodeURIComponent(s.replace(pluses, ' '));
            return config.json ? JSON.parse(s) : s;
        } catch (e) { }
    }

    function read(s, converter) {
        var value = config.raw ? s : parseCookieValue(s);
        return $.isFunction(converter) ? converter(value) : value;
    }

    var config = $.cookie = function (key, value, options) {

        // Write

        if (arguments.length > 1 && !$.isFunction(value)) {
            options = $.extend({}, config.defaults, options);

            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setMilliseconds(t.getMilliseconds() + days * 864e+5);
            }

            return (document.cookie = [
				encode(key), '=', stringifyCookieValue(value),
				options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
				options.path ? '; path=' + options.path : '',
				options.domain ? '; domain=' + options.domain : '',
				options.secure ? '; secure' : ''
			].join(''));
        }

        // Read

        var result = key ? undefined : {},
        // To prevent the for loop in the first place assign an empty array
        // in case there are no cookies at all. Also prevents odd result when
        // calling $.cookie().
			cookies = document.cookie ? document.cookie.split('; ') : [],
			i = 0,
			l = cookies.length;

        for (; i < l; i++) {
            var parts = cookies[i].split('='),
				name = decode(parts.shift()),
				cookie = parts.join('=');

            if (key === name) {
                // If second argument (value) is a function it's a converter...
                result = read(cookie, value);
                break;
            }

            // Prevent storing a cookie that we couldn't decode.
            if (!key && (cookie = read(cookie)) !== undefined) {
                result[name] = cookie;
            }
        }

        return result;
    };

    config.defaults = {};

    $.removeCookie = function (key, options) {
        // Must not alter options, thus extending a fresh object...
        // document.cookie=key+"=; expire=-1; path=/;domain=.abc.com";
        $.cookie(key, '', $.extend({}, options, { expires: -1 }));
        return !$.cookie(key);
    };

}));
/*************************cookie***************/

/*******************公用函数*******************/

function isURL(str) {
    return RegExp(/^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\?+!]*([^<>\"\"])*$/).test(str);
}

function isIE() {
    var explorer = window.navigator.userAgent;
    //ie 
    if (explorer.indexOf("MSIE") >= 0) {
        return true;
    }
    return false;
}


//兼容其他不支持placeholder的浏览器：
var PlaceHolder = {
    _support: (function () {
        return 'placeholder' in document.createElement('input');
    })(),

    //提示文字的样式，需要在页面中其他位置定义
    className: { color: "#898888" },

    init: function () {
        if (!PlaceHolder._support) {
            //未对textarea处理，需要的自己加上
            var inputs = document.getElementsByTagName('input');
            PlaceHolder.initJianRong();
            PlaceHolder.create(inputs);
        }
    },
    initJianRong: function () {
        //设置兼容js的方法
        $.fn.val = function (key) {
            var placetxt = this.attr("placeholder");

            var currVal = "";
            if (key === undefined) {
                if (this[0] !== undefined) {
                    currVal = this[0].value;
                    if (currVal === placetxt) {
                        return "";
                    }
                }
            } else {
                if (this[0] !== undefined) {
                    currVal = key;
                    this[0].value = key;
                }
            }
            return currVal;
        }
    },

    create: function (inputs) {

        var input;
        if (!inputs.length) {
            inputs = [inputs];
        }
        for (var i = 0; i < inputs.length; i++) {
            input = inputs[i];
            //hidType是新增加input对象，不需要二次处理
            var hidtype = $(input).attr("hidType");
            if (!PlaceHolder._support && input.attributes && input.attributes.placeholder && hidtype !== "password") {
                PlaceHolder._setValue(input);
                var type = $(input).attr("type");

                $(input).bind('focus', function (e) {
                    PlaceHolder._canlValue(this);
                });
                $(input).bind('blur', function (e) {
                    PlaceHolder._setValue(this);
                });

            }
        }
    },

    _setValue: function (input) {
        var html = input.outerHTML || "";
        var tipTxt = input.attributes.placeholder.nodeValue;
        input = $(input);

        var type = input.attr("type");
        if (input.val() === '') {
            if (type === "password") {
                var showTip = input.next();
                if (input.attr("hidType") != "password") {
                    html = html.replace(/\s*type=(['"])?password\1/gi, " type=text placeholderfriend")

                  .replace(/\s*(?:value|on[a-z]+|name)(=(['"])?\S*\1)?/gi, " ")

                  .replace(/\s*placeholderfriend/, " placeholderfriend value='" + tipTxt

                  + "' " + "onfocus='PlaceHolder._canlValue(this);'  hidType='password' onblur='PlaceHolder._setValue(this);' ");
                    html = $(html)
                    html.attr("id", "show" + html.attr("id"));
                    input.after(html.css(PlaceHolder.className));
                } else {
                    showTip.show();
                }
                input.hide();

            } else {
                input.val(tipTxt);
                input.css(PlaceHolder.className);
            }
        }
        return false;
    },
    _canlValue: function (input) {

        var tipTxt = input.attributes.placeholder.nodeValue;
        input = $(input);
        var type = input.attr("hidType");
        //如果为密码框
        if (type === "password") {
            var prevInput = input.prev();
            prevInput.show();
            prevInput.focus();
            input.hide();
        } else {
            if (input.val() === "") {
                input.val("");
                $(this).css({});
            }
        }
        return false;
    }
};







/*
js弹窗代码:用户体验极佳的Alert提示效果

*/

$(function () {

    var tipHtml = ['<div id="notice_tip" class="err" style="display:none; left: 50%; margin-left: -275px; top: 0pc;">',
                    '<span id="notice_icon">&nbsp;</span>',
                    '<span id="notice_txt">請先登錄!</span>',
                    '<span id="notice_close" style=" cursor:pointer;" onclick="$(this).parent().slideUp(800);" >&nbsp;</span>',
                    '</div>'].join('');
    $(document.body).append(tipHtml);


    //页面初始化时对所有input做初始化
    PlaceHolder.init();

})
//alert
//param status 0失败 1成功 2警告
window.alert = function (msg, status, time) {//time为消失时间
    time = time || 3000;
    var notice_tip = $("#notice_tip")
    status = status || 0;
    var msgbg, msgcolor, bordercolor, content, posLeft, posTop;

    if (!notice_tip || notice_tip.length == 0) {
        return true;
    }

    notice_tip.find("#notice_txt").text(msg);
    notice_tip.removeClass();
    if (status == 0) {
        notice_tip.addClass("err");
    } else if (status == 2) {
        notice_tip.addClass("warning");
    }
    notice_tip.slideDown(800);

    if (time != '') setTimeout("CloseMsg()", time);
    else setTimeout("CloseMsg()", 2000); //默认两秒后自动消失
    return false;
}
//移除对象
function CloseMsg() {
    $("#notice_tip").slideUp(800);
}

/*******************结束公用函数***************/

//设置按钮加载中的效果
//调用方式 $("#btn").btnLoading();
jQuery.fn.btnLoading = function () {
    with ($(this)) {

        attr("disabled", "disabled");
        addClass("btn_loading");
        return this;
    }

}
jQuery.fn.btnDone = function () {
    with ($(this)) {
        attr("disabled", false);
        removeClass("btn_loading");
        return this;
    }

}


//获取url中的参数
function getUrlParam(name, str) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    str = str || window.location.search.substr(1);
    var r = str.match(reg);  //匹配目标参数
    if (r != null) return decodeURIComponent(r[2]); return null; //返回参数值
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
/******************************
 两种调用方式
 var template1="我是{0}，今年{1}了";
 var template2="我是{name}，今年{age}了";
 var result1=template1.format("loogn",22);
 var result2=template2.format({name:"loogn",age:22});
 *******************************/
String.prototype.format = function (args) {
    var result = this; var reg;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                     reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出
                     reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

/*********************
随机生成数字
global_getrandomnum(1,100) 生成1-100的数字
global_getrandomnum(50,100) 生成50-100的数字
***********************/
function global_getrandomnum(Min, Max) {
    var Range = Max - Min;
    var Rand = Math.random();
    return (Min + Math.round(Rand * Range));
}

//获取IE版本
function ieCopyright() {
    var explorer = window.navigator.userAgent;
    //ie 
    //MSIE 8.0;
    if (explorer.indexOf("MSIE") >= 0) {
        var num = explorer.substr(explorer.indexOf("MSIE")+4, 6);
        return num;
    }
    return -1;
}