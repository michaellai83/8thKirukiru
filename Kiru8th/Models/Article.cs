using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 切切文章
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        [Key]//主鍵 PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }

        /// <summary>
        /// 使用者帳號Id
        /// </summary>
        [Display(Name = "使用者帳號Id")]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
        /// <summary>
        /// 封面圖片名稱
        /// </summary>
        [Display(Name = "封面圖片名稱")]
        public string FirstPicName { get; set; }
        /// <summary>
        /// 文章標題
        /// </summary>
        [Display(Name = "文章標題")]
        public string Title { get; set; }
        /// <summary>
        /// 是否付費
        /// </summary>
        [Display(Name = "是否付費")]
        public Boolean IsFree { get; set; }
        /// <summary>
        /// 文章簡介
        /// </summary>
        [Display(Name = "文章簡介")]
        public string Introduction { get; set; }
        /// <summary>
        /// 文章類型
        /// </summary>
        //ForeignKey
        [Display(Name = "文章類別ID")]
        public int ArticlecategoryId { get; set; }

        [ForeignKey("ArticlecategoryId")]
        public virtual Articlecategory Articlecategory { get; set; }
        /// <summary>
        /// 是否發布
        /// </summary>
        [Display(Name = "是否發布")]
        public Boolean IsPush { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        [Display(Name = "創建時間")]
        public DateTime InitDate { get; set; }
        /// <summary>
        /// 愛心的數量
        /// </summary>
        [Display(Name = "愛心數量")]
        public int Lovecount { get; set; }
        /// <summary>
        /// 文章備註
        /// </summary>
        [Display(Name = "備註")]
        public string FinalText { get; set; }
        /// <summary>
        /// 文章使否被管理者停權
        /// </summary>
        [Display(Name = "停權")]
        public bool IsManangerStop { get; set; }

        public virtual ICollection<Firstmission> Firstmissions { get; set; }
        public virtual ICollection<ArticleMain> ArticleMains { get; set; }
        public virtual ICollection<FinalMission> FinalMissions { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Collect> Collects { get; set; }
    }
}