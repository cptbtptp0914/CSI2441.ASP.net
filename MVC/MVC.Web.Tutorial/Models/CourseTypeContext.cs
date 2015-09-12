using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC.Web.Tutorial.Models
{
    public class CourseTypeContext : DbContext
    {
        public DbSet<CourseType> CourseTypes { get; set; }
    }
}