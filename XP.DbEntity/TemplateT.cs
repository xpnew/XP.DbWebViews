using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Attributes;

namespace XP.DB.DbEntity
{

    /// <summary>
    /// 模板循环类型
    /// </summary>
    /// <remarks>
    /// 1 不循环，当前model；2、整体解析+局部循环 3、循环（整体数据优先）4、循环（列表数据优先）
    /// </remarks>
    public enum TemplateLoopType {
        /// <summary>
        /// 不循环，当前model
        /// </summary>
        CurrentModel =1,
        /// <summary>
        /// 整体解析+局部循环 (需要子模板)
        /// </summary>
        CurrentModelAndLoop = 2,
        /// <summary>
        /// 循环（整体数据优先，即先用ModelList替换）
        /// </summary>
        Loop4ListFirst = 4,

        /// <summary>
        /// 循环（列表数据优先,即先用CurrentModel替换）
        /// </summary>
        Loop4ModelFirst = 8

    }
    /// <summary>
    /// 输出类型： 1 单个文件；2  多个文件；3 页面HTML ； 4 页面 文本域
    /// </summary>
    /// <remarks>
    /// 1 单个文件；2  多个文件；3 页面HTML ； 4 页面 文本域
    /// </remarks>
    public enum TemplateOutType
    {
        SingleFile = 1,

        MuiltFile = 2,
        PageHtml = 4,


        PageTextArea = 8

    }




    [Serializable]
    [IdentityClass]
    public class TemplateT
    {

        [IdentityFieldAttribute]
        [PrimaryKey]
        public int Id { get; set; }

        public string Tit { get; set; }
        public string Summary { get; set; }
        public string Cot { get; set; }
        public string FileNameTemplate { get; set; }



        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }


        public TemplateLoopType? LoopType { get; set; }


        public TemplateOutType? OutType { get; set; }



    }
}
