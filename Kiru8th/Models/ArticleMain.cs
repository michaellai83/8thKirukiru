using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 切切的內容
    /// </summary>
    public class ArticleMain
    {
        /// <summary>
        /// 切切內容的ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 切切內容的圖片名稱
        /// </summary>
        [Display(Name = "圖片名稱")]
        public string PicName { get; set; }
        /// <summary>
        /// 切切文章內容
        /// </summary>
        [Display(Name = "文章內容")]
        public string Main { get; set; }
        /// <summary>
        /// 文章的ID
        /// </summary>
        [Display(Name = "文章ID")]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Articles { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        [Display(Name = "創立時間")]
        public DateTime InDateTime { get; set; }
    }
}