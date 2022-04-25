using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 訂閱方案
    /// </summary>
    public class Subscriptionplan
    {
        [Key]
        [Display(Name = "編號")]
        public int ID { get; set; }
        /// <summary>
        /// 會員的ID
        /// </summary>
        [Display(Name = "會員ID")]
        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Members { get; set; }
        [Display(Name = "訂閱介紹")]
        public string Introduction { get; set; }
        [Display(Name = "訂閱方案金額")]
        public string Amount { get; set; }
        [Display(Name = "創建時間")]
        public DateTime InitDateTime { get; set; }
    }
}