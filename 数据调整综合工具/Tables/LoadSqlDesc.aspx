<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadSqlDesc.aspx.cs" Inherits="数据调整综合工具.Tables.LoadSqlDesc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/comm.css" rel="stylesheet" />
    <link href="../css/base.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.3.min.js"></script>
    <script src="../Scripts/comm.js"></script>
    
    <title>载入表格注释</title>

    <style type="text/css"  >

        .ExistGlobalName {min-width:4ch;}
        .Lucus {min-width:15ch;}
        .TimeColumn {min-width:12ch;}
        .FieldsDetails {max-width:50%;}
        .FieldsDetails span.FieldsDiscriptions {max-width:50%;width:inherit;}
        .FieldsDetails span.FieldsDiscriptions label {white-space: nowrap;}

    </style>
        <script type="text/javascript">


        //https://blog.csdn.net/hehemo/article/details/129620567


        //接下来，我们来看看如何用js实现一个简单的异步并发任务控制器。首先，我们需要定义一个类来表示控制器对象，并在构造函数中初始化以下三个属性：
        //`maxConcurrent`：最大并发数，也就是同时进行的异步任务数量。
        //`currentConcurrent`：当前并发数，也就是正在进行中或者已经完成但还未从队列中移除的异步任务数量。
        //`taskQueue`：任务队列，用一个数组来存储待执行或者正在执行中的返回promise对象函数。

        // 定义一个异步任务控制器类
        class AsyncTaskController {
            // 构造函数，接受最大并发数作为参数
            constructor(maxConcurrent) {
                // 初始化最大并发数
                this.maxConcurrent = maxConcurrent;
                // 初始化当前并发数
                this.currentConcurrent = 0;
                // 初始化任务队列
                this.taskQueue = [];
            }

            // 添加一个异步任务到队列中，接受一个返回promise的函数作为参数
            addTask(task) {
                // 将任务函数加入队列
                this.taskQueue.push(task);
                // 尝试执行下一个任务
                this.next();
            }

            // 执行下一个任务，如果当前并发数小于最大并发数，并且队列不为空，则从队列中取出一个任务并执行，否则什么都不做
            next() {
                if (this.currentConcurrent < this.maxConcurrent && this.taskQueue.length > 0) {
                    // 取出队列中的第一个任务函数，并从队列中移除它
                    const task = this.taskQueue.shift();
                    // 增加当前并发数
                    this.currentConcurrent++;
                    console.log('current  task : ', task);

                    // 执行任务函数，并处理其返回的promise对象
                    task()
                        .then((result) => {
                            // 如果成功，打印结果（或者做其他操作）
                            console.log('result ' ,  result);
                        })
                        .catch((error) => {
                            // 如果失败，打印错误（或者做其他操作）
                            console.error(error);
                        })
                        .finally(() => {
                            // 不管成功还是失败，都要减少当前并发数，并执行下一个任务（递归调用）
                            this.currentConcurrent--;
                            this.next();
                        });
                }
            }
        }


        </script>
        <script type="text/javascript">
            // 创建一个异步任务控制器实例，最大并发数为2
            const controller = new AsyncTaskController(3);

            $(function () {

                $('#GridView1 tr>td.Lucus').each(function (i, elm) {
                    var TableName = $(this).children('input[name = "TableName"]').val();
                    // SayTable(TableName);

                    //sleep(1);
                    //console.log('TableName sleep : ' + TableName, new Date())

                    //if (i > 30) return;
                    //GetCount(TableName, elm);


                    console.log('TableName : ' + TableName, new Date())

                    //controller.addTask(successTask(i));
                    controller.addTask(GetDesc(TableName, elm));
                    controller.addTask(GetFieldsDetails(TableName, elm));

                });

                $('#bt_Save').click(function () {

                    SendSave();

                })

            });



            // 模拟成功的异步任务，接受一个id作为参数，返回一个延迟1秒后resolve的promise对象，值为id * 2
            const successTask = (id) => () =>
                new Promise((resolve) => {
                    setTimeout(() => resolve(id * 2), 1000);
                });
            //从sql server 获取表格上的备注
            const GetDesc = (tb, elm) => () => new Promise((resolve) => {

                var _d = Math.random();
                var searchParams = new URLSearchParams()

                //searchParams.set('TableName', 'next')
                searchParams.set("TableName", tb);
                searchParams.set("_d", _d);
                console.log('will get TableName : ' + tb, new Date())


                var submitData = {
                    TableName: tb,
                    _d: _d
                };

                return $.post('?act=GetDesc', submitData, function (data) {
                    //$.jGrowl("------数据已经返回 数据 " + data, { life: 2000 });
                    if (0 < data.StatusCode) {

                        //$.jGrowl("------操作完成！ ", { life: 8000 });
                        //$.jGrowl("日志内容 " + data.Log, { life: 5000 });
                        //window.location.href = "";
                        var line = '';
                        var txt = '';

                        var tm = $('#TM_TableGlobalChk').html();
                        line = tm;
                        line = line.replace('{TM:InputValue}', tb + '|' + data.Body);
                        line = line.replace(' {TM:InputShowText}', data.Body );
                        txt = line;
                        $(elm).children('span.RowsCountText').html(' ' + txt + ' ');
                        resolve(data);

                    } else {
                        console.log(data.Title, data.StatusCode);
                        $(elm).children('span.RowsCountText').html('');
                        resolve(data);

                    }
                }, 'json');


            });

            //获取字段明细
            const GetFieldsDetails = (tb, elm) => () => new Promise((resolve) => {

                var _d = Math.random();
                var searchParams = new URLSearchParams()

                //searchParams.set('TableName', 'next')
                searchParams.set("TableName", tb);
                searchParams.set("_d", _d);
                console.log('will get TableName : ' + tb, new Date())


                var submitData = {
                    TableName: tb,
                    _d: _d
                };

                return $.post('?act=GetFieldsDetails', submitData, function (data) {
                    //$.jGrowl("------数据已经返回 数据 " + data, { life: 2000 });
                    if (0 < data.StatusCode) {

                        //$.jGrowl("------操作完成！ ", { life: 8000 });
                        //$.jGrowl("日志内容 " + data.Log, { life: 5000 });
                        //window.location.href = "";
                        var tm = $('#TM_FieldGlobalChk').html();
                        var line = '';
                        var txt = '';
                        var NeedShowList = false;
                        var lst = data.DataInfo;

                        if (null != lst && 0 < lst.length) NeedShowList = true;
                        
                        for (var i = 0; i < lst.length; i++) {
                            var item = lst[i];
                            line = tm;
                            line = line.replace('{TM:InputValue}', tb + '|' + item.ColumnName + '|' + item.GlobalName);
                            line = line.replace(' {TM:InputShowText}', item.ColumnName + '-' + item.GlobalName);
                            txt += line;
                        }

                        if (NeedShowList) {
                            $(elm).siblings('.FieldsDetails').children('span.FieldsDiscriptions').html(' ' + txt + ' ');
                        } else {
                            $(elm).siblings('.FieldsDetails').children('span.FieldsDiscriptions').html('');
                        }

                        resolve(data);

                    } else {
                        console.log(data.Title, data.StatusCode);
                        $(elm).siblings('.FieldsDetails').children('span.FieldsDiscriptions').html('');
                        resolve(data);

                    }
                }, 'json');


            });


        </script>
        <script  type="text/javascript">
            function SendSave() {
                var lst_tb = [];
                $('#GridView1').find('input[name="chk_TableGlobal"]:checked').each(function (i, e) {
                    //console.log('elememnt : ', e);
                    //console.log('value : ', e.value);
                    var valArr = e.value.split('|');

                    var item = { TableName: valArr[0], GlobalName: valArr[1] };
                    lst_tb.push(item);
                });


                var lst_field = [];
                $('#GridView1').find('input[name="chk_FieldGlobal"]:checked').each(function (i, e) {
                    //console.log('elememnt : ', e);
                    //console.log('value : ', e.value);
                    var valArr = e.value.split('|');

                    var item = { TableName: valArr[0], ColumnName: valArr[1], GlobalName: valArr[2] };
                    lst_field.push(item);
                });


                console.log('表格选项： ', lst_tb);
                console.log('字段选项： ', lst_field);

                var DataModel = { TableList: lst_tb, FieldList: lst_field };

                var SendStr = JSON.stringify(DataModel);
                console.log('SendStr： ', SendStr);

                if (0 == lst_tb.l) {
                    alert('至少选择一项（表格说明）');
                    return;
                }

                var Path = '?act=Save'
                Path += '&_dx=' + Math.random();
                $.ajax({
                    type: "POST",
                    url: Path,
                    data: SendStr,
                    contentType: 'application/json;charset=UTF-8',
                    dataType: "JSON",
                    error: function () {
                        alert('出错了');
                    },
                    success: function (data) {
                        if (typeof (data) == 'string') {
                            data = $.parseJSON(data);
                        }
                        console.log('data : ', data);
                        console.log('StatusCode : ', data.StatusCode);
                        if (null != data) {

                            if (0 < data.StatusCode) {

                                alert('保存完成！');
                            } else if (0 > data.StatusCode) {
                                alert('操作失败');
                            } else {
                                alert('返回异常');
                            }

                        } else {
                            alert("返回错误");
                        }
                    }
                });

            }
        </script>


