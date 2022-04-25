using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 文章類別
    /// </summary>
    public class Articlecategory
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填)")]
        [MaxLength(200)]
        [Display(Name = "文章類別")]
        public string Name { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<ArticleNormal> ArticleNormals { get; set; }
    }
}