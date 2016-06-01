
function importCss() {
    var b = document.getElementsByTagName("head")[0];
    if ($(b).find("link[href*='tip-yellow.css']").length == 0) {
        var a = document.createElement("link");
        a.setAttribute("rel", "stylesheet")
        a.setAttribute("type", "text/css")
        a.setAttribute("href", "/js/jquery-validator/css/tip-yellow.css")
        a.charset = "utf-8";
        b.insertBefore(a, b.firstChild);
    }
}

$(function () {
    //引入样式文件
    importCss();

    $(document.body).initValidator();

      var promptDiv = ['<div id="validator_msg" style="position: absolute;display:block;">',
                    '<div class="validator_msg_text"></div>',
                    '<div id="msg_arrow" class="validator_msg_arrow">',
                    '</div>', '</div>'].join('');
    $(document.body).append(promptDiv);
});

var re_dt = /^(\d{1,2})\-(\d{1,2})\-(\d{4})$/,
    re_tm = /^(\d{1,2})\:(\d{1,2})\:(\d{1,2})$/;

///功能说明：验证input数据
///使用说明：需要在input输入框中添加属性
///添加属性：chktype(number/dateTime/limit/split)
///添加属性：fmt(limit：min-max/split:','|'-',abs:绝对值为数字)
///添加属性：notnull=true 默认对验证的输入都做非空验证，设定为true可以为null
///添加属性：output 需要输出的提示语句
///添加属性：notVal 指定不允许输入的字，多个以逗号分隔（0,-1）
///添加属性：firstVal 指定开头字符如（字符以80开头） 

//初始化验证
jQuery.fn.checkBox = function(){
    var inputs = $(this).find("[chkType]");
    inputs.blur();
    return $(this).find(".validator_fail").length < 1;
};
jQuery.fn.initValidator = function () {
    var inputs = $(this).find("[chkType]");

    $(inputs).bind('blur', function (e) {
        var input = $(this);
        var result = v_input_param_check.checkBool(input);
        if(result == false){
          input.addClass("validator_fail");
        }else{
          input.removeClass("validator_fail");
        }
    }).hover(function () {
        var t = $(this);
        if (t.hasClass("validator_fail")) {
             t.initPrompt();
        }
    }, function () {
        $("#validator_msg").css({ display: "none" });
    });
};
jQuery.fn.initPrompt = function () {
    var input = $(this);
    var chktype = input.attr("chktype");
    var prompt = input.attr("prompt") || default_prompt[chktype];
    var firstVal = input.attr("firstVal")||"";
    if(firstVal != "" && input.val().indexOf(firstVal) !== 0 ){
        prompt =  "首字符必须是："+firstVal;
    }
    var promptDiv =  $("#validator_msg");
    promptDiv.find(".validator_msg_text").text(prompt);

    var floatType = v_input_param_check.getBase().float;
       var position = input.position();
       var top,left;
    if(floatType == "right"){
        left = position.left + input.width() +25; //left：input的位置+input的width +边距
        top = position.top + input.height() / 2 - 4; //left：input的位置+input的高/2 +边距
        
    }else if(floatType == "bottom"){
        top =  position.top + input.height()+20;
        
        if( promptDiv.width()< input.width()){
           left = position.left + input.width() -promptDiv.width()+2;
           promptDiv.find("#msg_arrow").css({left:promptDiv.width()-20});
        }else{
            left = position.left - (promptDiv.width()-input.width())/2;
        }
        
    }
    if (!promptDiv.find("#msg_arrow").hasClass("validator_msg_arrow_"+floatType)) {
        promptDiv.find("#msg_arrow").addClass("validator_msg_arrow_"+floatType)
    }
    
    promptDiv.css({left:left + 'px',top: top + 'px'}).appendTo(input.parent());
    promptDiv.show();

};

jQuery.fn.clearPrompt = function () {
    var inputs = $(this).find("[chkType]");
    inputs.removeClass("validator_fail");
};

 
 var  v_input_param_check = new Object;