</head>
<body>
    <form id="form1" runat="server">
        <div>
            222
        <asp:GridView ID="GridView1" runat="server" CssClass="tb01" Style="margin-top: 0px" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="表名" />
                <asp:BoundField DataField="GlobalName" HeaderText="中文名"  HeaderStyle-CssClass="ExistGlobalName" />
                <asp:BoundField DataField="crdate" HeaderText="创建时间" HeaderStyle-CssClass="TimeColumn" />
                <asp:BoundField DataField="refdate" HeaderText="修改时间" HeaderStyle-CssClass="TimeColumn" />
                <asp:TemplateField HeaderText="表名">
                    <ItemStyle CssClass="Lucus"></ItemStyle>
                    <ItemTemplate>
                        <span mydate="<%#Eval("name") %>" class="RowsCountText">载入中...</span>
                        <input id="Hidden5" name="id" type="hidden" value="<%#Eval("id") %>" />
                        <input id="Hidden0" name="TableGlobal" type="hidden" value="<%#Eval("id") %>" />
                        <input id="Hidden6" name="TableName" type="hidden" value="<%#Eval("name") %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="字段">
                    <ItemStyle CssClass="FieldsDetails"></ItemStyle>
                    <ItemTemplate>
                        <span class="FieldsDiscriptions">字段 明细</span>
                        <input id="Hidden5" name="DiscList" type="hidden" value="<%#Eval("id") %>" />

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

            <button type="button" id="bt_Save"  >保存选择</button>
            444
            22222
            <asp:GridView ID="GridView2" runat="server"></asp:GridView>
            <br />
            <br />
            <a href="../ShowAllTable.aspx">查看表&gt;&gt;&gt;</a><br />
            <br />
            <a href="../ShowAllViews.aspx">查看所有的视图&gt;&gt;&gt;</a><br />


        </div>
        <div>&nbsp;</div>
    </form>

    <div id="TM_FieldGlobalChk" style="display:none;">

        <label >        <input type="checkbox" name="chk_FieldGlobal" checked="checked" value="{TM:InputValue}" />
            {TM:InputShowText}
               
        </label>
    </div>
    <div id="TM_TableGlobalChk" style="display:none;">

        <label >        <input type="checkbox" name="chk_TableGlobal"  checked="checked" value="{TM:InputValue}" />
            {TM:InputShowText}
               
        </label>
    </div>

</body>
</html>
