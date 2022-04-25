using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.Swagger
{
    /// <summary>
    /// 回覆留言格式
    /// </summary>
    public class OutPutReMessage
    {
        /// <summary>
        /// 回覆留言本身ID
        /// </summary>
        public int reMessageId { get; set; }
        /// <summary>
        /// 回覆留言內容
        /// </summary>
        public string reMessageMain { get; set; }
        /// <summary>
        /// 回覆留言時間
        /// </summary>
        public string reMessageInitDate { get; set; }
    }
}