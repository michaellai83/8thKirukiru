using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.PutData
{
    public class DataArticle
    {
        /// <summary>
        /// 會員帳號
        /// </summary>
        public string memberUserName { get; set; }
        /// <summary>
        /// 切切標題
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 切切封面圖
        /// </summary>
        public string firstPhoto { get; set; }
        /// <summary>
        /// 切切簡介
        /// </summary>
        public string introduction { get; set; }
        /// <summary>
        /// 切切文章分類ID
        /// </summary>
        public int articlecategoryId { get; set; }
        /// <summary>
        /// 前置任務內容
        /// </summary>
        public List<FirstM> fArrayList { get; set; }
        /// <summary>
        /// 切切內容
        /// </summary>
        public List<Main> mArrayList { get; set; }
        /// <summary>
        /// 是否付費
        /// </summary>
        public bool isFree { get; set; }
        /// <summary>
        /// 是否發布
        /// </summary>
        public bool isPush { get; set; }
        /// <summary>
        /// 附屬任務內容
        /// </summary>
        public List<FinalM> fMissionList { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string final { get; set; }

        ///// <summary>
        ///// 備註
        ///// </summary>
        //public ICollection<Final> finaldata { get; set; }
        /// <summary>
        /// 前置任務
        /// </summary>
        public class FirstM
        {
            public string uuid { get; set; }
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
        /// 切切
        /// </summary>
        public class Main
        {
            public string uuid { get; set; }
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
        public class FinalM
        {
            public string uuid { get; set; }
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