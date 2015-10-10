using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace A2.University.Web.Models.StaffPortal
{
    [Validator(typeof(StaffBaseViewModelValidator))]
    public class StaffBaseViewModel
    {
        [Display(Name = "Staff ID")]
        public long StaffId { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        // return full name for dropdownlist
        [Display(Name = "Coordinator")]
        public string FullName { get; set; }
    }

    public class StaffIndexViewModel : StaffBaseViewModel
    {
        public List<StaffIndexViewModel> Staff = new List<StaffIndexViewModel>();
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