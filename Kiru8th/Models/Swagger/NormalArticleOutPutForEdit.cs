using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.Swagger
{
    /// <summary>
    /// 一般文章的編輯資訊
    /// </summary>
    public class NormalArticleOutPutForEdit
    {
        public bool success { get; set; }
        public List<NormalData> data { get; set; }
        public class NormalData
        {
            public int artId { get; set; }
            public string title { get; set; }
            public string introduction { get; set; }
            public string main { get; set; }
            public int articlecategoryId { get; set; }
            public DateTime artInitDate { get; set; }
            public bool isFree { get; set; }
            public bool isPush { get; set; }
        }
    }
}