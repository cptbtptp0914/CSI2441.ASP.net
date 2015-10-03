using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;

namespace A2.University.Web.Models
{
    public class StudentBaseViewModelValidator : AbstractValidator<StudentBaseViewModel>
    {
        public StudentBaseViewModelValidator()
        {
            // firstname
            RuleFor(field => field.firstname)
                .NotEmpty().WithMessage("* Required")
                .Matches("(^[a-zA-Z]+$)").WithMessage("* Must be a valid name");
            // lastname
            RuleFor(field => field.lastname)
                .NotEmpty().WithMessage("* Required")
                .Matches("(^[a-zA-Z]+$)").WithMessage("* Must be a valid name");
            // dob
            RuleFor(field => field.dob)
                .NotEmpty().WithMessage("* Required")
                .Must(IsValidDate).WithMessage("* Must be a valid date");
            // gender
            RuleFor(field => field.gender)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"(M|F)").WithMessage("* Must be a valid gender");
            // landline
            // regex from http://ilikekillnerds.com/2014/08/regular-expression-for-validating-australian-phone-numbers-including-landline-and-mobile/
            RuleFor(field => field.ph_landline)
                .NotEmpty().WithMessage("* Required")
                .Matches(
                    @"/^\({0,1}((0|\+61)(2|4|3|7|8)){0,1}\){0,1}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{1}(\ |-){0,1}[0-9]{3}$/")
                .WithMessage("* Must be a valid phone number");
            // mobile
            RuleFor(field => field.ph_mobile)
                .NotEmpty().WithMessage("* Required")
                .Matches(
                    @"/^\({0,1}((0|\+61)(2|4|3|7|8)){0,1}\){0,1}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{1}(\ |-){0,1}[0-9]{3}$/")
                .WithMessage("* Must be a valid phone number");
            // adrs
            RuleFor(field => field.adrs)
                .NotEmpty().WithMessage("* Required");
            // city
            RuleFor(field => field.adrs_city)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"(^[a-zA-Z ] +$)").WithMessage("* Must be a valid city");
            // state
            RuleFor(field => field.adrs_state)
                .NotEmpty().WithMessage("* Required");
            // postcode
            RuleFor(field => field.adrs_postcode)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"(^[0-9]{4}$)").WithMessage("* Must be a valid postcode");
        }

        private bool IsValidDate(DateTime date)
        {
            Regex regex = new Regex(@"(^(((0[1-9]|[12][0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)");

            if (regex.IsMatch(date.ToString()))
            {
                return true;
            }
            return false;
        }
    }
}