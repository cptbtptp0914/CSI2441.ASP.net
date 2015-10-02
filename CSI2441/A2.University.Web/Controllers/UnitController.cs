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
            var units = db.Units.Include(u => u.Staff).Include(u => u.UnitType);
            return View(units.ToList());
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
            // show full name in dropdownlist
            ViewBag.coodinator_id = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title");
            
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
        public ActionResult Create([Bind(Include = "unit_id,title,coodinator_id,credit_points,unit_type_id")] UnitCreateViewModel unitViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entity model, pass values from viewmodel
                Unit unitEntityModel = new Unit();
                SetUnitViewModel(unitViewModel, unitEntityModel);

                // update db using entitymodel
                db.Units.Add(unitEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

//            ViewBag.coodinator_id = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname", unit.coodinator_id);
//            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title", unit.unit_type_id);

            // populate dropdownlists
            unitViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            unitViewModel.UnitTypeTitleDropDownList = new SelectList(db.UnitTypes, "unit_type_id", "title");

            return View(unitViewModel);
        }

        // GET: Unit/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            ViewBag.coodinator_id = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname", unit.coodinator_id);
            // ViewBag.unit_type_id deprecated, see ViewData below
            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title", unit.unit_type_id);

            // ViewData replaces ViewBag for unit_type_id above, ensures default selection of value from db
            List<UnitType> unitTypeList = new List<UnitType>(db.UnitTypes.ToList());
            ViewData["unitTypeList"] = unitTypeList;

            return View(unit);
        }

        // POST: Unit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "unit_id,title,coodinator_id,credit_points,unit_type_id")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.coodinator_id = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname", unit.coodinator_id);
            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title", unit.unit_type_id);
            return View(unit);
        }

        // GET: Unit/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
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
            entityModel.coodinator_id = viewModel.coodinator_id;
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
            viewModel.coodinator_id = entityModel.coodinator_id;
            viewModel.credit_points = entityModel.credit_points;
            viewModel.unit_type_id = entityModel.unit_type_id;

            viewModel.coordinator_name = GetCoordinatorFullName(entityModel.coodinator_id);
            viewModel.unit_type_title = GetUnitTypeTitle(entityModel.unit_type_id);
        }


        /// <summary>
        /// SQL statement returns coordinator's full name.
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


        /// <summary>
        /// SQL statement returns unit type title.
        /// </summary>
        /// <param name="unit_type_id">long</param>
        /// <returns>string</returns>
        private string GetUnitTypeTitle(long unit_type_id)
        {
            var query = (
                from ut in db.UnitTypes
                where ut.unit_type_id == unit_type_id
                select ut.title
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
