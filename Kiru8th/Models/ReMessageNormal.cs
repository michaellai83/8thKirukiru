using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kiru8th.Models
{
    /// <summary>
    /// 一般文章回覆
    /// </summary>
    public class ReMessageNormal
    {
        /// <summary>
        /// 回復留言的ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 留言的ID
        /// </summary>
        [Display(Name = "留言ID")]
        public int MessageNorId { get; set; }

        [ForeignKey("MessageNorId")]
        public virtual MessageNormal MessageNormals { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Main { get; set; }
        /// <summary>
        /// 留言時間
        /// </summary>
        [Display(Name = "留言時間")]
        public DateTime InitDate { get; set; }
    }
}