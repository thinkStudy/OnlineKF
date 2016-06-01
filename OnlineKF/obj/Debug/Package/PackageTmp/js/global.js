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
$.timeFormatter = function (value) {

    var da = new Date(parseInt(value.replace("/Date(", "").replace(")/", "").split("+")[0]));

    return da.getFullYear() + "-" + (da.getMonth() + 1) + "-" + da.getDate();

}
$.setTimeFormatter = function (value) {
    var d = new Date(parseInt(value.replace("/Date(", "").replace(")/", "").split("+")[0]));
    //    var year = d.getFullYear();
    //    var month = d.getMonth();
    //    var day = d.getDate();
    //    var hour = d.getHours();
    //    var min = d.getMinutes();
    //    var s = d.getSeconds();
    //    var da = new Date(year, month, day, hour + 15, min, s);
    //    return da.getFullYear() + "-" + (da.getMonth() + 1) + "-" + da.getDate() +" " + da.getHours() + ":" + da.getMinutes() + ":" + da.getSeconds()
    d.setHours(d.getHours() + 15);
    return d.ChinaFormat("yyyyMMdd");

}
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

var global_months = ["Jan ", "Feb ", "Mar ", "Apr ", "May ", "June ", "July ", "Aug ", "Sept ", "Oct ", "Nov ", "Dec "];
function getEngilshTxt(time) {
    var startTime = time;
    if (typeof time === "string" && time.indexOf("/Date(") > -1) {
        startTime = new Date(parseInt(time.replace("/Date(", "").replace(")/", "").split("+")[0]));
    } else if (typeof time == "string") {
        startTime = new Date(time)
    }
    //设置英文文时间
    var timeTxt = global_months[startTime.getMonth()];
    timeTxt += intConvert(startTime.getDate()) + ",";
    timeTxt += startTime.getFullYear();
    return timeTxt;
}

function intConvert(val) {
    return val < 10 ? "0" + val : val;
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




