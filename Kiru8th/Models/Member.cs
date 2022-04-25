using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 會員
    /// </summary>
    public class Member
    {
        [Key]//主鍵 PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name = "帳號")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "密碼")]
        public string PassWord { get; set; }
        [Required]
        [Display(Name = "鹽")]
        public string PasswordSalt { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name = "暱稱")]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name = "信箱")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "興趣")]
        public int ArticlecategoryId { get; set; }
        [Display(Name = "信箱是否驗證")]
        public bool Isidentify { get; set; }

        public string Emailidentify { get; set; }
        [Display(Name = "註冊日期")]
        public DateTime initDate { get; set; }
        [Display(Name = "大頭貼")]
        public string MemberPicName { get; set; }
        [Display(Name = "自我介紹")]
        public string Introduction { get; set; }
        [Display(Name = "是否公開收藏")]
        public bool Opencollectarticles { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<ArticleNormal> ArticleNormals { get; set; }
        public virtual ICollection<Orderlist> Orderlists { get; set; }
        public virtual ICollection<Subscriptionplan> Subscriptionplans { get; set; }




    }
}