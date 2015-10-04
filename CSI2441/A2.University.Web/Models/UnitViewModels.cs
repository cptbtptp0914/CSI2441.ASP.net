using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    public class UnitIndexViewModel
    {
        public List<Unit> Units { get; set; }
    }

    [Validator(typeof(UnitBaseViewModelValidator))]
    public class UnitBaseViewModel
    {
        [Key]
        [Display(Name = "Unit ID")]
        public string unit_id { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Coordinator")]
        public long coordinator_id { get; set; }

        [Display(Name = "Credit Points")]
        public int credit_points { get; set; }

        [Display(Name = "Unit Type")]
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

    [Validator(typeof(UnitEditViewModelValidator))]
    public class UnitEditViewModel : UnitDropDownListViewModel
    {
        // Uses own validator, ignores unit id for validation since user cannot edit
    }

    public class UnitDeleteViewModel : UnitBaseViewModel
    {
        // No custom fields required
    }
}