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
        private UniversityEntities db = new UniversityEntities();

        // GET: Unit
        public ActionResult Index()
        {          
            UnitIndexViewModel unitViewModel = new UnitIndexViewModel();
            var unitsEntity = db.Units.Include(u => u.Staff).Include(u => u.UnitType).ToList();

            // transfer entity list to viewmodel list
            foreach (Unit unit in unitsEntity)
            {
                unitViewModel.Units.Add(new UnitIndexViewModel
                {
                    // core fields
                    unit_id = unit.unit_id,
                    title = unit.title,
                    coordinator_id = unit.coordinator_id,
                    credit_points = unit.credit_points,
                    unit_type_id = unit.unit_type_id,
                    // derived fields
                    staff_fullname = unit.Staff.firstname + " " + unit.Staff.surname,
                    unit_type_title = unit.UnitType.title
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
            Unit unitEntityModel = db.Units.Find(id);
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
        public ActionResult Create([Bind(Include = "unit_id,title,coordinator_id,credit_points,unit_type_id")] UnitCreateViewModel unitViewModel)
        {

            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entity model, pass values from viewmodel
                Unit unitEntityModel = new Unit();
                PopulateEntityModel(unitViewModel, unitEntityModel);

                // update db using entitymodel
                db.Units.Add(unitEntityModel);
                db.SaveChanges();
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
            Unit unitEntityModel = db.Units.Find(id);
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
        public ActionResult Edit([Bind(Include = "unit_id,title,coordinator_id,credit_points,unit_type_id")] Unit unitEntityModel, UnitEditViewModel unitViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitEntityModel).State = EntityState.Modified;
                db.SaveChanges();
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
            Unit unitEntityModel = db.Units.Find(id);
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
            Unit unit = db.Units.Find(id);
            db.Units.Remove(unit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populates dropdownlists for unit view.
        /// </summary>
        /// <param name="viewModel">UnitDropDownListViewModel</param>
        private void PopulateDropDownLists(UnitDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var staffEntity = (from staff in db.Staff
                                  select staff).ToList();
            var unitTypesEntity = (from unitType in db.UnitTypes
                               select unitType).ToList();

            // transfer relevant elements to viewmodel list
            foreach (Staff staff in staffEntity)
            {
                viewModel.Coordinators.Add(new UnitDropDownListViewModel
                {
                    coordinator_id = staff.staff_id,
                    staff_id_fullname = staff.staff_id + " " + staff.firstname + " " + staff.surname
                });
            }

            foreach (UnitType type in unitTypesEntity)
            {
                viewModel.UnitTypes.Add(new UnitDropDownListViewModel
                {
                    unit_type_id = type.unit_type_id,
                    unit_type_title = type.title
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.CoordinatorDropDownList = new SelectList(viewModel.Coordinators.OrderBy(s => s.coordinator_id), "coordinator_id", "staff_id_fullname");
            viewModel.UnitTypeTitleDropDownList = new SelectList(viewModel.UnitTypes.OrderBy(u => u.unit_type_id), "unit_type_id", "unit_type_title");
        }


        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(UnitBaseViewModel viewModel, Unit entityModel)
        {
            entityModel.unit_id = viewModel.unit_id;
            entityModel.title = viewModel.title;
            entityModel.coordinator_id = viewModel.coordinator_id;
            entityModel.credit_points = viewModel.credit_points;
            entityModel.unit_type_id = viewModel.unit_type_id;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(UnitBaseViewModel viewModel, Unit entityModel)
        {
            viewModel.unit_id = entityModel.unit_id;
            viewModel.title = entityModel.title;
            viewModel.coordinator_id = entityModel.coordinator_id;
            viewModel.credit_points = entityModel.credit_points;
            viewModel.unit_type_id = entityModel.unit_type_id;

            viewModel.staff_fullname = entityModel.Staff.firstname + " " + entityModel.Staff.surname;
            viewModel.unit_type_title = entityModel.UnitType.title;
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
                from c in db.Staff
                where c.staff_id == staff_id
                select c.firstname + " " + c.surname
            ).FirstOrDefault();

            return query;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
