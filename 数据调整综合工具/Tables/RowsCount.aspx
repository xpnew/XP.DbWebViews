<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RowsCount.aspx.cs" Inherits="数据调整综合工具.Tables.RowsCount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/comm.css" rel="stylesheet" />
    <link href="../css/base.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.3.min.js"></script>
    <script src="../Scripts/comm.js"></script>

    <script type="text/javascript">


        //https://www.jb51.net/javascript/287559p0l.htm


        class ConcurrencyControl {
            constructor(tasks, limit, callback) {
                this.tasks = tasks.slice() // 浅拷贝，避免修改原数据
                this.queue = new Set() // 任务队列
                this.limit = limit // 最大并发数
                this.callback = callback // 回调
            }
            runTask() {
                // 边界判断
                if (this.tasks.length == 0) return
                console.log("剩余任务：" + new Date(), this.tasks.length);
                // 当任务队列有剩余，继续添加任务 
                while (this.queue.size < this.limit) {
                    const task = this.tasks.shift() // 取出队头任务
                    this.queue.add(task) // 往队列中添加当前执行的任务
                    task()
                        .finally(() => {
                            this.queue.delete(task) // 当前任务执行完毕，从队列中删除改任务
                            if (this.queue.size == 0) {
                                this.callback() // 执行回调函数
                            } else {
                                this.runTask() // 继续执行下一个任务
                            }
                        })
                }
            }
            addTask(task) {
                // 同步添加任务
                this.tasks.push(task)
                // 当直接调用 addTask 也可直接执行
                this.runTask()
            }
        }
        // 生成用于测试的任务集合
        const tasks = new Array(2).fill(0).map((v, i) => {
            return function task() {
                return new Promise((resolve) => {
                    setTimeout(() => {
                        resolve(i + 1)
                    }, i * 1000);
                })
            }
        })

        var taskarr = [];
        taskarr.push(() => {

            console.log("一个新的任务", new Date());
            return new Promise((resolve) => {
                setTimeout(() => {
                    resolve()
                    console.log("任务延迟1秒返回 。。。", new Date());

                }, 1000);
            })

        });

        // 测试代码
        //const Control = new ConcurrencyControl(tasks, 2, () => {
        //    console.log(`task all finish！`)
        //})
        //Control.runTask() // 执行队列任务
        //Control.addTask(function task() { // 添加新任务
        //    return new Promise((resolve) => {
        //        setTimeout(() => {
        //            console.log(`task 9999 finish！`)
        //            resolve(999)
        //        }, 100);
        //    })
        //})
    </script>

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

                //if (i > 50) return;
                //GetCount(TableName, elm);


                console.log('TableName : ' + TableName, new Date())

                //controller.addTask(successTask(i));
                controller.addTask(Count2(TableName, elm));

            });


        });



        // 模拟成功的异步任务，接受一个id作为参数，返回一个延迟1秒后resolve的promise对象，值为id * 2
        const successTask = (id) => () =>
            new Promise((resolve) => {
                setTimeout(() => resolve(id * 2), 1000);
            });

        const Count2 = (tb,elm) => () => new Promise((resolve) => {

            var _d = Math.random();
            var searchParams = new URLSearchParams()

            //searchParams.set('TableName', 'next')
            searchParams.set("TableName", tb);
            searchParams.set("_d", _d);
            console.log('will get TableName : ' + tb, new Date())


            //setTimeout(() => resolve( "正在处理表：" + tb), 1000);
            //fetch('?act=GetRowCount', {
            //    method: 'POST',
            //    body: searchParams,
            //    //headers: {
            //    //    'Content-Type': 'application/x-www-form-urlencoded'
            //    //}
            //    //body: JSON.stringify(submitData)
            //}).then((response) => {
            //    if (response.status === 200) {
            //        return response.json();
            //    } else {
            //        throw new Error('Error fetching comments');
            //    }
            //}).then((data) => {
            //    resolve(data);
            //});

            var submitData = {
                TableName: tb,
                _d: _d
            };

            return $.post('?act=GetRowCount', submitData, function (data) {
                //$.jGrowl("------数据已经返回 数据 " + data, { life: 2000 });
                if (0 < data.StatusCode) {

                    //$.jGrowl("------操作完成！ ", { life: 8000 });
                    //$.jGrowl("日志内容 " + data.Log, { life: 5000 });
                    //window.location.href = "";
                    $(elm).children('span.RowsCountText').html('一共 ' + data.DataInfo + ' 行');
                    resolve(data);

                } else {
                    alert(data.Title);
                }
            }, 'json');



        });

        function GetCount(tb, elm) {
            //var _d = Math.round();

            //var submitData = {
            //    TableName: tb,
            //    _d: _d
            //};
            //var formData = new FormData();
            //formData.append("TableName", tb);
            //formData.append("_d", _d);

            //var searchParams = new URLSearchParams()

            ////searchParams.set('TableName', 'next')
            //searchParams.set("TableName", tb);
            //searchParams.set("_d", _d);

            //console.log('formData ', formData);

            return new Promise((resolve) => {
                console.log('will get TableName : ' + TableName, new Date())

                fetch('?act=GetRowCount', {
                    method: 'POST',
                    body: '_d=aadd&TableName=' + tb,
                    //headers: {
                    //    'Content-Type': 'application/x-www-form-urlencoded'
                    //}
                    //body: JSON.stringify(submitData)
                }).then((response) => {
                    if (response.status === 200) {
                        return response.json();
                    } else {
                        throw new Error('Error fetching comments');
                    }
                }).then((data) => {
                    resolve(data);
                });

            });
            //return fetch('?act=GetRowCount', {
            //    method: 'POST',
            //    body: searchParams,
            //    //headers: {
            //    //    'Content-Type': 'application/x-www-form-urlencoded'
            //    //}
            //    //body: JSON.stringify(submitData)
            //}).then((response) => {
            //    if (response.status === 200) {
            //        return response.json();
            //    } else {
            //        throw new Error('Error fetching comments');
            //    }
            //});



            //return $.post('?act=GetRowCount', submitData, function (data) {
            //    //$.jGrowl("------数据已经返回 数据 " + data, { life: 2000 });
            //    if (0 < data.StatusCode) {

            //        //$.jGrowl("------操作完成！ ", { life: 8000 });
            //        //$.jGrowl("日志内容 " + data.Log, { life: 5000 });
            //        //window.location.href = "";
            //        $(elm).children('span.RowsCountText').html('一共 ' + data.DataInfo + ' 行');


            //    } else {
            //        alert(data.Title);
            //    }
            //}, 'json');



        }

        function CountTable(tb, elm) {
            Control.addTask(function task() { // 添加新任务
                return new Promise((resolve) => {
                    var _d = Math.round();

                    var submitData = {
                        TableName: tb,
                        _d: _d
                    };



                    setTimeout(() => {

                        console.log("一个新的任务 tb :  " + tb, new Date());
                        $.post('?act=GetRowCount', submitData, function (data) {
                            //$.jGrowl("------数据已经返回 数据 " + data, { life: 2000 });
                            if (0 < data.StatusCode) {

                                //$.jGrowl("------操作完成！ ", { life: 8000 });
                                //$.jGrowl("日志内容 " + data.Log, { life: 5000 });
                                //window.location.href = "";
                                $(elm).children('span.RowsCountText').html('一共 ' + data.DataInfo + ' 行');


                            } else {
                                alert(data.Title);
                            }
                        }, 'json');



                        resolve(999)

                        console.log(`task ` + tb + ` finish！`)

                    }, 100);
                })
            })


        }

        async function SayTable(TableName) {


            //console.log('TableName : ' + TableName, new Date())

            await sleep(1);
            //console.log('TableName sleep : ' + TableName, new Date())


        }


        function sleep(seconds) {
            return new Promise(resolve => setTimeout(resolve, seconds * 1000));
        }


    </script>


    <!--
