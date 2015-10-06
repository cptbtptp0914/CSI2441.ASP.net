using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;
using Microsoft.Ajax.Utilities;

namespace A2.University.Web.Controllers
{
    public class UnitController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();

        // GET: Unit
        public ActionResult Index()
        {          
            UnitIndexViewModel unitViewModel = new UnitIndexViewModel();

            var unitsEntity = _db.Units
                .Include(u => 
                    u.Staff)
                .Include(u => 
                    u.UnitType)
                .ToList();

            // transfer entity list to viewmodel list
            foreach (Unit unit in unitsEntity)
            {
                unitViewModel.Units.Add(new UnitIndexViewModel
                {
                    // core fields
                    UnitId = unit.unit_id,
                    Title = unit.title,
                    CoordinatorId = unit.coordinator_id,
                    CreditPoints = unit.credit_points,
                    UnitTypeId = unit.unit_type_id,
                    // derived fields
                    StaffFullName = 
                        $"{unit.Staff.firstname} " + 
                        $"{unit.Staff.surname}",

                    UnitTypeTitle = unit.UnitType.title
                });
            }

            // render view using viewmodel list
            return View(unitViewModel.Units);
        }

        // GET: Unit/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Unit unitEntityModel = _db.Units.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitDetailsViewModel unitViewModel = new UnitDetailsViewModel();
            PopulateViewModel(unitViewModel, unitEntityModel);

            if (unitEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(unitViewModel);
        }

        // GET: Unit/Create
        public ActionResult Create()
        {   
            // create viewmodel
            UnitCreateViewModel unitViewModel = new UnitCreateViewModel();
            PopulateDropDownLists(unitViewModel);
           
            // render view using viewmodel
            return View(unitViewModel);
        }

        // POST: Unit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UnitId,Title,CoordinatorId,CreditPoints,UnitTypeId")] UnitCreateViewModel unitViewModel)
        {

            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entity model, pass values from viewmodel
                Unit unitEntityModel = new Unit();
                PopulateEntityModel(unitViewModel, unitEntityModel);

                // update db using entitymodel
                _db.Units.Add(unitEntityModel);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(unitViewModel);

            // render view using viewmodel
            return View(unitViewModel);
        }

        // GET: Unit/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Unit unitEntityModel = _db.Units.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEditViewModel unitViewModel = new UnitEditViewModel();
            PopulateViewModel(unitViewModel, unitEntityModel);

            // populate dropdownlists
            PopulateDropDownLists(unitViewModel);

            if (unitEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(unitViewModel);
        }

        // POST: Unit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnitId,Title,CoordinatorId,CreditPoints,UnitTypeId")] Unit unitEntityModel, UnitEditViewModel unitViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate entitymodel
                PopulateEntityModel(unitViewModel, unitEntityModel);

                // update db using entitymodel
                _db.Entry(unitEntityModel).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(unitViewModel);

            return View(unitViewModel);
        }

        // GET: Unit/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Unit unitEntityModel = _db.Units.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitDeleteViewModel unitViewModel = new UnitDeleteViewModel();
            PopulateViewModel(unitViewModel, unitEntityModel);

            if (unitEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(unitViewModel);
        }

        // POST: Unit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Unit unit = _db.Units.Find(id);
            _db.Units.Remove(unit);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populates dropdownlists for unit view.
        /// </summary>
        /// <param name="viewModel">UnitDropDownListViewModel</param>
        private void PopulateDropDownLists(UnitDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var staffEntity = _db.Staff.ToList();
            var unitTypesEntity = _db.UnitTypes.ToList();

            // transfer relevant elements to viewmodel list
            foreach (Staff staff in staffEntity)
            {
                viewModel.Coordinators.Add(new UnitDropDownListViewModel
                {
                    CoordinatorId = staff.staff_id,
                    StaffIdFullName = 
                        $"{staff.staff_id} " +
                        $"{staff.firstname} " +
                        $"{staff.surname}"
                });
            }

            foreach (UnitType type in unitTypesEntity)
            {
                viewModel.UnitTypes.Add(new UnitDropDownListViewModel
                {
                    UnitTypeId = type.unit_type_id,
                    UnitTypeTitle = type.title
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.CoordinatorDropDownList = new SelectList(viewModel.Coordinators.OrderBy(s => s.CoordinatorId), "CoordinatorId", "StaffIdFullName");
            viewModel.UnitTypeTitleDropDownList = new SelectList(viewModel.UnitTypes.OrderBy(u => u.UnitTypeId), "UnitTypeId", "UnitTypeTitle");
        }


        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(UnitBaseViewModel viewModel, Unit entityModel)
        {
            entityModel.unit_id = viewModel.UnitId;
            entityModel.title = viewModel.Title;
            entityModel.coordinator_id = viewModel.CoordinatorId;
            entityModel.credit_points = viewModel.CreditPoints;
            entityModel.unit_type_id = viewModel.UnitTypeId;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(UnitBaseViewModel viewModel, Unit entityModel)
        {
            viewModel.UnitId = entityModel.unit_id;
            viewModel.Title = entityModel.title;
            viewModel.CoordinatorId = entityModel.coordinator_id;
            viewModel.CreditPoints = entityModel.credit_points;
            viewModel.UnitTypeId = entityModel.unit_type_id;

            viewModel.StaffFullName =
                $"{entityModel.Staff.firstname} " +
                $"{entityModel.Staff.surname}";

            viewModel.UnitTypeTitle = entityModel.UnitType.title;
        }

        /// <summary>
        /// SQL statement returns coordinator's full name.
        /// Deprecated, leaving as example, may require similar function.
        /// </summary>
        /// <param name="staff_id">long</param>
        /// <returns>string</returns>
        private string GetCoordinatorFullName(long staff_id)
        {
            var query = (
                from c in _db.Staff
                where c.staff_id == staff_id
                select c.firstname + " " + c.surname
            ).FirstOrDefault();

            return query;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
