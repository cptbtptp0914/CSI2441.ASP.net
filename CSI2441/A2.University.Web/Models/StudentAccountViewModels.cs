using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    // inheriting template's AccountViewModels classes for Student

    public class StudentExternalLoginConfirmationViewModel : ExternalLoginConfirmationViewModel
    {

    }

    public class StudentExternalLoginListViewModel : ExternalLoginListViewModel
    {

    }

    public class StudentSendCodeViewModel : SendCodeViewModel
    {

    }

    public class StudentVerifyCodeViewModel : VerifyCodeViewModel
    {

    }

    public class StudentForgotViewModel : ForgotViewModel
    {

    }

    [Validator(typeof(StudentLoginViewModelValidator))]
    public class StudentLoginViewModel : LoginViewModel
    {

    }

    [Validator(typeof(StudentRegisterViewModelValidator))]
    public class StudentRegisterViewModel : RegisterViewModel
    {

    }

    public class StudentResetPasswordViewModel : ResetPasswordViewModel
    {

    }

    public class StudentForgotPasswordViewModel : ForgotPasswordViewModel
    {

    }
}