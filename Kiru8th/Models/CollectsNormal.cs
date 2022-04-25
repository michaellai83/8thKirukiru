using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 收藏一般文章列表
    /// </summary>
    public class CollectsNormal
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
        /// 一般文章ID
        /// </summary>
        [Display(Name = "一班文章ID")]
        public int ArticleNormalId { get; set; }

        [ForeignKey("ArticleNormalId")]
        public virtual ArticleNormal NormalArticles { get; set; }
    }
}