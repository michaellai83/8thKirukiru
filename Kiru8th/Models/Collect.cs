using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 收藏切切文章列表
    /// </summary>
    public class Collect
    {
        /// <summary>
        /// 收藏的ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 使用者帳號Id
        /// </summary>
        [Display(Name = "使用者帳號Id")]
        public int MemberId { get; set; }
        /// <summary>
        /// 切切文章ID
        /// </summary>
        [Display(Name = "切切文章ID")]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Articles { get; set; }

    }
}