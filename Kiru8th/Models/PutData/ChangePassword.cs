using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.PutData
{
    /// <summary>
    /// 更改密碼
    /// </summary>
    public class ChangePassword
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 舊密碼
        /// </summary>
        public string O_Password { get; set; }
        /// <summary>
        /// 新密碼
        /// </summary>
        public string N_Password { get; set; }
    }
}