using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }

        [Required]
        [MaxLength(100)]
        public string ?StudentName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ?StudentLastName { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
               = new List<StudentCourse>();
        
        [ForeignKey("TeamID")]
        public int TeamID { get; set; }

        public virtual Team Team { get; set; }
    }
}
