using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 後台管理員
    /// </summary>
    public class Backmember
    {
        [Key]//主鍵 PK
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
        [Required(ErrorMessage = "{0}必填)")]//必填，未填跳出Error訊息
        [MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "帳號")]
        public string Username { get; set; }
        [Required(ErrorMessage = "{0}必填)")]//必填，未填跳出Error訊息
        [Display(Name = "密碼")]
        public string Password { get; set; }
        [Display(Name = "鹽")]
        public string Salt { get; set; }
        [Display(Name = "暱稱")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}必填)")]//必填，未填跳出Error訊息
        [MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "信箱")]
        public string Email { get; set; }
        [Display(Name = "大頭照")]
        public string Photo { get; set; }
        [Display(Name = "註冊時間")]
        public DateTime IniDateTime { get; set; }

        public virtual ICollection<BackArticle> BackArticles { get; set; }
    }
}