(function (chk) {
     var base = {
         chkdiv:undefined,
         display: "default", //提示错误的显示方式。
         isbarek: true, //检查到错误输入，是否跳出。
         reult:true,
         float:"right" //浮动显示的位置
     };
     chk.getBase = function(){
       return base; 
     };
     chk.init = function(init){
        $.extend(base, init);
     };
     chk.checkBool= function(input){
        var result = true;
        if (input.attr("chktype")==undefined) return true;
        var output = input.attr("output")||"";
        var chktype = input.attr("chktype");
        var limitfmt = input.attr("limit")||"";
        var notnull =  input.attr("notnull") != undefined;
        var value = input.nodeName == "TEXTAREA" ? input.text() : input.val();
        var firstVal = input.attr("firstVal")||"";
        value = $.trim(value);
        //检验是否为空
        if(notnull==true && value==""){
             return true;
        }else if(value == ""){
            return false;
        }
        //检验指定开头字符串
        if(firstVal != "" && value.indexOf(firstVal) !== 0){
            return false;
        }
        //检验是否有不允许输入的值
        if(input.attr("notVal")){
            var notVal =input.attr("notVal").split(',');
            for(var j=0;j<notVal.length;j++){
                if(notVal[j]==value){
                    return false;
                }
            }
        }
        var min = 0, max =0;
        var fmtArr = limitfmt.split("-");
        if(fmtArr.length > 1){
            min =parseInt(fmtArr[0]), max =parseInt(fmtArr[1]);
        }else{
            min = parseInt(limitfmt);
            max =parseInt(Math.max);
        }
        switch (chktype) {
            case "string": //可针对文本或者数字的范围
                var length = value.length;
                if (length > max || length<min)  { 
                return false;
                }
                break;
            case "number": //针对数字的范围
                var length = value;
                if (!v_formats.number(value) || length< min || length > max) {
                    return false;
                }
                break;
            break;
            default:
                var metch = eval("v_formats." + chktype);
                result = metch.call(v_formats,value);
                break;
        };
        return result;
     },
     chk.startChk = function(chkDivParam){
         if(chkDivParam!=undefined)
            base.chkdiv = chkDivParam;
         if(base.chkdiv == undefined)
            base.chkdiv =$(document.forms[0]);

         var inputs = $(base.chkdiv).find("[chkType]");
         
         base.reult=true;
         for (var i = 0; i < inputs.length; i++) {
             var input = $(inputs[i]);
             var result = chk.checkBool(input);
             if(result == false){
                base.reult = false;
                input.initPrompt();
             }else{
                input.clearPrompt();
             }
         }
         return base.reult;
      }

    

    
 })(v_input_param_check)

 var default_prompt = {
     'string':'请输入字符串',
     // 输入的字段须为数字
     'number': '输入的字段须为数字',
     'email': '请输入合法的Email',
     // 电话号码字段必须符合一定的格式；
     'phone': '请输入合法的电话号码',
     // 手机字段必须符合一定的格式
     'mobile': '请输入合法的手机号码',
     // URL字段必须符合一定的格式
     'url': '请输入合法的URL',
     // 货币字段必须符合一定的格式
     'currency': '请输入合法的货币格式',
     // 邮编字段必须符合一定的格式
     'postCode': '请输入合法的邮编格式',
     // 必须输入整型字段
     'int': '请输入合法的整型字段',
     // 必须输入双精度型字段
     'double':'请输入合法的双精度型字段，如：12.25',
     // 输入的字段须为字母
     'abcletter': '输入的字段须为字母',
     // 输入的字段须为中文
     'chinese': '输入的字段须为中文',
     // 必须输入日期型字段yyyy-MM-dd格式
     'date': '必须输入日期型字段yyyy-MM-dd',
     // 必须输入日期型字段hh:mm:ss格式
     'time':'必须输入日期型字段hh:mm:ss',
     'dateTime': '必须输入日期格式yyyy-MM-dd hh:mm:ss',
     // IP地址字段必须符合一定的格式
     'ipAddress': '请输入合法的IP地址',
     // 输入的字段须为基本字符如a-z,0-9,_等
     'baseword': '输入的字段须为基本字符如a-z,0-9' 
 };
 //#region 验证参数正则
  v_formats = {
     // 输入的字段须为数字
     'number': function isNumber(s) {
         var digits = "0123456789";
         var i = 0;
         var sLength = s.length;
         while ((i < sLength)) {
             var c = s.charAt(i);
             if (digits.indexOf(c) == -1) return false;
             i++;
         }
         return true;
     },
     // Email字段必须符合一定的格式
     'email': function (s) {
         return RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).test(s);
     },
     // 电话号码字段必须符合一定的格式；
     'phone': function (s) {
         return RegExp(/^((\(\d{3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}$/).test(s);
     },
     // 手机字段必须符合一定的格式
     'mobile': function (s) {
         return RegExp(/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/).test(s);
     },
     // URL字段必须符合一定的格式
     'url': function (s) {
         return RegExp(/^(https:\/\/)?(http:\/\/)?[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\?+!]*([^<>\"\"])*$/).test(s);
     },
     // 货币字段必须符合一定的格式
     'currency': function (s) {
         return RegExp(/^\d+(\.\d+)?$/).test(s);
     },
     // 邮编字段必须符合一定的格式
     'postCode': function (s) {
         return RegExp(/^[1-9]\d{5}$/).test(s);
     },
     // 必须输入整型字段
     'int': function isInteger(str) {
         if (/^\d+$/.test(str) == true) {
             return true;
         }
         return false;
     },
     // 必须输入双精度型字段
     'double': function (s) {
         return RegExp(/^-?\d+(\.\d+)?$/).test(s);
     },
     // 输入的字段须为字母
     'abcletter': function (s) {
         return RegExp(/^[A-Za-z]+$/).test(s);
     },
     // 输入的字段须为中文
     'chinese': function (s) {
         return RegExp(/^[\u0391-\uFFE5]+$/).test(s);
     },
     // 必须输入日期型字段yyyy-MM-dd格式
     'date': function isdate(strDate) {
         if (strDate == "") return false;
         var strSeparator = strDate.indexOf("-") > 0 ? "-" : "/"; //日期分隔符
         var strDateArray;
         var intYear;
         var intMonth;
         var intDay;
         var boolLeapYear;

         strDateArray = strDate.split(strSeparator);

         if (strDateArray.length != 3) return false;

         intYear = parseInt(strDateArray[0], 10);
         intMonth = parseInt(strDateArray[1], 10);
         intDay = parseInt(strDateArray[2], 10);

         if (isNaN(intYear) || isNaN(intMonth) || isNaN(intDay)) return false;

         if (intMonth > 12 || intMonth < 1) return false;

         if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intDay > 31 || intDay < 1)) return false;

         if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intDay > 30 || intDay < 1)) return false;

         if (intMonth == 2) {
             if (intDay < 1) return false;

             boolLeapYear = false;
             if ((intYear % 100) == 0) {
                 if ((intYear % 400) == 0) boolLeapYear = true;
             }
             else {
                 if ((intYear % 4) == 0) boolLeapYear = true;
             }

             if (boolLeapYear) {
                 if (intDay > 29) return false;
             }
             else {
                 if (intDay > 28) return false;
             }
         }

         return true;
     },
     // 必须输入日期型字段hh:mm:ss格式
     'time': function (s_time) {
         if (s_time == "") return false;
         // check format
         if (!re_tm.test(s_time))
             return false;
         // check allowed ranges
         if (RegExp.$1 > 23 || RegExp.$2 > 59 || RegExp.$3 > 59)
             return false;
         return true;
     },
     'dateTime': function (s_datetime) {
         if (s_datetime == "") return false;
         var s_day = s_datetime.substring(0, s_datetime.indexOf(':') - 2);
         var s_time = s_datetime.substring(s_datetime.indexOf(':') - 2, s_datetime.length);
         //check date format
         if (this.date(s_day) == false)
             return false;
         // check format
         if (!re_tm.test(s_time))
             return false;
         // check allowed ranges
         if (RegExp.$1 > 23 || RegExp.$2 > 59 || RegExp.$3 > 59)
             return false;
         return true;
     },
     // IP地址字段必须符合一定的格式
     'ipAddress': function (s) {
         return RegExp(/(\d+)\.(\d+)\.(\d+)\.(\d+)/).test(s);
     },
     // 输入的字段须为基本字符如a-z,0-9,_等
     'baseword': function (s) {
         return RegExp(/^\\w*$/).test(s);
     }
 };
//#endregion