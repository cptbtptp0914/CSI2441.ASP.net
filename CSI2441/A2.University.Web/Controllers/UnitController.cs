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
            unitViewModel.Units = db.Units.Include(u => u.Staff).Include(u => u.UnitType).ToList();

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
            SetUnitViewModel(unitViewModel, unitEntityModel);

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
            unitViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            unitViewModel.UnitTypeTitleDropDownList = new SelectList(db.UnitTypes, "unit_type_id", "title");
           
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
                SetUnitEntityModel(unitViewModel, unitEntityModel);

                // update db using entitymodel
                db.Units.Add(unitEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            unitViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            unitViewModel.UnitTypeTitleDropDownList = new SelectList(db.UnitTypes, "unit_type_id", "title");

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
            SetUnitViewModel(unitViewModel, unitEntityModel);

            // populate dropdownlists
            unitViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            unitViewModel.UnitTypeTitleDropDownList = new SelectList(db.UnitTypes, "unit_type_id", "title");

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
            unitViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            unitViewModel.UnitTypeTitleDropDownList = new SelectList(db.UnitTypes, "unit_type_id", "title");

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
            SetUnitViewModel(unitViewModel, unitEntityModel);

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
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetUnitEntityModel(UnitBaseViewModel viewModel, Unit entityModel)
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
        private void SetUnitViewModel(UnitBaseViewModel viewModel, Unit entityModel)
        {
            viewModel.unit_id = entityModel.unit_id;
            viewModel.title = entityModel.title;
            viewModel.coordinator_id = entityModel.coordinator_id;
            viewModel.credit_points = entityModel.credit_points;
            viewModel.unit_type_id = entityModel.unit_type_id;

            viewModel.coordinator_name = entityModel.Staff.firstname + " " + entityModel.Staff.surname;
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