并发控制

    https://www.jb51.net/article/255195.htm
    https://www.jb51.net/javascript/285477naj.htm

    https://www.jb51.net/article/281145.htm


    https://www.jb51.net/article/280159.htm

    https://www.jb51.net/article/255195.htm

    -->


</head>
<body>
    <form id="form1" runat="server">
        <div>
            222
        <asp:GridView ID="GridView1" runat="server" CssClass="tb01" Style="margin-top: 0px" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="表名" />
                <asp:BoundField DataField="GlobalName" HeaderText="中文名" />
                <asp:BoundField DataField="crdate" HeaderText="创建时间" />
                <asp:BoundField DataField="refdate" HeaderText="修改时间" />
                <asp:TemplateField HeaderText="行数">
                    <ItemStyle CssClass="Lucus"></ItemStyle>
                    <ItemTemplate>
                        <span mydate="<%#Eval("name") %>" class="RowsCountText">载入中...</span>
                        <input id="Hidden5" name="id" type="hidden" value="<%#Eval("id") %>" />
                        <input id="Hidden6" name="TableName" type="hidden" value="<%#Eval("name") %>" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
            <br />
            <br />
            <a href="../ShowAllTable.aspx">查看表&gt;&gt;&gt;</a><br />
            <br />
            <a href="../ShowAllViews.aspx">查看所有的视图&gt;&gt;&gt;</a><br />


        </div>
    </form>
</body>
</html>
