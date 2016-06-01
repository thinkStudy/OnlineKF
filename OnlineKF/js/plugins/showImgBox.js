$(function () {
    $("head").append('<style type="text/css">.backgroundiv{width:100%;height:100%;background-color:black;opacity:0.6;position:absolute;z-index:9999;top:0;left:0px;}.bigImgDiv{left:0px;width:100%;height:100%;position:absolute;z-index:10000;top:0;text-align:center;}.bigImgDiv .close{background:url(/image/close.png);display:block;width:27px;height:27px;right:0;position:absolute;line-height:30px;background-color:#0C0C0C;border-radius:8px;}.close:hover{background-color:red;}.bigImgDivimg{border-radius:3px;}.bigImgContent{padding:4px;background-color:white;width:30px;height:30px;margin:50px auto;text-align:center;position:relative;border-radius:5px;}</style>');
    $("body").delegate(".lightbox", "click", function (eve) {
        var t = $(this);
        eve.preventDefault();
        if (!t.attr("href")) {
            return;
        }

        var target = $(".bigImgDiv");
        if (target.length === 0) {
            var str = ['<div class="backgroundiv"></div><div class="bigImgDiv" onclick="showImg_click(event)">',
                              '<a onclick="$(\'.bigImgDiv\').prev().remove();$(\'.bigImgDiv\').remove();" class="close" href="javascript:void(0)"></a>',
                              '<img src="/image/loading.gif"/><div class="bigImgContent"><img src="/image/loading.gif" class="bigImgShow" onload="bigImgLoadComplete(this)" style="display:none;cursor:pointer;"/>',
                              '</div></div>'];
            $("body").append(str.join(''));
        }
        $(".bigImgDiv .bigImgShow").attr("src", t.children().attr("src")).attr("title", t.children().attr("title"));
        var scrollTop = $(document).scrollTop();
        var imgHeight = $(".bigImgDiv .bigImgShow").height();
        var bodyHeight = $("body").height();
        while (imgHeight + scrollTop > bodyHeight) {
            scrollTop -= 5;
        }
        $(".backgroundiv").height(bodyHeight + scrollTop);
        $(".bigImgDiv").css({ "margin-top": scrollTop });
    });
})
function showImg_click(event) {
    //    var target = event.srcElement || event.toElement || event.relatedTarget
    //    if (target.className === 'bigImgDiv') {
    $('.bigImgDiv').prev().remove();
    $('.bigImgDiv').remove();
    //    }
}
function bigImgLoadComplete(t) {
    t = $(t);
    t.parent().prev().hide();
    t.show();
    t.parent().css({ "width": t.width(), "height": t.height() });
}