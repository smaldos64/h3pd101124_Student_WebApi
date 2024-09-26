using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class StudentCourseForSaveDto
    {
        public int StudentID { get; set; }

        public int CourseID { get; set; }
    }

    public class CityLanguageForSaveAndUpdateDto : StudentCourseForSaveDto
    {
        
    }

    public class CityLanguageForUpdateDto : StudentCourseForSaveDto
    {

    }

    public class StudentCourseDto : StudentCourseForSaveDto
    {
        public StudentDtoMinusRelations ?Student { get; set; }

        public CourseDtoMinusRelations ?Course { get; set; }
    }

    public class StudentCourseDtoMinusStudentRelations
    {
        public int CourseID { get; set; }
        public CourseDtoMinusRelations ?Course { get; set; }
    }

    public class StudentCourseDtoMinusCourseRelations
    {
        public int StudentID { get; set; }
        public StudentDtoMinusRelations ?Student { get; set; }
    }
}
