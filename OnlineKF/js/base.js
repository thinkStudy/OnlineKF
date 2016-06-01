
var global_maetceWeb = "http://m.asiaotcmarkets.com";
var ImgResourceWeb = "http://image.asiaotcmarkets.com";
var global_pdf_showeb = ImgResourceWeb + "/PDFJSInNet/web/viewer.html?file=";
ImgResourceWeb += "/resource";

$(function () {
  

    if (location.host.indexOf("localhost") !== -1 || location.host.indexOf("192.168.1.109") !== -1) {
        global_maetceWeb = "http://192.168.1.109:9010";
        global_pdf_showeb = "http://192.168.1.109:9090/PDFJSInNet/web/viewer.html?file=";
        ImgResourceWeb = "http://localhost:10833/resource";
    }
    if (location.host.indexOf("192.168.1.101") !== -1) {

        ImgResourceWeb = "http://192.168.1.101:99";
        global_pdf_showeb = ImgResourceWeb + "/PDFJSInNet/web/viewer.html?file=";
        ImgResourceWeb += "/resource";

    }


    //设置数据缓存对象
    $.dataCache = function (key, value) {
        if (value && key) {
            return $("#div_DataCash").data(key, value);
        } else if (key) {

            return $("#div_DataCash").data(key);
        } else {
            return "";
        }
    }

  

});

/***************************结束index页面js**********************/



/************************统计当前访问量************************/
//document.domain = "aetca.com";
$(function () {
    var account = $.cookie("userUv") || 0;
    if (account == 0) {
        $.post("/req.ashx?act=TotalUvDataAjax.addUv", function (obj) { });
        var expireTime = new Date();
        expireTime.setUTCHours(23); //根据世界时设置 Date 对象中的小时 (0 ~ 23)。
        expireTime.setUTCMinutes(59); //根据世界时设置 Date 对象中的分钟 (0 ~ 59)。
        expireTime.setUTCSeconds(59); //根据世界时设置 Date 对象中的秒钟 (0 ~ 59)。


        $.cookie("userUv", "true", { expires: expireTime, path: "/" });
    }

})

function testLoadUv() {
    $.post("/req.ashx?act=TotalUvDataAjax.getTotalUv", function (obj) {
        var data = eval("(" + obj.data + ")");
        alert("总访问量:" + data.sumUv + ", 今天访问量：" + data.toDayUv);
    });
}

/************************统计当前访问量************************/