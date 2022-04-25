using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Swagger
{
    /// <summary>
    /// 留言回傳格式
    /// </summary>
    public class OutPutMessage
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        public int messageId { get; set; }
        /// <summary>
        /// 留言者大頭貼
        /// </summary>
        public string messageMemberPic { get; set; }
        /// <summary>
        /// 留言帳號
        /// </summary>
        public string messageMember { get; set; }
        /// <summary>
        /// 留言內容
        /// </summary>
        public string messageMain { get; set; }
        /// <summary>
        /// 留言時間
        /// </summary>
        public string messageInitDate { get; set; }
    }
}