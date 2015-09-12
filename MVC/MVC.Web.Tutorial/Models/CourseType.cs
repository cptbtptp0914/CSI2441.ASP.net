using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Web.Tutorial.Models
{
    [Table("CourseType")]
    public class CourseType
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int CourseCP { get; set; }
        public int CourseDuration { get; set; }
    }
}