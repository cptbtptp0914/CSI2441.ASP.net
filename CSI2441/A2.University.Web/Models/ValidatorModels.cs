using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using A2.University.Web.Models.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace A2.University.Web.Models
{
    public class StudentBaseViewModelValidator : AbstractValidator<StudentBaseViewModel>
    {
        public StudentBaseViewModelValidator()
        {
            // firstname
            RuleFor(field => field.firstname)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+(\s+[a-zA-Z]+)*$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
            // lastname
            RuleFor(field => field.lastname)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+(\s+[a-zA-Z]+)*$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
            // dob
            RuleFor(field => field.dob)
                .NotEmpty().WithMessage("* Required");
            // TODO: Resolve this with proper datepicker instead, do not just rely on Chrome datepicker
            //.Must(IsValidDate).WithMessage("* Must be a valid date");
            // gender
            RuleFor(field => field.gender)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"M|F").WithMessage("* Must be a valid gender");
            // landline
            RuleFor(field => field.ph_landline)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"\+?\(?[0-9]{2}\)?[0-9 ]{6,}")
                .WithMessage("* Must be a valid phone number")
                .Length(8, 20).WithMessage("* Must be between 8 and 20 characters");
            // mobile
            RuleFor(field => field.ph_mobile)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"\+?\(?[0-9]{2}\)?[0-9 ]{6,}")
                .WithMessage("* Must be a valid phone number")
                .Length(10, 20).WithMessage("* Must be between 10 and 20 characters");
            // adrs
            RuleFor(field => field.adrs)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[-a-zA-Z0-9.\\/#]+(\s+[-a-zA-Z0-9.\\/#]+)*$").WithMessage("* Must be a valid address")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // city
            RuleFor(field => field.adrs_city)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+(\s+[a-zA-Z]+)*$").WithMessage("* Must be a valid city")
                .Length(2, 100).WithMessage("* Must be between 2 and 100 characters");
            // state
            RuleFor(field => field.adrs_state)
                .NotEmpty().WithMessage("* Required");
            // postcode
            RuleFor(field => field.adrs_postcode)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[0-9]{4}$").WithMessage("* Must be a valid postcode");
        }

        // see comment for dob above, to be deprecated
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
            // firstname
            RuleFor(field => field.firstname)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+(\s+[a-zA-Z]+)*$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
            // lastname
            RuleFor(field => field.surname)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"^[a-zA-Z]+(\s+[a-zA-Z]+)*$").WithMessage("* Must be a valid name")
                .Length(1, 50).WithMessage("* Must be between 1 and 50 characters");
        }
    }

    public class UnitBaseViewModelValidator : AbstractValidator<UnitBaseViewModel>
    {
        public UnitBaseViewModelValidator()
        {
            // create instance of db context to validate unit id uniqueness
            UniversityEntities db = new UniversityEntities();

            // unit id, pre post
            RuleFor(field => field.unit_id)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"[A-Z]{3}[0-6]{1}[0-9]{3}").WithMessage("* Must be a valid Unit ID");

            // unit id, undergrad
            When(field => field.unit_type_id == 1, () =>
            {
                RuleFor(field => field.unit_id)
                    .NotEmpty().WithMessage("* Required")
                    .Matches(@"[A-Z]{3}[0-4]{1}[0-9]{3}").WithMessage("* Must be a valid Undergraduate Unit ID");
            });

            // unit id, postgrad
            When(field => field.unit_type_id == 2, () =>
            {
                RuleFor(field => field.unit_id)
                    .NotEmpty().WithMessage("* Required")
                    .Matches(@"[A-Z]{3}[5-6]{1}[0-9]{3}").WithMessage("* Must be a valid Postgraduate Unit ID");
            });

            // title
            RuleFor(field => field.title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.#]+(\s+[-a-zA-Z0-9.#]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.coordinator_id)
                .NotEmpty().WithMessage("* Required");
            // credit points
            RuleFor(field => field.credit_points)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.unit_type_id)
                .NotEmpty().WithMessage("* Required");

            // validate unit id uniqueness
            Custom(field =>
            {
                var unitID = db.Units.FirstOrDefault(u => u.unit_id == field.unit_id);
                if (unitID != null)
                {
                    return new ValidationFailure("unit_id", "* Unit ID already exists");
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

            // title
            RuleFor(field => field.title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.#]+(\s+[-a-zA-Z0-9.#]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.coordinator_id)
                .NotEmpty().WithMessage("* Required");
            // credit points
            RuleFor(field => field.credit_points)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.unit_type_id)
                .NotEmpty().WithMessage("* Required");
        }
    }

    public class CourseBaseViewModelValidator : AbstractValidator<CourseBaseViewModel>
    {
        public CourseBaseViewModelValidator()
        {
            // create instance of db context to validate course id
            UniversityEntities db = new UniversityEntities();

            // course id
            RuleFor(field => field.course_id)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"[A-Z]{1}[0-9]{2}").WithMessage("* Must be a valid Course ID");
            // title
            RuleFor(field => field.title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.,#\(\)]+(\s+[-a-zA-Z0-9.,#\(\)]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.coordinator_id)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.course_type_id)
                .NotEmpty().WithMessage("* Required");

            // validate course id uniqueness
            Custom(field =>
            {
                var courseID = db.Courses.FirstOrDefault(c => c.course_id == field.course_id);
                if (courseID != null)
                {
                    return new ValidationFailure("course_id", "* Course ID already exists");
                }
                return null;
            });

            // validate title uniqueness
            Custom(field =>
            {
                var title = db.Courses.FirstOrDefault(c => c.title == field.title);
                if (title != null)
                {
                    return new ValidationFailure("title", "* Title already exists");
                }
                return null;
            });
        }
    }

    public class CourseEditViewModelValidator : AbstractValidator<CourseEditViewModel>
    {
        public CourseEditViewModelValidator()
        {
            // create instance of db context to validate title
            UniversityEntities db = new UniversityEntities();

            // user cannot edit course id, not validating

            // title
            RuleFor(field => field.title)
                .NotEmpty().WithMessage("* Required")
                // fwd/back slash causes crash in title, removed from regex
                .Matches(@"^[-a-zA-Z0-9.,#\(\)]+(\s+[-a-zA-Z0-9.,#\(\)]+)*$").WithMessage("* Must be a valid Title")
                .Length(5, 100).WithMessage("* Must be between 5 and 100 characters");
            // coordinator
            RuleFor(field => field.coordinator_id)
                .NotEmpty().WithMessage("* Required");
            // unit type
            RuleFor(field => field.course_type_id)
                .NotEmpty().WithMessage("* Required");

            // validate title uniqueness
            Custom(field =>
            {
                var title = db.Courses.FirstOrDefault(c => c.title == field.title);

                var query =
                    (from t in db.Courses
                        where t.title == field.title
                        select t).ToList();
                var count = query.Count;

                if (title != null && count > 1)
                {
                    return new ValidationFailure("title", "* Title already exists");
                }
                return null;
            });
        }    
    }

    public class UnitEnrolmentBaseViewModelValidator : AbstractValidator<UnitEnrolmentBaseViewModel>
    {
        public UnitEnrolmentBaseViewModelValidator()
        {
            // create instance of db context to perform post validation
            UniversityEntities db = new UniversityEntities();

            // student
            RuleFor(field => field.student_id)
                .NotEmpty().WithMessage("* Required");
            // unit
            RuleFor(field => field.unit_id)
                .NotEmpty().WithMessage("* Required");
            // year/sem
            RuleFor(field => field.year_sem)
                .NotEmpty().WithMessage("* Required")
                .Matches(@"[0-9]{2}[1|2]").WithMessage("* Must be a valid Year/Sem");
            // mark, required
            RuleFor(field => field.mark)
                .NotEmpty().WithMessage("* Required");
            // mark, range
            Custom(field =>
            {
                int result = int.Parse(field.mark);
                if (result < 0 || result > 100)
                {
                    return new ValidationFailure("mark", "* Must be within valid range");
                }
                return null;
            });

            // TODO: implement post db validation
            // max 3 unit attempts total
            // unit cannot be passed more than once
            // same semester unit uniqueness
        }
    }
}