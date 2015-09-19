using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSI2441.A2.Web.MVC.Models
{
    /*******************
     * MANAGE STUDENTS *
     *******************/

    public class AddStudentViewModel : StudentModel
    {

    }

    public class SearchStudentViewModel
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Search method")]
        public string SearchMethod { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Search string")]
        public string SearchString { get; set; }

        // may need separate fields for ID and name...
    }

    public class EditStudentViewModel : SearchStudentViewModel
    {

    }

    public class DeleteStudentViewModel : SearchStudentViewModel
    {

    }

    public class ManageUnitViewModel
    {
        
    }

    public class ManageCourseViewModel
    {
        
    }

    public class ManageResultViewModel
    {
        
    }
}