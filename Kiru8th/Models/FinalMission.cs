using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 切切的附屬任務
    /// </summary>
    public class FinalMission
    {
        [Key]
        [Display(Name = "編號")]
        public int ID { get; set; }
        [Display(Name = "文章ID")]
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public virtual Article Articles { get; set; }
        [Display(Name = "附屬任務標題")]
        public string Title { get; set; }
        [Display(Name = "附屬任務內容")]
        public string Main { get; set; }
        [Display(Name = "創建時間")]
        public DateTime InitDateTime { get; set; }
    }
}