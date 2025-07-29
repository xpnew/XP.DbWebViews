var isIE = (document.all) ? true : false;

//var $ = function(id) {
//    return "string" == typeof id ? document.getElementById(id) : id;
//};
var $$ = function (id) {
    return "string" == typeof id ? document.getElementById(id) : id;
};
String.prototype.trim = function () {
    if (!this) return;
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.EndWith = function (smallStr) {
    if (!this) return -1;
    var bigLength = this.length;
    var smallLength = smallStr.length;
    if (smallLength > bigLength)
        return false;
    if (this.substr(bigLength - smallLength, smallLength) == smallStr) {
        return true;
    }
    return false;
}

function CheckNull(target) {
    if (null == target || undefined == target || 'undefined' == typeof (target) || ('string' == typeof (target) && '' == target)) {
        return true;
    }
    return false;
}

function GetRandomNum(Min, Max) {
    var Range = Max - Min;
    var Rand = Math.random();
    return (Min + Math.round(Rand * Range));
}


//********** 用来控制菜单和显示和隐藏

function ShowSub(sid) {
    var obj = $$(sid);
    var parentUL = obj.parentNode.parentNode;
    var oldstatus = obj.style.display;
    //		 trace(parentUL.childNodes[0].childNodes[0].innerHTML);
    //		 trace(parentUL.childNodes[0].childNodes[1].innerHTML);
    //		 trace(parentUL.childNodes[0].childNodes[2].innerHTML);
    // alert(parentUL.childNodes[0].childNodes[i].style.display = 'none');
    //return ;
    for (var i = 0; i < parentUL.childNodes.length; i++) {
        if ("UL" == parentUL.childNodes[i].tagName) {
            parentUL.childNodes[i].style.display = 'none';
        }

        for (var j = 0; j < parentUL.childNodes[i].childNodes.length; j++) {
            //trace();
            if ("UL" == parentUL.childNodes[i].childNodes[j].tagName) {
                parentUL.childNodes[i].childNodes[j].style.display = 'none';
            }
        }

        //alert(parentUL.childNodes[i].childNodes[i].style.display = 'none');
        // parentUL.childNodes[i].childNodes[i].style.display = 'none';
    }
    if (oldstatus == 'none') {
        obj.style.display = '';
    }
    else {
        obj.style.display = 'none';
    }
};

function displaySubMenu(li) {
    var subMenu = li.getElementsByTagName("ul")[0];
    subMenu.style.display = "block";
}
function hideSubMenu(li) {
    var subMenu = li.getElementsByTagName("ul")[0];
    subMenu.style.display = "none";
}

function INIT_Nav() {
    var subli_w = $('#navigation li ul li').width();
    var subli_h = $('#navigation li ul li').height();
    $('#navigation>li>ul').each(function (index, element) {
        var newWidth = $(this).children('li').length * subli_w;
        //$(element).width( newWidth);
        var newHeight = $(this).children('li').length * subli_h;
        $(element).height(newHeight);
    });
    $('#navigation>li').each(function (index, element) {
        if ($(this).children('ul').length > 0) {
            var li_length = $(this).find('li').length;
            $(this).hover(function () {
                $(this).children('ul').show();
                NavShowSub(this, 0, li_length);
            }, function () {
                //$(this).children('ul').children('li').hide();
                NavHideSub(this, li_length);
            });
        }
    });

}
function NavShowSub(obj, idx, li_length) {
    if (idx >= li_length) {
        return;
    }
    $(obj).find('li').eq(idx).show();
    idx++;
    setTimeout(function () { NavShowSub(obj, idx, li_length); }, 20);
};
function NavHideSub(obj, idx) {
    idx--;
    if (idx < 0) {
        $(obj).children('ul').children('li').hide()
        return;
    }
    $(obj).find('li').eq(idx).hide();
    setTimeout(function () { NavHideSub(obj, idx); }, 20);
};

$(function () {
    //处理导航
    INIT_Nav();
    //填充工艺名称
    FillCraftName();

});

//通过Id字段填充名称
function FillCraftName() {
    //ValueInput=HiddenField_ParentId&amp;NameInput=TextBox_ParentName
    var InputArray = $('input:hidden[name="CraftIds"]');
    if (0 == InputArray.length) {

        return;
    }

    for (var i = 0 ; i < InputArray.length; i++) {
        try{
            FillCraftNameOne(InputArray[i]);

        } catch (e) {

        }


    }

    //if ('' == $('#TextBox_CameraId').val() && '' != $('#HiddenField_CameraId').val()) {
    //    var id = $('#HiddenField_CameraId').val();
    //    $.get('AJAX/FindItemName.aspx?act=JiankongMulti&id=' + id + '&d=' + Math.random(), null, function (msg) {
    //        if (null == msg || '' == msg || '00000' == msg || '0' == msg)
    //            return;
    //        $('#TextBox_CameraId').val(msg);
    //    });
    //}

}
function FillCraftNameOne(obj){
    //alert(obj.value);
    var IdString = obj.value;
    if (CheckNull(IdString))
        return;
    var SpanByClass = $(obj).siblings('span.CraftName').eq(0);

    if (null != SpanByClass && 0 != SpanByClass.length) {
        SpanByClass.text('/img/waiting08.gif');
    }
    $.get('/PageAjaxData/CraftAJAX/Ids2Names/?d=' + Math.random(), { IdString: IdString }, function (data) {
        var SpanByClass = $(obj).siblings('span.CraftName').eq(0);
        if (null != SpanByClass && 0 != SpanByClass.length) {
            SpanByClass.text('');
        }
        if (CheckNull(data)){
            ymPrompt.alert('信息格式错误');
            return;
        }
        if (data.StatusCode > 0) {
            var InputByName = $(obj).siblings('input[name="CraftName"]').eq(0);
            if (null != InputByName && 0 != InputByName.length) {
                InputByName.val(data.Body);
            } else {
                if (null != SpanByClass && 0 != SpanByClass.length) {
                    SpanByClass.text(data.Body);
                }
            }
        }
        else if (data.StatusCode == -60200591) {
            ymPrompt.alert('没有权限');
        }
        else {
            ymPrompt.alert('操作失败');
        }


    });

}

var MenuTimer01;
$(document).ready(function () {

    $('#nav01 li').hover(
	  function () {
	      $(this).addClass("hover");
	  },
	  function () {
	      $(this).removeClass("hover");
	  }
	);
    //搜索
    $('#search input.k').val('search');
    $('#search input.k').focus(function () {
        if ($(this).val() == 'search') {
            $(this).val('');
        }
    });
    $('#search input.k').blur(function () {
        if ($(this).val() == '') {
            $(this).val('search');
        }
    });
    $('#search a').bind('click', function () {
        var k_input = $('#search :text').eq(0);
        var search_keyword = k_input.val();
        //alert(search_keyword);
        if (null != search_keyword && '' != search_keyword) {
            window.location.href = 'search.aspx?k=' + search_keyword;
            return true;
        }
        return false;
    });


    var url_pathname = window.location.pathname;
    if (null != window.location.search && '' != window.location.search) {

    }

    var url_filename = url_pathname.substr(url_pathname.lastIndexOf('/') + 1);


    //自动展开包含链接菜单
    $('#menu dd').each(function () {

        //alert(url_filename);
        if (0 < $(this).find('a[href^="' + url_filename + '"]').length) {
            $(this).show();
            var dlobj = this.parentNode;
            var dtobj = $(this).siblings('dt').get(0);
            var imgDom = $(dtobj).find('img').get(0);
            var srcStr = imgDom.src;
            var regEx1 = /(.+)_hover(.gif)/;
            if (regEx1.test(srcStr)) {
                srcStr = srcStr.replace(regEx1, '$1$2')
            } else {
                var reg2 = /(.+)(\.gif)/;
                srcStr = srcStr.replace(reg2, '$1_hover$2')
            }
            imgDom.src = srcStr;

        }
    });


    $('#menu dd div.sub').hover(
								function () {
								    $('#menu dd div.sub').removeClass("iehover");
								    clearTimeout(MenuTimer01);
								    $(this).addClass("iehover");

								},
								function () {
								    //setTimeout($(this).removeClass("iehover"),3500);
								    MenuTimer01 = setTimeout('memu_out()', 5500);
								}
							);

});
function memu_out() {
    $('#menu dd div.sub').removeClass("iehover");
}


function ImgMouseEffect(pic, flag) {
    var filepath = pic.src;
    if (flag) {
        filepath = filepath.replace('.png', '_over.png');
        filepath = filepath.replace('.gif', '_over.gif');
        pic.src = filepath;
    } else {
        filepath = filepath.replace('_over.png', '.png');
        filepath = filepath.replace('_over.gif', '.gif');
        pic.src = filepath;
    }
}


$(document).ready(function () {
    //鼠标经过行变色
    //要求css定义好tr_on和tr_off的样式
    //最好是td本身未使用背景色
    //使用方法：
    //方法一，在需要使用的Table或者Table外面的容器上添加ID “DateList01”或者“QueryData”
    //依法二，在需要使用的Table添加class“tb01”
    $('#DateList01 tr').hover(
		function () {
		    $(this).addClass("tr_on");
		    $(this).removeClass("tr_off");
		},
		function () {
		    $(this).addClass("tr_off");
		    $(this).removeClass("tr_on");
		}
    );

    $('#QueryData tr').hover(
		function () {
		    $(this).addClass("tr_on");
		    $(this).removeClass("tr_off");
		},
		function () {
		    $(this).addClass("tr_off");
		    $(this).removeClass("tr_on");
		}
    );
    $('table.tb01 tr').hover(
		function () {
		    $(this).addClass("tr_on");
		    $(this).removeClass("tr_off");
		},
		function () {
		    $(this).addClass("tr_off");
		    $(this).removeClass("tr_on");
		}
    );

		

    $('table.tb01 tr').live('mouseover mouseout', function (event) {
        if (event.type == 'mouseover') {
            $(this).addClass("tr_on");
        } else {
            $(this).removeClass("tr_on");
        }
    });

    $('#DivList table tr').live('mouseover mouseout', function (event) {
        if (event.type == 'mouseover') {
            $(this).addClass("tr_on");
        } else {
            $(this).removeClass("tr_on");
        }
    });
})


function SetSelecterByHide(id) {
    var id_hide = id + "_hide";
    if ($$(id) && $$(id_hide)) {
        getValueFormHidden($$(id), $$(id_hide));
    }
}
function getValueFormHidden(s, s_hide) {
    if (s_hide.value != "") {
        for (i = 0; i < s.length; i++) {
            if (s.options[i].value == s_hide.value) {
                s.selectedIndex = i;
            }
        }
    }
}



if (!isIE) {
    HTMLElement.prototype.__defineGetter__("currentStyle", function () {
        return this.ownerDocument.defaultView.getComputedStyle(this, null);
    });
}

function getStyleWidth(obj) {
    if (!obj) return;
    var w = obj.currentStyle.width ? obj.currentStyle.width : null;
    w = obj.style.width ? obj.style.width : w;
    if (!w || w == 'auto') {
        w = obj.offsetWidth ? obj.offsetWidth : null;
    }
    w = parseInt(w);
    return w;
}
function getStyleHeight(obj) {
    if (!obj) return;
    var h = obj.currentStyle.height ? obj.currentStyle.height : null;
    h = obj.style.height ? obj.style.height : h;
    if (!h || h == 'auto') {
        h = obj.offsetHeight ? obj.offsetHeight : null;
    }
    h = parseInt(h);
    return h;
}
function getStyleMarginWidth(obj) {
    if (!obj) return;
    var left = 0;
    var right = 0;
    if (obj.currentStyle) {
        left = obj.currentStyle.marginLeft ? parseInt(obj.currentStyle.marginLeft) : 0
        right = obj.currentStyle.marginRight ? parseInt(obj.currentStyle.marginRight) : 0
    }
    left = obj.style.marginLeft ? obj.style.marginLeft : left;
    right = obj.style.marginRight ? obj.style.marginRight : right;
    var w = parseInt(left) + parseInt(right);
    return w;
}
function getStyleMarginHeight(obj) {
    if (!obj) return;
    var top = 0;
    var btm = 0;
    if (obj.currentStyle) {
        top = obj.currentStyle.marginTop ? parseInt(obj.currentStyle.marginTop) : 0
        btm = obj.currentStyle.marginBottom ? parseInt(obj.currentStyle.marginBottom) : 0
    }
    top = obj.style.marginTop ? obj.style.marginTop : top;
    btm = obj.style.marginBottom ? obj.style.marginBottom : btm;
    var h = parseInt(top) + parseInt(btm);
    return h;
}

/*******************************************************************
*addCookie(name,value,dateNum)添加cookie
*
*dateNum如果为负数，就是删除cookie
*
*
*********************************************************************/

function addCookie(name, value, dateNum, path, domain) {
    name = name.trim();
    value = value.trim();
    if (null == dateNum) {
        dateNum = 30;
    }
    if (name != '') {
        var newDate = new Date();
        newDate.setTime(newDate.getTime() + (dateNum) * 24 * 60 * 60 * 1000);
        newDate = newDate.toGMTString();
        var strCookie = name + '=' + escape(value);
        if (dateNum && dateNum != '')
            strCookie += ';expires=' + newDate;
        strCookie += (path) ? ';path=' + path : '';
        strCookie += (domain) ? ';domain=' + domain : '';
        document.cookie = strCookie;
    }
}
function delCookie(name) {
    addCookie(name, '', -1, '/');
}
function CookieExist(name) {
    name += '=';
    if (document.cookie.indexOf(name) == -1)
        return false
    else
        return true;

}
function getCookie(CookieName) {
    if (CookieName.trim() == '')
        return '';
    CookieName = CookieName.trim();
    var resultCookie = '';
    if (CookieExist(CookieName)) {
        var strCookie = document.cookie;
        if (!strCookie || strCookie == '')
            return false;
        var arrCookies = strCookie.split(';');
        for (var i = 0; i < arrCookies.length; i++) {
            arrCookie_single = arrCookies[i].split('=');
            if (arrCookie_single[0].trim() == CookieName) {
                if ('' == arrCookie_single[1])
                    return null;
                resultCookie = unescape(arrCookie_single[1]);
            }
        }
    }
    return resultCookie;
}
function getBackgroundImg(obj) {
    if (obj.style.backgroundImage != '')
        return obj.style.backgroundImage;
    if (obj.currentStyle.backgroundImage != '')
        return obj.currentStyle.backgroundImage;
    return null;
}

function getChileUL(obj) {
    var NodeList = obj.childNodes;
    var s;
    for (i = 0; i < NodeList.length; i++) {
        //if(NodeList[i].tagName.toLowerCase == 'ul')
        if (NodeList[i].tagName == 'UL')
            return NodeList[i];
        s += NodeList[i].tagName;

    }
    return null;
}
/********************************************************************
**
**比较通用的正则表达式，捕获url各个部分。
**注意各部分基本上都包含了相应的符号，例如端口号如果捕获成功，那就是':80'
**函数返回一个正则表达式捕获数组。
**注意，现在获得的是一个数组，所以需要通过arr[i]的方式引用。
**正则表达式所有的匹配说明::.........
**$0
**整个url本身。如果$0==null，那就是我的正则有意外，未捕获的可能。
**有一种未捕获的情况已经被发现，那就是域名后面没有以'/'结尾，如：'http://localhost'
**但是经过我的测试，IE和firefox会自动把域名后面加上'/'的。
**$1-$4  协议，域名，端口号，还有最重要的路径path！
**$5-$7  文件名，锚点(#top)，query参数(?id=55)
**
*********************************************************************/

function UrlRegEx(url) {
    //如果加上/g参数，那么只返回$0匹配。也就是说arr.length = 0
    var re = /(\w+):\/\/([^\:|\/]+)(\:\d*)?(.*\/)([^#|\?|\n]+)?(#.*)?(\?.*)?/i;
    //re.exec(url);
    var arr = url.match(re);
    return arr;

}
function Debug(msg,obj) {
    if (window.console && window.console.log) {
        if(null == obj)
            console.log(msg);
        else
            console.log(msg,obj);
    }
}


function trace(str) {
    if (!document.getElementById('DebugBox')) return;
    document.getElementById('DebugBox').value += str + '\n';
}
/******************************************************************
* URI 管理类。
* 一个URI分成两部分：URL和QueryString，这里主要是对后者进行管理。
* 默认以?打头，但是可以修改Url变量组成完成的URI
* .Url变量：设置跳转地址。
* .Add(name,key)  添加变量
* 主要的方法是getURI()，g()是前一个方法的简洁写法，另外重写了toString()(继承自Object）
******************************************************************/
function URI() {
    var _this = this; 	//防止类成员的this和类本身的this混淆，所以在类成员的代码段里有_this来引用。
    var str = '?'; 		//var打头，可以当作private用
    this.Url = ''; 		//this打头，可以当作public用
    this.Add = function (str1, str2) {
        if (str == '?') {
            str += str1 + '=' + str2;
        }
        else {
            str += '&' + str1 + '=' + str2;
        }
    }
    this.toString = function () {
        return _this.getURI();
    }
    this.getURI = function () {
        return _this.Url + str;
    }
    //------------  下面这种不行！
    //this.g = this.getURI();
    this.g = function () { //getURI()简写作g()
        return _this.getURI();
    }
}

function ResizeImg(obj, Predict_w, Predict_h) {
    if (!obj) return;
    //predict  预言；预告；预示；预报；预计：
    //预计的比率
    var r1 = Predict_w / Predict_h;
    var w = obj.width;
    var h = obj.height;
    //实际的比率
    var r2 = w / h;
    //window.status = "w:" + w + " h:" + h;
    if (r2 == 1) {
        if (w < Predict_w) { return; }
        obj.width = Predict_w;
        if (obj.parentNode.tagName != "A") {
            obj.onclick = function () { window.open(obj.src, '', ''); };
            obj.onmouseover = function () { this.style.cursor = 'hand'; };
            obj.alt = "点击查看大图!";
        }
        return;
    }
    if (r2 >= r1) {
        if (w <= Predict_w) { return; }
        obj.width = Predict_w;
        obj.height = Predict_w / r2;
    } else {
        if (h <= Predict_h) { return; }
        obj.height = Predict_h;
        obj.width = Predict_h * r2;
    }
    if (obj.parentNode.tagName != "A") {
        obj.onclick = function () { window.open(obj.src, '', ''); };
        obj.onmouseover = function () { this.style.cursor = 'hand'; };
        obj.alt = "点击查看大图!";
    }
}


function doZoom(size) {
    $('div.fzfw_viewnr').css('font-size', size + 'px');
}
function GetQueryItem(key) {
    return getRequestQueryString(key);
}
function getRequestQueryString(key, url) {
    var QueryString
    if (null == url || '' == url) {
        QueryString = location.search;
        if (0 > QueryString.indexOf('?')) {
            return null;
        }
        QueryString = QueryString.substr(1);
    }
    else {
        if (0 > url.indexOf('?')) {
            return null;
        }
        QueryString = url.substr(url.indexOf('?'));
    }
    var arr = QueryString.split("&");
    key = key.toLowerCase();
    for (var i = 0; i < arr.length; i++) {
        if (key == arr[i].split("=")[0].toLowerCase()) {
            return arr[i].split("=")[1];
        }
    }
    return null;
}


function SetSelecterByHide(id) {
    var id_hide = id + "_hide";
    if ($$(id) && $$(id_hide)) {
        getValueFormHidden($$(id), $$(id_hide));
    }
}
function getValueFormHidden(s, s_hide) {
    if (s_hide.value != "") {
        for (i = 0; i < s.length; i++) {
            if (s.options[i].value == s_hide.value) {
                s.selectedIndex = i;
            }
        }
    }
}

//控制退格键使网页退回
$(document).keydown(function (e) {
    var doPrevent;
    if (e.keyCode == 8) {
        var d = e.srcElement || e.target;
        if (d.tagName.toUpperCase() == 'INPUT' || d.tagName.toUpperCase() == 'TEXTAREA') {
            doPrevent = d.readOnly || d.disabled;
        }
        else
            doPrevent = true;
    }
    else
        doPrevent = false;

    if (doPrevent)
        e.preventDefault();
}); 