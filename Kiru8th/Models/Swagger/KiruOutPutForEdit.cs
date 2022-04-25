using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.Swagger
{
    /// <summary>
    /// 切切回傳格式給編輯文章用
    /// </summary>
    public class KiruOutPutForEdit
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 回傳資料內容
        /// </summary>
        public ICollection<Data> data { get; set; }
        /// <summary>
        /// 回傳資料
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 文章ID
            /// </summary>
            public int artId { get; set; }
            /// <summary>
            /// 文章標題
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 文章封面圖片
            /// </summary>
            public string firstPhoto { get; set; }
            /// <summary>
            /// 文章簡介
            /// </summary>
            public string introduction { get; set; }
            /// <summary>
            /// 文章類型ID
            /// </summary>
            public int articlecategoryId { get; set; }
            /// <summary>
            /// 文章類型名稱
            /// </summary>
            public string artArtlog { get; set; }
            /// <summary>
            /// 前置內容
            /// </summary>
            public ICollection<FirstM> fArrayList { get; set; }
            /// <summary>
            /// 切切內容
            /// </summary>

            public ICollection<Main> mArrayList { get; set; }
            /// <summary>
            /// 附屬任務內容
            /// </summary>
            public ICollection<FinamlM> fMissionList { get; set; }
            /// <summary>
            /// 是否付費
            /// </summary>

            public bool isFree { get; set; }
            /// <summary>
            /// 是否發布
            /// </summary>
            public bool isPush { get; set; }
            /// <summary>
            /// 備註
            /// </summary>
            public string final { get; set; }

        }
        /// <summary>
        /// 前置任務內容
        /// </summary>
        public class FirstM
        {
            /// <summary>
            /// 前置任務ID
            /// </summary>
            public int fId { get; set; }
            /// <summary>
            /// 前置任務圖片
            /// </summary>
            public string secPhoto { get; set; }
            /// <summary>
            /// 前置任務內容
            /// </summary>
            public string mission { get; set; }

        }
        /// <summary>
        /// 切切內容
        /// </summary>
        public class Main
        {
            /// <summary>
            /// 切切ID
            /// </summary>
            public int mId { get; set; }
            /// <summary>
            /// 切切圖片
            /// </summary>
            public string thirdPhoto { get; set; }
            /// <summary>
            /// 切切內容
            /// </summary>
            public string main { get; set; }
        }
        /// <summary>
        /// 附屬任務
        /// </summary>
        public class FinamlM
        {
            /// <summary>
            /// 附屬任務ID
            /// </summary>
            public int fId { get; set; }
            /// <summary>
            /// 附屬任務標題
            /// </summary>
            public string auxiliary { get; set; }
            /// <summary>
            /// 附屬任務內容
            /// </summary>
            public string auxiliarymain { get; set; }
        }
    }
}