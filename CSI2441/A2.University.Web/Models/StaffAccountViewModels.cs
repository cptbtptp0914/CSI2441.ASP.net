using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    // inheriting template's AccountViewModels classes for Staff

    public class StaffExternalLoginConfirmationViewModel : ExternalLoginConfirmationViewModel
    {

    }

    public class StaffExternalLoginListViewModel : ExternalLoginListViewModel
    {
        
    }

    public class StaffSendCodeViewModel : SendCodeViewModel
    {
        
    }

    public class StaffVerifyCodeViewModel : VerifyCodeViewModel
    {
        
    }

    public class StaffForgotViewModel : ForgotViewModel
    {
        
    }

    public class StaffLoginViewModel : LoginViewModel
    {
        
    }

    [Validator(typeof(StaffRegisterViewModelValidator))]
    public class StaffRegisterViewModel : RegisterViewModel
    {
        
    }

    public class StaffResetPasswordViewModel : ResetPasswordViewModel
    {
        
    }

    public class StaffForgotPasswordViewModel : ForgotPasswordViewModel
    {
        
    }
}