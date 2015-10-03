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

namespace A2.University.Web.Controllers
{
    public class UnitEnrolmentController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: UnitEnrolments
        public ActionResult Index()
        {
            UnitEnrolmentIndexViewModel unitEnrolmentViewModel = new UnitEnrolmentIndexViewModel();
            unitEnrolmentViewModel.UnitEnrolments =
                db.UnitEnrolments.Include(u => u.Student).Include(u => u.Unit).ToList();

            return View(unitEnrolmentViewModel.UnitEnrolments);
        }

        // GET: UnitEnrolments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            UnitEnrolment unitEnrolmentEntityModel = db.UnitEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEnrolmentDetailsViewModel unitEnrolmentViewModel = new UnitEnrolmentDetailsViewModel();
            SetUnitViewModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

            if (unitEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(unitEnrolmentViewModel);
        }

        // GET: UnitEnrolments/Create
        public ActionResult Create()
        {
            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname");
            ViewBag.unit_id = new SelectList(db.Units, "unit_id", "title");
            return View();
        }

        // POST: UnitEnrolments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "unit_enrolment_id,student_id,unit_id,year_sem,mark")] UnitEnrolment unitEnrolment)
        {
            if (ModelState.IsValid)
            {
                db.UnitEnrolments.Add(unitEnrolment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname", unitEnrolment.student_id);
            ViewBag.unit_id = new SelectList(db.Units, "unit_id", "title", unitEnrolment.unit_id);
            return View(unitEnrolment);
        }

        // GET: UnitEnrolments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitEnrolment unitEnrolment = db.UnitEnrolments.Find(id);
            if (unitEnrolment == null)
            {
                return HttpNotFound();
            }
            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname", unitEnrolment.student_id);
            ViewBag.unit_id = new SelectList(db.Units, "unit_id", "title", unitEnrolment.unit_id);
            return View(unitEnrolment);
        }

        // POST: UnitEnrolments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "unit_enrolment_id,student_id,unit_id,year_sem,mark")] UnitEnrolment unitEnrolment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitEnrolment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.student_id = new SelectList(db.Students, "student_id", "firstname", unitEnrolment.student_id);
            ViewBag.unit_id = new SelectList(db.Units, "unit_id", "title", unitEnrolment.unit_id);
            return View(unitEnrolment);
        }

        // GET: UnitEnrolments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitEnrolment unitEnrolment = db.UnitEnrolments.Find(id);
            if (unitEnrolment == null)
            {
                return HttpNotFound();
            }
            return View(unitEnrolment);
        }

        // POST: UnitEnrolments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            UnitEnrolment unitEnrolment = db.UnitEnrolments.Find(id);
            db.UnitEnrolments.Remove(unitEnrolment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetUnitEnrolmentEntityModel(UnitEnrolmentBaseViewModel viewModel, UnitEnrolment entityModel)
        {
            entityModel.unit_enrolment_id = viewModel.unit_enrolment_id;
            entityModel.student_id = viewModel.student_id;
            entityModel.unit_id = viewModel.unit_id;
            entityModel.year_sem = viewModel.year_sem;
            entityModel.mark = viewModel.mark;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetUnitViewModel(UnitEnrolmentBaseViewModel viewModel, UnitEnrolment entityModel)
        {
            viewModel.unit_enrolment_id = entityModel.unit_enrolment_id;
            viewModel.student_id = entityModel.student_id;
            viewModel.unit_id = entityModel.unit_id;
            viewModel.year_sem = entityModel.year_sem;
            viewModel.mark = entityModel.mark;

            viewModel.firstname = entityModel.Student.firstname;
            viewModel.lastname = entityModel.Student.lastname;
            viewModel.title = entityModel.Unit.title;
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
