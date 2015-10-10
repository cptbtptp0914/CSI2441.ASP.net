using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;
using A2.University.Web.Models.StaffPortal;
using FluentValidation;
using FluentValidation.Results;

namespace A2.University.Web.Models
{
    public class StudentBaseViewModelValidator : AbstractValidator<StudentBaseViewModel>
    {
        public StudentBaseViewModelValidator()
        {
            // FirstName
            RuleFor(field => field.FirstName)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
            // LastName
            RuleFor(field => field.LastName)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
            // Dob, min age
            RuleFor(field => field.Dob)
                .NotEmpty().WithMessage("* Required")
                .GreaterThanOrEqualTo(field => field.MinAge).WithMessage("* Must be at least 16 years old");
            // Gender
            RuleFor(field => field.Gender)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"M|F").WithMessage("* Must be a valid Gender");
            // landline
            RuleFor(field => field.LandLine)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"\+?\(?[0-9]{2}\)?[0-9 ]{6,}")
                .WithMessage("* Must be a valid phone number")
                .Length(8, 20).WithMessage("* Must be between 8 and 20 characters");
            // mobile
            RuleFor(field => field.Mobile)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"\+?\(?[0-9]{2}\)?[0-9 ]{6,}")
                .WithMessage("* Must be a valid phone number")
                .Length(10, 20).WithMessage("* Must be between 10 and 20 characters");
            // Adrs
            RuleFor(field => field.Adrs)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[-a-zA-Z0-9.\\/#']+(\s+[-a-zA-Z0-9.\\/#']+)*$").WithMessage("* Must be a valid address")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // city
            RuleFor(field => field.AdrsCity)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z',]+(\s+[a-zA-Z',]+)*$").WithMessage("* Must be a valid city")
                .Length(2, 100).WithMessage("* Must be between 2 and 100 characters");
            // state
            RuleFor(field => field.AdrsState)
                .NotEmpty().WithMessage("* Required");
            // postcode
            RuleFor(field => field.AdrsPostcode)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[0-9]{4}$").WithMessage("* Must be a valid postcode");
        }

        // see comment for Dob above, to be deprecated
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

    public class StaffBaseViewModelValidator : AbstractValidator<StaffBaseViewModel>
    {
        public StaffBaseViewModelValidator()
        {
            // FirstName
            RuleFor(field => field.FirstName)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
            // LastName
            RuleFor(field => field.LastName)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
        }
    }

    public class UnitBaseViewModelValidator : AbstractValidator<UnitBaseViewModel>
    {
        public UnitBaseViewModelValidator()
        {
            UnitRules unitRules = new UnitRules();

            // unit id, pre post
            RuleFor(field => field.UnitId)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"[A-Z]{3}[0-6]{1}[0-9]{3}").WithMessage("* Must be a valid Unit ID");

            // unit id, undergrad
            When(field => field.UnitTypeId == 1, () =>
            {
                RuleFor(field => field.UnitId)
                    .NotEmpty().WithMessage("* Required")
                    .Matches(@"[A-Z]{3}[0-4]{1}[0-9]{3}").WithMessage("* Must be a valid Undergraduate Unit ID");
            });

            // unit id, postgrad
            When(field => field.UnitTypeId == 2, () =>
            {
                RuleFor(field => field.UnitId)
                    .NotEmpty().WithMessage("* Required")
                    .Matches(@"[A-Z]{3}[5-6]{1}[0-9]{3}").WithMessage("* Must be a valid Postgraduate Unit ID");
            });

            // Title
            RuleFor(field => field.Title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in Title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.#]+(\s+[-a-zA-Z0-9.#]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.CoordinatorId)
                .NotEmpty().WithMessage("* Required");
            // credit points
            RuleFor(field => field.CreditPoints)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.UnitTypeId)
                .NotEmpty().WithMessage("* Required");

            /**************************
             * SERVER SIDE VALIDATION *
             **************************/

            // unit id uniqueness
            Custom(field =>
            {
                if (unitRules.IsNotUniqueUnitId(field.UnitId))
                {
                    return new ValidationFailure("UnitId", "* Unit ID already exists");
                }
                return null;
            });
        }
    }

    public class UnitEditViewModelValidator : AbstractValidator<UnitEditViewModel>
    {
        public UnitEditViewModelValidator()
        {
            // user cannot edit unit id, not validating

            // Title
            RuleFor(field => field.Title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in Title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.#]+(\s+[-a-zA-Z0-9.#]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.CoordinatorId)
                .NotEmpty().WithMessage("* Required");
            // credit points
            RuleFor(field => field.CreditPoints)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.UnitTypeId)
                .NotEmpty().WithMessage("* Required");
        }
    }

    public class CourseBaseViewModelValidator : AbstractValidator<CourseBaseViewModel>
    {
        public CourseBaseViewModelValidator()
        {
            // create instance of db context to perform serverside validation
            UniversityEntities db = new UniversityEntities();

            // course id
            RuleFor(field => field.CourseId)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"[A-Z]{1}[0-9]{2}").WithMessage("* Must be a valid Course ID");
            // Title
            RuleFor(field => field.Title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in Title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.,#\(\)]+(\s+[-a-zA-Z0-9.,#\(\)]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.CoordinatorId)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.CourseTypeId)
                .NotEmpty().WithMessage("* Required");

            /**************************
             * SERVER SIDE VALIDATION *
             **************************/

            // validate course id uniqueness
            Custom(field =>
            {
                var courseId = db.Courses.FirstOrDefault(c => c.course_id == field.CourseId);
                if (courseId != null)
                {
                    return new ValidationFailure("CourseId", "* Course ID already exists");
                }
                return null;
            });

            // validate Title uniqueness
            Custom(field =>
            {
                var title = db.Courses.FirstOrDefault(c => c.title == field.Title);
                if (title != null)
                {
                    return new ValidationFailure("Title", "* Title already exists");
                }
                return null;
            });
        }
    }
    
    /// <summary>
    /// Validator for CourseEditViewModelValidator.
    /// Should have inherited base class above to avoid duplication, but would have to override the constructor anyway.
    /// This class does not set CourseId to required.
    /// TODO: If there's time, try to define base class as generic to allow for inheritance.
    /// </summary>
    public class CourseEditViewModelValidator : AbstractValidator<CourseEditViewModel>
    {
        public CourseEditViewModelValidator()
        {
            // create instance of db context to perform serverside validation
            UniversityEntities db = new UniversityEntities();

            // user cannot edit course id, not validating

            // Title
            RuleFor(field => field.Title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in Title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.,#\(\)]+(\s+[-a-zA-Z0-9.,#\(\)]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.CoordinatorId)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.CourseTypeId)
                .NotEmpty().WithMessage("* Required");

            /**************************
             * SERVER SIDE VALIDATION *
             **************************/

            // validate Title uniqueness
            Custom(field =>
            {
                var title = db.Courses.FirstOrDefault(c => c.title == field.Title);

                // get count of matches
                var query =
                    (from t in db.Courses
                        where t.title == field.Title
                        select t).ToList();
                var count = query.Count;

                // only return error if new Title is not unique, else allow current Title
                if (title != null && count > 1)
                {
                    return new ValidationFailure("Title", "* Title already exists");
                }
                return null;
            });
        }    
    }

    public class UnitEnrolmentBaseViewModelValidator : AbstractValidator<UnitEnrolmentBaseViewModel>
    {
        public UnitEnrolmentBaseViewModelValidator()
        {
            // create instance of db context to serverside validation
            UniversityEntities db = new UniversityEntities();
            // create instance of unit/course rules model
            UnitRules unitRules = new UnitRules();
            CourseRules courseRules = new CourseRules();

            // student
            RuleFor(field => field.StudentId)
                .NotEmpty().WithMessage("* Required");
            // unit
            RuleFor(field => field.UnitId)
                .NotEmpty().WithMessage("* Required");
            // year/sem
            RuleFor(field => field.YearSem)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"[0-9]{2}[1|2]").WithMessage("* Must be a valid Year/Sem");
            // mark, required
            RuleFor(field => field.Mark)
                .NotEmpty().WithMessage("* Required");

            /**************************
             * SERVER SIDE VALIDATION *
             **************************/

            // mark, range
            Custom(field =>
            {
                int result = int.Parse(field.Mark);
                if (result < 0 || result > 100)
                {
                    return new ValidationFailure("Mark", "* Must be within valid range");
                }
                return null;
            });

            // unit pass uniqueness
            Custom(field =>
            {
                if (unitRules.IsNotUniquePassedUnit(field.UnitEnrolmentId, field.StudentId, field.UnitId, int.Parse(field.Mark)))
                {
                    return new ValidationFailure("UnitId", "* Student has already passed this Unit");
                }
                return null;
            });

            // unit uniqueness per semester
            Custom(field =>
            {
                if (unitRules.IsNotUniqueUnitInSem(field.UnitEnrolmentId, field.StudentId, field.UnitId, int.Parse(field.YearSem)))
                {
                    return new ValidationFailure("UnitId", "* Enrolment already exists in Semester");
                }
                return null;
            });

            // unit max attempts
            Custom(field =>
            {
                if (unitRules.IsMaxAttempts(field.UnitEnrolmentId, field.StudentId, field.UnitId))
                {
                    return new ValidationFailure("UnitId", "* Student has reached max attempts for Unit");
                }
                return null;
            });
        }
    }

    public class CourseEnrolmentBaseViewModelValidator : AbstractValidator<CourseEnrolmentBaseViewModel>
    {
        public CourseEnrolmentBaseViewModelValidator()
        {
            // create instance of db context to perform serverside validation
            UniversityEntities db = new UniversityEntities();
            // create instance of course rules model
            CourseRules courseRules = new CourseRules();

            // student
            RuleFor(field => field.StudentId)
                .NotEmpty().WithMessage("* Required");
            // course
            RuleFor(field => field.CourseId)
                .NotEmpty().WithMessage("* Required");

            /**************************
             * SERVER SIDE VALIDATION *
             **************************/
            
            // course uniqueness
            Custom(field =>
            {
                if (courseRules.IsNotUniqueCourse(field.StudentId, field.CourseId))
                {
                    return new ValidationFailure("CourseId", "* Enrolment already exists for Student");
                }
                return null;
            });
        }
    }

    public class StaffLoginViewModelValidator : AbstractValidator<StaffLoginViewModel>
    {
        public StaffLoginViewModelValidator()
        {
            // email
            RuleFor(field => field.Email)
                .NotEmpty().WithMessage("* Required")
                .EmailAddress().WithMessage("* Must be a valid email");
            // password
            RuleFor(field => field.Password)
                .NotEmpty().WithMessage("* Required");
        }
    }

    public class StaffRegisterViewModelValidator : AbstractValidator<StaffRegisterViewModel>
    {
        public StaffRegisterViewModelValidator()
        {
            // create instance of db context to perform serverside validation
            UniversityEntities dbUni = new UniversityEntities();
            ApplicationDbContext dbUser = new ApplicationDbContext();

            // email
            RuleFor(field => field.Email)
                .NotEmpty().WithMessage("* Required")
                .EmailAddress().WithMessage("* Must be a valid email");
            // password
            RuleFor(field => field.Password)
                .NotEmpty().WithMessage("* Required")
                .Length(8, 50).WithMessage("* Must be between 8 and 50 characters")
                .Matches(@"^(?=.*?[0-9].*?[0-9])^(?=.*?[A-Z].*?[A-Z])[0-9a-zA-Z!@#$%\\/|\-_^<>{}[\]\?.,0-9]{8,}$")
                    .WithMessage("* Must contain at least: two UPPERCASE, two numbers, eight characters");
            // confirm password
            RuleFor(field => field.ConfirmPassword)
                .NotEmpty().WithMessage("* Required")
                .Equal(field => field.ConfirmPassword).WithMessage("* Must match password");

            /**************************
             * SERVER SIDE VALIDATION *
             **************************/
            
            // staff exists in staff table
            Custom(field =>
            {
                var existingStaff = dbUni.Staff.FirstOrDefault(s => s.email == field.Email);
                if (existingStaff == null)
                {
                    return new ValidationFailure("Email", "* Staff email does not exist");
                }
                return null;
            });

            // user uniqueness
            Custom(field =>
            {
                var existingUser = dbUser.Users.FirstOrDefault(u => u.Email == field.Email);
                if (existingUser != null)
                {
                    return new ValidationFailure("Email", "* User already exists");
                }
                return null;
            });
        }
    }

    public class StaffChangePasswordViewModelValidator : AbstractValidator<StaffChangePasswordViewModel>
    {
        public StaffChangePasswordViewModelValidator()
        {
            // new password
            RuleFor(field => field.NewPassword)
                .NotEmpty().WithMessage("* Required")
                .Length(8, 50).WithMessage("* Must be between 8 and 50 characters")
                .Matches(@"^(?=.*?[0-9].*?[0-9])^(?=.*?[A-Z].*?[A-Z])[0-9a-zA-Z!@#$%\\/|\-_^<>{}[\]\?.,0-9]{8,}$")
                .WithMessage("* Must contain at least: two UPPERCASE, two numbers, eight characters");
            // confirm password
            RuleFor(field => field.ConfirmPassword)
                .NotEmpty().WithMessage("* Required")
                .Equal(field => field.ConfirmPassword).WithMessage("* Must match password");
        }
    }
}