using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Academy
    {
        [Key]
        [Required]
        public Guid AcademyId { get; set; }
        [Required]
        [MaxLength(100)]
        public string AcademyCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string AcademyName { get; set; }
    }
}
