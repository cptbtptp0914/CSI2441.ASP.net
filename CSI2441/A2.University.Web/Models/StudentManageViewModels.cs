using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    /// <summary>
    /// Defines view models for StudentManage.
    /// Inherits template ManageViewModels classes.
    /// All classes here deprecated, no longer using Identity Entity Framework.
    /// </summary>
    public class StudentManageIndexViewModel : IndexViewModel
    {
        public string Email { get; set; }
    }

    public class StudentManageLoginsViewModel : ManageLoginsViewModel
    {

    }

    public class StudentFactorViewModel : FactorViewModel
    {

    }

    public class StudentSetPasswordViewModel : SetPasswordViewModel
    {

    }

    [Validator(typeof(StudentChangePasswordViewModelValidator))]
    public class StudentChangePasswordViewModel : ChangePasswordViewModel
    {
        public string Email { get; set; }
    }

    public class StudentAddPhoneNumberViewModel : AddPhoneNumberViewModel
    {

    }

    public class StudentVerifyPhoneNumberViewModel : VerifyPhoneNumberViewModel
    {

    }

    public class StudentConfigureTwoFactorViewModel : ConfigureTwoFactorViewModel
    {

    }
}