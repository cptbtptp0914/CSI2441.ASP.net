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
    [Validator(typeof(UnitBaseViewModelValidator))]
    public class UnitBaseViewModel
    {
        [Key]
        [Display(Name = "Unit ID")]
        public string UnitId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Coordinator")]
        public long CoordinatorId { get; set; }

        [Display(Name = "Credit Points")]
        public int CreditPoints { get; set; }

        [Display(Name = "Unit Type")]
        public long UnitTypeId { get; set; }

        [Display(Name = "Coordinator")]
        public string StaffFullName { get; set; }

        [Display(Name = "Unit Type")]
        public string UnitTypeTitle { get; set; }
    }

    public class UnitIndexViewModel : UnitBaseViewModel
    {
        public List<UnitIndexViewModel> Units = new List<UnitIndexViewModel>();
    }

    public class UnitDropDownListViewModel : UnitBaseViewModel
    {
        // store lists of coordinators/unit types, will extract data for dropdownlists
        public List<UnitDropDownListViewModel> Coordinators = new List<UnitDropDownListViewModel>();
        public List<UnitDropDownListViewModel> UnitTypes = new List<UnitDropDownListViewModel>();
        // store derived items for dropdownlist
        public string StaffIdFullName { get; set; }

        // to be populated by controller
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