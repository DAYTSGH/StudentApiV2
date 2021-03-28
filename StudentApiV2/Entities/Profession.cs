using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Entities
{
    public class Profession
    {
        [Key]
        [Required]
        public Guid ProfessionId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProfessionCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProfessionName { get; set; }
        public Guid AcademyId { get; set; }
        public Academy Academy { get; set; }
    }
}
