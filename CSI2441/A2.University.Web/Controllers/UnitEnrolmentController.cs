using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business;
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
            var unitEnrolmentsEntity = db.UnitEnrolments.Include(u => u.Student).Include(u => u.Unit).ToList();

            // transfer entity list to viewmodel list
            foreach (UnitEnrolment unitEnrolment in unitEnrolmentsEntity)
            {
                unitEnrolmentViewModel.UnitEnrolments.Add(new UnitEnrolmentIndexViewModel
                {
                    unit_enrolment_id = unitEnrolment.unit_enrolment_id,
                    student_id = unitEnrolment.student_id,
                    firstname = unitEnrolment.Student.firstname,
                    lastname = unitEnrolment.Student.lastname,
                    unit_id = unitEnrolment.unit_id,
                    title = unitEnrolment.Unit.title,
                    year_sem = unitEnrolment.year_sem.ToString(),
                    mark = unitEnrolment.mark.ToString(),
                    grade = GradeRules.GetGrade(unitEnrolment.mark)
                });
            }

            // render view using viewmodel list
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
            PopulateViewModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

            if (unitEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(unitEnrolmentViewModel);
        }

        // GET: UnitEnrolments/Create
        public ActionResult Create()
        {
            // create viewmodel
            UnitEnrolmentCreateViewModel unitEnrolmentViewModel = new UnitEnrolmentCreateViewModel();
            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            return View(unitEnrolmentViewModel);
        }

        // POST: UnitEnrolments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "unit_enrolment_id,student_id,unit_id,year_sem,mark")] UnitEnrolmentCreateViewModel unitEnrolmentViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entitymodel, pass values from viewmodel
                UnitEnrolment unitEnrolmentEntityModel = new UnitEnrolment();
                PopulateEntityModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

                // update db using entitymodel
                db.UnitEnrolments.Add(unitEnrolmentEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            return View(unitEnrolmentViewModel);
        }

        // GET: UnitEnrolments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            UnitEnrolment unitEnrolmentEntityModel = db.UnitEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEnrolmentEditViewModel unitEnrolmentViewModel = new UnitEnrolmentEditViewModel();
            PopulateViewModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            if (unitEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(unitEnrolmentViewModel);
        }

        // POST: UnitEnrolments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "unit_enrolment_id,student_id,unit_id,year_sem,mark")] UnitEnrolment unitEnrolmentEntityModel, UnitEnrolmentEditViewModel unitEnrolmentViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitEnrolmentEntityModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            return View(unitEnrolmentViewModel);
        }

        // GET: UnitEnrolments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            UnitEnrolment unitEnrolmentEntityModel = db.UnitEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEnrolmentDeleteViewModel unitEnrolmentViewModel = new UnitEnrolmentDeleteViewModel();
            PopulateViewModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

            if (unitEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(unitEnrolmentViewModel);
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
        /// Populates dropdownlists for unit enrolment view.
        /// </summary>
        /// <param name="viewModel">UnitEnrolmentDropDownListViewModel</param>
        private void PopulateDropDownLists(UnitEnrolmentDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var studentsEntity = (from student in db.Students
                select student).ToList();
            var unitsEntity = (from units in db.Units
                select units).ToList();

            // transfer relevant elements to viewmodel list
            foreach (Student student in studentsEntity)
            {
                viewModel.Students.Add(new UnitEnrolmentDropDownListViewModel
                {
                    student_id = student.student_id,
                    student_id_fullname = student.student_id + " "  + student.firstname + " " + student.lastname
                });
            }

            foreach (Unit unit in unitsEntity)
            {
                viewModel.Units.Add(new UnitEnrolmentDropDownListViewModel
                {
                    unit_id = unit.unit_id,
                    unit_id_title = unit.unit_id + " " + unit.title
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.StudentDropDownList = new SelectList(viewModel.Students.OrderBy(s => s.student_id), "student_id", "student_id_fullname");
            viewModel.UnitDropDownList = new SelectList(viewModel.Units.OrderBy(u => u.unit_id), "unit_id", "unit_id_title");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(UnitEnrolmentBaseViewModel viewModel, UnitEnrolment entityModel)
        {
            entityModel.unit_enrolment_id = viewModel.unit_enrolment_id;
            entityModel.student_id = viewModel.student_id;
            entityModel.unit_id = viewModel.unit_id;
            entityModel.year_sem = int.Parse(viewModel.year_sem);
            entityModel.mark = int.Parse(viewModel.mark);
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(UnitEnrolmentBaseViewModel viewModel, UnitEnrolment entityModel)
        {
            viewModel.unit_enrolment_id = entityModel.unit_enrolment_id;
            viewModel.student_id = entityModel.student_id;
            viewModel.unit_id = entityModel.unit_id;
            viewModel.year_sem = entityModel.year_sem.ToString();
            viewModel.mark = entityModel.mark.ToString();

            viewModel.fullname = entityModel.Student.firstname + " " + entityModel.Student.lastname;
            viewModel.title = entityModel.Unit.title;
            viewModel.grade = GradeRules.GetGrade(entityModel.mark);
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
