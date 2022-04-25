using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.EunData
{
    /// <summary>
    /// 最多訂閱會員
    /// </summary>
    public class MemberOrderNum
    {
        public string Username { get; set; }
        public string Author { get; set; }
        public string AuthorPic { get; set; }
        public int? Num { get; set; }
    }
}