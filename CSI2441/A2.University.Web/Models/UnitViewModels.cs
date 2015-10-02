using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Models
{
    public class UnitBaseViewModel
    {
        [Key]
        [Display(Name = "Unit ID")]
        [Required(ErrorMessage = "The Unit ID field is required.")]
        [RegularExpression("([A-Z]{3}[0-9]{4})", ErrorMessage = "Must be a valid Unit ID.")]
        public string unit_id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The Title field is required.")]
        [RegularExpression("(^[a-zA-Z0-9\\.\\,\\# ]{5,}$)", ErrorMessage = "Must be a valid Title.")]
        public string title { get; set; }

        [Display(Name = "Coordinator")]
        [Required(ErrorMessage = "The Coordinator field is required.")]
        public long coodinator_id { get; set; }

        [Display(Name = "Credit Points")]
        [Required(ErrorMessage = "The Credit Points field is required.")]
        public int credit_points { get; set; }

        [Display(Name = "Unit Type")]
        [Required(ErrorMessage = "The Unit Type field is required.")]
        public long unit_type_id { get; set; }

        [Display(Name = "Coordinator")]
        public string coordinator_name { get; set; }

        [Display(Name = "Unit Type")]
        public string unit_type_title { get; set; }
    }

    public class UnitDropDownListViewModel : UnitBaseViewModel
    {
        // to be populated by db
        public IEnumerable<SelectListItem> CoordinatorDropDownList { get; set; }
        public IEnumerable<SelectListItem> UnitTypeTitleDropDownList { get; set; }

        // credit points
        public IEnumerable<SelectListItem> CreditPointsDropDownList = new List<SelectListItem>
        {
            new SelectListItem {Value = "15", Text = "15"},
            new SelectListItem {Value = "20", Text = "20"},
            new SelectListItem {Value = "60", Text = "60"}
        };
    }

    public class UnitDetailsViewModel : UnitBaseViewModel
    {
        // No custom fields required
    }

    public class UnitCreateViewModel : UnitDropDownListViewModel
    {
        // Inherits UnitDropDownListViewModel, no custom fields required
    }

    public class UnitEditViewModel : UnitDropDownListViewModel
    {
        
    }
}