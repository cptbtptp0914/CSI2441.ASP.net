﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CSI2441.A2.Web.MVC.Utilities;

namespace CSI2441.A2.Web.MVC.Models
{
    /**
     * Student superclass.
     */
    public class StudentModel
    {

        [Display(Name = "First name")]
        [Required(ErrorMessage = "The First name field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "The Surname field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string Surname { get; set; }

        // Generated by DB, may use for temp storage, else remove if redundant.
        // TODO: Add method at controller to retrieve generated ID.
        public string StudentID { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date, ErrorMessage = "Must be a date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Dob { get; set; }

        // Generated by controller, may use for temp storage, else remove if redundant.
        // TODO: Add method at controller to generate email.
        public string Email { get; set; }

        [Display(Name = "Landline")]
        [Required(ErrorMessage = "The Landline field is required.")]
        [RegularExpression("(\\+?\\(?[0-9]{2}\\)?[0-9 ]{10,})", ErrorMessage = "Must be a phone number.")]
        public string Landline { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "The Mobile field is required.")]
        [RegularExpression("(\\+?\\(?[0-9]{2}\\)?[0-9 ]{10,})", ErrorMessage = "Must be a phone number.")]
        public string Mobile { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "The City field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "The State field is required.")]
        public string State { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "The Postcode field is required.")]
        [RegularExpression("(^[0-9]{4}$)", ErrorMessage = "Must be a valid postcode.")]
        public int Postcode { get; set; }
    }
}