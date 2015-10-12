﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    public class StudentAccountBaseViewModel
    {
        // generated by db
        public long StudentUserId { get; set; }

        // fk
        public long StudentId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        // default = STUDENT, hidden from user
        public string Role { get; set; }
    }

    [Validator(typeof(StudentLoginViewModelValidator))]
    public class StudentLoginViewModel : StudentAccountBaseViewModel
    {

    }

    [Validator(typeof (StudentRegisterViewModelValidator))]
    public class StudentRegisterViewModel : StudentAccountBaseViewModel
    {
        public StudentRegisterViewModel()
        {
            // default role
            Role = "STUDENT";
        }
    }
}