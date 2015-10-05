﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    [Validator(typeof(StaffBaseViewModelValidator))]
    public class StaffBaseViewModel
    {
        [Display(Name = "Staff ID")]
        public long staff_id { get; set; }

        [Display(Name = "First name")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        public string surname { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        // return full name for dropdownlist
        [Display(Name = "Coordinator")]
        public string fullname { get; set; }
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