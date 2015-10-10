using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    // inherit template ManageViewModels classes for Staff

    public class StaffManageIndexViewModel : IndexViewModel
    {

    }

    public class StaffManageLoginsViewModel : ManageLoginsViewModel
    {
        
    }

    public class StaffFactorViewModel : FactorViewModel
    {
        
    }

    public class StaffSetPasswordViewModel : SetPasswordViewModel
    {
        
    }

    [Validator(typeof(StaffChangePasswordViewModelValidator))]
    public class StaffChangePasswordViewModel : ChangePasswordViewModel
    {
        
    }

    public class StaffAddPhoneNumberViewModel : AddPhoneNumberViewModel
    {
        
    }

    public class StaffVerifyPhoneNumberViewModel : VerifyPhoneNumberViewModel
    {
        
    }

    public class StaffConfigureTwoFactorViewModel : ConfigureTwoFactorViewModel
    {
        
    }
}