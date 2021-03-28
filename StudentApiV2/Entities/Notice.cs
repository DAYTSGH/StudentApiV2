using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Notice
    {
        [Key]
        [Required]
        public Guid NoticeId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength]
        public string Content { get; set; }
        public string PhotoSource { get; set; }
        public DateTime EditTime { get; set; }
        public string Editor { get; set; }
    }
}
