//**********************************************************
//jquery扩展插件，数据绑定
//author:think
//createDate:2015-06-09
//**********************************************************
//文件上传类

$.bind = {
    createNew: function () {
        return this;
    },
    //默认参数
    defaults: {
        page: 1,
        pageSize: 10,
        totalCount: 0,
        opacity: 0.2,
        divBox: '.content',
        bindId: "bindId",
        trObj: undefined,
        otherBind: {}, //对字段特殊处理, 示例  price:function(value){ return value+"元"; } 
        done: function () { }, //加在完成
        load: function () { }, //加载前的处理
        loadImgUrl: "/image/load.gif", //加载时显示的图片
        showMinLine: 0, //显示最小的行数，如果大于0会进行添加默认行数
        lineDefalut: "--" //数据为空时，显示的text
    },

    setParam: function (option) {
        this.defaults = $.extend(this.defaults, option);
    },
    removeLoadBox: function (divBox) {
        divBox.find(".loadPanl").remove();
    },
    setLoadBox: function (divBox, isResult) {


        isResult = isResult || false;

        //动态创建一个层
        var loadDivHtml = $('<div class="loadPanl"  ><center>' +
                                '<img src="' + this.defaults.loadImgUrl + '" >' +
                                '</center></div>');
        if (isResult) {
            loadDivHtml = $("<div  align='center' class='loadPanl' style='text-algin:center'><center>" + language.common.tips.NoResult + "</center></div>");

        }
        if (!divBox) {
            return loadDivHtml;
        }
        //设置加载图片
        var loadWidth = divBox.width();
        var loadHeigth = divBox.height();

        //指定样式：透明度与背景 位置
        loadDivHtml.css({
            left: "0px",
            top: "0px",
            width: loadWidth + "px",
            //height: loadHeigth + "px",
            "margin": "15px 0px"
        });


        if (divBox.find(".dataContainer:first").length > 0) {
            divBox.find(".dataContainer:first").hide();
            divBox.find(".dataContainer:gt(0)").remove();
        }
        else if (divBox.find("tr").length > 0) {
            divBox.find("tr:eq(1)").hide();
            divBox.find("tr:gt(1)").remove();
        }
        else if (divBox.find("li").length > 0) {
            divBox.find("li:first").hide();
            divBox.find("li:gt(0)").remove();
        }

        if (divBox.find("tr").length > 0) {
            loadDivHtml = $("<tr align='center'  class='loadPanl'  ><td style='padding:15px 0px;border:0px' colspan='" + divBox.find("tr:first th,td").length + "' >" + loadDivHtml.html() + "</td></tr>");

        }

        //divBox.html("");
        divBox.find(".loadPanl").remove();
        divBox.append(loadDivHtml);
    },
    //divBox:显示内容的容器
    //data:查询出的数据 示例：{page:1,pageSize:20,totalCount:20,data:[],msg:""}
    dataPanl: function (divBox, trBox, dataList, options) {
        options = $.extend(this.defaults, options);
        dataList = dataList || [];
        if (dataList.length < options.showMinLine) {
            dataList.length = options.showMinLine;
        }
        $.each(dataList, function (i, dataObj) {
            var trclone = trBox.clone();
            $.each(trclone.find("[bindId]"), function (j, bindObj) {
                with ($(bindObj)) {
                    var bindKey = attr("bindId");
                    var showTxt = options.lineDefalut;
                    var _this = $(this);
                    if (!dataObj) {
                        showTxt = options.lineDefalut;
                    } else if (options.otherBind && options.otherBind[bindKey]) {
                        showTxt = options.otherBind[bindKey].call(dataObj, _this);
                    } else if (dataObj[bindKey]) {
                        showTxt = dataObj[bindKey];
                    }
                    if (showTxt !== false) {
                        _this.html(showTxt);
                    }
                    if (_this.get(0).style["overflow"] == "hidden") {
                        _this.attr("title", _this.text());
                    }
                }
            });
            if (i == 0)
                trclone.addClass("odd");

            trclone.toggleClass("odd");
            trclone.show();
            divBox.append(trclone);
        });
    },
    //divBox:显示内容的容器
    //dataList:查询出的数据 示例：{page:1,pageSize:20,totalCount:20,data:[],msg:""}
    //options:插件参数设置
    //data:查询到的完整数据，用于执行完成时，把data返回
    data: function (divBox, dataList, options, data) {

        options = $.extend(this.defaults, options);

        var lastObj = "";
        divBox = this.defaults.loadDivBox;
        if (options.trObj !== undefined) {
            lastObj = options.trObj.clone();
        }
        else if (divBox.find(".dataContainer").length > 0) {
            lastObj = divBox.find(".dataContainer:first").clone();
        }
        else if (divBox.find("tr").length > 0) {
            lastObj = divBox.find("tr:eq(1)").clone();
        }
        else if (divBox.find("li").length > 0) {
            lastObj = divBox.find("li:first").clone();
        }
        if (lastObj) {
            lastObj.show();
        }
        divBox.find(".loadPanl").remove();

        if ((options.showMinLine === 0 && dataList && dataList.length < 1) || dataList === "null") {

            this.setLoadBox(divBox, true);

        } else {
            this.dataPanl(divBox, lastObj, dataList);
        }

        if (options.callback) {
            options.callback(data);
        }

    },

    //数据查询
    //执行分页查询必须有参数 page,pageSize
    post: function (postParams) {
        with (postParams) {

            option = option || {};

            if (typeof pageBox != "undefined") {
                pageBox.html("");
            }

            option.loadDivBox = divBox;

            if (divBox.find("table").length > 0)
                option.loadDivBox = divBox.find("table");

            this.setLoadBox(option.loadDivBox);

            var that = this;
            //执行查询
            $.post(postUrl, params, function (data) {
                if (data) {
                    var resultData = data.data || data.list || data.dataList || data.datalist;
                    var options = $.extend({ pageNum: data.page, pageSize: data.pageSize, totalCount: data.totalCount }, option);
                    //执行数据绑定
                    that.data(divBox, resultData, options, data);


                    /************设定分页控件*************/
                    if (typeof pageBox != "undefined" && typeof data.page == "number") {
                        that.pager(postUrl, params, options, divBox, pageBox);
                    }
                }
            }, "json");
        }
    },

    timeFormatter: function (time, fmt) {
        var da;

        fmt = fmt || "yyyy-MM-dd";
        if (fmt.indexOf("-mm") !== -1) {
            fmt = fmt.replace(/-mm/, "-MM");
        }
        var str = fmt;
        var result = "";
        try {

            if (typeof time === "string" && time.indexOf("/Date(") > -1) {
                da = new Date(parseInt(time.replace("/Date(", "").replace(")/", "").split("+")[0]));
            } else if (typeof time == "string") {
                da = new Date(time)
            } else if (typeof time == "object") {
                da = time;
            }

            if (global_language && global_language == "EN") {
                str = str.replace(fmt.split(' ')[0], getEngilshTxt(da));
            } else {
                str = str.replace(/yyyy|YYYY/, da.getFullYear());
                str = str.replace(/yy|YY/, (da.getYear() % 100) > 9 ? (da.getYear() % 100).toString() : '0' + (da.getYear() % 100));

                str = str.replace(/MM/, da.getMonth() > 8 ? parseInt(da.getMonth() + 1) : '0' + parseInt(da.getMonth() + 1));
                str = str.replace(/M/g, parseInt(da.getMonth() + 1));


                str = str.replace(/dd|DD/, da.getDate() > 9 ? da.getDate().toString() : '0' + da.getDate());
                str = str.replace(/d|D/g, da.getDate());


            }
            str = str.replace(/hh|HH/, da.getHours() > 9 ? da.getHours().toString() : '0' + da.getHours());
//            str = str.replace(/h|H/g, da.getHours());   //时间不允许单位数，造成英文时间格式错误！
            str = str.replace(/mm/, da.getMinutes() > 9 ? da.getMinutes().toString() : '0' + da.getMinutes());
//            str = str.replace(/m/g, da.getMinutes());

            str = str.replace(/ss|SS/, da.getSeconds() > 9 ? da.getSeconds().toString() : '0' + da.getSeconds());
//            str = str.replace(/s|S/g, da.getSeconds());

            return str;
        } catch (e) {
            return "";
        }

    },
    //分页方法
    pager: function (postUrl, params, option, divBox, pageBox) {
        var options = {
            pageNum: 1,
            pageSize: 10,
            firstSize: 0,
            totalCount: 0,
            totalPage: 0,
            loadFunction: undefined,
            pageObject: pageBox
        };
        options = $.extend(options, option);
        with (options) {
            pageNum = parseInt(pageNum);
            totalCount = parseInt(totalCount);
            pageSize = parseInt(pageSize);
            //设置总页数
            totalPage = parseInt(totalCount / pageSize);
            if (totalCount % pageSize > 0) {
                totalPage++;
            }
            if (totalCount == 0) {
                if (typeof (pageObject) == "string") {
                    pageObject = $(pageObject);
                }
                pageObject.html(""); return false;
            };
            var ps = '';

            if (pageNum > 1)
            { ps += '<li  class="btnPage" pageNum="' + (pageNum - 1) + '"> ' + language.common.prevPage + ' </li>'; }

            var stratPN = 1;
            var endPN = totalPage + 1;

            var tag = false;

            if (pageNum <= 4) {

                if (totalPage >= 6) {
                    endPN = 6; tag = true;
                }
                else { endPN = totalPage + 1; }
            }
            else {
                stratPN = pageNum - 2;

                if (pageNum + 4 > totalPage) {
                    stratPN = totalPage - 4;
                    endPN = totalPage + 1;
                }
                else {
                    endPN = pageNum + 3;
                    tag = true;
                }
                if (pageNum >= 5 && stratPN > 1) {
                    ps += '<li  class="btnPage"  pageNum="1">1</li><li class="wd_page_span" ><span>...</span></li>';
                }
                else {
                    stratPN = 1;
                }
            }
            for (var i = stratPN; i < endPN; i++) {
                if (i == pageNum) {
                    ps += '<li class="active">' + i + '</li>';
                }
                else {
                    ps += '<li  class="btnPage" pageNum="' + i + '">' + i + '</li>';
                }
            }
            if (tag) {
                ps += '<li class="wd_page_span" ><span>...</span></li><li  class="btnPage" pageNum="' + totalPage + '">' + totalPage + '</li>';
            }
            if (pageNum < totalPage) {
                ps += '<li  class="btnPage" pageNum="' + (pageNum + 1) + '">  ' + language.common.nextPage + ' </li>';
            }
            firstSize = firstSize == 0 ? pageSize : firstSize;
            //ps += '<li  class="btnPage" pageNum="' + (totalPage) + '"> 末 頁 </li>  ';
            if (typeof (pageObject) == "string") {
                pageObject = $(pageObject);
            }
            pageObject.html("<ul>" + ps + "</ul>");

            var currmyPager = options;
            var that = this;
            currmyPager.pageObject.find(".btnPage").click(function () {
                currmyPager.pageNum = $(this).attr("pageNum");

                //如果有设置加载方法时，调用加载方法进行分页
                if (currmyPager.loadFunction !== undefined) {
                    currmyPager.loadFunction.call(currmyPager);
                } else {
                    params.page = currmyPager.pageNum;
                    //post请求当前页的数据
                    that.post({ postUrl: postUrl, params: params, divBox: divBox, pageBox: currmyPager.pageObject, option: options });
                }
            });
            //设置pagesize初始值
            currmyPager.pageObject.find("select").val(currmyPager.pageSize);
            currmyPager.pageObject.find("select").one("change", function () {
                currmyPager.pageSize = $(this).val();
                params.pageSize = currmyPager.pageSize;
                //post请求当前页的数据
                that.post({ postUrl: postUrl, params: params, divBox: divBox, pageBox: currmyPager.pageObject, option: options });

            });
        }

    }

}


