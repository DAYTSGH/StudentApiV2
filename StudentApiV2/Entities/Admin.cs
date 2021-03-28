using StudentApiV2.CodeTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Admin
    {
        [Key]
        [Required]
        public Guid AdminId { get; set; }
        [Required]
        [MaxLength(100)]
        public string AdminCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string AdminName { get; set; }
        [Required]
        public UserType AdminType { get; set; } = UserType.管理员;
        [Required]
        [MaxLength(100)]
        public string AdminPassword { get; set; } = "e10adc3949ba59abbe56e057f20f883e";

    }
}
