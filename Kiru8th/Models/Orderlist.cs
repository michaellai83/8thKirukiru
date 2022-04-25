using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 訂閱清單
    /// </summary>
    public class Orderlist
    {
        /// <summary>
        /// 訂單ID
        /// </summary>
        [Key]//主鍵 PK
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        [Display(Name = "訂單編號")]
        public string Ordernumber { get; set; }
        /// <summary>
        /// 訂閱者的ID
        /// </summary>
        [Display(Name = "訂閱者的會員ID")]
        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Members { get; set; }
        /// <summary>
        /// 被訂閱者的帳號
        /// </summary>
        [Display(Name = "被訂閱者的帳號")]
        public string AuthorName { get; set; }
        /// <summary>
        /// 訂閱金額
        /// </summary>
        [Display(Name = "訂閱金額")]
        public int Amount { get; set; }
        /// <summary>
        /// 是否交易成功
        /// </summary>
        [Display(Name = "是否交易成功")]
        public bool Issuccess { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        [Display(Name = "訂閱時間")]
        public DateTime InitDateTime { get; set; }
    }
}