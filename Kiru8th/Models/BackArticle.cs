using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 後台文章
    /// </summary>
    public class BackArticle
    {
        [Key]//主鍵 PK
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
        [Display(Name = "後台管理員ID")]
        public int BackmemberID { get; set; }
        [ForeignKey("BackmemberID")]
        public virtual Backmember Backmembers { get; set; }
        [Required(ErrorMessage = "{0}必填)")]//必填，未填跳出Error訊息
        [MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "文章標題")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0}必填)")]//必填，未填跳出Error訊息
        [MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "文章封面照片")]
        public string Titlepic { get; set; }
        [Display(Name = "文章內容")]
        public string Main { get; set; }
        [Display(Name = "文章創立時間")]

        public DateTime IniDateTime { get; set; }
    }
}