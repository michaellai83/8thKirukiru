using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 一般文章留言
    /// </summary>
    public class MessageNormal
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
        /// 一般文章ID
        /// </summary>
        [Display(Name = "一般文章ID")]
        public int ArticleNorId { get; set; }

        [ForeignKey(" ArticleNorId")]
        public virtual ArticleNormal ArticleNormals { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Main { get; set; }
        /// <summary>
        /// 留言時間
        /// </summary>
        [Display(Name = "留言時間")]
        public DateTime InitDate { get; set; }
        public virtual ICollection<ReMessageNormal> ReMessageNormals { get; set; }
    }
}