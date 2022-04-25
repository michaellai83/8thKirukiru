using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 切切的留言
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 留言的ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 會員的帳號
        /// </summary>
        [Display(Name = "會員帳號")]
        public string UserName { get; set; }
        /// <summary>
        /// 切切文章ID
        /// </summary>
        [Display(Name = "切切文章ID")]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Articles { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Main { get; set; }
        /// <summary>
        /// 留言時間
        /// </summary>
        [Display(Name = "留言時間")]
        public DateTime InitDate { get; set; }
        public virtual ICollection<ReMessage> ReMessages { get; set; }
    }
}