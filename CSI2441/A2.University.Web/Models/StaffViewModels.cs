using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Models
{
    public class StaffIndexViewModel
    {
        public List<Staff> StaffList { get; set; }
    }

    public class StaffBaseViewModel
    {
        [Display(Name = "Staff ID")]
        public long staff_id { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "The First name field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "The Surname field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string surname { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        // return full name for dropdownlist
        [Display(Name = "Coordinator")]
        public string fullname
        {
            get { return firstname + " " + surname; }
        }
    }

    public class StaffDetailsViewModel : StaffBaseViewModel
    {
        // No custom fields required
    }

    public class StaffCreateViewModel : StaffBaseViewModel
    {
        // No custom fields required
    }

    public class StaffEditViewModel : StaffBaseViewModel
    {
        // No custom fields required
    }

    public class StaffDeleteViewModel : StaffBaseViewModel
    {
        // No custom fields required
    }
}