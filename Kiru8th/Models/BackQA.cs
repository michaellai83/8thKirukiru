using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 後台QA文章
    /// </summary>
    public class BackQA
    {
        /// <summary>
        /// 編號ID
        /// </summary>
        [Key]//主鍵 PK
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        [Display(Name = "問題")]
        public string Title { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        [Display(Name = "答案")]
        public string Answer { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        [Display(Name = "建立時間")]
        public DateTime InitDateTime { get; set; }
    }
}