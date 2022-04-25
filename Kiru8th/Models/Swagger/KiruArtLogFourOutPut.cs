using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.Swagger
{
    /// <summary>
    /// 依照類別取得前四筆切切文章(用時間)
    /// </summary>
    public class KiruArtLogFourOutPut
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 回傳資料內容
        /// </summary>
        public List<Data> data { get; set; }

        /// <summary>
        /// 回傳資料
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 切切文章ID
            /// </summary>
            public int ArticleID { get; set; }
            /// <summary>
            /// 切切文章作者帳號
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 切切標題
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 切切簡介
            /// </summary>
            public string ArtInfo { get; set; }
            /// <summary>
            /// 切切文章類別名稱
            /// </summary>
            public string Articlecategory { get; set; }
            /// <summary>
            /// 切切封面照片
            /// </summary>
            public string ArtPic { get; set; }
            /// <summary>
            /// 切切是否付費
            /// </summary>
            public bool Isfree { get; set; }
            /// <summary>
            /// 切切愛心數量
            /// </summary>
            public int Lovecount { get; set; }
            /// <summary>
            /// 切切建立時間
            /// </summary>
            public DateTime InitDateTime { get; set; }
        }
    }
}