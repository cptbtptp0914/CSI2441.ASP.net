using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace A2.University.Web.Models
{
    public class StudentDetailsViewModel
    {
        [Key]
        [Display(Name = "Student ID")]
        public long student_id { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "The First name field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "The Surname field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string lastname { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date, ErrorMessage = "Must be a date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime dob { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "The Gender field is required.")]
        [RegularExpression("(M|F)", ErrorMessage = "Must be a valid gender.")]
        public string gender { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Landline")]
        [Required(ErrorMessage = "The Landline field is required.")]
        public string ph_landline { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "The Mobile field is required.")]
        public string ph_mobile { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The Address field is required.")]
        public string adrs { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "The City field is required.")]
        public string adrs_city { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "The State field is required.")]
        public string adrs_state { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "The Postcode field is required.")]
        [RegularExpression("(^[0-9]{4}$)", ErrorMessage = "Must be a valid postcode.")]
        public int adrs_postcode { get; set; }
    }
}