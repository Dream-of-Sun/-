$(function () {
    $.extend({
        serializeMVC: function (serializeArray) {
            var strJson = "{";
            $.each(serializeArray, function (obj) {
                strJson += this.name + ":";
                strJson += "'";
                var arr1 = this.value.split("'");
                var value = "";
                for (var i = 0; i < arr1.length; i++) {
                    if (i > 0) {
                        value += "\\'";
                    }
                    value += arr1[i];
                }
                var arr2 = value.split("\n");
                value = "";
                for (var i = 0; i < arr2.length; i++) {
                    if (i > 0) {
                        value += "\\n";
                    }
                    value += arr2[i];

                }
                strJson += value + "',";
            });
            strJson += "}";
            strJson = strJson.replace(",}", "}");
            return eval('(' + strJson + ')');
        },
        loadingresize: function () {//只针对loading加载框
            var top = parseInt((document.documentElement.clientHeight -95) / 2);
            var left = parseInt((document.documentElement.clientWidth -91) / 2);
            $("#weui_toast").css({ "position": "fixed", "width": "7.6em", "min-height": "7.6em", "top": top, "left": left, "z-index": "999999", "background": "#1e60c0", "border-radius": "5px", "color": "#FFFFFF", "opacity": "0.8" });
        },//加载loading
        loading: function (msg,url) {
            var str = "<div id=\"loadingToast\" class=\"weui_loading_toast\"><div class=\"weui_mask_transparent\"></div>";
            str += "<div class=\"weui_toast\" id=\"weui_toast\"><div class=\"weui_loading\"><div class=\"weui_loading_leaf weui_loading_leaf_0\"></div>";
            str += "<div class=\"weui_loading_leaf weui_loading_leaf_1\"></div><div class=\"weui_loading_leaf weui_loading_leaf_2\"></div>";
            str += "<div class=\"weui_loading_leaf weui_loading_leaf_3\"></div><div class=\"weui_loading_leaf weui_loading_leaf_4\"></div>";
            str += "<div class=\"weui_loading_leaf weui_loading_leaf_5\"></div><div class=\"weui_loading_leaf weui_loading_leaf_6\"></div>";
            str += "<div class=\"weui_loading_leaf weui_loading_leaf_7\"></div><div class=\"weui_loading_leaf weui_loading_leaf_8\"></div>";
            str += "<div class=\"weui_loading_leaf weui_loading_leaf_9\"></div><div class=\"weui_loading_leaf weui_loading_leaf_10\"></div>";
            str += "<div class=\"weui_loading_leaf weui_loading_leaf_11\"></div></div><p class=\"weui_toast_content\">";
            str += msg != null && msg != undefined && msg != "" && msg != "undefined" ? (msg.length > 4 ? msg.substr(0, 4) : msg) : '加载中...';
            str += "</p></div></div>";
            if ($("#loadingToast").length > 0) {
                $("body").find("#loadingToast").remove();
            }
            $("body").append(str);
            $("#loadingToast").fadeIn(500);
            $.loadingresize();
            if (url != undefined && url != "" && url != null && url != "undefined") {
                setTimeout(function () { location.href = url; }, 1000);
            }
        },//关闭loading
        closeloading: function () {
            $("body").find("#loadingToast").remove();
        },//显示自定义对话框(有确定跟取消两种)
        weuidialog: function (content, func, title) {
            var dialog = "<div id=\"showweuidialog\" class=\"weui_dialog_confirm\"><div class=\"weui_mask\"></div><div class=\"weui_dialog\">";
            dialog += "<div class=\"weui_dialog_hd\"><strong class=\"weui_dialog_title\">" + (title == null ? "消息提示框" : title) + "</strong></div><div class=\"weui_dialog_bd\" style=\"text-align:center\">";
            dialog += content+ "<br></div><div class=\"weui_dialog_ft\">";
            dialog += "<a href=\"javascript:void(0);\" class=\"weui_btn_dialog default\" onclick=\"$.closeweuidialog()\">取消</a>";
            dialog += "<a href=\"javascript:void(0);\" class=\"weui_btn_dialog primary\" onclick=\"$.closeweuidialog(" + func + ")\">确定</a></div></div></div>";
            if ($("#showweuidialog").length == 0) {
                $("body").append(dialog);
            }
            $("#showweuidialog").fadeIn(500);
        },//影藏自定义对话框
        closeweuidialog: function (func) {
            $("body").find("#showweuidialog").remove();
            if (func != null && func != undefined && func != "" && func != "undefined") {
               eval("("+func+")");
            }
        },//弹出对话框 类似于alert
        alert: function (info, url, title) {
            var alertstr = "<div id=\"showweiuialert\"  class=\"weui_dialog_confirm\" style=\"background-color:#31749E\"><div class=\"weui_mask\"></div><div class=\"weui_dialog\"><div class=\"weui_dialog_hd\">";
            alertstr += "<strong class=\"weui_dialog_title\">" + (title != null ? title : "提示信息") + "</strong></div><div class=\"weui_dialog_bd\" style=\"text-align:center;\">";
            alertstr += info + "</div><div class=\"weui_dialog_ft\"><a href=\"javascript:;\" class=\"weui_btn_dialog primary\" onclick=\"$.closealert('" + url + "')\">确定</a></div></div></div>";
            if ($("#showweiuialert").length == 0) {
                $("body").append(alertstr);
            }
            $("#showweiuialert").fadeIn(500);
            if (url != undefined && url != "" && url != null && url != "undefined") {
                setTimeout(function () { $.closealert(url) }, 1000);
            } else {
                setTimeout(function () { $.closealert()}, 4000);
            }
        },
        closealert: function (url) {
            $("body").find("#showweiuialert").remove();
            if (url != undefined && url != "" && url != null && url != "undefined") {
                $.loading("跳转中..");
                setTimeout(function () { location.href = url; },1000);
            }
        },//弹出成功窗口
        success: function (info, func) {
            var msg = "<div class=\"weui_dialog_alert\" id=\"alertweiuisuccess\"><div class=\"weui_mask\"></div><div class=\"weui_dialog\"><div class=\"weui_msg\">";
            msg += "<div class=\"weui_icon_area\"><i class=\"weui_icon_success weui_icon_msg\"></i></div><div class=\"weui_text_area\"><h2 class=\"weui_msg_title\">";
            msg += "<a style=\"cursor:pointer\" title=\"" + info + "\">" + (info == null ? "&nbsp;&nbsp;操作成功！" : (info.length > 12 ? info.substr(0, 12) + "..." : info)) + "</a></h2>";
            msg += "</div></div><div class=\"weui_dialog_ft\"><a href=\"javascript:;\" class=\"weui_btn_dialog primary\" onclick=\"$.closesuccess(" + func + ")\">确定</a></div></div></div>";
            if ($("#alertweiuisuccess").length == 0) {
                $("body").append(msg);
            }
            $("#alertweiuisuccess").fadeIn(500);
            if (func != undefined && func != null && func != "" && func != "undefined") {
                setTimeout(function () {
                    $.closesuccess(func);
                }, 1000);
            }
        },//关闭弹出成功窗口
        closesuccess: function (func) {
            $("body").find("#alertweiuisuccess").remove();
            if (func != null && url != "" && func != undefined && func != "undefined") {
                func();
            }
        },//弹出失败窗口
        error: function (info) {
            var msg = "<div class=\"weui_dialog_alert\" id=\"\alertweiuierror\"><div class=\"weui_mask\"></div><div class=\"weui_dialog\"><div class=\"weui_msg\">";
            msg += "<div class=\"weui_icon_area\"><i class=\"weui_icon_msg weui_icon_warn\"></i></div><div class=\"weui_text_area\">";
            msg += "<p class=\"weui_msg_desc\"><a style=\"cursor:pointer\" title=\"" + info + "\">" + (info == null ? "&nbsp;&nbsp;操作失败！" : (info.length > 12 ? info.substr(0, 12) + "..." : info)) + "</a></p></div></div><div class=\"weui_dialog_ft\">";
            msg += "<a href=\"javascript:void(0);\"class=\"weui_btn_dialog primary\" onclick=\"$.closeerror()\">关闭错误</a></div></div></div>";
            if ($("#alertweiuierror").length == 0) {
                $("body").append(msg);
            }
            $("#alertweiuierror").fadeIn(500);
        },//关闭失败窗口
        closeerror: function () {
            $("body").find("#alertweiuierror").remove();
        },
        login: function (url) {
            if (url != undefined && url != "" && url != null && url != "undefined") {
                $.loading("跳转中..");
               parent.window.location.href = "/login/index.html?ReturnUrl=" + encodeURI(url);
            }
        },getpage: function (_url,_form, tag) {  //获取公用查询入口
            $.ajax({
                type: "Get",
                cache: false,
                async: true,
                url: _url.indexOf("?") > 0 ? (_url.indexOf("times=") > 0 ? "" : _url + "&times=" + new Date().getMilliseconds()) : (_url.indexOf("times=") > 0 ? "" : _url + "?times=" + new Date().getMilliseconds()),
                data: _form != undefined && _form != "" && _form != null && _form != "undefined" ? (_form.indexOf("{") >= 0 ? eval("(" + _form + ")") : $.serializeMVC($(_form).serializeArray())) : {},
                beforeSend: function () {
                    $.loading();
                },
                success: function (msg) {
                    if ($(tag).is("input")) {
                        $(tag).val(msg);
                    } else {
                        $(tag).html(msg);
                    }
                    $.closeloading();
                },
                complete: function (msg) {
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.closeloading();
                }
            });
        },getcommstr: function (_url, _form, tag) {  //获取公用查询入口
            $.ajax({
                type: "Get",
                cache: false,
                async: true,
                url: _url.indexOf("?") > 0 ? (_url.indexOf("times=") > 0 ? "" : _url + "&times=" + new Date().getMilliseconds()) : (_url.indexOf("times=") > 0 ? "" : _url + "?times=" + new Date().getMilliseconds()),
                data: _form != undefined && _form != "" && _form != null && _form != "undefined" ? (_form.indexOf("{") >= 0 ? eval("(" + _form + ")") : $.serializeMVC($(_form).serializeArray())) : {},
                beforeSend: function () {
                    $.loading("请稍后...");
                },
                success: function (msg) {
                    if (msg.indexOf("CS00") >= 0) {
                        href = msg.indexOf("|") > 0 ? msg.split("|")[1] : "/login/index";
                        window.parent.location.href = href;
                    } else {
                        if ($(tag).is("input")) {
                            $(tag).val(msg);
                        } else {
                            $(tag).html(msg);
                        }
                    }
                    $.closeloading();
                },
                complete: function (msg) {
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.closeloading();
                }
            });
        }, setcommstr: function (_url, _form, returnurl, func, info) {  //调用第三方窗口
            $.ajax({
                type: "Post",
                url: _url,
                cache: false,
                async: false,
                data: _form != undefined && _form != "" && _form != null && _form != "undefined" ? (_form.indexOf("{") >= 0 ? eval('(' + _form + ')') : $.serializeMVC($(_form).serializeArray())) : {},
                beforeSend: function () {
                    $.loading("请稍后...");
                },
                success: function (msg) {
                    var href = "";
                    if (msg.indexOf("CS00") >= 0) {
                        href=msg.indexOf("|") > 0 ? msg.split("|")[1] : "/login/index";
                        window.parent.location.href = href;
                    }
                    else if (msg.indexOf("true") >= 0 || msg.indexOf("ok") >= 0) {
                        if (func != null && func != undefined && func != "" && func != "undefined") {
                            func();
                        }
                        if (msg.indexOf("true") >= 0) {
                            href = msg.indexOf("|") >= 0 ? msg.split("|")[1] : returnurl;
                            $.alert(info != undefined && info != null && info != "" && info != "undefined" ? info : "操作成功！", href, "消息窗口");
                        }
                        else{
                            href = msg.indexOf("|") >= 0 ? msg.split("|")[1] : "/home/right";
                            $.loading("成功跳转..",href);
                        }
                    }else{
                        var message = msg == "false" ? "操作失败！" : (msg.indexOf("|") >= 0 ? msg.split("|")[1] : msg);
                        $.closeloading();
                        $.alert(message);
                    }
                },
                complete: function (msg) {
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.closeloading();
                }
            });
        }, del: function (_url, returnurl, func,msg) {  //调用第三方窗口
            $.weuidialog((msg==undefined? "一经确认无法撤销，您确定要执行此操作吗？":msg), "$.weidel('" + _url + "','" + returnurl + "','"+func+"')");
        }, weidel: function (_url, returnurl, func) {
            $.ajax({
                type: "Post",
                url: _url,
                cache: false,
                async: false,
                beforeSend: function () {
                    $.loading("请稍后...");
                },
                success: function (msg) {
                    if (msg.indexOf("CS00") >= 0) {
                        $.loading("跳转中..");
                        window.location.href = msg.indexOf("|") > 0 ? msg.split("|")[1] : "/login/index";
                    }
                    if (msg.indexOf("true") >= 0) {
                        if (func != null && func != undefined && func != "" && func != "undefined") {
                            $("#leftFrame", parent.document).attr("src", "/home/left");
                        } else {
                            $.alert("删除成功！", returnurl);
                        }
                    }
                    else {
                        $.alert(msg);
                    }
                    $.closeloading();
                },
                complete: function (msg) {
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.closeloading();
                }
            });
        }, dialog: function (url, info) {  //bootstrap自带的对话框，区别于上述的weui自带的对话框
            $.get(url, function (a) { $("#modalbody").html(a); });
            //$.getcommstr(url, "", "#modalbody");
            $("#myModal").fadeIn(500);
            $("#myModal").removeClass("fade");
            $("#myModalLabel").html(info != undefined && info != null && info != "" && info != "undefined" ? info : "标题");
        }, closedialog: function () {
            $("#myModal").fadeOut(500);
        }, iszhanghao: function (zhanghao, tag) {
            var reg = /^[a-zA-Z0-9_]{2,20}$/;///^[a-zA-z][a-zA-Z0-9_]{3,19}$/;
            if (!reg.test($(zhanghao).val())) {
                $.alert(tag + " 由2-20位的字母、下划线或数字组成！");
                return false;
            } else {
                return true;
            }
        }, ismima: function (pwd, tag) {
            var reg = /^[a-zA-Z0-9]{6,12}$/;
            if (!reg.test($(pwd).val())) {
                $.alert(tag + " 由数字或大小写字母组合而成，最少6位，最多12位！");
                return false;
            } else {
                return true;
            }
        }, isphone: function (phone, tag) {
            var reg = /^((1[0-9]{10})|((\d{3,4}-)+\d{7,8}))$/;
            if (!reg.test($(phone).val())) {
                $.alert(tag + " 不合法！");
                return false;
            } else {
                return true;
            }
        }, isnumeric: function (isword, tag) {
            var reg = /^[1-9]+(\.\d{1,2})?$|^0(\.\d{1,2})?$|^[1-9]+[0-9]*(\.\d{1,2})?$/;
            if (!reg.test($(isword).val())) {
                $.alert(tag + " 需是数字类型，如25，12.34，45.6等！");
                return false;
            } else {
                return true;
            }
        }, isword: function (isword, tag) {
            var reg = /^(?!(?:\d+|[a-zA-Z]+[-_]?)$)[\da-zA-Z-_]{4,20}$/;
            if (!reg.test($(isword).val())) {
                $.alert(tag + " 下划线或4~20位字母与数字组合！");
                return false;
            } else {
                return true;
            }
        }, isshuzi: function (isnum, tag) {
            var reg = /^\d+$/;
            if (!reg.test($(isnum).val())) {
                $.alert(tag + " 必须是纯数字类型，如0,3、15等！");
                return false;
            } else {
                return true;
            }
        }, ispost: function (isnum, tag) {//邮政编码
            var reg = /^(\d{6})$/;
            if (!reg.test($(isnum).val())) {
                $.alert(tag + " 需是6位数字！");
                return false;
            } else {
                return true;
            }
        }, isemail: function (isnum, tag) {//邮箱
            var reg = /\w@\w*\.\w/;
            if (!reg.test($(isnum).val())) {
                $.alert(tag + " 格式不正确！");
                return false;
            } else {
                return true;
            }
        }, isint: function (isnum, tag) {
            var reg = /^(0|([1-9][0-9]*))$/;
            if (!reg.test($(isnum).val())) {
                $.alert(tag + " 必须是整数类型，如3、15等！");
                return false;
            } else {
                return true;
            }
        }, isequ: function (exp1, exp2, tag) {//判断是否相等
            if ($(exp1).val() != $(exp2).val()) {
                $.alert(tag);
                return false;
            } else {
                return true;
            }
        }, isempty: function (exp, tag) {
            if ($.trim($(exp).val()) == "") {
                $.alert(tag + " 不能为空！");
                return false;
            } else {
                return true;
            }
        }, isyanzhengma: function (exp, tag) {
            if ($.trim($(exp).val()) == "") {
                $.alert(tag + " 不能为空！");
                $(exp).next("img").attr("src", "/login/validatecode.html?times=" + new Date().getMilliseconds());
                return false;
            } else {
                return true;
            }
        }, isselect: function (exp, tag) {
            if ($.trim($(exp).val()) == "" || $.trim($(exp).val()) == "-1") {
                $.alert(tag + " 不能为空！");
                return false;
            } else {
                return true;
            }
        }, yanzheng: function (_url, _form, _returnurl,func) {
            var result = $.innerfunc($(_form).find("input").not(":radio").not(":button"));
            if (!result && $("#yanzhengcode").length > 0) {
                $("#yanzhengcode").attr("src", "/login/validatecode.html?times=" + new Date().getMilliseconds());
            }
            result = result ? $.innerfunc($(_form).find("textarea")) : result;
            result = result ? $.innerfunc($(_form).find("select")) : result;
            if (result) {
                var $radio = $(_form).find("input[type=radio]").parent("div");
                if ($radio.hasClass("checked") && $radio.length > 0) {
                    var temp = "false";
                    for (var j = 0; j < $radio.length; j++) {
                        if ($radio.eq(j).hasClass("checked")) {
                            temp = "true";
                        }
                    }
                    if (temp != "true") {
                        $.alert($radio.eq(0).attr("msg"));
                        result = false;
                    }
                }
                if (result) {
                    if (_returnurl != null && _returnurl != undefined && _returnurl != "" && _returnurl != "undefined") {
                        $.setcommstr(_url, _form, _returnurl, func);
                    } else {
                        $.alert("操作已成功！");
                        if ($("#myModal").length > 0) { $("#myModal").fadeOut(200); }
                    }
                }
            }
        }, innerfunc: function ($obj) {
            var result = true;
            for (var i = 0; i < $obj.length; i++) {
                if ($obj.eq(i).attr("tag") != null && $obj.eq(i).attr("tag") != undefined && $obj.eq(i).attr("tag") != "" && $obj.eq(i).attr("tag") != "undefined" && $obj.eq(i).attr("tag").indexOf("equ") < 0) {
                    var item = "#" + $obj.eq(i).attr("id");
                    var tag = "$.is" + $obj.eq(i).attr("tag");
                    var msg = $obj.eq(i).attr("msg") != null && $obj.eq(i).attr("msg") != undefined && $obj.eq(i).attr("msg") != "" && $obj.eq(i).attr("msg") != "undefined" ? $obj.eq(i).attr("msg") : "文本框";
                    result = eval(tag + "('" + item + "', '" + msg + "')");
                    if (result == false) {
                        break;
                    }
                } else if ($obj.eq(i).attr("tag") != null && $obj.eq(i).attr("tag") != undefined && $obj.eq(i).attr("tag") != "" && $obj.eq(i).attr("tag") != "undefined" && $obj.eq(i).attr("tag").indexOf("equ") >= 0) {
                    var tag = "$.is" + $obj.eq(i).attr("tag");
                    tag = tag.substr(0, tag.length - 1);
                    var array = tag.replace("$.isequ(", "").split(",");
                    var msg = $obj.eq(i).attr("msg") != null && $obj.eq(i).attr("msg") != undefined && $obj.eq(i).attr("msg") != "" && $obj.eq(i).attr("msg") != "undefined" ? $obj.eq(i).attr("msg") : "文本框";
                    result = eval("$.isequ('#" + array[0] + "'" + ",'#" + array[1] + "'" + ",'" + msg + "')");
                    if (result == false) {
                        break;
                    }
                }
                else {
                    result == true;
                }
            }
            return result;
        }, go: function (_url, _form, info) {
            $.ajax({
                type: "Post",
                url: _url,
                cache: false,
                async: false,
                data: _form != undefined && _form != "" && _form != null && _form != "undefined" ? $.serializeMVC($(_form).serializeArray()) : {},
                beforeSend: function () {
                    $.loading();
                },
                success: function (msg) {
                    if (msg.indexOf("CS00") >= 0) {
                        window.location.href = msg.indexOf("|") > 0 ? msg.split("|")[1] : "/login/index.html";
                    }
                    else if (msg.indexOf("true") >= 0) {
                        if ($("#myModal").length > 0) { $("#myModal").fadeOut(200); }
                        $.alert(info, msg.split("|")[1], "消息窗口");
                    }
                    else {
                        $.alert(msg);
                    }
                    $.closeloading();
                },
                complete: function (msg) {
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.closeloading();
                }
            });
        }, code: function (s) {
            var code = base64encode(utf16to8(s));
            return code;
        }, dcode: function (s) {
            return utf8to16(base64decode(s))
        }, toolbar: function (a, b, c, d) {
            var array = new Array();
            array[0] = {
                iconCls: 'icon-reload', text: '刷新',
                handler: function () { c.treegrid('reload'); }
            };
            if (a == "True") {
                array[1] = "-";
                array[2] = {
                    iconCls: 'icon-add', text: '创建',
                    handler: function () { d(); }
                }
                if (b == "True") {
                    array[3] = "-";
                    array[4] = {
                        iconCls: 'icon-edit', text: '修改',
                        handler: function () {
                            var row = $(c).treegrid('getSelected');
                            if (row.Id != undefined && row.Id != null) { d(row.Id); } else {
                                d(row.id);
                            }
                        }
                    };
                }
            } else if (b == "True") {
                array[1] = "-";
                array[2] = {
                    iconCls: 'icon-edit', text: '修改',
                    handler: function () {
                        var row = $(c).treegrid('getSelected');
                        if (row) { d(row.Id); }
                    }
                };
            }
            return array;
        }
    });
    $(window).resize(function () {
        $.loadingresize();
    })
});
